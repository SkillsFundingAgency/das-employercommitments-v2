using System.Globalization;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Contracts;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Responses;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice;

public class ApprenticeshipApprovalRequestToViewModelMapper(IApprovalsApiClient approvalsApiClient) : IMapper<ApprenticeshipApprovalRequest, ApprenticeshipApprovalRequestViewModel>
{
    public async Task<ApprenticeshipApprovalRequestViewModel> Map(ApprenticeshipApprovalRequest source)
    {
        var approvalRequest = await approvalsApiClient.GetApprenticeshipApprovalRequest(source.AccountId, source.ApprenticeshipId, source.ApprovalRequestId);

        return new ApprenticeshipApprovalRequestViewModel
        {
            ApprenticeshipHashedId = source.ApprenticeshipHashedId,
            AccountHashedId = source.AccountHashedId,
            ApprovalRequestId = source.ApprovalRequestId,
            ApprovalRequestStatus = approvalRequest.ApprovalRequestStatus,
            ExceedsFundingCap = approvalRequest.ExceedsFundingCap,
            DisplayFundingCapPrice = ToCurrency(approvalRequest.FundingCap),
            Items = ConvertToDisplayItems(approvalRequest.Items),
            Name = approvalRequest.Name,
            ULN = approvalRequest.ULN,
            CourseName = approvalRequest.CourseName,
            ProviderName = approvalRequest.ProviderName,
            UKPRN = approvalRequest.UKPRN,
            IsSupersededOrCancelled = approvalRequest.ApprovalRequestStatus is CocApprovalResultStatus.Superseded or CocApprovalResultStatus.Cancelled
        };
    }

    private ICollection<ApprenticeshipApprovalRequestViewModel.ChangeItem> ConvertToDisplayItems(ICollection<GetApprenticeshipApprovalResponse.ChangeItem> items)
    {
        var displayItems = new List<ApprenticeshipApprovalRequestViewModel.ChangeItem>();
        foreach (var item in items)
        {
            if(item.FieldName == "TNP1")
            {
                displayItems.Add(new ApprenticeshipApprovalRequestViewModel.ChangeItem
                {
                    FieldName = "Training price (TNP1)",
                    OldValue = ToCurrency(item.OldValue),
                    NewValue = ToCurrency(item.NewValue),
                    EffectiveFromDate = item.EffectiveFromDate
                });
            }
            else if (item.FieldName == "TNP2")
            {
                displayItems.Add(new ApprenticeshipApprovalRequestViewModel.ChangeItem
                {
                    FieldName = "Assessment price (TNP2)",
                    OldValue = ToCurrency(item.OldValue),
                    NewValue = ToCurrency(item.NewValue),
                    EffectiveFromDate = item.EffectiveFromDate
                });

            }
            else
            {
                displayItems.Add(new ApprenticeshipApprovalRequestViewModel.ChangeItem
                {
                    FieldName = item.FieldName,
                    OldValue = item.OldValue,
                    NewValue = item.NewValue,
                });
            }
        }

        return displayItems;
    }

    public static string ToCurrency(int? input)
    {
        return ToCurrency(input?.ToString());
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