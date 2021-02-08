using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Extensions;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using System;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.CommitmentsV2.Types;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice
{
    public class ConfirmDetailsAndSendViewModelMapper : IMapper<ChangeOfProviderRequest, ConfirmDetailsAndSendViewModel>
    {
        private readonly ICommitmentsApiClient _commitmentsApiClient;

        public ConfirmDetailsAndSendViewModelMapper(ICommitmentsApiClient commitmentsApiClient)
        {
            _commitmentsApiClient = commitmentsApiClient;
        }

        public async Task<ConfirmDetailsAndSendViewModel> Map(ChangeOfProviderRequest source)
        {
            var apprenticeship = await _commitmentsApiClient.GetApprenticeship(source.ApprenticeshipId.Value, CancellationToken.None);
            var priceHistory = await _commitmentsApiClient.GetPriceEpisodes(source.ApprenticeshipId.Value, CancellationToken.None);

            var course = await _commitmentsApiClient.GetTrainingProgramme(apprenticeship.CourseCode);
            var newStartDate = new DateTime(source.NewStartYear.Value, source.NewStartMonth.Value, 1);

            var result = new ConfirmDetailsAndSendViewModel
            {
                AccountHashedId = source.AccountHashedId,
                ApprenticeshipHashedId = source.ApprenticeshipHashedId,
                ProviderId = source.ProviderId.Value,
                ProviderName = source.ProviderName,
                NewStartDate = newStartDate,
                NewEndDate = new DateTime(source.NewEndYear.Value, source.NewEndMonth.Value, 1),
                NewPrice = source.NewPrice,
                ApprenticeFullName = $"{apprenticeship.FirstName} {apprenticeship.LastName}",
                ApprenticeshipStopDate = apprenticeship.StopDate,
                CurrentProviderName = apprenticeship.ProviderName,
                CurrentStartDate = apprenticeship.StartDate,
                CurrentEndDate = apprenticeship.EndDate,
                CurrentPrice = decimal.ToInt32(priceHistory.PriceEpisodes.GetPrice()),
                EmployerWillAdd = source.EmployerWillAdd,
                MaxFunding = GetFundingBandCap(course.TrainingProgramme, newStartDate),
            };

            return result;
        }

        private int? GetFundingBandCap(TrainingProgramme course, DateTime? startDate)
        {
            if (course == null)
            {
                return null;
            }

            var cap = course.FundingCapOn(startDate.Value);

            if (cap > 0)
            {
                return cap;
            }

            return null;
        }
    }
}
