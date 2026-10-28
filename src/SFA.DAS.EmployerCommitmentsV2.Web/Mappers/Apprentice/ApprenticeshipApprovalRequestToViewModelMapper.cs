using System.Globalization;
using System.Text.Json;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Contracts;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Responses;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice;

public class ApprenticeshipApprovalRequestToViewModelMapper(IApprovalsApiClient approvalsApiClient) : IMapper<ApprenticeshipApprovalRequest, ApprenticeshipApprovalRequestViewModel>
{
    private static readonly JsonSerializerOptions PriceJsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

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
            ChangeApprovalAllowed = IsChangeApprovalAllowed(approvalRequest.ApprenticeshipStatus),

            Items = ConvertToDisplayItems(approvalRequest.Items),
            Name = approvalRequest.Name,
            ULN = approvalRequest.ULN,
            CourseName = approvalRequest.CourseName,
            ProviderName = approvalRequest.ProviderName,
            UKPRN = approvalRequest.UKPRN,
        };
    }

    private ICollection<ApprenticeshipApprovalRequestViewModel.ChangeItem> ConvertToDisplayItems(ICollection<GetApprenticeshipApprovalResponse.ChangeItem> items)
    {
        var displayItems = new List<ApprenticeshipApprovalRequestViewModel.ChangeItem>();
        foreach (var item in items)
        {
            if (item.FieldName == "TNP1")
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
            else if (item.FieldName == "Prices")
            {
                displayItems.Add(ToTotalCost(item));
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

    private static ApprenticeshipApprovalRequestViewModel.ChangeItem ToTotalCost(GetApprenticeshipApprovalResponse.ChangeItem item)
    {
        var previous = LatestEpisode(item.OldValue);
        var changedTo = LatestEpisode(item.NewValue);

        return new ApprenticeshipApprovalRequestViewModel.ChangeItem
        {
            FieldName = "Total cost",
            OldValue = previous == null ? "#error#" : ToPrice(previous.Cost),
            NewValue = changedTo == null ? "#error#" : ToPrice(changedTo.Cost),
            NewValueHint = changedTo == null ? null : FormatEffectiveFrom(changedTo.EffectiveFrom)
        };
    }

    private static PriceScheduleEpisode LatestEpisode(string schedule)
    {
        if (string.IsNullOrWhiteSpace(schedule))
        {
            return null;
        }

        try
        {
            var episodes = JsonSerializer.Deserialize<List<PriceScheduleEpisode>>(schedule, PriceJsonOptions);
            return episodes?
                .Where(episode => episode != null && !string.IsNullOrWhiteSpace(episode.EffectiveFrom))
                .OrderBy(episode => episode.EffectiveFrom, StringComparer.Ordinal)
                .ThenBy(episode => episode.Cost)
                .LastOrDefault();
        }
        catch (JsonException)
        {
            return null;
        }
    }

    private static string FormatEffectiveFrom(string effectiveFrom)
    {
        if (!DateTime.TryParseExact(effectiveFrom, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var date))
        {
            return null;
        }

        return "From " + date.ToString("d MMMM yyyy", CultureInfo.GetCultureInfo("en-GB"));
    }

    private static string ToPrice(decimal value)
    {
        var culture = CultureInfo.GetCultureInfo("en-GB");
        var rounded = decimal.Round(value, 2, MidpointRounding.AwayFromZero);
        return rounded == decimal.Truncate(rounded)
            ? rounded.ToString("C0", culture)
            : rounded.ToString("C2", culture);
    }

    private sealed class PriceScheduleEpisode
    {
        public string EffectiveFrom { get; set; }
        public decimal Cost { get; set; }
    }

    public static string ToCurrency(int? input)
    {
        return ToCurrency(input?.ToString());
    }

    private bool IsChangeApprovalAllowed(ApprenticeshipStatus currentStatus) =>
        currentStatus == ApprenticeshipStatus.Live
        || currentStatus == ApprenticeshipStatus.Paused
        || currentStatus == ApprenticeshipStatus.WaitingToStart;

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