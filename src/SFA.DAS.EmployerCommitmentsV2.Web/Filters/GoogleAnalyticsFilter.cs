using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using SFA.DAS.EmployerCommitmentsV2.Infrastructure;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Responses;
using SFA.DAS.EmployerCommitmentsV2.Web.Authentication;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Shared;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Filters;

public class GoogleAnalyticsFilter : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (context.Controller is not Controller controller)
        {
            return;
        }

        controller.ViewBag.GaData = PopulateGaData(context);

        base.OnActionExecuting(context);
    }

    private static GaData PopulateGaData(ActionExecutingContext context)
    {
        string hashedAccountId = null;
        string levyFlag = null;

        var userId = context.HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals(EmployeeClaims.IdamsUserIdClaimTypeIdentifier))?.Value;

        if (context.RouteData.Values.TryGetValue("AccountHashedId", out var accountHashedId))
        {
            hashedAccountId = accountHashedId.ToString();

            var accountsJson = context.HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals(EmployerClaims.AccountsClaimsTypeIdentifier))?.Value;
            if (accountsJson is not null)
            {
                var accounts = JsonConvert.DeserializeObject<Dictionary<string, EmployerIdentifier>>(accountsJson);
                levyFlag = accounts.TryGetValue(hashedAccountId, out var employer) ? employer.ApprenticeshipEmployerType.ToString() : null;
            }
        }

        return new GaData
        {
            UserId = userId,
            Acc = hashedAccountId,
            LevyFlag = levyFlag
        };
    }
}