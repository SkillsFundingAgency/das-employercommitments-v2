using System;
using System.ComponentModel.DataAnnotations;
using SFA.DAS.CommitmentsV2.Shared.Models;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;

public class WhatIsTheNewEndDateViewModel : ChangeOfProviderBaseViewModel
{
    public DateTime NewStartDate { get; set; }
    public MonthYearModel NewEndDate { get; }
    public WhatIsTheNewEndDateViewModel()
    {
        NewEndDate = new MonthYearModel("");
    }

    [Display(Name = "Month")]
    public override int? NewEndMonth { get => NewEndDate.Month; set => NewEndDate.Month = value; }

    [Display(Name = "Year")]
    public override int? NewEndYear { get => NewEndDate.Year; set => NewEndDate.Year = value; }

}