using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.Authorization.ModelBinding;
using System.ComponentModel;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice
{
    public class WhyStopApprenticeshipViewModel : IAuthorizationContextModel
    {
        public WhyStopApprenticeshipViewModel()
        {

        }

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

    public enum StopApprenticeStatus : short
    {
        [Description("LeftEmployment")]
        LeftEmployment = 0,
        [Description("ChangeProvider")]
        ChangeProvider = 1,
        [Description("TrainingEnded")]
        TrainingEnded=2,
        [Description("Withdrawn")]
        Withdrawn = 3,
        [Description("NeverStarted")]
        NeverStarted = 4
    }
}
