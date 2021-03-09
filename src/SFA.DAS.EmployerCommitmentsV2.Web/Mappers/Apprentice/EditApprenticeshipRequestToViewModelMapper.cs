using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Extensions;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using System.Threading;
using System.Threading.Tasks;


namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice
{
    public class EditApprenticeshipRequestToViewModelMapper : IMapper<EditApprenticeshipRequest, EditApprenticeshipRequestViewModel>
    {
        private readonly ICommitmentsApiClient _commitmentsApiClient;

        public EditApprenticeshipRequestToViewModelMapper(ICommitmentsApiClient commitmentsApiClient)
        {
            _commitmentsApiClient = commitmentsApiClient;
        }
        public async Task<EditApprenticeshipRequestViewModel> Map(EditApprenticeshipRequest source)
        {
            var apprenticeship = await _commitmentsApiClient.GetApprenticeship(source.ApprenticeshipId, CancellationToken.None);
            var priceEpisodes = await _commitmentsApiClient.GetPriceEpisodes(source.ApprenticeshipId, CancellationToken.None);
            var courses = await _commitmentsApiClient.GetAllTrainingProgrammes(CancellationToken.None);

            var result = new EditApprenticeshipRequestViewModel(apprenticeship.DateOfBirth, apprenticeship.StartDate, apprenticeship.EndDate)
            {
                FirstName = apprenticeship.FirstName,
                LastName = apprenticeship.LastName,
                ULN= apprenticeship.Uln,
                CourseCode = apprenticeship.CourseCode,
                Cost = priceEpisodes.PriceEpisodes.GetPrice(),
                Reference = apprenticeship.EmployerReference,
                Courses = courses.TrainingProgrammes
            };

            return result;
        }
    }
}
