namespace Insite.Spire.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Net;
    using System.Net.Http;
    using System.Web;
    using Insite.Catalog.Services;
    using Insite.Catalog.Services.Parameters;
    using Insite.Catalog.Services.Results;
    using Insite.Common.Extensions;
    using Insite.Common.Logging;
    using Insite.Common.Providers;
    using Insite.Core.Context;
    using Insite.Core.Interfaces.Data;
    using Insite.Core.Interfaces.Dependency;
    using Insite.Core.Interfaces.Plugins.RulesEngine;
    using Insite.Core.Plugins.Catalog;
    using Insite.Core.Plugins.Content;
    using Insite.Core.Plugins.Pipelines;
    using Insite.Core.Services;
    using Insite.Core.Spire;
    using Insite.Core.WebApi;
    using Insite.Data.Entities;
    using Insite.Data.Extensions;
    using Insite.Data.Repositories.Interfaces;
    using Insite.Spire.Models;
    using Insite.WebFramework.Routing.Pipelines;
    using Insite.WebFramework.Routing.Pipelines.Parameters;
    using Insite.WebFramework.Routing.Pipelines.Results;
    using Newtonsoft.Json;

    public class RetrievePageService : IRetrievePageService
    {
        private readonly ICatalogPathBuilder catalogPathBuilder;
        private readonly ICatalogService catalogService;
        private readonly INavigationFilterService navigationFilterService;
        private readonly IContentModeProvider contentModeProvider;
        private readonly ISiteContextService siteContextService;
        private readonly Lazy<ICatalogPathFinder> catalogPathFinder;
        private readonly IRulesEngine rulesEngine;
        private readonly IHtmlRedirectPipeline htmlRedirectPipeline;

        public RetrievePageService(
            ICatalogPathBuilder catalogPathBuilder,
            ICatalogService catalogService,
            INavigationFilterService navigationFilterService,
            IContentModeProvider contentModeProvider,
            ISiteContextServiceFactory siteContextServiceFactory,
            Lazy<ICatalogPathFinder> catalogPathFinder,
            IRulesEngine rulesEngine,
            IHtmlRedirectPipeline htmlRedirectPipeline)
        {
            this.catalogPathBuilder = catalogPathBuilder;
            this.catalogService = catalogService;
            this.navigationFilterService = navigationFilterService;
            this.contentModeProvider = contentModeProvider;
            this.siteContextService = siteContextServiceFactory.GetSiteContextService();
            this.catalogPathFinder = catalogPathFinder;
            this.rulesEngine = rulesEngine;
            this.htmlRedirectPipeline = htmlRedirectPipeline;
        }

        public RetrievePageResult GetPageByType(
            IUnitOfWork unitOfWork,
            ISiteContext siteContext,
            string type)
        {
            var pageVersionQuery = this.PageVersionQuery(
                unitOfWork,
                siteContext);
            var pageVersions = pageVersionQuery.Where(
                o => o.Page.Node.Type == type).ToList();

            return this.CreateResult(
                this.GetPageVersionVariantJson(
                    unitOfWork,
                    pageVersions,
                    siteContext),
                unitOfWork,
                siteContext,
                null);
        }

        public PagesCollectionModel GetPagesByParent(
            IUnitOfWork unitOfWork,
            ISiteContext siteContext,
            Guid parentNodeId)
        {
            var pageVersionQuery = this.PageVersionQuery(
                unitOfWork,
                siteContext);
            var pageVersions = pageVersionQuery.Where(
                o => o.Page.Node.ParentId == parentNodeId)
            .AsEnumerable().GroupBy(o => o.Page.NodeId).ToDictionary(
                o => o.Key,
                o => o.ToList());

            var pages = new List<PageModel>();
            foreach (var keyValuePair in pageVersions)
            {
                pages.Add(
                    JsonConvert.DeserializeObject<PageModel>(
                        this.GetPageVersionVariantJson(
                            unitOfWork,
                            keyValuePair.Value,
                            siteContext)));
            }

            return new PagesCollectionModel { Pages = pages };
        }

        public IQueryable<PageUrl> GetPublishedPageUrlsByType(
            IUnitOfWork unitOfWork,
            ISiteContext siteContext,
            string type)
        {
            var nodeQuery = unitOfWork.GetRepository<Node>().GetTableAsNoTracking().Where(
                node => node.WebsiteId == siteContext.WebsiteDto.Id && node.Type == type).Select(
                node => node.Id);

            var now = DateTimeProvider.Current.Now;

            return unitOfWork.GetRepository<PageUrl>().GetTableAsNoTracking().Where(
                pageUrl => nodeQuery.Contains(
                    pageUrl.NodeId) && pageUrl.LanguageId == siteContext.LanguageDto.Id && pageUrl.PublishOn <= now && (pageUrl.PublishUntil == null || pageUrl.PublishUntil > now));
        }

        public RetrievePageResult GetPageByUrl(
            IUnitOfWork unitOfWork,
            ISiteContext siteContext,
            string url)
        {
            var htmlRedirect = this.GetHtmlRedirect(url);
            if (htmlRedirect?.HtmlRedirect != null)
            {
                return new RetrievePageResult
                {
                    StatusCode = HttpStatusCode.Redirect,
                    RedirectTo = htmlRedirect.HtmlRedirect.NewUrl
                };
            }

            if (url.StartsWithIgnoreCase("/Content/Page/"))
            {
                var id = Guid.Parse(url.Substring("/Content/Page/".Length, 36));
                return this.PageById(unitOfWork, siteContext, id, url);
            }

            Guid? nodeId = null;

            var uri = new Uri("https://test.com" + url);
            var queryString = uri.ParseQueryString();
            var isSwitchingLanguage = queryString[
                "SwitchingLanguage"
            ].EqualsIgnoreCase("true");

            var potentialUrls = this.GetPotentialUrls(
                unitOfWork,
                siteContext,
                uri.AbsolutePath).Select(o => new { o.NodeId, o.LanguageId });
            nodeId = potentialUrls.FirstOrDefault()?.NodeId;

            if (nodeId.HasValue && !potentialUrls.Any(
                o => o.LanguageId == siteContext.LanguageDto.Id))
            {

                if (isSwitchingLanguage)
                {
                    var allNodeUrls = this.GetNodeUrls(
                        unitOfWork,
                        siteContext,
                        nodeId.Value);
                    var pageUrl = allNodeUrls.FirstOrDefault(
                        o => o.LanguageId == siteContext.LanguageDto.Id) ?? allNodeUrls.First();

                    var redirectTo = $"{pageUrl.Url}?{queryString}";
                    if (!url.EqualsIgnoreCase(redirectTo))
                    {
                        return new RetrievePageResult
                        {
                            StatusCode = HttpStatusCode.Found,
                            RedirectTo = redirectTo
                        };
                    }
                }
                else
                {
                    var defaultLanguageId = unitOfWork.GetTypedRepository<ILanguageRepository>().GetCachedDefault(
                        siteContext.WebsiteDto.Id)?.Id;

                    var languageId = defaultLanguageId.HasValue && potentialUrls.Any(
                        o => o.LanguageId == defaultLanguageId) ? defaultLanguageId : potentialUrls.First().LanguageId;

                    this.siteContextService.SetLanguage(languageId);
                }
            }

            GetCatalogPageResult catalogPageResult = null;
            if (nodeId == null)
            {
                nodeId = this.GetCatalogNodeId(
                    unitOfWork,
                    siteContext,
                    url,
                    isSwitchingLanguage,
                    out var retrievePageResult,
                    ref catalogPageResult);
                if (retrievePageResult != null)
                {
                    return retrievePageResult;
                }
            }

            var pageVersions = this.PageVersionQuery(
                unitOfWork,
                siteContext).Where(o => o.Page.NodeId == nodeId).ToList();

            var pageVersionJson = this.GetPageVersionVariantJson(
                unitOfWork,
                pageVersions,
                siteContext,
                catalogPageResult);

            if (pageVersionJson != null)
            {
                return this.CreateResult(
                    pageVersionJson,
                    unitOfWork,
                    siteContext,
                    url);
            }

            return this.NotFoundPage(unitOfWork, siteContext);
        }

        private string GetPageVersionVariantJson(
            IUnitOfWork unitOfWork,
            ICollection<PageVersion> pageVersions,
            ISiteContext siteContext,
            GetCatalogPageResult catalogPageResult)
        {
            var ruleObjects = new List<object>
            {
                siteContext.ShipTo, siteContext.UserProfileDto, catalogPageResult?.Category, catalogPageResult?.Product
            };

            var pageVersion = pageVersions.Where(
                o => !o.Page.IsDefaultVariant && o.Page.RuleManager != null && this.rulesEngine.Execute(
                    o.Page,
                    ruleObjects)).FirstOrDefault() ?? pageVersions.FirstOrDefault(
                o => o.Page.IsDefaultVariant);

            if (pageVersion == null)
            {
                return null;
            }

            if (pageVersion.Page.LayoutPageId.HasValue)
            {
                return this.CombineWithLayout(unitOfWork, pageVersion);
            }

            return pageVersion.Value;
        }

        private Guid? GetCatalogNodeId(
            IUnitOfWork unitOfWork,
            ISiteContext siteContext,
            string url,
            bool isSwitchingLanguage,
            RetrievePageResult retrievePageResult,
            GetCatalogPageResult catalogPageResult)
        {
            var relativeUrlNoQuery = url.Contains("?") ? url.Substring(
                0,
                url.IndexOf("?", StringComparison.Ordinal)) : url;
            var urlParts = relativeUrlNoQuery.Trim('/', ' ').Split('/');
            var result = this.catalogService.GetCatalogPage(
                new GetCatalogPageParameter(relativeUrlNoQuery)
                {
                    GetSubCategories = true
                });
            var activeLanguage = siteContext.LanguageDto.Id;

            catalogPageResult = result;
            retrievePageResult = null;

            if (result?.RedirectUrl != null && urlParts.Length > 0)
            {
                if (isSwitchingLanguage)
                {
                    retrievePageResult = new RetrievePageResult
                    {
                        StatusCode = HttpStatusCode.Found,
                        RedirectTo = result.RedirectUrl
                    };
                    return null;
                }
                else
                {
                    var languageIds = this.catalogPathFinder.Value.GetLanguageIdsForCatalogUrlPath(
                        siteContext.WebsiteDto.Id,
                        urlParts[0]);
                    if (languageIds.Count != 0 && !languageIds.Contains(
                        activeLanguage))
                    {
                        this.siteContextService.SetLanguage(
                            languageIds.First());
                        retrievePageResult = new RetrievePageResult
                        {
                            StatusCode = HttpStatusCode.Found,
                            RedirectTo = url
                        };
                        return null;
                    }
                }
            }

            if (result?.Product != null)
            {
                return GetNodeIdByType(
                    unitOfWork,
                    siteContext,
                    "ProductDetailsPage");
            }

            if (result?.Category != null && result?.SubCategories != null && result.SubCategories.Any())
            {
                return GetNodeIdByType(
                    unitOfWork,
                    siteContext,
                    "CategoryDetailsPage");
            }

            if (urlParts.Length == 2 && urlParts[0].EqualsIgnoreCase(
                this.catalogPathBuilder.GetBrandRootPath()))
            {
                return GetNodeIdByType(
                    unitOfWork,
                    siteContext,
                    "BrandDetailsPage");
            }

            var brandsLanguageIds = this.catalogPathFinder.Value.GetLanguageIdsForBrandPath(
                siteContext.WebsiteDto.Id,
                urlParts[0]);

            if (brandsLanguageIds.Count != 0 && !brandsLanguageIds.Contains(
                activeLanguage))
            {
                if (isSwitchingLanguage)
                {
                    retrievePageResult = new RetrievePageResult
                    {
                        StatusCode = HttpStatusCode.Found,
                        RedirectTo = $"/{this.catalogPathBuilder.GetBrandRootPath()}/{string.Join(
                            "/",
                            urlParts.Skip(1))}"
                    };
                    return null;
                }
                else
                {
                    this.siteContextService.SetLanguage(
                        brandsLanguageIds.First());
                    retrievePageResult = new RetrievePageResult
                    {
                        StatusCode = HttpStatusCode.Found,
                        RedirectTo = url
                    };
                    return null;
                }
            }

            if (result?.Category != null || result?.Brand != null || result?.ProductLine != null || (urlParts.Length == 1 && urlParts[
                0
            ].EqualsIgnoreCase("search")))
            {
                return GetNodeIdByType(
                    unitOfWork,
                    siteContext,
                    "ProductListPage");
            }

            if (result?.ResultCode == ResultCode.Error && result?.SubCode == SubCode.NotFound)
            {
                return null;
            }

            return null;
        }

        private static Guid? GetNodeIdByType(
            IUnitOfWork unitOfWork,
            ISiteContext siteContext,
            string pageType)
        {
            return unitOfWork.GetRepository<Node>().GetTableAsNoTracking().Where(
                o => o.WebsiteId == siteContext.WebsiteDto.Id && o.Type == pageType).Select(
                o => o.Id).FirstOrDefault();
        }

        private RetrievePageResult PageById(
            IUnitOfWork unitOfWork,
            ISiteContext siteContext,
            Guid id,
            string url)
        {
            PageTemporary pageTemporary = null;
            PageModel pageTemporaryModel = null;

            var query = unitOfWork.GetRepository<PageVersion>().GetTableAsNoTracking().Expand(
                o => o.Page).Where(o => o.PageId == id);
            query = this.ApplyPublishedFilter(query, unitOfWork);

            var pageVersion = query.OrderByDescending(
                o => o.PublishOn ?? DateTimeOffset.MaxValue).FirstOrDefault();

            if (pageVersion == null)
            {
                pageTemporary = unitOfWork.GetRepository<PageTemporary>().GetTableAsNoTracking().FirstOrDefault(
                    o => o.Id == id);

                if (pageTemporary != null)
                {
                    pageTemporaryModel = JsonConvert.DeserializeObject<PageModel>(
                        pageTemporary.Value);
                }
            }

            var pageJson = pageVersion?.Value ?? pageTemporary?.Value;
            if ((pageVersion?.Page.LayoutPageId.HasValue ?? false) || (pageTemporaryModel?.LayoutPageId.HasValue ?? false))
            {
                pageJson = this.CombineWithLayout(
                    unitOfWork,
                    pageVersion,
                    pageTemporaryModel);
            }

            return this.CreateResult(pageJson, unitOfWork, siteContext, url);
        }

        private string CombineWithLayout(
            IUnitOfWork unitOfWork,
            PageVersion pageVersion,
            PageModel pageTemporary)
        {
            var layoutPageId = pageVersion != null ? pageVersion.Page.LayoutPageId : pageTemporary.LayoutPageId;
            var layoutPageVersion = unitOfWork.GetRepository<PageVersion>().GetTableAsNoTracking().Where(
                o => o.PageId == layoutPageId).OrderByDescending(
                o => o.PublishOn ?? DateTimeOffset.MaxValue).FirstOrDefault();

            var pageVersionPageModel = pageVersion != null ? JsonConvert.DeserializeObject<PageModel>(
                pageVersion.Value) : pageTemporary;

            if (layoutPageVersion != null)
            {
                var layoutPageModel = JsonConvert.DeserializeObject<PageModel>(
                    layoutPageVersion.Value);
                layoutPageModel.Widgets.Where(
                    o => o.ParentId == layoutPageModel.Id).Each(
                    o => o.ParentId = pageVersionPageModel.Id);
                layoutPageModel.Widgets.Each(o => o.IsLayout = true);
                pageVersionPageModel.Widgets.AddRange(layoutPageModel.Widgets);
            }

            return JsonConvert.SerializeObject(pageVersionPageModel);
        }

        private RetrievePageResult NotFoundPage(
            IUnitOfWork unitOfWork,
            ISiteContext siteContext)
        {
            var notFoundPage = this.PageVersionQuery(
                unitOfWork,
                siteContext).Where(
                o => o.Page.Node.Type == "NotFoundErrorPage").Select(
                o => o.Value).FirstOrDefault();

            if (notFoundPage == null)
            {
                LogHelper.For(this).Warn("NotFoundErrorPage is not found");
            }

            var errorResult = new RetrievePageResult
            {
                StatusCode = HttpStatusCode.NotFound,
                Page = notFoundPage != null ? JsonConvert.DeserializeObject<PageModel>(
                    notFoundPage) : null
            };

            return errorResult;
        }

        private IQueryable<PageVersion> PageVersionQuery(
            IUnitOfWork unitOfWork,
            ISiteContext siteContext)
        {
            var query = unitOfWork.GetRepository<PageVersion>().GetTableAsNoTracking().Expand(
                o => o.Page.RuleManager.RuleClauses).Where(
                o => o.Page.Node.WebsiteId == siteContext.WebsiteDto.Id);

            return this.ApplyPublishedFilter(query, unitOfWork);
        }

        private IQueryable<PageVersion> ApplyPublishedFilter(
            IQueryable<PageVersion> query,
            IUnitOfWork unitOfWork)
        {
            var pageVersionTable = unitOfWork.GetRepository<PageVersion>().GetTableAsNoTracking();
            if (this.contentModeProvider.DisplayUnpublishedContent)
            {
                return query.Where(
                    pageVersion => !pageVersionTable.Any(
                        otherPageVersion => otherPageVersion.PageId == pageVersion.PageId && (otherPageVersion.PublishOn ?? DateTimeOffset.MaxValue) > (pageVersion.PublishOn ?? DateTimeOffset.MaxValue)));
            }

            return query.Where(
                pageVersion => pageVersion.PublishOn != null && pageVersion.PublishOn < DateTimeProvider.Current.Now && !pageVersionTable.Any(
                    otherPageVersion => otherPageVersion.PageId == pageVersion.PageId && otherPageVersion.PublishOn < DateTimeProvider.Current.Now && otherPageVersion.PublishOn > pageVersion.PublishOn));
        }

        private RetrievePageResult CreateResult(
            string pageJson,
            IUnitOfWork unitOfWork,
            ISiteContext siteContext,
            string url)
        {
            var result = new RetrievePageResult();

            if (pageJson == null)
            {
                return this.NotFoundPage(unitOfWork, siteContext);
            }

            result.Page = JsonConvert.DeserializeObject<PageModel>(pageJson);
            if (result.Page.Type.EqualsIgnoreCase("UnhandledErrorPage"))
            {
                result.StatusCode = HttpStatusCode.InternalServerError;
            }

            if (this.contentModeProvider.BypassFilters)
            {
                return result;
            }

            var parameter = new NavigationFilterParameter
            {
                SiteContext = siteContext,
                Url = url,
                Type = NavigationFilterType.RetrievingPage,
                Page = result.Page,
                SignInPageUrl = new Lazy<string>(
                    () => GetSignInPageUrl(this, unitOfWork, siteContext))
            };

            var filterResult = this.navigationFilterService.ApplyFilters(
                result.Page.Type,
                parameter);
            if (filterResult == null)
            {
                return result;
            }

            if (filterResult.StatusCode == HttpStatusCode.NotFound)
            {
                return this.NotFoundPage(unitOfWork, siteContext);
            }

            result.Page = null;
            result.StatusCode = filterResult.StatusCode;
            result.RedirectTo = filterResult.RedirectTo;
            result.AuthorizationFailed = filterResult.AuthorizationFailed;

            return result;
        }

        public static string GetSignInPageUrl(
            IRetrievePageService retrievePageService,
            IUnitOfWork unitOfWork,
            ISiteContext siteContext)
        {
            return retrievePageService.GetPublishedPageUrlsByType(
                unitOfWork,
                siteContext,
                "SignInPage").Select(o => o.Url).FirstOrDefault();
        }

        private IQueryable<PageUrl> GetNodeUrls(
            IUnitOfWork unitOfWork,
            ISiteContext siteContext,
            Guid nodeId)
        {
            return this.GetUrls(
                unitOfWork,
                siteContext,
                o => o.NodeId == nodeId);
        }

        public IQueryable<PageUrl> GetPotentialUrls(
            IUnitOfWork unitOfWork,
            ISiteContext siteContext,
            string url)
        {
            return this.GetUrls(unitOfWork, siteContext, o => o.Url == url);
        }

        private GetByUriResult GetHtmlRedirect(string url)
        {
            if (url.IsBlank())
            {
                return null;
            }

            var baseUrl = HttpContext.Current.Request.ActualUrl().GetLeftPart(
                UriPartial.Authority);
            var getByUriResult = this.htmlRedirectPipeline.GetByUri(
                new GetByUriParameter { Uri = new Uri($"{baseUrl}{url}") });
            PipelineHelper.VerifyResults(getByUriResult);

            return getByUriResult;
        }

        private IQueryable<PageUrl> GetUrls(
            IUnitOfWork unitOfWork,
            ISiteContext siteContext,
            Expression<Func<PageUrl, bool>> where)
        {
            var displayUnpublishedContent = this.contentModeProvider.DisplayUnpublishedContent;

            return unitOfWork.GetRepository<PageUrl>().GetTableAsNoTracking().Where(
                where).Where(
                o => o.WebsiteId == siteContext.WebsiteDto.Id && (displayUnpublishedContent || (o.PublishOn.HasValue && o.PublishOn < DateTimeOffset.Now && (o.PublishUntil == null || o.PublishUntil > DateTimeOffset.Now)))).OrderByDescending(
                o => o.PublishOn ?? DateTimeOffset.MaxValue);
        }
    }

    public interface IRetrievePageService : IDependency
    {
        RetrievePageResult GetPageByUrl(
            IUnitOfWork unitOfWork,
            ISiteContext siteContext,
            string url);


        IQueryable<PageUrl> GetPublishedPageUrlsByType(
            IUnitOfWork unitOfWork,
            ISiteContext siteContext,
            string type);


        RetrievePageResult GetPageByType(
            IUnitOfWork unitOfWork,
            ISiteContext siteContext,
            string type);


        PagesCollectionModel GetPagesByParent(
            IUnitOfWork unitOfWork,
            ISiteContext siteContext,
            Guid parentNodeId);
    }
}
