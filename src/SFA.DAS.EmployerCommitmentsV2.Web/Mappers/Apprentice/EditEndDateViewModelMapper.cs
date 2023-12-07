using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice;

public class EditEndDateViewModelMapper : IMapper<EditEndDateRequest, EditEndDateViewModel>
{
    public Task<EditEndDateViewModel> Map(EditEndDateRequest source)
    {
        var result = new EditEndDateViewModel
        {
            AccountHashedId = source.AccountHashedId,
            ApprenticeshipHashedId = source.ApprenticeshipHashedId,
        };

        return Task.FromResult(result);
    }
}