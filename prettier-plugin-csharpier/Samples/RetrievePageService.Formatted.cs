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
    using Insite.Data.Entities;
    using Insite.Data.Extensions;
    using Insite.Data.Repositories.Interfaces;
    using Insite.Spire.Models;
    using Insite.WebFramework.Routing.Pipelines;
    using Insite.WebFramework.Routing.Pipelines.Parameters;
    using Insite.WebFramework.Routing.Pipelines.Results;
    using Newtonsoft.Json;

    public class RetrievePageService
    {
        private readonly ICatalogPathBuilder catalogPathBuilder;

        private readonly ICatalogService catalogService;

        private readonly INavigationFilterService navigationFilterService;

        private readonly IContentModeProvider contentModeProvider;

        private readonly ISiteContextService siteContextService;

        private readonly Lazy<ICatalogPathFinder> catalogPathFinder;

        private readonly IRulesEngine rulesEngine;

        private readonly IHtmlRedirectPipeline htmlRedirectPipeline;

        public RetrievePageService()
        {
            this.catalogPathBuilder = catalogPathBuilder;
            this.catalogService = catalogService;
            this.navigationFilterService = navigationFilterService;
            this.contentModeProvider = contentModeProvider;
            this.siteContextService = siteContextServiceFactory.GetSiteContextService(

            );
            this.catalogPathFinder = catalogPathFinder;
            this.rulesEngine = rulesEngine;
            this.htmlRedirectPipeline = htmlRedirectPipeline;
        }

        public RetrievePageResult GetPageByType()
        {
            var pageVersionQuery = this.PageVersionQuery(
                unitOfWork,
                siteContext
            );
            var pageVersions = pageVersionQuery.Where(
                o => o.Page.Node.Type == type
            ).ToArray().GroupBy(o => o.PageId).Select(o => o.First()).ToArray();
            return this.CreateResult(
                this.GetPageVariantVersion(pageVersions, siteContext),
                unitOfWork,
                siteContext,
                null
            );
        }

        public IList<PageModel> GetPagesByParent()
        {
            var pageVersionQuery = this.PageVersionQuery(
                unitOfWork,
                siteContext
            );
            var pageVersions = pageVersionQuery.Where(
                o => o.Page.Node.ParentId == parentNodeId
            ).GroupBy(o => o.PageId).Select(o => o.FirstOrDefault()).ToList();
            return pageVersions.Select(
                o => JsonConvert.DeserializeObject<PageModel>(o.Value)
            ).ToList();
        }

        public IQueryable<PageUrl> GetPublishedPageUrlsByType()
        {
            var nodeQuery = unitOfWork.GetRepository<Node>(

            ).GetTableAsNoTracking().Where(
                node => TODO LogicalAndExpression
            ).Select(node => node.Id);
            var now = DateTimeProvider.Current.Now;
            return unitOfWork.GetRepository<PageUrl>().GetTableAsNoTracking(

            ).Where(pageUrl => TODO LogicalAndExpression);
        }

        public RetrievePageResult GetPageByUrl()
        {
            var htmlRedirect = this.GetHtmlRedirect(url);
            if (TODO ConditionalAccessExpression != null)
            {
                return TODO ObjectCreationExpression;
            }
            if (url.StartsWithIgnoreCase("/Content/Page/"))
            {
                var id = Guid.Parse(url.Substring("/Content/Page/".Length, 36));
                return this.PageById(unitOfWork, siteContext, id, url);
            }
            Guid? nodeId = null;
            var uri = TODO ObjectCreationExpression;
            var queryString = uri.ParseQueryString();
            var isSwitchingLanguage = TODO ElementAccessExpression.EqualsIgnoreCase(
                "true"
            );
            var potentialUrls = this.GetPotentialUrls(
                unitOfWork,
                siteContext,
                uri.AbsolutePath
            ).Select(o => TODO AnonymousObjectCreationExpression);
            nodeId = TODO ConditionalAccessExpression;
            if (TODO LogicalAndExpression)
            {
                if (isSwitchingLanguage)
                {
                    var allNodeUrls = this.GetNodeUrls(
                        unitOfWork,
                        siteContext,
                        nodeId.Value
                    );
                    var pageUrl = TODO CoalesceExpression;
                    var redirectTo = TODO InterpolatedStringExpression;
                    if (TODO LogicalNotExpression)
                    {
                        return TODO ObjectCreationExpression;
                    }
                }
            }
            if (nodeId == null)
            {
                nodeId = this.GetCatalogNodeId(
                    unitOfWork,
                    siteContext,
                    url,
                    isSwitchingLanguage,
                    TODO DeclarationExpression
                );
                if (retrievePageResult != null)
                {
                    return retrievePageResult;
                }
            }
            var pageVersions = this.PageVersionQuery(
                unitOfWork,
                siteContext
            ).Where(o => o.Page.NodeId == nodeId).ToArray().GroupBy(
                o => o.PageId
            ).Select(o => o.First()).ToArray();
            var pageVersionJson = this.GetPageVariantVersion(
                pageVersions,
                siteContext
            );
            if (pageVersionJson != null)
            {
                return this.CreateResult(
                    pageVersionJson,
                    unitOfWork,
                    siteContext,
                    url
                );
            }
            return this.NotFoundPage(unitOfWork, siteContext);
        }

        private string GetPageVariantVersion()
        {
            var ruleObjects = TODO ObjectCreationExpression;
            return TODO CoalesceExpression;
        }

        private Guid? GetCatalogNodeId()
        {
            var relativeUrlNoQuery = TODO ConditionalExpression;
            var urlParts = relativeUrlNoQuery.Trim('/', ' ').Split('/');
            var result = this.catalogService.GetCatalogPage(
                TODO ObjectCreationExpression
            );
            var activeLanguage = siteContext.LanguageDto.Id;
            retrievePageResult = null;
            if (TODO LogicalAndExpression)
            {
                if (isSwitchingLanguage)
                {
                    retrievePageResult = TODO ObjectCreationExpression;
                    return null;
                }
            }
            if (TODO ConditionalAccessExpression != null)
            {
                return GetNodeIdByType(
                    unitOfWork,
                    siteContext,
                    "ProductDetailsPage"
                );
            }
            if (TODO LogicalAndExpression)
            {
                return GetNodeIdByType(
                    unitOfWork,
                    siteContext,
                    "CategoryDetailsPage"
                );
            }
            if (TODO LogicalAndExpression)
            {
                return GetNodeIdByType(
                    unitOfWork,
                    siteContext,
                    "BrandDetailsPage"
                );
            }
            var brandsLanguageIds = this.catalogPathFinder.Value.GetLanguageIdsForBrandPath(
                siteContext.WebsiteDto.Id,
                TODO ElementAccessExpression
            );
            if (TODO LogicalAndExpression)
            {
                if (isSwitchingLanguage)
                {
                    retrievePageResult = TODO ObjectCreationExpression;
                    return null;
                }
            }
            if (TODO LogicalAndExpression)
            {
                return null;
            }
            if (TODO LogicalOrExpression)
            {
                return GetNodeIdByType(
                    unitOfWork,
                    siteContext,
                    "ProductListPage"
                );
            }
            return null;
        }

        private static Guid? GetNodeIdByType()
        {
            return unitOfWork.GetRepository<Node>().GetTableAsNoTracking(

            ).Where(o => TODO LogicalAndExpression).Select(
                o => o.Id
            ).FirstOrDefault();
        }

        private RetrievePageResult PageById()
        {
            var query = unitOfWork.GetRepository<PageVersion>(

            ).GetTableAsNoTracking().Where(o => o.PageId == id);
            query = this.ApplyPublishedFilter(query);
            var pageJson = query.OrderByDescending(
                o => TODO CoalesceExpression
            ).Select(o => o.Value).FirstOrDefault();
            if (pageJson == null)
            {
                pageJson = unitOfWork.GetRepository<PageTemporary>(

                ).GetTableAsNoTracking().Where(o => o.Id == id).Select(
                    o => o.Value
                ).FirstOrDefault();
            }
            return this.CreateResult(pageJson, unitOfWork, siteContext, url);
        }

        private RetrievePageResult NotFoundPage()
        {
            var notFoundPage = this.PageVersionQuery(
                unitOfWork,
                siteContext
            ).Where(o => o.Page.Node.Type == "NotFoundErrorPage").Select(
                o => o.Value
            ).FirstOrDefault();
            if (notFoundPage == null)
            {
                LogHelper.For(this).Warn("NotFoundErrorPage is not found");
            }
            var errorResult = TODO ObjectCreationExpression;
            return errorResult;
        }

        private IQueryable<PageVersion> PageVersionQuery()
        {
            var query = unitOfWork.GetRepository<PageVersion>(

            ).GetTableAsNoTracking().Expand(
                o => o.Page.RuleManager.RuleClauses
            ).Where(o => o.Page.Node.WebsiteId == siteContext.WebsiteDto.Id);
            query = this.ApplyPublishedFilter(query);
            return query.OrderByDescending(o => TODO CoalesceExpression);
        }

        private IQueryable<PageVersion> ApplyPublishedFilter()
        {
            if (this.contentModeProvider.DisplayUnpublishedContent)
            {
                return query;
            }
            return query.Where(o => TODO LogicalAndExpression);
        }

        private RetrievePageResult CreateResult()
        {
            var result = TODO ObjectCreationExpression;
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
            var parameter = TODO ObjectCreationExpression;
            var filterResult = this.navigationFilterService.ApplyFilters(
                result.Page.Type,
                parameter
            );
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
            return result;
        }

        public static string GetSignInPageUrl()
        {
            return retrievePageService.GetPublishedPageUrlsByType(
                unitOfWork,
                siteContext,
                "SignInPage"
            ).Select(o => o.Url).FirstOrDefault();
        }

        private IQueryable<PageUrl> GetNodeUrls()
        {
            return this.GetUrls(
                unitOfWork,
                siteContext,
                o => o.NodeId == nodeId
            );
        }

        public IQueryable<PageUrl> GetPotentialUrls()
        {
            return this.GetUrls(unitOfWork, siteContext, o => o.Url == url);
        }

        private GetByUriResult GetHtmlRedirect()
        {
            if (url.IsBlank())
            {
                return null;
            }
            var baseUrl = HttpContext.Current.Request.ActualUrl().GetLeftPart(
                UriPartial.Authority
            );
            var getByUriResult = this.htmlRedirectPipeline.GetByUri(
                TODO ObjectCreationExpression
            );
            PipelineHelper.VerifyResults(getByUriResult);
            return getByUriResult;
        }

        private IQueryable<PageUrl> GetUrls()
        {
            var displayUnpublishedContent = this.contentModeProvider.DisplayUnpublishedContent;
            return unitOfWork.GetRepository<PageUrl>().GetTableAsNoTracking(

            ).Where(where).Where(
                o => TODO LogicalAndExpression
            ).OrderByDescending(o => TODO CoalesceExpression);
        }
    }

    public interface IRetrievePageService
    {
        RetrievePageResult GetPageByUrl() { }

        IQueryable<PageUrl> GetPublishedPageUrlsByType() { }

        RetrievePageResult GetPageByType() { }

        IList<PageModel> GetPagesByParent() { }
    }
}
