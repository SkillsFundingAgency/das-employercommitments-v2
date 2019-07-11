using System.Collections.Generic;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.CreateCohort
{
    public class ConfirmProviderRequest : SelectProviderRequest
    {
        public long ProviderId { get; set; }

        public override Dictionary<string, string> ToDictionary()
        {
            var result = base.ToDictionary();

            result.Add("ProviderId", ProviderId.ToString());

            return result;
        }
    }
}