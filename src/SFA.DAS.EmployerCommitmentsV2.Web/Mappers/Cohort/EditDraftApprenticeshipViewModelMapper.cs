﻿using System.Threading.Tasks;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.CommitmentsV2.Shared.Models;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.DraftApprenticeship;
using SFA.DAS.Encoding;
using StructureMap.Query;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort
{
    public class EditDraftApprenticeshipViewModelMapper : IMapper<EditDraftApprenticeshipDetails, EditDraftApprenticeshipViewModel>
    {
        public Task<EditDraftApprenticeshipViewModel> Map(EditDraftApprenticeshipDetails source) =>
            Task.FromResult(new EditDraftApprenticeshipViewModel(source.DateOfBirth, source.StartDate, source.EndDate)
            {
                DraftApprenticeshipHashedId = source.DraftApprenticeshipHashedId,
                CohortId = source.CohortId,
                CohortReference = source.CohortReference,
                ProviderId = source.ProviderId,
                ReservationId = source.ReservationId,
                FirstName = source.FirstName,
                LastName = source.LastName,
                Email = source.Email,
                Uln = source.UniqueLearnerNumber,
                DeliveryModel = source.DeliveryModel,
                CourseCode = source.CourseCode,
                Cost = source.Cost,
                Reference = source.OriginatorReference
            });
    }
}