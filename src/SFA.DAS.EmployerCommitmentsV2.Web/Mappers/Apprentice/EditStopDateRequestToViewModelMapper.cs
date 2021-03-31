using Microsoft.Extensions.Logging;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.CommitmentsV2.Shared.Models;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.Encoding;
using System;
using System.Threading;
using System.Threading.Tasks;


namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice
{
    public class EditStopDateRequestToViewModelMapper : IMapper<EditStopDateRequest, EditStopDateViewModel>
    {
        private readonly ICommitmentsApiClient _commitmentsApiClient;
        private readonly IEncodingService _encodingService;
        private readonly ILogger<EditStopDateRequestToViewModelMapper> _logger;
        private readonly IAcademicYearDateProvider _academicYearDateProvider;
        private readonly ICurrentDateTime _currentDateTime;

        public EditStopDateRequestToViewModelMapper(ICommitmentsApiClient commitmentsApiClient, IEncodingService encodingService,
            IAcademicYearDateProvider academicYearDateProvider,
            ICurrentDateTime currentDateTime,
            ILogger<EditStopDateRequestToViewModelMapper> logger)
        {
            _commitmentsApiClient = commitmentsApiClient;
            _encodingService = encodingService;
            _academicYearDateProvider = academicYearDateProvider;
            _currentDateTime = currentDateTime;
            _logger = logger;
        }
        
        public async Task<EditStopDateViewModel> Map(EditStopDateRequest source)
        {
            try
            {
                //var apprenticeshipId = _encodingService.Decode(source.ApprenticeshipHashedId, EncodingType.ApprenticeshipId);
                //TODO : AcademicYearRestriction tests

                var apprenticeship = await  _commitmentsApiClient.GetApprenticeship(source.ApprenticeshipId, CancellationToken.None);

                var result = new EditStopDateViewModel
                {   
                    ApprenticeshipId = source.ApprenticeshipId, //apprenticeship.Id,
                    AccountHashedId = source.AccountHashedId,
                    ApprenticeshipULN = apprenticeship.Uln,
                    ApprenticeshipName = $"{apprenticeship.FirstName} {apprenticeship.LastName}",
                    ApprenticeshipHashedId = source.ApprenticeshipHashedId,
                    ApprenticeshipStartDate = apprenticeship.StartDate,
                    AcademicYearRestriction = _currentDateTime.UtcNow > _academicYearDateProvider.LastAcademicYearFundingPeriod ? //if r14 grace period has past for last a.y.
                    _academicYearDateProvider.CurrentAcademicYearStartDate : default(DateTime?),
                    CurrentStopDate = apprenticeship.StopDate.Value,
                    NewStopDate = new MonthYearModel("")
                };

                result.EarliestDate = result.AcademicYearRestriction.HasValue && result.AcademicYearRestriction.Value > result.ApprenticeshipStartDate ? result.AcademicYearRestriction.Value : result.ApprenticeshipStartDate;

                return result;
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error mapping for accountId {source.AccountHashedId}  and apprenticeship {source.ApprenticeshipHashedId} to EditStopDateViewModel");
                throw;
            }
        }
    }
}
