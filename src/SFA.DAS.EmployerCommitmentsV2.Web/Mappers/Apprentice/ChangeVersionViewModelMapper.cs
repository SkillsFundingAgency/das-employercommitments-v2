using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice
{
    public class ChangeVersionViewModelMapper : IMapper<ChangeVersionRequest, ChangeVersionViewModel>
    {
        private readonly ICommitmentsApiClient _commitmentsApiClient;
        
        public ChangeVersionViewModelMapper(ICommitmentsApiClient commitmentsApiClient)
        {
            _commitmentsApiClient = commitmentsApiClient;
        }

        public async Task<ChangeVersionViewModel> Map(ChangeVersionRequest source)
        {
            var apprenticeship = await _commitmentsApiClient.GetApprenticeship(source.ApprenticeshipId);

            var standardVersions = await _commitmentsApiClient.GetTrainingProgrammeVersions(apprenticeship.CourseCode);

            var currentVersion = standardVersions.TrainingProgrammeVersions.FirstOrDefault(v => v.StandardUId == apprenticeship.StandardUId);

            var newerVersions = standardVersions.TrainingProgrammeVersions.Where(v => float.Parse(v.Version) > float.Parse(currentVersion.Version));

            return new ChangeVersionViewModel
            {
                CurrentVersion = apprenticeship.Version,
                StandardTitle = currentVersion.Name,
                StandardUrl = currentVersion.StandardPageUrl,
                NewerVersions = newerVersions.Select(x => x.Version)
            };
        }
    }
}
