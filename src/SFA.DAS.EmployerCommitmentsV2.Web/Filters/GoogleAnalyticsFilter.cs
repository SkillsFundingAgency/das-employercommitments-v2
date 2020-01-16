using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SFA.DAS.EmployerCommitmentsV2.Web.Authentication;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Shared;
using System.Linq;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Filters
{
    public class GoogleAnalyticsFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!(context.Controller is Controller controller))
            {
                return;
            }

            controller.ViewBag.GaData = PopulateGaData(context);

            base.OnActionExecuting(context);
        }

        private GaData PopulateGaData(ActionExecutingContext context)
        {
            string hashedAccountId = null;

            var userId = context.HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals(EmployeeClaims.Id))?.Value;

            if (context.RouteData.Values.TryGetValue("AccountHashedId", out var accountHashedId))
            {
                hashedAccountId = accountHashedId.ToString();
            }

            return new GaData
            {
                UserId = userId,
                Acc = hashedAccountId
            };
        }
    }
}