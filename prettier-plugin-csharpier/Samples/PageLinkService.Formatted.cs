namespace Insite.Spire.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using Insite.Core.Context;
    using Insite.Core.Interfaces.Data;
    using Insite.Core.Interfaces.Dependency;
    using Insite.Core.Interfaces.Plugins.Caching;
    using Insite.Core.Plugins.Content;
    using Insite.Core.Spire;
    using Insite.Data.Entities;
    using Insite.Spire.Models;

    public class PageLinkService : IPageLinkService
    {
        private readonly INavigationFilterService navigationFilterService;

        private readonly IContentModeProvider contentModeProvider;

        private readonly IRetrievePageService retrievePageService;

        private readonly Lazy<IPageUrlService> pageUrlService;

        private readonly ICacheManager cacheManager;

        public PageLinkService(
            INavigationFilterService navigationFilterService,
            IContentModeProvider contentModeProvider,
            IRetrievePageService retrievePageService,
            Lazy<IPageUrlService> pageUrlService,
            ICacheManager cacheManager)
        {
            this.navigationFilterService = navigationFilterService;
            this.contentModeProvider = contentModeProvider;
            this.retrievePageService = retrievePageService;
            this.pageUrlService = pageUrlService;
            this.cacheManager = cacheManager;
        }

        public IList<PageLinkModel> GetLinks(
            IUnitOfWork unitOfWork,
            ISiteContext siteContext)
        {
            var checkedPageUrlsCacheKey = $"PageLinkService_CheckedPageUrls_{siteContext.LanguageDto.Id}_{siteContext.WebsiteDto.Id}";
            if (!this.cacheManager.Contains(checkedPageUrlsCacheKey))
            {
                if (!unitOfWork.GetRepository<PageUrl>().GetTableAsNoTracking().Any(
                    o => o.LanguageId == siteContext.LanguageDto.Id && o.WebsiteId == siteContext.WebsiteDto.Id))
                {
                    var homePageNodes = unitOfWork.GetRepository<Node>().GetTableAsNoTracking().Where(
                        o => o.Type == "HomePage" && o.WebsiteId == siteContext.WebsiteDto.Id).Select(
                        o => o.Id).ToList();

                    foreach (var nodeId in homePageNodes)
                    {

                        this.pageUrlService.Value.UpdateUrls(
                            unitOfWork,
                            nodeId);
                    }
                }

                this.cacheManager.Add(checkedPageUrlsCacheKey, true);
            }

            var queryable = unitOfWork.GetRepository<PageUrl>().GetTableAsNoTracking().Where(
                o => o.LanguageId == siteContext.LanguageDto.Id && o.WebsiteId == siteContext.WebsiteDto.Id);
            var pageUrls = queryable.Where(
                o => o.PublishOn.HasValue && o.PublishOn < DateTimeOffset.Now && (o.PublishUntil == null || o.PublishUntil > DateTimeOffset.Now)).Select(
                o => new PageUrlSelector
                {
                    NodeId = o.NodeId,
                    Url = o.Url,
                    Type = o.Node.Type,
                    SortOrder = o.Node.SortOrder,
                    ParentId = o.Node.ParentId,
                    Title = o.Title,
                    PublishOn = o.PublishOn,
                    HideFromSearchEngines = o.HideFromSearchEngines,
                    ExcludeFromNavigation = o.ExcludeFromNavigation,
                    ExcludeFromSignInRequired = o.ExcludeFromSignInRequired
                }).ToList();

            var foundNodeIds = pageUrls.Select(o => o.NodeId).ToHashSet();

            Dictionary<Guid, PageUrlSelector> unpublishedPageUrls = null;

            if (this.contentModeProvider.DisplayUnpublishedContent)
            {
                unpublishedPageUrls = queryable.Where(
                    o => !o.PublishOn.HasValue).Select(
                    o => new PageUrlSelector
                    {
                        NodeId = o.NodeId,
                        Url = o.Url,
                        Type = o.Node.Type,
                        SortOrder = o.Node.SortOrder,
                        ParentId = o.Node.ParentId,
                        Title = o.Title,
                        HideFromSearchEngines = o.HideFromSearchEngines,
                        ExcludeFromNavigation = o.ExcludeFromNavigation,
                        ExcludeFromSignInRequired = o.ExcludeFromSignInRequired
                    }).ToDictionary(o => o.NodeId, o => o);
            }

            if (unpublishedPageUrls != null)
            {
                foreach (var pageUrl in unpublishedPageUrls)
                {
                    if (!foundNodeIds.Contains(pageUrl.Key))
                    {
                        pageUrls.Add(pageUrl.Value);
                    }
                }
            }

            var allLinks = new Dictionary<Guid?, IList<PageLinkModel>>();
            var getSignInPageUrl = new Lazy<string>(
                () => RetrievePageService.GetSignInPageUrl(
                    this.retrievePageService,
                    unitOfWork,
                    siteContext));
            foreach (var thePageUrl in pageUrls)
            {
                if (unpublishedPageUrls == null || !unpublishedPageUrls.TryGetValue(
                    thePageUrl.NodeId,
                    out var pageUrl))
                {
                    pageUrl = thePageUrl;
                }

                var excludeFromNavigation = pageUrl.ExcludeFromNavigation;
                var hideFromSearchEngines = pageUrl.HideFromSearchEngines;

                var parameter = new NavigationFilterParameter
                {
                    SiteContext = siteContext,
                    Type = NavigationFilterType.RetrievingLinks,
                    SignInPageUrl = getSignInPageUrl,
                    ExcludeFromSignInRequired = pageUrl.ExcludeFromSignInRequired
                };

                var navigationFilterResult = this.navigationFilterService.ApplyFilters(
                    pageUrl.Type,
                    parameter);
                if (navigationFilterResult != null)
                {
                    if (!navigationFilterResult.DisplayLink)
                    {
                        excludeFromNavigation = true;
                    }

                    if (navigationFilterResult.StatusCode == HttpStatusCode.Redirect)
                    {
                        hideFromSearchEngines = true;
                    }
                }

                var parentId = pageUrl.ParentId ?? Guid.Empty;

                if (!allLinks.ContainsKey(parentId))
                {
                    allLinks[parentId] = new List<PageLinkModel>();
                }

                var pageLink = new PageLinkModel
                {
                    Title = pageUrl.Title,
                    Url = pageUrl.Url,
                    Id = pageUrl.NodeId,
                    ExcludeFromNavigation = excludeFromNavigation,
                    HideFromSearchEngines = hideFromSearchEngines,
                    SortOrder = pageUrl.SortOrder,
                    PublishOn = pageUrl.PublishOn
                };

                if (pageUrl.Type != "Page")
                {
                    pageLink.Type = pageUrl.Type;
                }

                allLinks[parentId].Add(pageLink);
            }

            if (!allLinks.TryGetValue(Guid.Empty, out var result))
            {
                return null;
            }

            void LoadChildren(IEnumerable<PageLinkModel> pageLinks)
            {
                foreach (var pageLink in pageLinks)
                {
                    if (!allLinks.ContainsKey(pageLink.Id))
                    {
                        continue;
                    }

                    pageLink.Children = allLinks[pageLink.Id].GroupBy(
                        o => o.Id).Select(
                        o => o.OrderBy(x => x.PublishOn).First()).OrderBy(
                        o => o.SortOrder).ToList();
                    LoadChildren(pageLink.Children);
                }
            }

            LoadChildren(result);

            return result.OrderBy(o => o.SortOrder).ToList();
        }

        private class PageUrlSelector
        {
            public Guid NodeId { get; set; }
            public string Url { get; set; }
            public string Type { get; set; }
            public Guid? ParentId { get; set; }
            public string Title { get; set; }
            public int SortOrder { get; set; }
            public DateTimeOffset? PublishOn { get; set; }
            public bool ExcludeFromNavigation { get; set; }
            public bool HideFromSearchEngines { get; set; }
            public bool ExcludeFromSignInRequired { get; set; }
        }
    }

    public interface IPageLinkService : IDependency
    {
        IList<PageLinkModel> GetLinks(
            IUnitOfWork unitOfWork,
            ISiteContext siteContext);
    }
}
