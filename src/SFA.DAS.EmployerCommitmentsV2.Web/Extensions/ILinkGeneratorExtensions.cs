using SFA.DAS.Authorization.Services;
using SFA.DAS.EmployerCommitmentsV2.Features;
using SFA.DAS.EmployerCommitmentsV2.Web.Enums;
using SFA.DAS.EmployerUrlHelper;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Extensions
{
    public static class ILinkGeneratorExtensions
    {
        public static string CohortDetails(this ILinkGenerator linkGenerator, string accountHashedId,
            string cohortReference)
        {
            return linkGenerator.CommitmentsLink($"accounts/{accountHashedId}/apprentices/{cohortReference}/details");
        }

        public static string YourOrganisationsAndAgreements(this ILinkGenerator linkGenerator, string accountHashedId)
        {
            return linkGenerator.AccountsLink($"accounts/{accountHashedId}/agreements");
        }

        public static string PayeSchemes(this ILinkGenerator linkGenerator, string accountHashedId)
        {
            return linkGenerator.AccountsLink($"accounts/{accountHashedId}/schemes");
        }

        public static string ViewApprentice(this ILinkGenerator linkGenerator, 
            string accountHashedId, 
            string cohortReference, 
            string draftApprenticeshipHashedId)
        {
            return linkGenerator.CommitmentsLink($"accounts/{accountHashedId}/apprentices/{cohortReference}/apprenticeships/{draftApprenticeshipHashedId}/view");
        }

        public static string DeleteApprentice(this ILinkGenerator linkGenerator,
            IAuthorizationService authorizationService,
            string accountHashedId,
            string cohortReference,
            string draftApprenticeshipHashedId,
            Origin origin)
        {
            if (authorizationService.IsAuthorized(EmployerFeature.EnhancedApproval))
            {
                return linkGenerator.CommitmentsV2Link($"{accountHashedId}/unapproved/{cohortReference}/apprentices/{draftApprenticeshipHashedId}/Delete/{origin}");
            }
            else
            {
                return linkGenerator.CommitmentsLink($"accounts/{accountHashedId}/apprentices/{cohortReference}/apprenticeships/{draftApprenticeshipHashedId}/delete");
            }
        }
    }
}