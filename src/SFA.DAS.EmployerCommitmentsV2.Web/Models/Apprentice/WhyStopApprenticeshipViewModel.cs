using SFA.DAS.Authorization.ModelBinding;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice
{
    public class WhyStopApprenticeshipViewModel : IAuthorizationContextModel
    {
        public string AccountHashedId { get; set; }

        public string ApprenticeshipHashedId { get; set; }
        public long ApprenticeshipId { get; set; }
        public StopStatusReason? SelectedStatusChange {get;set;}

        public StopStatusReason CurrentStatus { get; set; }

        public enum StopStatusReason
        {
            LeftEmployment,
            ChangeProvider,
            TrainingEnded,
            Withdrawn,
            NeverStarted
        }
    }
}
