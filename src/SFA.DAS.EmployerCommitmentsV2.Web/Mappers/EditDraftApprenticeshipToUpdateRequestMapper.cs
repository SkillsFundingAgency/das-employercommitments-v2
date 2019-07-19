﻿using SFA.DAS.Commitments.Shared.Interfaces;
using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using SFA.DAS.EmployerCommitmentsV2.Web.Authentication;
using SFA.DAS.EmployerCommitmentsV2.Web.Models;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers
{
    public class EditDraftApprenticeshipToUpdateRequestMapper : SaveDataMapper<CreateCohortRequest>, IMapper<EditDraftApprenticeshipViewModel, UpdateDraftApprenticeshipRequest>
    {
        public EditDraftApprenticeshipToUpdateRequestMapper(IAuthenticationService authenticationService) : base(authenticationService)
        {
        }

        public UpdateDraftApprenticeshipRequest Map(EditDraftApprenticeshipViewModel source) =>
            new UpdateDraftApprenticeshipRequest
            {
                ReservationId = source.ReservationId,
                FirstName = source.FirstName,
                LastName = source.LastName,
                DateOfBirth = source.DateOfBirth.Date,
                Uln = source.Uln,
                CourseCode = source.CourseCode,
                Cost = source.Cost,
                StartDate = source.StartDate.Date,
                EndDate = source.EndDate.Date,
                Reference = source.Reference,
                UserInfo = GetUserInfo()
            };
    }
}
