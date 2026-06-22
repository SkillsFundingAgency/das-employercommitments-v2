namespace SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Responses;

public class GetChangePaymentsResponse
{
    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Uln { get; set; }

    public string CourseName { get; set; }

    public bool FreezeStatus { get; set; }

    public DateTime? PaymentFreezeDate { get; set; }
}
