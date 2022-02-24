using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Extensions;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice.Edit;
using System;
using System.Linq;
using System.Threading.Tasks;
using SFA.DAS.CommitmentsV2.Types;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice
{
    public class ViewApprenticehipUpdatesRequestToViewModelMapper : IMapper<ViewApprenticeshipUpdatesRequest, ViewApprenticeshipUpdatesViewModel>
    {
        private readonly ICommitmentsApiClient _commitmentsApiClient;

        public ViewApprenticehipUpdatesRequestToViewModelMapper(ICommitmentsApiClient commitmentsApiClient)
        {
            _commitmentsApiClient = commitmentsApiClient;
        }

        public async Task<ViewApprenticeshipUpdatesViewModel> Map(ViewApprenticeshipUpdatesRequest source)
        {
            var updatesTask = _commitmentsApiClient.GetApprenticeshipUpdates(source.ApprenticeshipId,
                   new CommitmentsV2.Api.Types.Requests.GetApprenticeshipUpdatesRequest { Status = CommitmentsV2.Types.ApprenticeshipUpdateStatus.Pending });

            var apprenticeshipTask = _commitmentsApiClient.GetApprenticeship(source.ApprenticeshipId);

            await Task.WhenAll(updatesTask, apprenticeshipTask);

            var updates = updatesTask.Result;
            var apprenticeship = apprenticeshipTask.Result;

            if (updates.ApprenticeshipUpdates.Count == 1)
            {
                var update = updates.ApprenticeshipUpdates.First();

                var apprenticeshipUpdates = GetApprenticeshipUpdates(update);
                var originalApprenticeship = await GetOriginalApprenticeship(apprenticeship, update.Cost.HasValue);

                return new ViewApprenticeshipUpdatesViewModel
                {
                    ApprenticeshipUpdates = apprenticeshipUpdates,
                    OriginalApprenticeship = originalApprenticeship,
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
                Email = update.Email,
                DateOfBirth = update.DateOfBirth,
                Cost = update.Cost,
                StartDate = update.StartDate,
                EndDate = update.EndDate,
                DeliveryModel = update.DeliveryModel,
                CourseCode = update.TrainingCode,
                CourseName = update.TrainingName,
                Version = update.Version,
                Option = update.Option
            };
        }

        private async Task<BaseEdit> GetOriginalApprenticeship(GetApprenticeshipResponse apprenticeship, bool costChanged)
        {
            var OriginalApprenticeship = new BaseEdit
            {
                FirstName = apprenticeship.FirstName,
                LastName = apprenticeship.LastName,
                Email = apprenticeship.Email,
                DateOfBirth = apprenticeship.DateOfBirth,
                ULN = apprenticeship.Uln,
                StartDate = apprenticeship.StartDate,
                EndDate = apprenticeship.EndDate,
                DeliveryModel = apprenticeship.DeliveryModel.Code,
                CourseCode = apprenticeship.CourseCode,
                CourseName = apprenticeship.CourseName,
                Version = apprenticeship.Version,
                Option = apprenticeship.Option
            };

            if (costChanged)
            {
                var priceEpisodes = await _commitmentsApiClient.GetPriceEpisodes(apprenticeship.Id);
                OriginalApprenticeship.Cost = priceEpisodes.PriceEpisodes.GetPrice();
            }

            return OriginalApprenticeship;
        }
    }
}
