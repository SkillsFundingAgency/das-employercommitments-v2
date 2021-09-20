using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Extensions;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.Encoding;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice
{
    public class ConfirmEditApprenticeshipRequestToConfirmEditViewModelMapper : IMapper<EditApprenticeshipRequestViewModel, ConfirmEditApprenticeshipViewModel>
    {
        private readonly ICommitmentsApiClient _commitmentApi;
        private readonly IEncodingService _encodingService;

        public ConfirmEditApprenticeshipRequestToConfirmEditViewModelMapper(ICommitmentsApiClient commitmentsApi, IEncodingService encodingService)
        {
            _commitmentApi = commitmentsApi;
            _encodingService = encodingService;
        }

        public async Task<ConfirmEditApprenticeshipViewModel> Map(EditApprenticeshipRequestViewModel source)
        {
            source.ApprenticeshipId = _encodingService.Decode(source.HashedApprenticeshipId, EncodingType.ApprenticeshipId);
            source.AccountId = _encodingService.Decode(source.AccountHashedId, EncodingType.AccountId);

            var apprenticeshipTask = _commitmentApi.GetApprenticeship(source.ApprenticeshipId);
            var priceEpisodesTask = _commitmentApi.GetPriceEpisodes(source.ApprenticeshipId);

            await Task.WhenAll(apprenticeshipTask, priceEpisodesTask);

            var apprenticeship = apprenticeshipTask.Result;
            var priceEpisodes = priceEpisodesTask.Result;

            var currentPrice = priceEpisodes.PriceEpisodes.GetPrice();

            var vm = new ConfirmEditApprenticeshipViewModel
            {
                AccountHashedId = source.AccountHashedId,
                ApprenticeshipHashedId = source.HashedApprenticeshipId,
                OriginalApprenticeship = new ConfirmEditApprenticeshipViewModel()
                {
                    ULN = apprenticeship.Uln
                },
                ProviderName = apprenticeship.ProviderName
            };

            if (source.FirstName != apprenticeship.FirstName || source.LastName != apprenticeship.LastName)
            {
                vm.FirstName = source.FirstName;
                vm.LastName = source.LastName;
            }
            vm.OriginalApprenticeship.FirstName = apprenticeship.FirstName;
            vm.OriginalApprenticeship.LastName = apprenticeship.LastName;

            if (source.Email != apprenticeship.Email)
            {
                vm.Email = source.Email;
            }
            vm.OriginalApprenticeship.Email = apprenticeship.Email;

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

            if (source.Version != apprenticeship.Version || source.CourseCode != apprenticeship.CourseCode)
            {
                vm.Version = source.Version;
            }
            vm.OriginalApprenticeship.Version = apprenticeship.Version;

            if (source.CourseCode != apprenticeship.CourseCode)
            {
                var courseDetails = !string.IsNullOrEmpty(source.Version)
                       ? await _commitmentApi.GetTrainingProgrammeVersionByCourseCodeAndVersion(source.CourseCode, source.Version)
                       : await _commitmentApi.GetTrainingProgramme(source.CourseCode);
                vm.CourseCode = source.CourseCode;
                vm.CourseName = courseDetails?.TrainingProgramme.Name;
            }
            vm.OriginalApprenticeship.CourseCode = apprenticeship.CourseCode;
            vm.OriginalApprenticeship.CourseName = apprenticeship.CourseName;

            if (source.Option != apprenticeship.Option)
            {
                vm.Option = source.Option;
            }
            vm.OriginalApprenticeship.Option = apprenticeship.Option;

            if (source.HasOptions)
            {
                vm.ReturnToChangeOption = source.HasOptions;
            }
            else
            {
                vm.ReturnToChangeVersion = !string.IsNullOrEmpty(vm.Version) && string.IsNullOrEmpty(vm.CourseCode) && !vm.StartDate.HasValue;
            }

            return vm;
        }
    }
}
