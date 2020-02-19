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
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Cohort
{
    [TestFixture]
    [Parallelizable(ParallelScope.All)]
    public class WhenMappingCohortsRequestToCohortsViewModel
    {
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
            public Mock<IUrlHelper> UrlHelper { get; }
            public CohortsByAccountRequest CohortsRequest { get; }
            public CohortsSummaryViewModelMapper Sut { get; }

            private Fixture _fixture;

            public WhenMappingCohortsRequestToCohortsViewModelFixture()
            {
                _fixture = new Fixture();
                CohortsRequest = _fixture.Create<CohortsByAccountRequest>();
                
                CommitmentsApiClient = new Mock<ICommitmentsApiClient>();
                CommitmentsApiClient.Setup(x => x.GetCohorts(It.IsAny<GetCohortsRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(CreateGetCohortsResponse());

                UrlHelper = new Mock<IUrlHelper>();
                UrlHelper.Setup(x => x.Action(It.IsAny<UrlActionContext>())).Returns<UrlActionContext>((ac) => $"http://{ac.Controller}/{ac.Action}/");

                Sut = new CohortsSummaryViewModelMapper(CommitmentsApiClient.Object, UrlHelper.Object);
            }

            public WhenMappingCohortsRequestToCohortsViewModelFixture WithNoCohortsFound()
            {
                CommitmentsApiClient.Setup(x => x.GetCohorts(It.IsAny<GetCohortsRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(new GetCohortsResponse(new List<CohortSummary>()));
                return this;
            }

            public void VerifyCohortsInDraftIsCorrect(CohortsViewModel result)
            {
                Assert.IsNotNull(result.CohortsInDraft);
                Assert.AreEqual(5, result.CohortsInDraft.Count);
                Assert.AreEqual("drafts", result.CohortsInDraft.Description);
                UrlHelper.Verify(x => x.Action(It.Is<UrlActionContext>(p => p.Controller == "Cohort" && p.Action == "Draft")));
            }

            public void VerifyCohortsInReviewIsCorrect(CohortsViewModel result)
            {
                Assert.IsNotNull(result.CohortsInReview);
                Assert.AreEqual(4, result.CohortsInReview.Count);
                Assert.AreEqual("ready to review", result.CohortsInReview.Description);
                UrlHelper.Verify(x => x.Action(It.Is<UrlActionContext>(p => p.Controller == "Cohort" && p.Action == "Review")));
            }

            public void VerifyCohortsWithProviderIsCorrect(CohortsViewModel result)
            {
                Assert.IsNotNull(result.CohortsWithTrainingProvider);
                Assert.AreEqual(3, result.CohortsWithTrainingProvider.Count);
                Assert.AreEqual("with training providers", result.CohortsWithTrainingProvider.Description);
                UrlHelper.Verify(x => x.Action(It.Is<UrlActionContext>(p => p.Controller == "Cohort" && p.Action == "WithTrainingProvider")));
            }

            public void VerifyCohortsWithTransferSenderIsCorrect(CohortsViewModel result)
            {
                Assert.IsNotNull(result.CohortsWithTransferSender);
                Assert.AreEqual(2, result.CohortsWithTransferSender.Count);
                Assert.AreEqual("with transfer sending employers", result.CohortsWithTransferSender.Description);
                UrlHelper.Verify(x => x.Action(It.Is<UrlActionContext>(p => p.Controller == "Cohort" && p.Action == "WithTransferSender")));
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