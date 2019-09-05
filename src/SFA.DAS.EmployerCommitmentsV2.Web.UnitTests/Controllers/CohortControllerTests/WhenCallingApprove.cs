using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.Commitments.Shared.Interfaces;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.EmployerCommitmentsV2.Web.Services;
using SFA.DAS.Http;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.Encoding;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.CohortControllerTests
{
    [TestFixture]
    [Parallelizable(ParallelScope.All)]
    public class ApproveCohortTests
    { 
        [Test]
        public async Task WhenCohortIsNotFound_ShouldReturnNotFound()
        {
            var fixtures = new ApproveCohortTestFixtures()
                .SetCohortIdForTest(123)
                .SetAccountIdForTest(456)
                .SetCommitmentsToThrowNotFoundException();

            var result = await fixtures.GetResultFromSut();

            result.VerifyReturnsNotFound();
        }

        [Test]
        public async Task WhenCohortIsApproved_ShouldReturnNotFound()
        {
            var fixtures = new ApproveCohortTestFixtures()
                .SetCohortIdForTest(123)
                .SetAccountIdForTest(456)
                .SetCommitmentsToReturnCohortWithParty(Party.None);

            var result = await fixtures.GetResultFromSut();

            result.VerifyReturnsNotFound();
        }

        [Test]
        public async Task WhenCohortWithOtherParty_ShouldReturnRedirectToV1()
        {
            const string v1Url = "v1Url";

            var fixtures = new ApproveCohortTestFixtures()
                .SetCohortIdForTest(123)
                .SetAccountIdForTest(456)
                .SetCommitmentsToReturnCohortWithParty(Party.Employer)
                .SetCohortToBeWithOtherParty(v1Url);

            var result = await fixtures.GetResultFromSut();

            result.VerifyReturnsRedirect().ThatMatches(redirect => redirect.Url == v1Url);
        }

        [Test]
        public async Task WhenCohortWithEmployer_ShouldReturnViewModel()
        {
            var fixtures = new ApproveCohortTestFixtures()
                .SetCohortIdForTest(123)
                .SetAccountIdForTest(456)
                .SetCommitmentsToReturnCohortWithParty(Party.Employer)
                .SetMapperResult();

            var result = await fixtures.GetResultFromSut();

            result.VerifyReturnsViewModel()
                .WithModel<ApproveViewModel>()
                .ThatMatches(vm => vm.CohortReference == fixtures.CohortReference && vm.AccountHashedId == fixtures.AccountHashedId);
        }
    }

    public class ApproveCohortTestFixtures
    {
        private long? _cohortIdForTest;
        private long? _accountIdForTest;

        public ApproveCohortTestFixtures()
        {
            CommitmentsApiClientMock = new Mock<ICommitmentsApiClient>();
            LoggerMock = new Mock<ILogger<CohortController>>();
            CommitmentsServiceMock = new Mock<ICommitmentsService>();
            ModelMapperMock = new Mock<IModelMapper>();
            UrlSelectorServiceMock = new Mock<IUrlSelectorService>();
            EncodingServiceMock = new Mock<IEncodingService>();
        }

        public Mock<ICommitmentsApiClient> CommitmentsApiClientMock { get;  }
        public ICommitmentsApiClient CommitmentsApiClient => CommitmentsApiClientMock.Object;

        public Mock<ILogger<CohortController>> LoggerMock { get;  }
        public ILogger<CohortController> Logger => LoggerMock.Object;

        public Mock<ICommitmentsService> CommitmentsServiceMock { get;  }
        public ICommitmentsService CommitmentsService => CommitmentsServiceMock.Object;

        public Mock<IModelMapper> ModelMapperMock { get; }
        public IModelMapper ModelMapper => ModelMapperMock.Object;

        public Mock<IUrlSelectorService> UrlSelectorServiceMock { get; }
        public IUrlSelectorService UrlSelectorService => UrlSelectorServiceMock.Object;

        public Mock<IEncodingService> EncodingServiceMock { get; }
        public IEncodingService EncodingService => EncodingServiceMock.Object;

        public long CohortId
        {
            get
            {
                if (_cohortIdForTest == null)
                {
                    throw new InvalidOperationException("Test setup has not set the cohort id before it is required");
                }

                return _cohortIdForTest.Value;
            }
            private set => _cohortIdForTest = value;
        }

        public string CohortReference => $"#{CohortId}";

        public long AccountId
        {
            get
            {
                if (_accountIdForTest == null)
                {
                    throw new InvalidOperationException("Test setup has not set the account id before it is required");
                }

                return _accountIdForTest.Value;
            }
            private set => _accountIdForTest = value;
        }

        public string AccountHashedId=> $"#{AccountId}";

        public ApproveCohortTestFixtures SetCommitmentsToThrowNotFoundException()
        {
            var httpError = new HttpResponseMessage(HttpStatusCode.NotFound)
            {
                ReasonPhrase = "Not found",
                RequestMessage = new HttpRequestMessage(HttpMethod.Get, "SomeUrl")
            };

            CommitmentsApiClientMock
                .Setup(c => c.GetCohort(CohortId, It.IsAny<CancellationToken>()))
                .ThrowsAsync(new RestHttpClientException(httpError, httpError.ReasonPhrase));

            return this;
        }

        public ApproveCohortTestFixtures SetCommitmentsToReturnCohortWithParty(Party withParty)
        {
            var cohort = new GetCohortResponse
            {
                CohortId = CohortId,
                WithParty = withParty
            };

            CommitmentsApiClientMock
                .Setup(c => c.GetCohort(CohortId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(cohort);

            return this;
        }

        public ApproveCohortTestFixtures SetCohortToBeWithOtherParty(string v1Url)
        {
            UrlSelectorServiceMock
                .Setup(u => u.RedirectToV1IfCohortWithOtherParty(AccountHashedId, CohortReference,
                    It.IsAny<GetCohortResponse>()))
                .Returns(new RedirectResult(v1Url));

            return this;
        }

        public ApproveCohortTestFixtures SetMapperResult()
        {
            ModelMapperMock
                .Setup(m => m.Map<ApproveViewModel>(It.IsAny<ApproveRequest>()))
                .ReturnsAsync((ApproveRequest request) => new ApproveViewModel
                {
                    AccountHashedId = request.AccountHashedId,
                    CohortReference = request.CohortReference
                });

            return this;
        }

        public ApproveCohortTestFixtures SetCohortIdForTest(long cohortId)
        {
            CohortId = cohortId;
            SetEncoding(CohortReference, CohortId, EncodingType.CohortReference);
            return this;
        }

        public ApproveCohortTestFixtures SetAccountIdForTest(long accountId)
        {
            AccountId = accountId;
            SetEncoding(AccountHashedId, AccountId, EncodingType.AccountId);
            return this;
        }

        public CohortController CreateSut()
        {
            return new CohortController(
                CommitmentsApiClient,
                Logger,
                CommitmentsService,
                ModelMapper,
                UrlSelectorService,
                EncodingService
                );
        }

        public Task<IActionResult> GetResultFromSut()
        {
            var sut = CreateSut();
            var request = new ApproveRequest
            {
                CohortReference = CohortReference,
                AccountHashedId = AccountHashedId
            };

            return sut.Approve(request);
        }

        private void SetEncoding(string encoded, long decoded, EncodingType encodingType)
        {
            EncodingServiceMock.Setup(e => e.Decode(encoded, encodingType)).Returns(decoded);
            EncodingServiceMock.Setup(e => e.Encode(decoded, encodingType)).Returns(encoded);
        }
    }
}