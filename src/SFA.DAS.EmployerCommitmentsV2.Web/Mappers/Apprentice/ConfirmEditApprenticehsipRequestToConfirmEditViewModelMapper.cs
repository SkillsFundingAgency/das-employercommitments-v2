using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Extensions;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.Encoding;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice
{
    public class ConfirmEditApprenticehsipRequestToConfirmEditViewModelMapper : IMapper<EditApprenticeshipRequestViewModel, ConfirmEditApprenticeshipViewModel>
    {
        private readonly ICommitmentsApiClient _commitmentApi;
        private readonly IEncodingService _encodingService;

        public ConfirmEditApprenticehsipRequestToConfirmEditViewModelMapper(ICommitmentsApiClient commitmentsApi, IEncodingService encodingService)
        {
            _commitmentApi = commitmentsApi;
            _encodingService = encodingService;
        }

        public async Task<ConfirmEditApprenticeshipViewModel> Map(EditApprenticeshipRequestViewModel source)
        {
            source.ApprenticeshipId = _encodingService.Decode(source.HashedApprenticeshipId, EncodingType.ApprenticeshipId);
            source.AccountId = _encodingService.Decode(source.AccountHashedId, EncodingType.AccountId);

            var apprenticehsipTask = _commitmentApi.GetApprenticeship(source.ApprenticeshipId);
            var priceEpisodesTask = _commitmentApi.GetPriceEpisodes(source.ApprenticeshipId);

            await Task.WhenAll(apprenticehsipTask, priceEpisodesTask);

            var apprenticeship = apprenticehsipTask.Result;
            var priceEpisodes = priceEpisodesTask.Result;

            GetTrainingProgrammeResponse courseDetails;
            var courseDetailsOriginal = await _commitmentApi.GetTrainingProgramme(apprenticeship.CourseCode);
            var currentPrice = priceEpisodes.PriceEpisodes.GetPrice();

            if (apprenticeship.CourseCode == source.CourseCode)
            {
                courseDetails = courseDetailsOriginal;
            }
            else
            {
                courseDetails = await _commitmentApi.GetTrainingProgramme(source.CourseCode);
            }

            var vm = new ConfirmEditApprenticeshipViewModel
            {
                AccountHashedId = source.AccountHashedId,
                ApprenticeshipHashedId = source.HashedApprenticeshipId,
                OriginalApprenticeship = new ConfirmEditApprenticeshipViewModel()
                {
                    ULN = apprenticeship.Uln
                }
            };

            if (source.FirstName != apprenticeship.FirstName || source.LastName != apprenticeship.LastName)
            {
                vm.FirstName = source.FirstName;
                vm.LastName = source.LastName;
            }

            vm.OriginalApprenticeship.FirstName = apprenticeship.FirstName;
            vm.OriginalApprenticeship.LastName = apprenticeship.LastName;

            if (source.DateOfBirth.Date != apprenticeship.DateOfBirth)
            {
                vm.BirthDay =   source.BirthDay;
                vm.BirthMonth = source.BirthMonth;
                vm.BirthYear =  source.BirthYear;
            }
            vm.OriginalApprenticeship.BirthDay = apprenticeship.DateOfBirth.Day;
            vm.OriginalApprenticeship.BirthMonth = apprenticeship.DateOfBirth.Month;
            vm.OriginalApprenticeship.BirthYear = apprenticeship.DateOfBirth.Year;

            if (source.Cost != currentPrice)
            {
                vm.Cost = source.Cost;
            }
            vm.OriginalApprenticeship.Cost = currentPrice;

            
            vm.EmployerReference = source.EmployerReference;
            vm.OriginalApprenticeship.EmployerReference = apprenticeship.EmployerReference;

            if (source.StartDate.Date != apprenticeship.StartDate)
            {
                vm.StartMonth = source.StartMonth;
                vm.StartYear = source.StartYear;
            }
            vm.OriginalApprenticeship.StartMonth = apprenticeship.StartDate.Month;
            vm.OriginalApprenticeship.StartYear = apprenticeship.StartDate.Year;

            if (source.EndDate.Date != apprenticeship.EndDate)
            {
                vm.EndMonth = source.EndMonth;
                vm.EndYear = source.EndYear;
            }
            vm.OriginalApprenticeship.EndMonth = apprenticeship.EndDate.Month;
            vm.OriginalApprenticeship.EndYear = apprenticeship.EndDate.Year;

            if (source.CourseCode != apprenticeship.CourseCode)
            {
                vm.CourseCode = source.CourseCode;
                vm.TrainingName = courseDetails?.TrainingProgramme.Name;
            }
            vm.OriginalApprenticeship.CourseCode = apprenticeship.CourseCode;
            vm.OriginalApprenticeship.TrainingName = courseDetailsOriginal?.TrainingProgramme.Name;

            return vm;
        }
    }
}
