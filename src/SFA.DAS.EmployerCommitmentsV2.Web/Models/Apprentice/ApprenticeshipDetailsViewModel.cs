using System;
using System.Collections.Generic;
using SFA.DAS.CommitmentsV2.Types;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice
{
    public class ApprenticeshipDetailsViewModel
    {
        public string EncodedApprenticeshipId { get; set; }
        public string ApprenticeName { get ; set ; }
        public string CourseName { get; set; }
        public DateTime PlannedStartDate { get; set; }
        public DateTime PlannedEndDate { get; set; }
        public string ProviderName { get; set; }
        public ConfirmationStatus? ConfirmationStatus { get; set; }
        public ApprenticeshipStatus Status { get; set; }
        public IEnumerable<string> Alerts { get; set; }
    }
}