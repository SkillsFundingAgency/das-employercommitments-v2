﻿using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Extensions;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice.Edit;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice;

public class ReviewApprenticeshipUpdatesRequestToViewModelMapper : IMapper<ReviewApprenticeshipUpdatesRequest, ReviewApprenticeshipUpdatesViewModel>
{
    private readonly ICommitmentsApiClient _commitmentsApiClient;

    public ReviewApprenticeshipUpdatesRequestToViewModelMapper(ICommitmentsApiClient commitmentsApiClient)
    {
        _commitmentsApiClient = commitmentsApiClient;
    }

    public async Task<ReviewApprenticeshipUpdatesViewModel> Map(ReviewApprenticeshipUpdatesRequest source)
    {
        var updatesTask = _commitmentsApiClient.GetApprenticeshipUpdates(source.ApprenticeshipId,
            new CommitmentsV2.Api.Types.Requests.GetApprenticeshipUpdatesRequest { Status = CommitmentsV2.Types.ApprenticeshipUpdateStatus.Pending });

        var apprenticeshipTask = _commitmentsApiClient.GetApprenticeship(source.ApprenticeshipId);

        await Task.WhenAll(updatesTask, apprenticeshipTask);

        var updates = updatesTask.Result;
        var apprenticeship = apprenticeshipTask.Result;

        if (updates.ApprenticeshipUpdates.Count != 1)
        {
            throw new Exception("Multiple pending updates found");
        }
        
        var update = updates.ApprenticeshipUpdates.First();
                
        if (!string.IsNullOrWhiteSpace(update.FirstName + update.LastName))
        {
            update.FirstName = string.IsNullOrWhiteSpace(update.FirstName) ? apprenticeship.FirstName : update.FirstName;
            update.LastName = string.IsNullOrWhiteSpace(update.LastName) ? apprenticeship.LastName : update.LastName;
        }

        var vm = new ReviewApprenticeshipUpdatesViewModel
        {
            AccountHashedId = source.AccountHashedId,
            ApprenticeshipHashedId = source.ApprenticeshipHashedId,
            ProviderName = apprenticeship.ProviderName,
            ApprenticeshipUpdates = new BaseEdit
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
                Option = update.Option,
                EmploymentEndDate = update.EmploymentEndDate,
                EmploymentPrice = update.EmploymentPrice
            },
            OriginalApprenticeship = new BaseEdit
            {
                FirstName = apprenticeship.FirstName,
                LastName = apprenticeship.LastName,
                Email = apprenticeship.Email,
                DateOfBirth = apprenticeship.DateOfBirth,
                ULN = apprenticeship.Uln,
                StartDate = apprenticeship.StartDate,
                EndDate = apprenticeship.EndDate,
                DeliveryModel = apprenticeship.DeliveryModel,
                CourseCode = apprenticeship.CourseCode,
                CourseName = apprenticeship.CourseName,
                Version = apprenticeship.Version,
                Option = apprenticeship.Option,
                EmploymentEndDate = apprenticeship.EmploymentEndDate,
                EmploymentPrice = apprenticeship.EmploymentPrice
            }
        };

        if (update.Cost.HasValue)
        {
            var priceEpisodes = await _commitmentsApiClient.GetPriceEpisodes(source.ApprenticeshipId);
            vm.OriginalApprenticeship.Cost = priceEpisodes.PriceEpisodes.GetPrice();
        }

        return vm;

    }
}