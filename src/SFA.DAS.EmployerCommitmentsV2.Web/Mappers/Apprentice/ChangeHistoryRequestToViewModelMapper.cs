using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Contracts;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.Encoding;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice;

public class ChangeHistoryRequestToViewModelMapper(IApprovalsApiClient approvalsApiClient,
        IEncodingService encodingService) : IMapper<ChangeHistoryRequest, ChangeHistoryListViewModel>
{
    public async Task<ChangeHistoryListViewModel> Map(ChangeHistoryRequest source)
    {
        var apprenticeshipId = encodingService.Decode(source.ApprenticeshipHashedId, EncodingType.ApprenticeshipId);
        var changeHistory = await approvalsApiClient.GetChangeHistory(apprenticeshipId);

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
            Name = changeHistory.ChangeHistory.FirstOrDefault().LearnerName
        };
    }
}