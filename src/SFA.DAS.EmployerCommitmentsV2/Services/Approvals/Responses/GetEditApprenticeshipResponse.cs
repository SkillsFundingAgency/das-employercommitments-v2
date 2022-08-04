namespace SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Responses
{
    public class GetEditApprenticeshipResponse
    {
        public bool HasMultipleDeliveryModelOptions { get; set; }
        public bool IsFundedByTransfer { get; set; }
        public string CourseName { get; set; }
    }
}
