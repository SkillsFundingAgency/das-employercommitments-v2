using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.EmployerCommitmentsV2.Web.Services;
using System;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort;

public class SelectLegalEntityRequestToSelectLegalEntityViewModelMapper : IMapper<SelectLegalEntityRequest, SelectLegalEntityViewModel>     
{
    private readonly IEmployerAccountsService _employerAccountsService;

    public SelectLegalEntityRequestToSelectLegalEntityViewModelMapper(IEmployerAccountsService employerAccountsService)
    {           
        _employerAccountsService = employerAccountsService;
    }

    public async Task<SelectLegalEntityViewModel> Map(SelectLegalEntityRequest source)
    {
        var legalEntities = await _employerAccountsService.GetLegalEntitiesForAccount(source.AccountHashedId);

        return new SelectLegalEntityViewModel {
            AccountHashedId = source.AccountHashedId,
            TransferConnectionCode = source.transferConnectionCode,
            LegalEntities = legalEntities,
            CohortRef = string.IsNullOrWhiteSpace(source.cohortRef) ? Guid.NewGuid().ToString().ToUpper() : source.cohortRef,
            EncodedPledgeApplicationId = source.EncodedPledgeApplicationId
        };
    }        
}