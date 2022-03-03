using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Extensions;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using System.Linq;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice
{
    public class ChangeOptionViewModelToEditApprenticeshipRequestViewModelMapper : IMapper<ChangeOptionViewModel, EditApprenticeshipRequestViewModel>
    {
        private readonly ICommitmentsApiClient _commitmentsApiClient;
        private readonly IHttpContextAccessor _httpContext;
        private readonly ITempDataDictionaryFactory _tempDataDictionaryFactory;

        public ChangeOptionViewModelToEditApprenticeshipRequestViewModelMapper(ICommitmentsApiClient commitmentsApiClient, IHttpContextAccessor httpContext, ITempDataDictionaryFactory tempDataDictionaryFactory)
        {
            _commitmentsApiClient = commitmentsApiClient;
            _httpContext = httpContext;
            _tempDataDictionaryFactory = tempDataDictionaryFactory; ;
        }

        public async Task<EditApprenticeshipRequestViewModel> Map(ChangeOptionViewModel source)
        {
            var httpContext = _httpContext.HttpContext;
            var tempData = _tempDataDictionaryFactory.GetTempData(httpContext);

            var editViewModel = tempData.GetButDontRemove<EditApprenticeshipRequestViewModel>("EditApprenticeshipRequestViewModel");

            if (editViewModel == null)
            {
                var apprenticeshipTask = _commitmentsApiClient.GetApprenticeship(source.ApprenticeshipId);
                var priceEpisodesTask = _commitmentsApiClient.GetPriceEpisodes(source.ApprenticeshipId);
                
                await Task.WhenAll(apprenticeshipTask, priceEpisodesTask);

                var apprenticeship = apprenticeshipTask.Result;
                var priceEpisodes = priceEpisodesTask.Result;

                var currentPrice = priceEpisodes.PriceEpisodes.GetPrice();

                var standardVersion = await _commitmentsApiClient.GetTrainingProgrammeVersionByCourseCodeAndVersion(apprenticeship.CourseCode, apprenticeship.Version);

                editViewModel = new EditApprenticeshipRequestViewModel(apprenticeship.DateOfBirth, apprenticeship.StartDate, apprenticeship.EndDate)
                {
                    AccountHashedId = source.AccountHashedId,
                    HashedApprenticeshipId = source.ApprenticeshipHashedId,
                    ULN = apprenticeship.Uln,
                    FirstName = apprenticeship.FirstName,
                    LastName = apprenticeship.LastName,
                    Email = apprenticeship.Email,
                    Cost = currentPrice,
                    DeliveryModel = apprenticeship.DeliveryModel,
                    CourseCode = apprenticeship.CourseCode,
                    Version = apprenticeship.Version,
                    TrainingName = apprenticeship.CourseName,
                    EmployerReference = apprenticeship.EmployerReference,
                    HasOptions = standardVersion.TrainingProgramme.Options.Any()
                };
            }

            editViewModel.Option = source.SelectedOption == "TBC" ? string.Empty : source.SelectedOption;

            return editViewModel;
        }
    }
}
