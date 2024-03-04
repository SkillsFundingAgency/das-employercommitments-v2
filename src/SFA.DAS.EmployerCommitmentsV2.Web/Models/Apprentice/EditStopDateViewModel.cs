using SFA.DAS.CommitmentsV2.Shared.Models;
using System.ComponentModel.DataAnnotations;
using SFA.DAS.EmployerCommitmentsV2.Contracts;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;

public class EditStopDateViewModel : IAuthorizationContextModel
{
    public EditStopDateViewModel()
    {
        NewStopDate = new MonthYearModel("");
    }
        
    public long ApprenticeshipId { get; set; }

    public string AccountHashedId { get; set; }

    public string ApprenticeshipULN { get; set; }
        
    public string ApprenticeshipHashedId { get; set; }
        
    public string ApprenticeshipName { get; set; }        
        
    public DateTime CurrentStopDate { get; set; }       
        
    public DateTime ApprenticeshipStartDate { get; set; }       
        
    public MonthYearModel NewStopDate { get; set; } 
        
    public long AccountId { get; set; }

    [Display(Name = "Month")]
    public int? NewStopMonth { get => NewStopDate.Month; set => NewStopDate.Month = value; }

    [Display(Name = "Year")]
    public int? NewStopYear { get => NewStopDate.Year; set => NewStopDate.Year = value; }
}