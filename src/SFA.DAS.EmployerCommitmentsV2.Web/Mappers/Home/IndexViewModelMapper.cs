using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Home;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Home;

public class IndexViewModelMapper : IMapper<IndexRequest, IndexViewModel>
{
    private readonly ICommitmentsApiClient _commitmentsApiClient;
    private readonly ILogger<IndexViewModelMapper> _logger;

    public IndexViewModelMapper(ICommitmentsApiClient commitmentsApiClient, ILogger<IndexViewModelMapper> logger)
    {
        _commitmentsApiClient = commitmentsApiClient;
        _logger = logger;
    }

    public async Task<IndexViewModel> Map(IndexRequest source)
    {
        try
        {
            var response = await _commitmentsApiClient.GetProviderPaymentsPriority(source.AccountId);
            return new IndexViewModel
            {
                AccountHashedId = source.AccountHashedId,
                ShowSetPaymentOrderLink = response.ProviderPaymentPriorities.Count > 1
            };
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, "Unable to map index view model");
            throw;
        }
    }
}