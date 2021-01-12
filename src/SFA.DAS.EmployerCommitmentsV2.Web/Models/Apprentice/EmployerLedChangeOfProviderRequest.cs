
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Authorization.ModelBinding;
using System;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice
{
    public class EmployerLedChangeOfProviderRequest : IAuthorizationContextModel
    {
        [FromRoute]
        public string AccountHashedId { get; set; }
        [FromRoute]
        public string ApprenticeshipHashedId { get; set; }

        public long ApprenticeshipId { get; set; }
        public long ProviderId { get; set; }
        public string ProviderName { get; set; }

        public int? NewStartMonth { get; set; }
        public int? NewStartYear { get; set; }
        public int? NewEndMonth { get; set; }
        public int? NewEndYear { get; set; }
        public int? NewPrice { get; set; }
        public string NewStartDate => new DateTime(NewStartYear.Value, NewStartMonth.Value, 1).ToString();
        public string NewEndDate => new DateTime(NewEndYear.Value, NewEndMonth.Value, 1).ToString();
        public bool? IsEdit { get; set; }
    }
}
