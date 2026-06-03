using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Contracts;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice;

public class ChangeHistoryRequestToViewModelMapper(IApprovalsApiClient approvalsApiClient) : IMapper<ChangeHistoryRequest, ChangeHistoryListViewModel>
{
    public async Task<ChangeHistoryListViewModel> Map(ChangeHistoryRequest source)
    {
        var changeHistory = await approvalsApiClient.GetChangeHistory(source.ApprenticeshipId);

        return new ChangeHistoryListViewModel
        {
            ApprenticeshipHashedId = source.ApprenticeshipHashedId,
            AccountHashedId = source.AccountHashedId,
            ChangeHistory = [.. changeHistory.ChangeHistory.Select(x => new ChangeHistoryViewModel
             {
                 Description = x.Description,
                 AppliedDate = x.AppliedDate,
                 ChangeType = (LearningChangeType)x.ChangeType,
                 Id = x.Id
             })],
            Name = changeHistory.ChangeHistory.FirstOrDefault()?.LearnerName
        };
    }
}