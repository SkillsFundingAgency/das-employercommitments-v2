using SFA.DAS.Authorization.ModelBinding;
using SFA.DAS.CommitmentsV2.Shared.Models;
using System;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice
{
    public class WhatIsTheNewStartDateViewModel : IAuthorizationContextModel
    {
        public WhatIsTheNewStartDateViewModel()
        {
            NewStartDate = new MonthYearModel("");
        }

        public string AccountHashedId { get; set; }
        public string ApprenticeshipHashedId { get; set; }
        public string ProviderName { get; set; }
        public long ProviderId { get; set; }
        public DateTime StopDate { get; set; }
        public MonthYearModel NewStartDate { get; }
        public bool Edit { get; set; }
        public int? NewStartMonth { get => NewStartDate.Month; set => NewStartDate.Month = value; }
        public int? NewStartYear { get => NewStartDate.Year; set => NewStartDate.Year = value; } 
        public int? NewEndMonth { get; }
        public int? NewEndYear { get; }
        public int? NewPrice { get; }
    }
}
