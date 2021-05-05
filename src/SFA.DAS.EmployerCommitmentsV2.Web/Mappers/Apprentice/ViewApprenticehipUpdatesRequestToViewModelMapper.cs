using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Extensions;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice.Edit;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice
{
    public class ViewApprenticehipUpdatesRequestToViewModelMapper : IMapper<ViewApprenticehipUpdatesRequest, ViewApprenticeshipUpdatesRequestViewModel>
    {
        private readonly ICommitmentsApiClient _commitmentsApiClient;

        public ViewApprenticehipUpdatesRequestToViewModelMapper(ICommitmentsApiClient commitmentsApiClient)
        {
            _commitmentsApiClient = commitmentsApiClient;
        }

        public async Task<ViewApprenticeshipUpdatesRequestViewModel> Map(ViewApprenticehipUpdatesRequest source)
        {
            var updates = await _commitmentsApiClient.GetApprenticeshipUpdates(source.ApprenticeshipId,
                   new CommitmentsV2.Api.Types.Requests.GetApprenticeshipUpdatesRequest { Status = CommitmentsV2.Types.ApprenticeshipUpdateStatus.Pending });

            var apprenticeship = await _commitmentsApiClient.GetApprenticeship(source.ApprenticeshipId);

            if (updates.ApprenticeshipUpdates.Count == 1)
            {
                var update = updates.ApprenticeshipUpdates.First();

                var ApprenticeshipUpdates = GetApprenticeshipUpdates(update);
                var OriginalApprenticeship = await GetOriginalApprenticeship(apprenticeship, update.Cost.HasValue);

                return new ViewApprenticeshipUpdatesRequestViewModel
                {
                    ApprenticeshipUpdates = ApprenticeshipUpdates,
                    OriginalApprenticeship = OriginalApprenticeship,
                    ProviderName = apprenticeship.ProviderName,
                    AccountHashedId = source.AccountHashedId,
                    ApprenticeshipHashedId = source.ApprenticeshipHashedId
                };
            }

            throw new Exception("Multiple pending updates found");
        }

        private static BaseEdit GetApprenticeshipUpdates(GetApprenticeshipUpdatesResponse.ApprenticeshipUpdate update)
        {
            return new BaseEdit
            {
                FirstName = update.FirstName,
                LastName = update.LastName,
                DateOfBirth = update.DateOfBirth,
                Cost = update.Cost,
                StartDate = update.StartDate,
                EndDate = update.EndDate,
                CourseCode = update.TrainingCode,
                TrainingName = update.TrainingName,
            };
        }

        private async Task<BaseEdit> GetOriginalApprenticeship(GetApprenticeshipResponse apprenticeship, bool costChanged)
        {
            var OriginalApprenticeship = new BaseEdit
            {
                FirstName = apprenticeship.FirstName,
                LastName = apprenticeship.LastName,
                DateOfBirth = apprenticeship.DateOfBirth,
                ULN = apprenticeship.Uln,
                StartDate = apprenticeship.StartDate,
                EndDate = apprenticeship.EndDate,
                CourseCode = apprenticeship.CourseCode
            };

            var courseDetailsOriginal = await _commitmentsApiClient.GetTrainingProgramme(apprenticeship.CourseCode);
            OriginalApprenticeship.TrainingName = courseDetailsOriginal?.TrainingProgramme.Name;

            if (costChanged)
            {
                var priceEpisodes = await _commitmentsApiClient.GetPriceEpisodes(apprenticeship.Id);
                OriginalApprenticeship.Cost = priceEpisodes.PriceEpisodes.GetPrice();
            }

            return OriginalApprenticeship;
        }
    }
}
