using SFA.DAS.CommitmentsV2.Shared.Models;
using SFA.DAS.EmployerCommitmentsV2.Attributes;
using SFA.DAS.EmployerCommitmentsV2.Contracts;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;

public class StopRequestViewModel : IAuthorizationContextModel
{
    public StopRequestViewModel()
    {
        StopDate = new MonthYearModel("");
    }

    public string AccountHashedId { get; set; }

    public string ApprenticeshipHashedId { get; set; }

    public long ApprenticeshipId { get; set; }

    [SuppressArgumentException(nameof(StopDate), "The stop date must be a real date")]
    public int? StopMonth { get => StopDate.Month; set => StopDate.Month = value; }

    [SuppressArgumentException(nameof(StopDate), "The stop date must be a real date")]
    public int? StopYear { get => StopDate.Year; set => StopDate.Year = value; }

    public MonthYearModel StopDate { get; set; }

    public DateTime StartDate { get; set; }

    public bool IsCoPJourney { get; set; }
}