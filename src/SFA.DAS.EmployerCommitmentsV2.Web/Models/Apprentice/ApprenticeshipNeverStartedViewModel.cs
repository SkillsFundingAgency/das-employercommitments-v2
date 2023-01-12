using System;
using SFA.DAS.Authorization.ModelBinding;
using SFA.DAS.CommitmentsV2.Shared.Models;
using SFA.DAS.EmployerCommitmentsV2.Web.Attributes;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice
{
    public class ApprenticeshipNeverStartedViewModel : IAuthorizationContextModel
    {
        public ApprenticeshipNeverStartedViewModel()
        {
            StopDate = new MonthYearModel("");
        }
        public string AccountHashedId { get; set; }

        public string ApprenticeshipHashedId { get; set; }
        public long ApprenticeshipId { get; set; }

        public DateTime PlannedStartDate { get; set; }

        [SuppressArgumentException(nameof(StopDate), "The stop date must be a real date")]
        public int? StopMonth { get => StopDate.Month; set => StopDate.Month = value; }

        [SuppressArgumentException(nameof(StopDate), "The stop date must be a real date")]
        public int? StopYear { get => StopDate.Year; set => StopDate.Year = value; }

        public MonthYearModel StopDate { get; set; }

        public bool IsCoPJourney { get; set; }
    }
}