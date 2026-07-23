using Microsoft.AspNetCore.WebUtilities;
using SFA.DAS.CommitmentsV2.Types;

namespace SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Requests;

public class GetApprenticeshipsRequest(long? accountId, int pageNumber, int pageItemCount, string sortField, bool reverseSort, string searchTerm, string employerName, string providerName, string courseName, ApprenticeshipStatus? status, DateTime? startDate, DateTime? endDate, int? accountLegalEntityId, DateTime? startDateRangeFrom, DateTime? startDateRangeTo, Alerts? alert, ConfirmationStatus? apprenticeConfirmationStatus, DeliveryModel? deliveryModel)
{
    public long? AccountId { get; set; } = accountId;

    public int PageNumber { get; set; } = pageNumber;

    public int PageItemCount { get; set; } = pageItemCount;

    public string SortField { get; set; } = sortField;

    public bool ReverseSort { get; set; } = reverseSort;

    public string SearchTerm { get; set; } = searchTerm;
    public string EmployerName { get; set; } = employerName;

    public string ProviderName { get; set; } = providerName;

    public string CourseName { get; set; } = courseName;

    public ApprenticeshipStatus? Status { get; set; } = status;

    public DateTime? StartDate { get; set; } = startDate;

    public DateTime? EndDate { get; set; } = endDate;
    public int? AccountLegalEntityId { get; set; } = accountLegalEntityId;

    public DateTime? StartDateRangeFrom { get; set; } = startDateRangeFrom;

    public DateTime? StartDateRangeTo { get; set; } = startDateRangeTo;

    public Alerts? Alert { get; set; } = alert;

    public ConfirmationStatus? ApprenticeConfirmationStatus { get; set; } = apprenticeConfirmationStatus;
    public DeliveryModel? DeliveryModel { get; set; } = deliveryModel;
    public string GetUrl => QueryHelpers.AddQueryString($"employer/{AccountId}/apprentices", CreateFilterQuery(this));

    private static Dictionary<string, string> CreateFilterQuery(GetApprenticeshipsRequest request)
    {
        var queryParameters = new Dictionary<string, string>();       

        if (!string.IsNullOrEmpty(request.SearchTerm))
            queryParameters.Add("searchTerm", request.SearchTerm);

        if (!string.IsNullOrEmpty(request.EmployerName))
            queryParameters.Add("employerName", request.EmployerName);

        if (request.PageNumber > 0)
            queryParameters.Add("pageNumber", request.PageNumber.ToString());

        if (request.PageItemCount > 0)
            queryParameters.Add("pageItemCount", request.PageItemCount.ToString());

        if (!string.IsNullOrEmpty(request.SortField))
            queryParameters.Add("sortField", request.SortField);

        if (request.ReverseSort)
            queryParameters.Add("reverseSort", request.ReverseSort.ToString().ToLower());

        if (!string.IsNullOrEmpty(request.CourseName))
            queryParameters.Add("courseName", request.CourseName);

        if (!string.IsNullOrEmpty(request.ProviderName))
            queryParameters.Add("providerName", request.ProviderName);

        if (request.Status.HasValue)
            queryParameters.Add("status", request.Status.Value.ToString());

        if (request.StartDate.HasValue)
            queryParameters.Add("startDate", request.StartDate.Value.ToString("u"));

        if (request.EndDate.HasValue)
            queryParameters.Add("endDate", request.EndDate.Value.ToString("u"));

        if (request.StartDateRangeFrom.HasValue)
            queryParameters.Add("startDateRangeFrom", request.StartDateRangeFrom.Value.ToString("u"));

        if (request.StartDateRangeTo.HasValue)
            queryParameters.Add("startDateRangeTo", request.StartDateRangeTo.Value.ToString("u"));

        if (request.Alert.HasValue)
            queryParameters.Add("alert", request.Alert.Value.ToString());

        if (request.ApprenticeConfirmationStatus.HasValue)
            queryParameters.Add("apprenticeConfirmationStatus", request.ApprenticeConfirmationStatus.ToString());

        if (request.DeliveryModel.HasValue)
            queryParameters.Add("deliveryModel", request.DeliveryModel.ToString());

        return queryParameters;
    }
}