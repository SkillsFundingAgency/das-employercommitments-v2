using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Shared;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort;

public class ApprenticeRequestToSelectCourseViewModelMapper : IMapper<ApprenticeRequest, SelectCourseViewModel>
{
    private readonly ICommitmentsApiClient _commitmentsApiClient;

    public ApprenticeRequestToSelectCourseViewModelMapper(ICommitmentsApiClient commitmentsApiClient)
        => _commitmentsApiClient = commitmentsApiClient;

    public async Task<SelectCourseViewModel> Map(ApprenticeRequest source)
    {
        var ale = await _commitmentsApiClient.GetAccountLegalEntity(source.AccountLegalEntityId);

        var courses = (await _commitmentsApiClient.GetAllTrainingProgrammeStandards()).TrainingProgrammes;

        return new SelectCourseViewModel
        {
            AccountHashedId = source.AccountHashedId,
            AccountLegalEntityHashedId = source.AccountLegalEntityHashedId,
            AccountLegalEntityId = source.AccountLegalEntityId,
            CourseCode = source.CourseCode,
            Courses = courses.ToArray(),
            ProviderId = source.ProviderId,
            ReservationId = source.ReservationId,
            StartMonthYear = source.StartMonthYear,
            DeliveryModel = source.DeliveryModel,
            TransferSenderId = source.TransferSenderId,
            CacheKey = source.CacheKey
        };
    }
}