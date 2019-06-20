using System;
using System.Security.Claims;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static Guid? UserRef(this ClaimsPrincipal claimsPrincipal)
        {
            var id = claimsPrincipal?.Identity?.Id();

            if (id == null)
            {
                return null;
            }

            return Guid.Parse(id);
        }
    }
}