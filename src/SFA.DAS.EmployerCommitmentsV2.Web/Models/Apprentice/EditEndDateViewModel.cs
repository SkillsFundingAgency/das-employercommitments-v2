using SFA.DAS.CommitmentsV2.Shared.Models;
using System.ComponentModel.DataAnnotations;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;

public class EditEndDateViewModel : IAuthorizationContextModel
{
    public EditEndDateViewModel()
    {
        EndDate = new MonthYearModel("");
    }

    public string AccountHashedId { get; set; }

    public string ApprenticeshipHashedId { get; set; }

    public long ApprenticeshipId { get; set; }

    public MonthYearModel EndDate { get; }

    [Display(Name = "Month")]
    public int? EndMonth { get => EndDate.Month; set => EndDate.Month = value; }

    [Display(Name = "Year")]
    public int? EndYear { get => EndDate.Year; set => EndDate.Year = value; }
}