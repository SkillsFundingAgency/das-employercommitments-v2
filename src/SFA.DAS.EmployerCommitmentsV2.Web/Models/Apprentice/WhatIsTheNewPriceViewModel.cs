namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;

public class WhatIsTheNewPriceViewModel : ChangeOfProviderBaseViewModel
{        
    public DateTime StopDate { get; set; }     
    public string NewStartMonthYear { get; set; } 
    public string NewEndMonthYear { get; set; }           
}