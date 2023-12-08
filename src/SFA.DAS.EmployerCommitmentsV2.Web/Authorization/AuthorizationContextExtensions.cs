using SFA.DAS.CommitmentsV2.Types;

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

    internal static (long CohortId, long ApprenticeshipId, Party Party, long PartyId) GetPermissionValues(
        this IAuthorizationContext authorizationContext)
    {
        authorizationContext.TryGet<long>(AuthorizationContextKey.CohortId, out var cohortId);
        authorizationContext.TryGet<long>(AuthorizationContextKey.ApprenticeshipId, out var apprenticeshipId);

        if (cohortId == 0 && apprenticeshipId == 0)
        {
            throw new KeyNotFoundException(
                $"At least one key of '{AuthorizationContextKey.CohortId}' or '{AuthorizationContextKey.ApprenticeshipId}' should be present in the authorization context");
        }

        return (cohortId,
            apprenticeshipId,
            authorizationContext.Get<Party>(AuthorizationContextKey.Party),
            authorizationContext.Get<long>(AuthorizationContextKey.PartyId));
    }

    internal static bool TryGetPermissionValues(this IAuthorizationContext authorizationContext, out long cohortId,
        out long apprenticeshipId, out Party party, out long partyId)
    {
        return (authorizationContext.TryGet(AuthorizationContextKey.CohortId, out cohortId) |
                authorizationContext.TryGet(AuthorizationContextKey.ApprenticeshipId, out apprenticeshipId)) &
               authorizationContext.TryGet(AuthorizationContextKey.Party, out party) &
               authorizationContext.TryGet(AuthorizationContextKey.PartyId, out partyId);
    }
}