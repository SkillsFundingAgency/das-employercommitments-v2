﻿using System;
using SFA.DAS.Authorization.ModelBinding;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Requests
{
    public class ReservationsAddDraftApprenticeshipRequest : IAuthorizationContextModel
    {
        public Guid ReservationId { get; set; }
        public string CohortReference { get; set; }
        public long? CohortId { get; set; }
        public string StartMonthYear { get; set; }
        public string CourseCode { get; set; }
    }
}
