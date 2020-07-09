using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Moq;
using NUnit.Framework;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Web.Extensions;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Extensions
{
    [TestFixture]
    public class GetCohortCardLinkViewModelExtentsionsTests
    {
        [Test]
        public void TheCohortsInDraftIsPopulatedCorrectly()
        {
            var f = new GetCohortCardLinkViewModelTestsFixture();
            var result = f.GetCohortCardLinkViewModel();

            f.VerifyCohortsInDraftIsCorrect(result);
        }

        [Test]
        public void TheCohortsInReviewIsPopulatedCorrectly()
        {
            var f = new GetCohortCardLinkViewModelTestsFixture();
            var result = f.GetCohortCardLinkViewModel();

            f.VerifyCohortsInReviewIsCorrect(result);
        }

        [Test]
        public void TheCohortsWithTrainingProviderIsPopulatedCorrectly()
        {
            var f = new GetCohortCardLinkViewModelTestsFixture();
            var result = f.GetCohortCardLinkViewModel();

            f.VerifyCohortsWithProviderIsCorrect(result);
        }

        [Test]
        public void TheCohortsWithTransferSenderIsPopulatedCorrectly()
        {
            var f = new GetCohortCardLinkViewModelTestsFixture();
            var result = f.GetCohortCardLinkViewModel();

            f.VerifyCohortsWithTransferSenderIsCorrect(result);
        }

        [TestCase(CohortStatus.Draft)]
        [TestCase(CohortStatus.Review)]
        [TestCase(CohortStatus.WithProvider)]
        [TestCase(CohortStatus.WithTransferSender)]
        public void TheCohortsStatusIsSetCorrectly(CohortStatus selectedCohortStatus)
        {
            var f = new GetCohortCardLinkViewModelTestsFixture();
            var result = f.GetCohortCardLinkViewModel(selectedCohortStatus);

            f.VerifySelectedCohortStatusIsCorrect(result, selectedCohortStatus);
        }

        public class GetCohortCardLinkViewModelTestsFixture
        {
            private Fixture _fixture;
            public Mock<IUrlHelper> UrlHelper { get; }

            public CohortSummary[] CohortSummaries { get; set; }

            private string accountHashed => "ABC123";

            public GetCohortCardLinkViewModelTestsFixture()
            {
                UrlHelper = new Mock<IUrlHelper>();
                UrlHelper.Setup(x => x.Action(It.IsAny<UrlActionContext>())).Returns<UrlActionContext>((ac) => $"http://{ac.Controller}/{ac.Action}/");
                _fixture = new Fixture();

                CohortSummaries = CreateGetCohortsResponse();
            }

            public void VerifyCohortsInDraftIsCorrect(ApprenticeshipRequestsHeaderViewModel result)
            {
                Assert.IsNotNull(result.CohortsInDraft);
                Assert.AreEqual(5, result.CohortsInDraft.Count);
                Assert.AreEqual("Drafts", result.CohortsInDraft.Description);
                UrlHelper.Verify(x => x.Action(It.Is<UrlActionContext>(p => p.Controller == "Cohort" && p.Action == "Draft")));
            }

            public void VerifyCohortsInReviewIsCorrect(ApprenticeshipRequestsHeaderViewModel result)
            {
                Assert.IsNotNull(result.CohortsInReview);
                Assert.AreEqual(4, result.CohortsInReview.Count);
                Assert.AreEqual("Ready to review", result.CohortsInReview.Description);
                UrlHelper.Verify(x => x.Action(It.Is<UrlActionContext>(p => p.Controller == "Cohort" && p.Action == "Review")));
            }

            public void VerifyCohortsWithProviderIsCorrect(ApprenticeshipRequestsHeaderViewModel result)
            {
                Assert.IsNotNull(result.CohortsWithTrainingProvider);
                Assert.AreEqual(3, result.CohortsWithTrainingProvider.Count);
                Assert.AreEqual("With training providers", result.CohortsWithTrainingProvider.Description);
                UrlHelper.Verify(x => x.Action(It.Is<UrlActionContext>(p => p.Controller == "Cohort" && p.Action == "WithTrainingProvider")));
            }

            public void VerifyCohortsWithTransferSenderIsCorrect(ApprenticeshipRequestsHeaderViewModel result)
            {
                Assert.IsNotNull(result.CohortsWithTransferSender);
                Assert.AreEqual(2, result.CohortsWithTransferSender.Count);
                Assert.AreEqual("With transfer sending employers", result.CohortsWithTransferSender.Description);
                UrlHelper.Verify(x => x.Action(It.Is<UrlActionContext>(p => p.Controller == "Cohort" && p.Action == "WithTransferSender")));
            }

            public void VerifySelectedCohortStatusIsCorrect(ApprenticeshipRequestsHeaderViewModel result, CohortStatus expectedCohortStatus)
            {
                Assert.AreEqual(expectedCohortStatus == CohortStatus.WithTransferSender, result.CohortsWithTransferSender.IsSelected);
                Assert.AreEqual(expectedCohortStatus == CohortStatus.Draft, result.CohortsInDraft.IsSelected);
                Assert.AreEqual(expectedCohortStatus == CohortStatus.Review, result.CohortsInReview.IsSelected);
                Assert.AreEqual(expectedCohortStatus == CohortStatus.WithProvider, result.CohortsWithTrainingProvider.IsSelected);
            }

            public ApprenticeshipRequestsHeaderViewModel GetCohortCardLinkViewModel(CohortStatus selectedCohortStatus = CohortStatus.Draft)
            {
                return CohortSummaries.GetCohortCardLinkViewModel(UrlHelper.Object, accountHashed, selectedCohortStatus);
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

            private CohortSummary[] CreateGetCohortsResponse()
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

                return cohorts.ToArray();
            }
        }
    }
}
