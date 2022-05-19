using SFA.DAS.Authorization.Services;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.CommitmentsV2.Shared.Models;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Features;
using SFA.DAS.EmployerCommitmentsV2.Web.Exceptions;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.DraftApprenticeship;
using SFA.DAS.Encoding;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.DraftApprenticeship
{
    public class AddDraftApprenticeshipViewModelMapper : IMapper<AddDraftApprenticeshipRequest, AddDraftApprenticeshipViewModel>
    {
        private readonly ICommitmentsApiClient _commitmentsApiClient;
        private readonly IEncodingService _encodingService;
        private readonly IAuthorizationService _authorizationService;

        public AddDraftApprenticeshipViewModelMapper(ICommitmentsApiClient commitmentsApiClient, IEncodingService encodingService, IAuthorizationService authorizationService)
            => (_commitmentsApiClient, _encodingService, _authorizationService) = (commitmentsApiClient, encodingService, authorizationService);        

        public async Task<AddDraftApprenticeshipViewModel> Map(AddDraftApprenticeshipRequest source)
        {
            var cohort = await _commitmentsApiClient.GetCohort(source.CohortId);

            if (cohort.WithParty != Party.Employer)
                throw new CohortEmployerUpdateDeniedException($"Cohort {cohort} is not with the Employer");

            var result = new AddDraftApprenticeshipViewModel
            {
                AccountHashedId = source.AccountHashedId,
                CohortReference = source.CohortReference,
                CohortId = source.CohortId,
                AccountLegalEntityHashedId = source.AccountLegalEntityHashedId,
                AccountLegalEntityId = source.AccountLegalEntityId,
                LegalEntityName = cohort.LegalEntityName,
                ReservationId = source.ReservationId,
                StartDate = new MonthYearModel(source.StartMonthYear),
                CourseCode = source.CourseCode,
                ProviderId = source.ProviderId,
                ProviderName = cohort.ProviderName,
                Courses = await GetCoursesIfPreDeliveryModel(cohort.IsFundedByTransfer, cohort.LevyStatus),   
                TransferSenderHashedId = cohort.IsFundedByTransfer ? _encodingService.Encode(cohort.TransferSenderId.Value, EncodingType.PublicAccountId) : string.Empty,
                AutoCreatedReservation = source.AutoCreated,
                DeliveryModel = source.DeliveryModel,
            };

            return result;
        }

        private async Task<IEnumerable<TrainingProgramme>> GetCoursesIfPreDeliveryModel(bool IsFundedByTransfer, ApprenticeshipEmployerType levyStatus)
        {
            if (_authorizationService.IsAuthorized(EmployerFeature.DeliveryModel))
            {
                return null;
            }
            else
            {
                return IsFundedByTransfer || levyStatus == ApprenticeshipEmployerType.NonLevy
                    ? (await _commitmentsApiClient.GetAllTrainingProgrammeStandards()).TrainingProgrammes
                    : (await _commitmentsApiClient.GetAllTrainingProgrammes()).TrainingProgrammes;  
            }
        }
    }
}
