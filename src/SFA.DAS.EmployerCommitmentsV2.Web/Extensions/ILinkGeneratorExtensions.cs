using SFA.DAS.EmployerUrlHelper;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Extensions
{
    public static class ILinkGeneratorExtensions
    {
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
            string accountHashedId,
            string cohortReference,
            string draftApprenticeshipHashedId)
        {
            return linkGenerator.CommitmentsLink($"accounts/{accountHashedId}/apprentices/{cohortReference}/apprenticeships/{draftApprenticeshipHashedId}/delete");
        }

        public static string EmployerHome(this ILinkGenerator linkGenerator, string accountHashedId)
        {
            return linkGenerator.UsersLink($"accounts/{accountHashedId}/teams");
        }
    }
}