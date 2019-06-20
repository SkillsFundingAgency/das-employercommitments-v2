using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using SFA.DAS.EmployerCommitmentsV2.Web.Authentication;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Extensions
{
    public static class IIdentityExtensions
    {
        public static string Id(this IIdentity identity)
        {
            switch (identity)
            {
                case ClaimsIdentity claimsIdentity:
                    return claimsIdentity.Claims.FirstOrDefault(claim => claim.Type == EmployeeClaims.Id)?.Value;

                default:
                    return null;
            }
        }
    }
}
