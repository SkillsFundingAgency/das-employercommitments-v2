using SFA.DAS.Authorization.ModelBinding;
using SFA.DAS.CommitmentsV2.Shared.Models;
using System;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice
{
    public class WhatIsTheNewPriceViewModel : IAuthorizationContextModel
    {
        //public WhatIsTheNewPriceViewModel()
        //{
        //    NewStartDate = new MonthYearModel("");
        //    NewEndDate = new MonthYearModel("");
        //}

        public string AccountHashedId { get; set; }
        public string ApprenticeshipHashedId { get; set; }
        public string ProviderName { get; set; }
        public long ProviderId { get; set; }
        public DateTime StopDate { get; set; }
        //public DateTime NewStartDate { get; set; }
        //public DateTime NewEndDate { get; set; }
        public string NewStartDate { get; set; }
        public string NewEndDate { get; set; }
        //public MonthYearModel NewStartDate { get; }
        //public MonthYearModel NewEndDate { get; }
        public bool Edit { get; set; }
        //public int? NewStartMonth { get => NewStartDate.Month; set => NewStartDate.Month = value; }
        //public int? NewStartYear { get => NewStartDate.Year; set => NewStartDate.Year = value; }
        //public int? NewEndMonth { get => NewEndDate.Month; set => NewEndDate.Month = value; }
        //public int? NewEndYear { get => NewEndDate.Year; set => NewEndDate.Year = value; }
        public int? NewPrice { get; set; }
    }
}
