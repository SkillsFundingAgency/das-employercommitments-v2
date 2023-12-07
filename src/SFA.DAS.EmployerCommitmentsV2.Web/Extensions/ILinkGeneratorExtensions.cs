using SFA.DAS.EmployerUrlHelper;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Extensions;

public static class ILinkGeneratorExtensions
{
    public static string YourOrganisationsAndAgreements(this ILinkGenerator linkGenerator, string accountHashedId)
    {
        return linkGenerator.AccountsLink($"accounts/{accountHashedId}/agreements");
    }
        
    public static string YourTeam(this ILinkGenerator linkGenerator, string accountHashedId)
    {
        return linkGenerator.AccountsLink($"accounts/{accountHashedId}/teams/view");
    }

    public static string PayeSchemes(this ILinkGenerator linkGenerator, string accountHashedId)
    {
        return linkGenerator.AccountsLink($"accounts/{accountHashedId}/schemes");
    }

    public static string EmployerHome(this ILinkGenerator linkGenerator, string accountHashedId)
    {
        return linkGenerator.AccountsLink($"accounts/{accountHashedId}/teams");
    }

    public static string EmployerFinanceTransfers(this ILinkGenerator linkGenerator, string accountHashedId)
    {
        return linkGenerator.FinanceLink($"accounts/{accountHashedId}/transfers/connections");
    }

    public static string EmployerAccountsHome(this ILinkGenerator linkGenerator)
    {
        return linkGenerator.AccountsLink();
    }

    public static string SenderApplicationDetails(this ILinkGenerator linkGenerator,
        string hashedAccountId,
        string hashedPledgeId,
        string hashedPledgeApplicationId)
    {
        return linkGenerator.LevyTransferMatchingLink($"/accounts/{hashedAccountId}/pledges/{hashedPledgeId}/applications/{hashedPledgeApplicationId}");
    }

    public static string ReceiverApplicationDetails(this ILinkGenerator linkGenerator,
        string hashedAccountId,
        string hashedPledgeApplicationId)
    {
        return linkGenerator.LevyTransferMatchingLink($"/accounts/{hashedAccountId}/applications/{hashedPledgeApplicationId}");
    }
}