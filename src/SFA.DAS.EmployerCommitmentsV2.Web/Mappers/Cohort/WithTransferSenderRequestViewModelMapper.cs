﻿using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.EmployerCommitmentsV2.Web.Extensions;
using SFA.DAS.Encoding;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort;

public class WithTransferSenderRequestViewModelMapper : IMapper<CohortsByAccountRequest, WithTransferSenderViewModel>
{
    public const string Title = "Apprentice details with transfer sending employer";
    private readonly IEncodingService _encodingService;
    private readonly ICommitmentsApiClient _commitmentsApiClient;
    private readonly IUrlHelper _urlHelper;

    public WithTransferSenderRequestViewModelMapper(ICommitmentsApiClient commitmentApiClient, IEncodingService encodingSummary, IUrlHelper urlHelper)
    {
        _encodingService = encodingSummary;
        _urlHelper = urlHelper;
        _commitmentsApiClient = commitmentApiClient;
    }

    public async Task<WithTransferSenderViewModel> Map(CohortsByAccountRequest source)
    {
        var cohortsResponse = await _commitmentsApiClient.GetCohorts(new GetCohortsRequest { AccountId = source.AccountId });

        var reviewViewModel = new WithTransferSenderViewModel
        {
            Title = Title,
            AccountHashedId = source.AccountHashedId,
            ApprenticeshipRequestsHeaderViewModel = cohortsResponse.Cohorts.GetCohortCardLinkViewModel(_urlHelper, source.AccountHashedId, CohortStatus.WithTransferSender),
            Cohorts = cohortsResponse.Cohorts
                .Where(x => x.GetStatus() == CohortStatus.WithTransferSender)
                .OrderBy(GetOrderByDate)
                .Select(y => new WithTransferSenderCohortSummaryViewModel
                {
                    TransferSenderId = y.TransferSenderId.Value,
                    TransferSenderName = y.TransferSenderName,
                    CohortReference = _encodingService.Encode(y.CohortId, EncodingType.CohortReference),
                    ProviderName = y.ProviderName,
                    NumberOfApprentices = y.NumberOfDraftApprentices,
                }).ToList()
        };

        if (reviewViewModel.Cohorts?.GroupBy(x => x.TransferSenderId).Count() > 1)
        {
            reviewViewModel.Title += "s";
        }

        return reviewViewModel;
    }

    private static DateTime GetOrderByDate(CohortSummary s)
    {
        return new[] { s.LatestMessageFromEmployer?.SentOn, s.LatestMessageFromProvider?.SentOn, s.CreatedOn }.Max().Value;
    }
}