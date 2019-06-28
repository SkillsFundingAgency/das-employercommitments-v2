using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using SFA.DAS.EmployerCommitmentsV2.Web.Extensions;

namespace SFA.DAS.EmployerCommitmentsV2.Extensions
{
    public static class IIdentityExtensions
    {
        public static string Upn(this IIdentity identity)
        {
            switch (identity)
            {
                case ClaimsIdentity claimsIdentity:
                    return claimsIdentity.Claims.FirstOrDefault(claim => claim.Type == EmployeClaims.Upn)?.Value;

                default:
                    return null;
            }
        }
    }
}
