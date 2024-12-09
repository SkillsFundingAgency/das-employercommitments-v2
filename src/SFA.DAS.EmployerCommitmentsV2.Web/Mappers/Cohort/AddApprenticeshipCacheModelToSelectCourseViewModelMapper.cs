using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Shared;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort;

public class AddApprenticeshipCacheModelToSelectCourseViewModelMapper(ICommitmentsApiClient commitmentsApiClient) : IMapper<AddApprenticeshipCacheModel, SelectCourseViewModel>
{
    public async Task<SelectCourseViewModel> Map(AddApprenticeshipCacheModel source)
    {
        var ale = await commitmentsApiClient.GetAccountLegalEntity(source.AccountLegalEntityId);

        var courses = (await commitmentsApiClient.GetAllTrainingProgrammeStandards()).TrainingProgrammes;

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
            CacheKey = source.CacheKey,
            AddApprenticeshipCacheKey = source.CacheKey
        };
    }
}