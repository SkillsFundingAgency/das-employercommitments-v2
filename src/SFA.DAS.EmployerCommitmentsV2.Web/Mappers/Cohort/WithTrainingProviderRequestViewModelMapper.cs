﻿using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.EmployerCommitmentsV2.Web.Extensions;
using SFA.DAS.Encoding;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort;

public class WithTrainingProviderRequestViewModelMapper : IMapper<CohortsByAccountRequest, WithTrainingProviderViewModel>
{
    public const string Title = "Apprentice details with training provider";
    private readonly IEncodingService _encodingService;
    private readonly ICommitmentsApiClient _commitmentsApiClient;
    private readonly IUrlHelper _urlHelper;


    public WithTrainingProviderRequestViewModelMapper(ICommitmentsApiClient commitmentApiClient, IEncodingService encodingSummary, IUrlHelper urlHelper)
    {
        _encodingService = encodingSummary;
        _commitmentsApiClient = commitmentApiClient;
        _urlHelper = urlHelper;
    }

    public async Task<WithTrainingProviderViewModel> Map(CohortsByAccountRequest source)
    {
        var cohortsResponse = await _commitmentsApiClient.GetCohorts(new GetCohortsRequest { AccountId = source.AccountId });

        var reviewViewModel = new WithTrainingProviderViewModel
        {
            Title = Title,
            AccountHashedId = source.AccountHashedId,
            ApprenticeshipRequestsHeaderViewModel = cohortsResponse.Cohorts.GetCohortCardLinkViewModel(_urlHelper,source.AccountHashedId, CohortStatus.WithProvider),
            Cohorts = cohortsResponse.Cohorts
                .Where(x => x.GetStatus() == CohortStatus.WithProvider)
                .OrderBy(z => z.LatestMessageFromEmployer != null ? z.LatestMessageFromEmployer.SentOn : z.CreatedOn)
                .Select(y => new WithTrainingProviderCohortSummaryViewModel
                {
                    ProviderId = y.ProviderId,
                    CohortReference = _encodingService.Encode(y.CohortId, EncodingType.CohortReference),
                    ProviderName = y.ProviderName,
                    NumberOfApprentices = y.NumberOfDraftApprentices,
                    LastMessage = GetMessage(y.LatestMessageFromEmployer)
                }).ToList()
        };

        if (reviewViewModel.Cohorts?.GroupBy(x => x.ProviderId).Count() > 1)
        {
            reviewViewModel.Title += "s";
        }

        return reviewViewModel;
    }

    private static string GetMessage(Message latestMessageFromEmployer)
    {
        return !string.IsNullOrWhiteSpace(latestMessageFromEmployer?.Text) 
            ? latestMessageFromEmployer.Text
            : "No message added";
    }
}