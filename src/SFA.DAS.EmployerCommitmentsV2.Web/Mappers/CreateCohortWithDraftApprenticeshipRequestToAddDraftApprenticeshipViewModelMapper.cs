using System;
using System.Threading.Tasks;
using SFA.DAS.Apprenticeships.Api.Client;
using SFA.DAS.Commitments.Shared.Interfaces;
using SFA.DAS.Commitments.Shared.Models;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Shared;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers
{
    public class CreateCohortWithDraftApprenticeshipRequestToAddDraftApprenticeshipViewModelMapper : IMapper<CreateCohortWithDraftApprenticeshipRequest, AddDraftApprenticeshipViewModel>
    {
        private readonly ICommitmentsApiClient _commitmentsApiClient;
        private readonly ITrainingProgrammeApiClient _trainingProgrammeApiClient;

        public CreateCohortWithDraftApprenticeshipRequestToAddDraftApprenticeshipViewModelMapper(
            ICommitmentsApiClient commitmentsApiClient,
            ITrainingProgrammeApiClient trainingProgrammeApiClient)
        {
            _commitmentsApiClient = commitmentsApiClient;
            _trainingProgrammeApiClient = trainingProgrammeApiClient;
        }

        public async Task<AddDraftApprenticeshipViewModel> Map(CreateCohortWithDraftApprenticeshipRequest source)
        {
            var courses = await _trainingProgrammeApiClient.GetAllTrainingProgrammes();
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
}
