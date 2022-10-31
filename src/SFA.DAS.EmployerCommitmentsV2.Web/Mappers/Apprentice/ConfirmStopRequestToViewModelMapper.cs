using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using System;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice
{
    public class ConfirmStopRequestToViewModelMapper : IMapper<ConfirmStopRequest, ConfirmStopRequestViewModel>
    {
        private readonly ICommitmentsApiClient _client;
        public ConfirmStopRequestToViewModelMapper(ICommitmentsApiClient client)
        {
            _client = client;
        }

        public async Task<ConfirmStopRequestViewModel> Map(ConfirmStopRequest source)
        {
            var apprenticeship = await _client.GetApprenticeship(source.ApprenticeshipId);
            DateTime stoppedDate = GetStoppedDate(source, apprenticeship);

            return new ConfirmStopRequestViewModel
            {
                AccountHashedId = source.AccountHashedId,
                ApprenticeshipHashedId = source.ApprenticeshipHashedId,
                IsCoPJourney = source.IsCoPJourney,
                StopMonth = source.StopMonth,
                StopYear = source.StopYear,
                ApprenticeName = $"{apprenticeship.FirstName} {apprenticeship.LastName}",
                ULN = apprenticeship.Uln,
                Course = apprenticeship.CourseName,
                StopDate = stoppedDate,
                MadeRedundant = source.MadeRedundant
            };
        }

        private DateTime GetStoppedDate(ConfirmStopRequest source, GetApprenticeshipResponse apprenticeship)
        {
           if (apprenticeship.Status == CommitmentsV2.Types.ApprenticeshipStatus.WaitingToStart)
            {
                return apprenticeship.StartDate.Value;
            }

            return new DateTime(source.StopYear.Value, source.StopMonth.Value, 1);
        }
    }
}
