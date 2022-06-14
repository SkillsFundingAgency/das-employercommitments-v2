﻿using System;
using System.Threading.Tasks;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.CommitmentsV2.Shared.Models;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.DraftApprenticeship;
using SFA.DAS.Encoding;
using SFA.DAS.EmployerCommitmentsV2.Web.Services;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.DraftApprenticeship
{

    public class EditDraftApprenticeshipViewModelMapper : IMapper<EditDraftApprenticeshipRequest, IDraftApprenticeshipViewModel>
    {
        private readonly ICommitmentsApiClient _commitmentsApiClient;
        private readonly IEncodingService _encodingService;
        private readonly IFjaaAgencyService _fjaaAgencyService;

        public EditDraftApprenticeshipViewModelMapper(ICommitmentsApiClient commitmentsApiClient,
            IEncodingService encodingService,
            IFjaaAgencyService fjaaAgencyService
            )
        {
            _commitmentsApiClient = commitmentsApiClient;
            _encodingService = encodingService;
            _fjaaAgencyService = fjaaAgencyService;
        }

        public async Task<IDraftApprenticeshipViewModel> Map(EditDraftApprenticeshipRequest source)
        {
            var cohort = source.Cohort;

            var draftApprenticeship = await _commitmentsApiClient.GetDraftApprenticeship(source.Request.CohortId, source.Request.DraftApprenticeshipId);

            bool agencyExists = await _fjaaAgencyService.AgencyExists((int)source.Cohort.AccountLegalEntityId);

            return new EditDraftApprenticeshipViewModel(draftApprenticeship.DateOfBirth, draftApprenticeship.StartDate, draftApprenticeship.EndDate)
            {
                DraftApprenticeshipId = draftApprenticeship.Id,
                DraftApprenticeshipHashedId = _encodingService.Encode(draftApprenticeship.Id, EncodingType.ApprenticeshipId),
                CohortId = source.Request.CohortId,
                CohortReference = _encodingService.Encode(source.Request.CohortId, EncodingType.CohortReference),
                ReservationId = draftApprenticeship.ReservationId,
                FirstName = draftApprenticeship.FirstName,
                LastName = draftApprenticeship.LastName,
                Email = draftApprenticeship.Email,
                Uln = draftApprenticeship.Uln,
                DeliveryModel = draftApprenticeship.DeliveryModel,
                CourseCode = draftApprenticeship.CourseCode,
                StandardUId = draftApprenticeship.StandardUId,
                Cost = draftApprenticeship.Cost,
                EmploymentPrice = draftApprenticeship.EmploymentPrice,
                EmploymentEndMonth = draftApprenticeship.EmploymentEndDate.HasValue ? draftApprenticeship.EmploymentEndDate.Value.Month : (int?)null,
                EmploymentEndYear = draftApprenticeship.EmploymentEndDate.HasValue ? draftApprenticeship.EmploymentEndDate.Value.Year : (int?)null,
                Reference = draftApprenticeship.Reference,
                AccountHashedId = source.Request.AccountHashedId,
                ProviderName = cohort.ProviderName,
                LegalEntityName = source.Cohort.LegalEntityName,
                IsContinuation = draftApprenticeship.IsContinuation,
                FjaaAgencyExists = agencyExists,
                Courses = (cohort.IsFundedByTransfer || cohort.LevyStatus == ApprenticeshipEmployerType.NonLevy) && !draftApprenticeship.IsContinuation
                    ? (await _commitmentsApiClient.GetAllTrainingProgrammeStandards()).TrainingProgrammes
                    : (await _commitmentsApiClient.GetAllTrainingProgrammes()).TrainingProgrammes
            };
        }
    }
}
