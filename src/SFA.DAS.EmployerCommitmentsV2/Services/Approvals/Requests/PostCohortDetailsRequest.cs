namespace SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Requests
{
    public class PostCohortDetailsRequest
    {
        public CohortSubmissionType SubmissionType { get; set; }
        public string Message { get; set; }
        public ApimUserInfo UserInfo { get; set; }

        public enum CohortSubmissionType
        {
            Send,
            Approve
        }
    }
}
