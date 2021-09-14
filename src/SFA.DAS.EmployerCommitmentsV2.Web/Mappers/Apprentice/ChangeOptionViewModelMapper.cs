using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice
{
    public class ChangeOptionViewModelMapper : IMapper<EditApprenticeshipRequestViewModel, ChangeOptionViewModel>
    {
        private readonly ICommitmentsApiClient _commitmentsApiClient;

        public ChangeOptionViewModelMapper(ICommitmentsApiClient commitmentsApiClient)
        {
            _commitmentsApiClient = commitmentsApiClient;
        }

        public async Task<ChangeOptionViewModel> Map(EditApprenticeshipRequestViewModel source)
        {
            var selectedVersion = await _commitmentsApiClient.GetTrainingProgrammeVersionByCourseCodeAndVersion(source.CourseCode, source.Version);

            return new ChangeOptionViewModel
            {
                SelectedVersion = source.Version,
                SelectedVersionName = selectedVersion.TrainingProgramme.Name,
                SelectedVersionUrl = selectedVersion.TrainingProgramme.StandardPageUrl,
                Options = selectedVersion.TrainingProgramme.Options
            };
        }
    }
}
