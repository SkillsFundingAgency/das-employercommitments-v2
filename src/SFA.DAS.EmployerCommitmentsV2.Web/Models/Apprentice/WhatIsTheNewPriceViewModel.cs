using SFA.DAS.Authorization.ModelBinding;
using SFA.DAS.CommitmentsV2.Shared.Models;
using System;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice
{
    public class WhatIsTheNewPriceViewModel : IAuthorizationContextModel
    {        
        public string AccountHashedId { get; set; }
        public string ApprenticeshipHashedId { get; set; }
        public string ProviderName { get; set; }
        public long ProviderId { get; set; }
        public DateTime StopDate { get; set; }     
        public string NewStartDate { get; set; }
        public string NewEndDate { get; set; }        
        public bool Edit { get; set; }        
        public int? NewPrice { get; set; }
    }
}
