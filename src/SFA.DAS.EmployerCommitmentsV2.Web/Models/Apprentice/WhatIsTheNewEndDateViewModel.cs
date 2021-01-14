using System;
using System.ComponentModel.DataAnnotations;
using SFA.DAS.CommitmentsV2.Shared.Models;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice
{
    public class WhatIsTheNewEndDateViewModel
    {
        public WhatIsTheNewEndDateViewModel()
        {
            NewEndDate = new MonthYearModel("");
        }

        public string AccountHashedId { get; set; }
        public string ApprenticeshipHashedId { get; set; }
        public string ProviderName { get; set; }
        public long ProviderId { get; set; }
        public int? NewStartMonth { get; set; }
        public int? NewStartYear { get; set; }
        public DateTime NewStartDate { get; set; }
        public int? NewPrice { get; }
        public bool Edit { get; set; }
        public MonthYearModel NewEndDate { get; }

        [Display(Name = "Month")]
        public int? NewEndMonth { get => NewEndDate.Month; set => NewEndDate.Month = value; }

        [Display(Name = "Year")]
        public int? NewEndYear { get => NewEndDate.Year; set => NewEndDate.Year = value; }

    }
}
