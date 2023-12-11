using Microsoft.Extensions.Logging;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;


namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice;

public class EditStopDateRequestToViewModelMapper : IMapper<EditStopDateRequest, EditStopDateViewModel>
{
    private readonly ICommitmentsApiClient _commitmentsApiClient;        
    private readonly ILogger<EditStopDateRequestToViewModelMapper> _logger;       

    public EditStopDateRequestToViewModelMapper(ICommitmentsApiClient commitmentsApiClient, ILogger<EditStopDateRequestToViewModelMapper> logger)
    {
        _commitmentsApiClient = commitmentsApiClient;           
        _logger = logger;
    }
        
    public async Task<EditStopDateViewModel> Map(EditStopDateRequest source)
    {
        try
        {               
            var apprenticeship = await  _commitmentsApiClient.GetApprenticeship(source.ApprenticeshipId, CancellationToken.None);

            var result = new EditStopDateViewModel
            {   
                ApprenticeshipId = source.ApprenticeshipId,
                AccountHashedId = source.AccountHashedId,
                ApprenticeshipULN = apprenticeship.Uln,                    
                ApprenticeshipHashedId = source.ApprenticeshipHashedId,
                ApprenticeshipStartDate = apprenticeship.StartDate.Value,
                CurrentStopDate = apprenticeship.StopDate.Value
            };

            return result;
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Error mapping for accountId {source.AccountHashedId}  and apprenticeship {source.ApprenticeshipHashedId} to EditStopDateViewModel");
            throw;
        }
    }
}