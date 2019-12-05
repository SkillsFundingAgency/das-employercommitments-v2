using System.Linq;
using System.Threading.Tasks;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.EmployerCommitmentsV2.Web.Extensions;
using SFA.DAS.EmployerUrlHelper;
using SFA.DAS.Encoding;
using System;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort
{
    public class WithTransferSenderRequestViewModelMapper : IMapper<WithTransferSenderRequest, WithTransferSenderViewModel>
    {
        public const string Title = "Apprentice details with transfer sending employer";
        private readonly IEncodingService _encodingService;
        private readonly ICommitmentsApiClient _commitmentsApiClient;
        private readonly ILinkGenerator _linkGenerator;


        public WithTransferSenderRequestViewModelMapper(ICommitmentsApiClient commitmentApiClient, IEncodingService encodingSummary, ILinkGenerator linkGenerator)
        {
            _encodingService = encodingSummary;
            _commitmentsApiClient = commitmentApiClient;
            _linkGenerator = linkGenerator;
        }

        public async Task<WithTransferSenderViewModel> Map(WithTransferSenderRequest source)
        {
            var cohortsResponse = await _commitmentsApiClient.GetCohorts(new GetCohortsRequest { AccountId = source.AccountId });

            var reviewViewModel = new WithTransferSenderViewModel
            {
                Title = Title,
                AccountHashedId = source.AccountHashedId,
                BackLink = _linkGenerator.Cohorts(source.AccountHashedId),
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

        private DateTime GetOrderByDate(CohortSummary s)
        {
            if (s.LatestMessageFromEmployer != null && s.LatestMessageFromProvider != null)
            {
                if (s.LatestMessageFromProvider.SentOn < s.LatestMessageFromEmployer.SentOn)
                    return s.LatestMessageFromProvider.SentOn;
                else
                    return s.LatestMessageFromEmployer.SentOn;
            }
            else if (s.LatestMessageFromEmployer == null && s.LatestMessageFromProvider == null)
            {
                return s.CreatedOn;
            }
            else if (s.LatestMessageFromEmployer == null)
            {
                return s.LatestMessageFromProvider.SentOn;
            }
            else 
            {
                return s.LatestMessageFromEmployer.SentOn;
            }
        }
    }
}