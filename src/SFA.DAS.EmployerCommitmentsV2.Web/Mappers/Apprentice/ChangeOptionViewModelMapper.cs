using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Extensions;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice
{
    public class ChangeOptionViewModelMapper : IMapper<ChangeOptionRequest, ChangeOptionViewModel>
    {
        private readonly ICommitmentsApiClient _commitmentsApiClient;
        private readonly IHttpContextAccessor _httpContext;
        private readonly ITempDataDictionaryFactory _tempDataDictionaryFactory;

        public ChangeOptionViewModelMapper(ICommitmentsApiClient commitmentsApiClient, IHttpContextAccessor httpContext, ITempDataDictionaryFactory tempDataDictionaryFactory)
        {
            _commitmentsApiClient = commitmentsApiClient;
            _httpContext = httpContext;
            _tempDataDictionaryFactory = tempDataDictionaryFactory;;
        }

        public async Task<ChangeOptionViewModel> Map(ChangeOptionRequest source)
        {
            var apprenticeship = await _commitmentsApiClient.GetApprenticeship(source.ApprenticeshipId);

            var tempData = _tempDataDictionaryFactory.GetTempData(_httpContext.HttpContext);

            var editViewModel = tempData.GetButDontRemove<EditApprenticeshipRequestViewModel>("EditApprenticeshipRequestViewModel");

            string selectedVersion;
            string selectedCourseCode;
            string selectedOption;

            bool returnToChangeVersion = false;
            bool returnToEdit = false;

            if (editViewModel != null)
            {
                selectedCourseCode = editViewModel.CourseCode;
                selectedVersion = editViewModel.Version;
                selectedOption = editViewModel.Option;

                if (selectedCourseCode != apprenticeship.CourseCode || editViewModel.StartDate.Date.Value != apprenticeship.StartDate.Date)
                {
                    returnToEdit = true; 
                }
                else if (selectedVersion != apprenticeship.Version)
                {
                    returnToChangeVersion = true;
                }
            }
            else
            {
                selectedCourseCode = apprenticeship.CourseCode;
                selectedVersion = apprenticeship.Version;
                selectedOption = apprenticeship.Option;
            }

            var standardVersion = await _commitmentsApiClient.GetTrainingProgrammeVersionByCourseCodeAndVersion(selectedCourseCode, selectedVersion);
            
            return new ChangeOptionViewModel
            {
                AccountHashedId = source.AccountHashedId,
                ApprenticeshipHashedId = source.ApprenticeshipHashedId,
                CurrentOption = apprenticeship.Option == string.Empty ? "N/A" : apprenticeship.Option,
                SelectedVersion = selectedVersion,
                SelectedOption = selectedOption,
                SelectedVersionName = standardVersion.TrainingProgramme.Name,
                SelectedVersionUrl = standardVersion.TrainingProgramme.StandardPageUrl,
                Options = standardVersion.TrainingProgramme.Options,
                ReturnToChangeVersion = returnToChangeVersion,
                ReturnToEdit = returnToEdit
            };
        }
    }
}
