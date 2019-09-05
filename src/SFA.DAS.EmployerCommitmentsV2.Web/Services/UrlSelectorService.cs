using System;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Authorization.Services;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Features;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerUrlHelper;
using SFA.DAS.EmployerCommitmentsV2.Web.Extensions;
using SFA.DAS.Encoding;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Services
{
    public class UrlSelectorService : IUrlSelectorService
    {
        private readonly Lazy<Func<IUrlHelper, string, string, ActionResult>> _lazyCohortDetailsLink;
        private readonly IAuthorizationService _authorizationService;
        private readonly ILinkGenerator _linkGenerator;

        public UrlSelectorService(
            IAuthorizationService authorizationService, 
            ILinkGenerator linkGenerator)
        {
            _linkGenerator = linkGenerator ?? throw new ArgumentNullException(nameof(linkGenerator));
            _authorizationService = authorizationService ?? throw new ArgumentNullException(nameof(authorizationService));
            _lazyCohortDetailsLink = new Lazy<Func<IUrlHelper, string, string, ActionResult>>(InitialiseCohortDetailsLink);
        }

        public ActionResult RedirectToCohortDetails(IUrlHelper urlHelper, string accountHashedId, string cohortReference)
        {
            return _lazyCohortDetailsLink.Value(urlHelper, accountHashedId, cohortReference);
        }

        public ActionResult RedirectToV1IfCohortWithOtherParty(string accountHashId, string cohortReference, GetCohortResponse cohort)
        {
            if (cohort.WithParty == Party.Provider)
            {
                return GetLinkToV1(accountHashId, cohortReference);
            }

            return null;
        }

        private Func<IUrlHelper, string, string, ActionResult> InitialiseCohortDetailsLink()
        {
            if (_authorizationService.IsAuthorized(EmployerFeature.EnhancedApproval))
            {
                return GetLinkToV2Delegate();
            }

            return GetLinkToV1Delegate();
        }

        private Func<IUrlHelper, string, string, ActionResult> GetLinkToV1Delegate()
        {
            return (httpContext, accountHashId, cohortReference) => GetLinkToV1(accountHashId, cohortReference);
        }

        private ActionResult GetLinkToV1(string accountHashId, string cohortReference)
        {
            return new RedirectResult(_linkGenerator.CohortDetails(accountHashId, cohortReference));
        }

        private static Func<IUrlHelper, string, string, ActionResult> GetLinkToV2Delegate()
        {
            return (urlHelper, accountHashId, cohortReference) =>
                new RedirectToActionResult(
                    nameof(CohortController.Approve),
                    "Cohort",
                    new
                    {
                        AccountHashedId = accountHashId,
                        CohortReference = cohortReference
                    },
                    null)
                {
                    UrlHelper = urlHelper,
                    Permanent = false
                };
        }
    }
}
