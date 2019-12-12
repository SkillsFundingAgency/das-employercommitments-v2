using System.Linq;
using Moq;
using NUnit.Framework;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.CommitmentsV2.Types;
using System.Collections.Generic;
using SFA.DAS.CommitmentsV2.Api.Client;
using System.Threading;
using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using System.Threading.Tasks;
using AutoFixture;
using SFA.DAS.EmployerUrlHelper;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Cohort
{
    [TestFixture]
    [Parallelizable(ParallelScope.All)]
    public class WhenMappingCohortsRequestToCohortsViewModel
    {
        [Test]
        public async Task TheBackIsPopulatedCorrectly()
        {
            var f = new WhenMappingCohortsRequestToCohortsViewModelFixture();
            var result = await f.Sut.Map(f.CohortsRequest);

            f.VerifyBackLinkIsCorrect(result.BackLink);
        }

        [Test]
        public async Task TheCohortsInDraftIsPopulatedCorrectly()
        {
            var f = new WhenMappingCohortsRequestToCohortsViewModelFixture();
            var result = await f.Sut.Map(f.CohortsRequest);

            f.VerifyCohortsInDraftIsCorrect(result);
        }

        [Test]
        public async Task TheCohortsInReviewIsPopulatedCorrectly()
        {
            var f = new WhenMappingCohortsRequestToCohortsViewModelFixture();
            var result = await f.Sut.Map(f.CohortsRequest);

            f.VerifyCohortsInReviewIsCorrect(result);
        }

        [Test]
        public async Task TheCohortsWithTrainingProviderIsPopulatedCorrectly()
        {
            var f = new WhenMappingCohortsRequestToCohortsViewModelFixture();
            var result = await f.Sut.Map(f.CohortsRequest);

            f.VerifyCohortsWithProviderIsCorrect(result);
        }

        [Test]
        public async Task TheCohortsWithTransferSenderIsPopulatedCorrectly()
        {
            var f = new WhenMappingCohortsRequestToCohortsViewModelFixture();
            var result = await f.Sut.Map(f.CohortsRequest);

            f.VerifyCohortsWithTransferSenderIsCorrect(result);
        }

        [Test]
        public async Task WhenNoCohortsAreFoundThereAreNoDrilldownLinks()
        {
            var f = new WhenMappingCohortsRequestToCohortsViewModelFixture().WithNoCohortsFound();
            var result = await f.Sut.Map(f.CohortsRequest);

            f.VerifyNoDrillDownLinks(result);
        }

        public class WhenMappingCohortsRequestToCohortsViewModelFixture
        {
            public Mock<ICommitmentsApiClient> CommitmentsApiClient { get; }
            public Mock<ILinkGenerator> LinkGenerator { get; }
            public CohortsByAccountRequest CohortsRequest { get; }
            public CohortsSummaryViewModelMapper Sut { get; }

            private Fixture _fixture;

            public WhenMappingCohortsRequestToCohortsViewModelFixture()
            {
                _fixture = new Fixture();
                CohortsRequest = _fixture.Create<CohortsByAccountRequest>();
                
                CommitmentsApiClient = new Mock<ICommitmentsApiClient>();
                CommitmentsApiClient.Setup(x => x.GetCohorts(It.IsAny<GetCohortsRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(CreateGetCohortsResponse());
                
                LinkGenerator = new Mock<ILinkGenerator>();
                LinkGenerator.Setup(x => x.CommitmentsV2Link(It.IsAny<string>())).Returns<string>(p => $"http://{p}");
                LinkGenerator.Setup(x => x.CommitmentsLink(It.IsAny<string>())).Returns<string>(p => $"http://{p}");

                Sut = new CohortsSummaryViewModelMapper(CommitmentsApiClient.Object, LinkGenerator.Object);
            }

            public WhenMappingCohortsRequestToCohortsViewModelFixture WithNoCohortsFound()
            {
                CommitmentsApiClient.Setup(x => x.GetCohorts(It.IsAny<GetCohortsRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(new GetCohortsResponse(new List<CohortSummary>()));
                return this;
            }

            public void VerifyBackLinkIsCorrect(string backLink)
            {
                Assert.AreEqual($"http://accounts/{CohortsRequest.AccountHashedId}/apprentices/home", backLink);
            }

            public void VerifyCohortsInDraftIsCorrect(CohortsViewModel result)
            {
                Assert.IsNotNull(result.CohortsInDraft);
                Assert.AreEqual(5, result.CohortsInDraft.Count);
                Assert.AreEqual("drafts", result.CohortsInDraft.Description);
                Assert.AreEqual($"http://{CohortsRequest.AccountHashedId}/unapproved/draft", result.CohortsInDraft.Url);
            }

            public void VerifyCohortsInReviewIsCorrect(CohortsViewModel result)
            {
                Assert.IsNotNull(result.CohortsInReview);
                Assert.AreEqual(4, result.CohortsInReview.Count);
                Assert.AreEqual("ready to review", result.CohortsInReview.Description);
                Assert.AreEqual($"http://{CohortsRequest.AccountHashedId}/unapproved/review", result.CohortsInReview.Url);
            }

            public void VerifyCohortsWithProviderIsCorrect(CohortsViewModel result)
            {
                Assert.IsNotNull(result.CohortsWithTrainingProvider);
                Assert.AreEqual(3, result.CohortsWithTrainingProvider.Count);
                Assert.AreEqual("with providers", result.CohortsWithTrainingProvider.Description);
                Assert.AreEqual($"http://{CohortsRequest.AccountHashedId}/unapproved/with-training-provider", result.CohortsWithTrainingProvider.Url);
            }

            public void VerifyCohortsWithTransferSenderIsCorrect(CohortsViewModel result)
            {
                Assert.IsNotNull(result.CohortsWithTransferSender);
                Assert.AreEqual(2, result.CohortsWithTransferSender.Count);
                Assert.AreEqual("with transfer sending employers", result.CohortsWithTransferSender.Description);
                Assert.AreEqual($"http://{CohortsRequest.AccountHashedId}/unapproved/with-transfer-sender", result.CohortsWithTransferSender.Url);
            }

            public void VerifyNoDrillDownLinks(CohortsViewModel result)
            {
                Assert.IsNull(result.CohortsInDraft.Url);
                Assert.IsNull(result.CohortsInReview.Url);
                Assert.IsNull(result.CohortsWithTrainingProvider.Url);
                Assert.IsNull(result.CohortsWithTransferSender.Url);
            }

            private static List<CohortSummary> PopulateWith(List<CohortSummary> list, bool draft, Party withParty)
            {
                foreach (var item in list)
                {
                    item.IsDraft = draft;
                    item.WithParty = withParty;
                }

                return list;
            }

            private GetCohortsResponse CreateGetCohortsResponse()
            {
                var listInDraft = _fixture.CreateMany<CohortSummary>(5).ToList();
                PopulateWith(listInDraft, true, Party.Employer);

                var listInReview = _fixture.CreateMany<CohortSummary>(4).ToList();
                PopulateWith(listInReview, false, Party.Employer);

                var listWithProvider = _fixture.CreateMany<CohortSummary>(3).ToList();
                PopulateWith(listWithProvider, false, Party.Provider).ToList();

                var listWithTransferSender = _fixture.CreateMany<CohortSummary>(2).ToList();
                PopulateWith(listWithTransferSender, false, Party.TransferSender);

                var cohorts = listInDraft.Union(listInReview).Union(listWithProvider).Union(listWithTransferSender);

                return new GetCohortsResponse(cohorts);
            }
        }
    }
}