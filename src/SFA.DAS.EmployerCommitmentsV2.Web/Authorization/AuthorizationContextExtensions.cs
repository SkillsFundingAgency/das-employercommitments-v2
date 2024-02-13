using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Contracts;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Authorization;

public static class AuthorizationContextExtensions
{
    public static void AddEmployerUserRoleValues(this IAuthorizationContext authorizationContext, long accountId,
        Guid userRef)
    {
        authorizationContext.Set(AuthorizationContextKey.AccountId, accountId);
        authorizationContext.Set(AuthorizationContextKey.UserRef, userRef);
    }

    public static void AddCommitmentPermissionValues(this IAuthorizationContext authorizationContext, long cohortId,
        Party party, long partyId)
    {
        authorizationContext.Set(AuthorizationContextKey.CohortId, cohortId);
        authorizationContext.Set(AuthorizationContextKey.Party, party);
        authorizationContext.Set(AuthorizationContextKey.PartyId, partyId);
    }

    public static void AddApprenticeshipPermissionValues(this IAuthorizationContext authorizationContext,
        long apprenticeshipId, Party party, long partyId)
    {
        authorizationContext.Set(AuthorizationContextKey.ApprenticeshipId, apprenticeshipId);
        authorizationContext.Set(AuthorizationContextKey.Party, party);
        authorizationContext.Set(AuthorizationContextKey.PartyId, partyId);
    }
}