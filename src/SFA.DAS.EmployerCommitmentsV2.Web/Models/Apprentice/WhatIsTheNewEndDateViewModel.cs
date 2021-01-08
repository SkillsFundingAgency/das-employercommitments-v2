using System;
using System.ComponentModel.DataAnnotations;
using SFA.DAS.CommitmentsV2.Shared.Models;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice
{
    public class WhatIsTheNewEndDateViewModel
    {
        public WhatIsTheNewEndDateViewModel()
        {
            EndDate = new MonthYearModel("");
        }

        public string ProviderName { get; set; }
        public DateTime StartDate { get; set; }
        public MonthYearModel EndDate { get; }

        [Display(Name = "Month")]
        public int? EndMonth { get => EndDate.Month; set => EndDate.Month = value; }

        [Display(Name = "Year")]
        public int? EndYear { get => EndDate.Year; set => EndDate.Year = value; }

    }
}
