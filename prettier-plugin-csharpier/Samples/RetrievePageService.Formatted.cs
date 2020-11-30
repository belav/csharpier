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

        private readonly TODO GenericName catalogPathFinder;

        private readonly IRulesEngine rulesEngine;

        private readonly IHtmlRedirectPipeline htmlRedirectPipeline;

        TODO ConstructorDeclaration

        public RetrievePageResult GetPageByType() {}

        public TODO GenericName GetPagesByParent() {}

        public TODO GenericName GetPublishedPageUrlsByType() {}

        public RetrievePageResult GetPageByUrl() {}

        private string GetPageVariantVersion() {}

        private TODO NullableType GetCatalogNodeId() {}

        private static TODO NullableType GetNodeIdByType() {}

        private RetrievePageResult PageById() {}

        private RetrievePageResult NotFoundPage() {}

        private TODO GenericName PageVersionQuery() {}

        private TODO GenericName ApplyPublishedFilter() {}

        private RetrievePageResult CreateResult() {}

        public static string GetSignInPageUrl() {}

        private TODO GenericName GetNodeUrls() {}

        public TODO GenericName GetPotentialUrls() {}

        private GetByUriResult GetHtmlRedirect() {}

        private TODO GenericName GetUrls() {}
    }TODO InterfaceDeclaration
}
