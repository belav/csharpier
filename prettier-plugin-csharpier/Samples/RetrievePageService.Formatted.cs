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
                TODO SimpleLambdaExpression
            ).ToArray().GroupBy(TODO SimpleLambdaExpression).Select(
                TODO SimpleLambdaExpression
            ).ToArray();
            return this.CreateResult(
                this.GetPageVariantVersion(pageVersions, siteContext),
                unitOfWork,
                siteContext,
                TODO NullLiteralExpression
            );
        }

        public IList<PageModel> GetPagesByParent()
        {
            var pageVersionQuery = this.PageVersionQuery(
                unitOfWork,
                siteContext
            );
            var pageVersions = pageVersionQuery.Where(
                TODO SimpleLambdaExpression
            ).GroupBy(TODO SimpleLambdaExpression).Select(
                TODO SimpleLambdaExpression
            ).ToList();
            return pageVersions.Select(TODO SimpleLambdaExpression).ToList();
        }

        public IQueryable<PageUrl> GetPublishedPageUrlsByType()
        {
            var nodeQuery = unitOfWork.GetRepository<Node>(

            ).GetTableAsNoTracking().Where(TODO SimpleLambdaExpression).Select(
                TODO SimpleLambdaExpression
            );
            var now = DateTimeProvider.Current.Now;
            return unitOfWork.GetRepository<PageUrl>().GetTableAsNoTracking(

            ).Where(TODO SimpleLambdaExpression);
        }

        public RetrievePageResult GetPageByUrl()
        {
            var htmlRedirect = this.GetHtmlRedirect(url);
            if (TODO NotEqualsExpression)
            {
                return TODO ObjectCreationExpression;
            }
            if (url.StartsWithIgnoreCase("/Content/Page/"))
            {
                var id = Guid.Parse(
                    url.Substring(
                        "/Content/Page/".Length,
                        TODO NumericLiteralExpression
                    )
                );
                return this.PageById(unitOfWork, siteContext, id, url);
            }
            Guid? nodeId = TODO NullLiteralExpression;
            var uri = TODO ObjectCreationExpression;
            var queryString = uri.ParseQueryString();
            var isSwitchingLanguage = TODO ElementAccessExpression.EqualsIgnoreCase(
                "true"
            );
            var potentialUrls = this.GetPotentialUrls(
                unitOfWork,
                siteContext,
                uri.AbsolutePath
            ).Select(TODO SimpleLambdaExpression);
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
            if (TODO EqualsExpression)
            {
                nodeId = this.GetCatalogNodeId(
                    unitOfWork,
                    siteContext,
                    url,
                    isSwitchingLanguage,
                    TODO DeclarationExpression
                );
                if (TODO NotEqualsExpression) { return retrievePageResult; }
            }
            var pageVersions = this.PageVersionQuery(
                unitOfWork,
                siteContext
            ).Where(TODO SimpleLambdaExpression).ToArray().GroupBy(
                TODO SimpleLambdaExpression
            ).Select(TODO SimpleLambdaExpression).ToArray();
            var pageVersionJson = this.GetPageVariantVersion(
                pageVersions,
                siteContext
            );
            if (TODO NotEqualsExpression)
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
            var urlParts = relativeUrlNoQuery.Trim(
                TODO CharacterLiteralExpression,
                TODO CharacterLiteralExpression
            ).Split(TODO CharacterLiteralExpression);
            var result = this.catalogService.GetCatalogPage(
                TODO ObjectCreationExpression
            );
            var activeLanguage = siteContext.LanguageDto.Id;
            retrievePageResult = TODO NullLiteralExpression;
            if (TODO LogicalAndExpression)
            {
                if (isSwitchingLanguage)
                {
                    retrievePageResult = TODO ObjectCreationExpression;
                    return TODO NullLiteralExpression;
                }
            }
            if (TODO NotEqualsExpression)
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
                    return TODO NullLiteralExpression;
                }
            }
            if (TODO LogicalAndExpression)
            {
                return TODO NullLiteralExpression;
            }
            if (TODO LogicalOrExpression)
            {
                return GetNodeIdByType(
                    unitOfWork,
                    siteContext,
                    "ProductListPage"
                );
            }
            return TODO NullLiteralExpression;
        }

        private static Guid? GetNodeIdByType()
        {
            return unitOfWork.GetRepository<Node>().GetTableAsNoTracking(

            ).Where(TODO SimpleLambdaExpression).Select(
                TODO SimpleLambdaExpression
            ).FirstOrDefault();
        }

        private RetrievePageResult PageById()
        {
            var query = unitOfWork.GetRepository<PageVersion>(

            ).GetTableAsNoTracking().Where(TODO SimpleLambdaExpression);
            query = this.ApplyPublishedFilter(query);
            var pageJson = query.OrderByDescending(
                TODO SimpleLambdaExpression
            ).Select(TODO SimpleLambdaExpression).FirstOrDefault();
            if (TODO EqualsExpression)
            {
                pageJson = unitOfWork.GetRepository<PageTemporary>(

                ).GetTableAsNoTracking().Where(
                    TODO SimpleLambdaExpression
                ).Select(TODO SimpleLambdaExpression).FirstOrDefault();
            }
            return this.CreateResult(pageJson, unitOfWork, siteContext, url);
        }

        private RetrievePageResult NotFoundPage()
        {
            var notFoundPage = this.PageVersionQuery(
                unitOfWork,
                siteContext
            ).Where(TODO SimpleLambdaExpression).Select(
                TODO SimpleLambdaExpression
            ).FirstOrDefault();
            if (TODO EqualsExpression)
            {
                LogHelper.For(this).Warn("NotFoundErrorPage is not found");
            }
            var errorResult = TODO ObjectCreationExpression;
            return errorResult;
        }

        private IQueryable<PageVersion> PageVersionQuery()
        {
            var query = unitOfWork.GetRepository<PageVersion>(

            ).GetTableAsNoTracking().Expand(TODO SimpleLambdaExpression).Where(
                TODO SimpleLambdaExpression
            );
            query = this.ApplyPublishedFilter(query);
            return query.OrderByDescending(TODO SimpleLambdaExpression);
        }

        private IQueryable<PageVersion> ApplyPublishedFilter()
        {
            if (this.contentModeProvider.DisplayUnpublishedContent)
            {
                return query;
            }
            return query.Where(TODO SimpleLambdaExpression);
        }

        private RetrievePageResult CreateResult()
        {
            var result = TODO ObjectCreationExpression;
            if (TODO EqualsExpression)
            {
                return this.NotFoundPage(unitOfWork, siteContext);
            }
            result.Page = JsonConvert.DeserializeObject<PageModel>(pageJson);
            if (result.Page.Type.EqualsIgnoreCase("UnhandledErrorPage"))
            {
                result.StatusCode = HttpStatusCode.InternalServerError;
            }
            if (this.contentModeProvider.BypassFilters) { return result; }
            var parameter = TODO ObjectCreationExpression;
            var filterResult = this.navigationFilterService.ApplyFilters(
                result.Page.Type,
                parameter
            );
            if (TODO EqualsExpression) { return result; }
            if (TODO EqualsExpression)
            {
                return this.NotFoundPage(unitOfWork, siteContext);
            }
            result.Page = TODO NullLiteralExpression;
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
            ).Select(TODO SimpleLambdaExpression).FirstOrDefault();
        }

        private IQueryable<PageUrl> GetNodeUrls()
        {
            return this.GetUrls(
                unitOfWork,
                siteContext,
                TODO SimpleLambdaExpression
            );
        }

        public IQueryable<PageUrl> GetPotentialUrls()
        {
            return this.GetUrls(
                unitOfWork,
                siteContext,
                TODO SimpleLambdaExpression
            );
        }

        private GetByUriResult GetHtmlRedirect()
        {
            if (url.IsBlank()) { return TODO NullLiteralExpression; }
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

            ).Where(where).Where(TODO SimpleLambdaExpression).OrderByDescending(
                TODO SimpleLambdaExpression
            );
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
