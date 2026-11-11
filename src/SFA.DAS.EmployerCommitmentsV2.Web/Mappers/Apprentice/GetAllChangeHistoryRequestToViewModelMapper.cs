using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Contracts;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Responses;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice;

public class GetAllChangeHistoryRequestToViewModelMapper(IApprovalsApiClient approvalsApiClient) : IMapper<GetAllChangeHistoryRequest, GetAllChangeHistoryListViewModel>
{
    public async Task<GetAllChangeHistoryListViewModel> Map(GetAllChangeHistoryRequest source)
    {
        var changeHistory = await approvalsApiClient.GetChangeHistoryForEmployer(source.AccountId);

        var allChanges = changeHistory?.ChangeHistory?.ToList() ?? new List<GetChangeHistoryItem>();
        var pageNumber = Math.Max(source.PageNumber, 1);
        var pageSize = Constants.ChangeHistory.NumberOfChangesPerPage;

        return new GetAllChangeHistoryListViewModel
        {
            AccountHashedId = source.AccountHashedId,
            ChangeHistory = allChanges
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new GetAllChangeHistoryViewModel
                {
                    Description = x.Description,
                    AppliedDate = x.AppliedDate,
                    ChangeType = (LearningChangeType)x.ChangeType,
                    Id = x.Id,
                    LearnerName = x.LearnerName,
                    ProviderName = x.ProviderName
                })
                .ToList(),
            AvailableFrom = allChanges.OrderBy(t => t.Created).FirstOrDefault()?.Created ?? DateTime.UtcNow,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = allChanges.Count
        };
    }
}