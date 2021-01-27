using SFA.DAS.Authorization.ModelBinding;
using SFA.DAS.CommitmentsV2.Shared.Models;
using System;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice
{
    public class StopRequestViewModel : IAuthorizationContextModel
    {
        public StopRequestViewModel()
        {
            StopDate = new MonthYearModel("");
        }

        public string AccountHashedId { get; set; }

        public string ApprenticeshipHashedId { get; set; }

        public long ApprenticeshipId { get; set; }

        public int? StopMonth { get => StopDate.Month; set => StopDate.Month = value; }
        
        public int? StopYear { get => StopDate.Year; set => StopDate.Year = value; }

        public MonthYearModel StopDate { get; set; }

        public DateTime StartDate { get; set; }

        public bool IsCoPJourney { get; set; }
    }
}
