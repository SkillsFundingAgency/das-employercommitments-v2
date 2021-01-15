using System;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice
{
    public class ConfirmDetailsAndSendViewModel : ChangeOfProviderBaseViewModel
    {
        public string ApprenticeFullName { get; set; }
        public DateTime? ApprenticeshipStopDate { get; set; }
        public string CurrentProviderName { get; set; }
        public DateTime CurrentStartDate { get; set; }
        public DateTime CurrentEndDate { get; set; }
        public int CurrentPrice { get; set; }
        public int MaxFunding { get; set; }
        public DateTime NewStartDate { get; set; }
        public DateTime NewEndDate { get; set; }
    }
}
