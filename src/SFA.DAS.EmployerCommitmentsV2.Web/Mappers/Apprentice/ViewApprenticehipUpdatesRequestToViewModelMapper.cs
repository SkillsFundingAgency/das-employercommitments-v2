using SFA.DAS.CommitmentsV2.Api.Client;
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

                var vm = new ViewApprenticeshipUpdatesRequestViewModel
                {
                    ApprenticeshipUpdates = new BaseEdit
                    {
                        FirstName = update.FirstName,
                        LastName = update.LastName,
                        DateOfBirth = update.DateOfBirth,
                        Cost = update.Cost,
                        StartDate = update.StartDate,
                        EndDate = update.EndDate,
                        CourseCode = update.TrainingCode,
                        TrainingName = update.TrainingName,
                    },
                    OriginalApprenticeship = new BaseEdit
                    {
                        AccountHashedId = source.AccountHashedId,
                        ApprenticeshipHashedId = source.ApprenticeshipHashedId,
                        FirstName = apprenticeship.FirstName,
                        LastName = apprenticeship.LastName,
                        DateOfBirth = apprenticeship.DateOfBirth,
                        ULN = apprenticeship.Uln,
                        StartDate = apprenticeship.StartDate,
                        EndDate = apprenticeship.EndDate,
                        CourseCode = apprenticeship.CourseCode
                    },
                    ProviderName = apprenticeship.ProviderName,
                };

                if (update.Cost.HasValue)
                {
                    var priceEpisodes = await _commitmentsApiClient.GetPriceEpisodes(source.ApprenticeshipId);
                    vm.OriginalApprenticeship.Cost = priceEpisodes.PriceEpisodes.GetPrice();
                }

                if (!string.IsNullOrWhiteSpace(update.TrainingCode))
                {
                    var courseDetailsOriginal = await _commitmentsApiClient.GetTrainingProgramme(apprenticeship.CourseCode);
                    vm.OriginalApprenticeship.TrainingName = courseDetailsOriginal?.TrainingProgramme.Name;
                }

                return vm;
            }

            throw new Exception("Multiple pending updates found");
        }
    }
}
