using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.CommitmentsV2.Shared.Models;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.DraftApprenticeship;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort;

public class AddDraftApprenticeshipViewModelMapper : IMapper<ApprenticeRequest, AddDraftApprenticeshipViewModel>
{
    private readonly ICommitmentsApiClient _commitmentsApiClient;

    public AddDraftApprenticeshipViewModelMapper(ICommitmentsApiClient commitmentsApiClient)
    {
        _commitmentsApiClient = commitmentsApiClient;
    }

    public async Task<AddDraftApprenticeshipViewModel> Map(ApprenticeRequest source)
    {
        var ale = await _commitmentsApiClient.GetAccountLegalEntity(source.AccountLegalEntityId);

        var courses = !string.IsNullOrWhiteSpace(source.TransferSenderId) || ale.LevyStatus == ApprenticeshipEmployerType.NonLevy
            ? (await _commitmentsApiClient.GetAllTrainingProgrammeStandards()).TrainingProgrammes
            : (await _commitmentsApiClient.GetAllTrainingProgrammes()).TrainingProgrammes;

        var provider = await _commitmentsApiClient.GetProvider(source.ProviderId);

        var result = new AddDraftApprenticeshipViewModel
        {
            AccountLegalEntityId = source.AccountLegalEntityId,
            AccountLegalEntityHashedId = source.AccountLegalEntityHashedId,
            StartDate = new MonthYearModel(source.StartMonthYear),
            ReservationId = source.ReservationId,
            CourseCode = source.CourseCode,
            ProviderId = (int)source.ProviderId,
            ProviderName = provider.Name,
            Courses =  courses
        };

        return result;
    }
}