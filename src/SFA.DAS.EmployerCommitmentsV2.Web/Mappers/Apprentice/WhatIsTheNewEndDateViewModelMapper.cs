using System;
using System.Threading.Tasks;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice
{
    public class WhatIsTheNewEndDateViewModelMapper : IMapper<EmployerLedChangeOfProviderRequest,WhatIsTheNewEndDateViewModel>
    {
        public Task<WhatIsTheNewEndDateViewModel> Map(EmployerLedChangeOfProviderRequest source)
        {
            return Task.FromResult(new WhatIsTheNewEndDateViewModel
            {
                AccountHashedId = source.AccountHashedId,
                ApprenticeshipHashedId = source.ApprenticeshipHashedId,
                ProviderName = source.ProviderName,
                ProviderId = source.ProviderId,
                NewStartMonth = source.NewStartMonth,
                NewStartYear = source.NewStartYear,
                NewStartDate = new DateTime(source.NewStartYear.Value, source.NewStartMonth.Value, 1),
                Edit = source.Edit ?? false
            });
        }
    }
}
