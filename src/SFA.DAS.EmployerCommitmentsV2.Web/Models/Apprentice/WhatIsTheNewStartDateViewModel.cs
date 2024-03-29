﻿using SFA.DAS.CommitmentsV2.Shared.Models;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;

public class WhatIsTheNewStartDateViewModel : ChangeOfProviderBaseViewModel
{
    public DateTime StopDate { get; set; }
    public MonthYearModel NewStartDate { get; }
    public DateTime? NewEndDate { get; set; }

    public WhatIsTheNewStartDateViewModel()
    {
        NewStartDate = new MonthYearModel("");
    }

    public override int? NewStartMonth { get => NewStartDate.Month; set => NewStartDate.Month = value; }
    public override int? NewStartYear { get => NewStartDate.Year; set => NewStartDate.Year = value; } 
}