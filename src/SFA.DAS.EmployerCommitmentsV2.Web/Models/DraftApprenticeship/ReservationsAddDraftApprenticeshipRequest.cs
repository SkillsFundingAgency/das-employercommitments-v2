namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.DraftApprenticeship;

public class ReservationsAddDraftApprenticeshipRequest : IAuthorizationContextModel
{
    public Guid ReservationId { get; set; }
    public string CohortReference { get; set; }
    public long? CohortId { get; set; }
    public string StartMonthYear { get; set; }
    public string CourseCode { get; set; }
}