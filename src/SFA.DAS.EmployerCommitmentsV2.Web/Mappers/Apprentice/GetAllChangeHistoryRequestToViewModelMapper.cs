using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Contracts;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice;

public class GetAllChangeHistoryRequestToViewModelMapper(IApprovalsApiClient approvalsApiClient) : IMapper<GetAllChangeHistoryRequest, GetAllChangeHistoryListViewModel>
{
    public async Task<GetAllChangeHistoryListViewModel> Map(GetAllChangeHistoryRequest source)
    {
        var changeHistory = await approvalsApiClient.GetChangeHistoryForEmployer(source.AccountId);

        return new GetAllChangeHistoryListViewModel
        {
            AccountHashedId = source.AccountHashedId,
            ChangeHistory = changeHistory?.ChangeHistory?.ConvertAll(x => new GetAllChangeHistoryViewModel
            {
                Description = x.Description,
                AppliedDate = x.AppliedDate,
                ChangeType = (LearningChangeType)x.ChangeType,
                Id = x.Id,
                LearnerName = x.LearnerName,
                ProviderName = x.ProviderName
            }),
            AvailableFrom = changeHistory?.ChangeHistory?.OrderBy(t => t.Created).FirstOrDefault()?.Created ?? DateTime.UtcNow
        };
    }
}