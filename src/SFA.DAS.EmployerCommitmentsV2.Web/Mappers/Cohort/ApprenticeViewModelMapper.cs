﻿using System.Threading.Tasks;
using SFA.DAS.Apprenticeships.Api.Client;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.CommitmentsV2.Shared.Models;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort
{
    public class ApprenticeViewModelMapper : IMapper<ApprenticeRequest, ApprenticeViewModel>
    {
        private readonly ICommitmentsApiClient _commitmentsApiClient;
        private readonly ITrainingProgrammeApiClient _trainingProgrammeApiClient;

        public ApprenticeViewModelMapper(ICommitmentsApiClient commitmentsApiClient,
            ITrainingProgrammeApiClient trainingProgrammeApiClient)
        {
            _commitmentsApiClient = commitmentsApiClient;
            _trainingProgrammeApiClient = trainingProgrammeApiClient;
        }

        public async Task<ApprenticeViewModel> Map(ApprenticeRequest source)
        {
            var ale = await _commitmentsApiClient.GetAccountLegalEntity(source.AccountLegalEntityId);

            var courses = !string.IsNullOrWhiteSpace(source.TransferSenderId) || ale.LevyStatus == ApprenticeshipEmployerType.NonLevy
                ? await _trainingProgrammeApiClient.GetStandardTrainingProgrammes()
                : await _trainingProgrammeApiClient.GetAllTrainingProgrammes();

            var provider = await _commitmentsApiClient.GetProvider(source.ProviderId);

            var result = new ApprenticeViewModel
            {
                AccountHashedId = source.AccountHashedId,
                AccountLegalEntityId = source.AccountLegalEntityId,
                AccountLegalEntityHashedId = source.AccountLegalEntityHashedId,
                StartDate = new MonthYearModel(source.StartMonthYear),
                ReservationId = source.ReservationId,
                CourseCode = source.CourseCode,
                ProviderId = (int)source.ProviderId,
                ProviderName = provider.Name,
                Courses = courses,
                TransferSenderId = source.TransferSenderId,
                Origin = source.Origin,
                AutoCreatedReservation = source.AutoCreated
            };

            return result;
        }
    }
}
