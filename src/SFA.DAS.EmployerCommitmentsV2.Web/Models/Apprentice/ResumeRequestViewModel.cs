using SFA.DAS.Authorization.ModelBinding;
using System;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice
{
    public class ResumeRequestViewModel : IAuthorizationContextModel
    {
        public ResumeRequestViewModel()
        {
        }

        public string AccountHashedId { get; set; }

        public string ApprenticeshipHashedId { get; set; }

        public long ApprenticeshipId { get; set; }

        public string ApprenticeName { get; set; }

        public string ULN { get; set; }

        public string Course { get; set; }

        public DateTime? PauseDate { get; set; }

        public DateTime ResumeDate { get; set; }
        public bool? ResumeConfirmed { get; set; }
    }
}
