using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.Encoding;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice
{
    public class DownloadApprenticesRequestMapper : IMapper<DownloadRequest, DownloadViewModel>
    {
        private readonly ICommitmentsApiClient _client;
        private readonly ICreateCsvService _createCsvService;
        private readonly ICurrentDateTime _currentDateTime;
        private readonly IEncodingService _encodingService;

        public DownloadApprenticesRequestMapper(
            ICommitmentsApiClient client, 
            ICreateCsvService createCsvService, 
            ICurrentDateTime currentDateTime,
            IEncodingService encodingService)
        {
            _client = client;
            _createCsvService = createCsvService;
            _currentDateTime = currentDateTime;
            _encodingService = encodingService;
        }

        public async Task<DownloadViewModel> Map(DownloadRequest request)
        {
            var decodedAccountId =
                _encodingService.Decode(request.AccountHashedId, EncodingType.AccountId);

            var downloadViewModel = new DownloadViewModel();
            var getApprenticeshipsRequest = new GetApprenticeshipsRequest
            {
                AccountId = decodedAccountId,
                SearchTerm = request.SearchTerm,
                ProviderName = request.SelectedProvider,
                CourseName = request.SelectedCourse,
                Status = request.SelectedStatus,
                EndDate = request.SelectedEndDate,
                PageNumber = 1,
                PageItemCount = Constants.ApprenticesSearch.NumberOfApprenticesPerDownloadPage
            };
            
            downloadViewModel.Request = getApprenticeshipsRequest;
            downloadViewModel.GetAndCreateContent = Handler;
            downloadViewModel.Dispose = DisposeService;
            downloadViewModel.Name = $"{"Manageyourapprentices"}_{_currentDateTime.UtcNow:yyyyMMddhhmmss}.csv";
            return await Task.FromResult(downloadViewModel);
        }

        public async Task<MemoryStream> Handler(GetApprenticeshipsRequest getApprenticeshipsRequest)
        {
            var result = await _client.GetApprenticeships(getApprenticeshipsRequest);

            var totalAllowedPages = Math.Ceiling((decimal)result.TotalApprenticeshipsFound / getApprenticeshipsRequest.PageItemCount);

            if (getApprenticeshipsRequest.PageNumber > totalAllowedPages)
            {
                return new MemoryStream();
            }

            var csvContent = result.Apprenticeships.Select(c => (ApprenticeshipDetailsCsvModel)c).ToList();

            return _createCsvService.GenerateCsvContent(csvContent, getApprenticeshipsRequest.PageNumber == 1);
        }

        public bool DisposeService()
        {
            _createCsvService.Dispose();

            return true;
        }
    }
}