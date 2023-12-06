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
                Assert.That(result.CohortsInDraft, Is.Not.Null);
                Assert.That(result.CohortsInDraft.Count, Is.EqualTo(5));
                Assert.That(result.CohortsInDraft.Description, Is.EqualTo("Drafts"));
                UrlHelper.Verify(x => x.Action(It.Is<UrlActionContext>(p => p.Controller == "Cohort" && p.Action == "Draft")));
            }

            public void VerifyCohortsInReviewIsCorrect(ApprenticeshipRequestsHeaderViewModel result)
            {
                Assert.That(result.CohortsInReview, Is.Not.Null);
                Assert.That(result.CohortsInReview.Count, Is.EqualTo(4));
                Assert.That(result.CohortsInReview.Description, Is.EqualTo("Ready to review"));
                UrlHelper.Verify(x => x.Action(It.Is<UrlActionContext>(p => p.Controller == "Cohort" && p.Action == "Review")));
            }

            public void VerifyCohortsWithProviderIsCorrect(ApprenticeshipRequestsHeaderViewModel result)
            {
                Assert.That(result.CohortsWithTrainingProvider, Is.Not.Null);
                Assert.That(result.CohortsWithTrainingProvider.Count, Is.EqualTo(3));
                Assert.That(result.CohortsWithTrainingProvider.Description, Is.EqualTo("With training providers"));
                UrlHelper.Verify(x => x.Action(It.Is<UrlActionContext>(p => p.Controller == "Cohort" && p.Action == "WithTrainingProvider")));
            }

            public void VerifyCohortsWithTransferSenderIsCorrect(ApprenticeshipRequestsHeaderViewModel result)
            {
                Assert.That(result.CohortsWithTransferSender, Is.Not.Null);
                Assert.That(result.CohortsWithTransferSender.Count, Is.EqualTo(2));
                Assert.That(result.CohortsWithTransferSender.Description, Is.EqualTo("With transfer sending employers"));
                UrlHelper.Verify(x => x.Action(It.Is<UrlActionContext>(p => p.Controller == "Cohort" && p.Action == "WithTransferSender")));
            }

            public void VerifySelectedCohortStatusIsCorrect(ApprenticeshipRequestsHeaderViewModel result, CohortStatus expectedCohortStatus)
            {
                Assert.That(result.CohortsWithTransferSender.IsSelected, Is.EqualTo(expectedCohortStatus == CohortStatus.WithTransferSender));
                Assert.That(result.CohortsInDraft.IsSelected, Is.EqualTo(expectedCohortStatus == CohortStatus.Draft));
                Assert.That(result.CohortsInReview.IsSelected, Is.EqualTo(expectedCohortStatus == CohortStatus.Review));
                Assert.That(result.CohortsWithTrainingProvider.IsSelected, Is.EqualTo(expectedCohortStatus == CohortStatus.WithProvider));
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
