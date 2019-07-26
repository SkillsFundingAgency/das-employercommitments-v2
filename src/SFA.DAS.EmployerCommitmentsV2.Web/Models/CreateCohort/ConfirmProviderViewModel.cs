using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.CreateCohort
{
    public class ConfirmProviderViewModel : IndexViewModel
    {
        public long ProviderId { get; set; }
        public string ProviderName { get; set; }

        [Required(ErrorMessage = "Select a training provider")]
        public bool? UseThisProvider { get; set; }

        public override Dictionary<string, string> ToDictionary()
        {
            var result = base.ToDictionary();

            result.Add("ProviderId", ProviderId.ToString());

            return result;
        }
    }
}