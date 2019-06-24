using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using SFA.DAS.EmployerCommitmentsV2.Web.Extensions;

namespace SFA.DAS.EmployerCommitmentsV2.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static string Upn(this ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal?.Identity?.Upn();
        }
    }
}
