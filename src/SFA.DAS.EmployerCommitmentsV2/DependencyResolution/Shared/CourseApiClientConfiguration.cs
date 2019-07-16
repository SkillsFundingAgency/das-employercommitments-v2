using SFA.DAS.Commitments.Shared.Configuration;

namespace SFA.DAS.EmployerCommitmentsV2.Configuration
{
    public class _CommitmentsSharedConfiguration
    {
        public CourseApiClientConfiguration CourseApi { get; set; }
    }

    public class _CourseApiClientConfiguration
    {
        public string BaseUrl { get; set; }
    }
}