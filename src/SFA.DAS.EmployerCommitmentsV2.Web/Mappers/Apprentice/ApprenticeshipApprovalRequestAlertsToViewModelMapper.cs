using System.Globalization;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Contracts;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Requests;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Responses;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice;

public class ApprenticeshipApprovalRequestAlertsToViewModelMapper(IApprovalsApiClient approvalsApiClient) : IMapper<ApprenticeshipApprovalRequestAlertsRequest, ApprenticeshipApprovalRequestAlertsViewModel>
{
    public async Task<ApprenticeshipApprovalRequestAlertsViewModel> Map(ApprenticeshipApprovalRequestAlertsRequest source)
    {
        var approvalRequest = await approvalsApiClient.GetApprenticeshipApprovalRequestAlerts(new GetApprovalRequestAlertRequest(source.AccountId,source.ApprenticeshipId));

        return new ApprenticeshipApprovalRequestAlertsViewModel
        {
            ApprenticeName = approvalRequest.ApprenticeName,
            ApprenticeshipHashedId = source.ApprenticeshipHashedId,
            ApprenticeshipId = source.ApprenticeshipId,
            AccountHashedId = source.AccountHashedId,
            ApprovalRequests = approvalRequest.ApprovalRequests.Select(r => new ApprovalRequestAlertViewModel
            {
                Id = r.Id,
                ApprovalRequestFieldItems = ConvertToDisplayItems(r.Items)
            }).ToList()
        };
    }

    private List<ApprovalFieldRequestAlertViewModel> ConvertToDisplayItems(List<ApprovalFieldRequest> items)
    {
        var displayItems = new List<ApprovalFieldRequestAlertViewModel>();
        foreach (var item in items)
        {
            if (item.Field == "TNP1")
            {
                displayItems.Add(new ApprovalFieldRequestAlertViewModel
                {
                    Field = "Training price (TNP1)",
                    Old = ToCurrency(item.Old),
                    New = ToCurrency(item.New),
                    Created = item.Created,
                    Status = (CocApprovalItemStatus)item.Status
                });
            }
            else if (item.Field == "TNP2")
            {
                displayItems.Add(new ApprovalFieldRequestAlertViewModel
                {
                    Field = "Assessment price (TNP2)",
                    Old = ToCurrency(item.Old),
                    New = ToCurrency(item.New),
                    Created = item.Created,
                    Status = (CocApprovalItemStatus)item.Status
                });
            }
            else
            {
                displayItems.Add(new ApprovalFieldRequestAlertViewModel
                {
                    Field = item.Field,
                    Old = item.Old,
                    New = item.New,
                    Created = item.Created,
                    Status = (CocApprovalItemStatus)item.Status
                });
            }
        }

        return displayItems;
    }

    public static string ToCurrency(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return "#error#";

        var culture = new CultureInfo("en-GB");

        if (decimal.TryParse(input, NumberStyles.Any, culture, out decimal value))
        {
            return value.ToString("C0", culture);
        }

        return "#error#";
    }
}