using System.Security.Claims;

namespace SFA.DAS.EmployerCommitmentsV2.Extensions;

public static class ClaimsExtensions
{
    public static bool ClaimsAreEmpty(this ClaimsPrincipal user)
    {
        return !user.Claims.Any();
    }
}