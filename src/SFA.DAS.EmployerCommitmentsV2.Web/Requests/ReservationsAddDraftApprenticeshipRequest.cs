using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Requests
{
    public class ReservationsAddDraftApprenticeshipRequest
    {
        public Guid ReservationId { get; set; }
        public string CohortReference { get; set; }
        public long? CohortId { get; set; }
        public string StartMonthYear { get; set; }
        public string CourseCode { get; set; }
    }
}
