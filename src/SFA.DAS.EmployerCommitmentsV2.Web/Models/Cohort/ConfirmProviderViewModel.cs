using System.Collections.Generic;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort
{
    public class ConfirmProviderViewModel : IndexViewModel
    {
        public long ProviderId { get; set; }
        public string ProviderName { get; set; }
        public string LegalEntityName { get; set; }

        public bool? UseThisProvider { get; set; }
        public string TransferSenderId { get; set; }

        public override Dictionary<string, string> ToDictionary()
        {
            var result = base.ToDictionary();

            result.Add("ProviderId", ProviderId.ToString());

            return result;
        }
    }
}