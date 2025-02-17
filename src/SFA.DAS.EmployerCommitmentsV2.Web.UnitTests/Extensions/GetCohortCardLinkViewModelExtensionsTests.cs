using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Routing;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Web.Extensions;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Extensions;

[TestFixture]
public class GetCohortCardLinkViewModelExtensionsTests
{
    [Test]
    public void TheCohortsInDraftIsPopulatedCorrectly()
    {
        var fixture = new GetCohortCardLinkViewModelTestsFixture();
        var result = fixture.GetCohortCardLinkViewModel();

        fixture.VerifyCohortsInDraftIsCorrect(result);
    }

    [Test]
    public void TheCohortsInReviewIsPopulatedCorrectlyForSingles()
    {
        var fixture = new GetCohortCardLinkViewModelTestsFixture(useSingles: true);
        var result = fixture.GetCohortCardLinkViewModel();

        fixture.VerifyCohortsInReviewIsCorrectForSingles(result);
    }

    [Test]
    public void TheCohortsInReviewIsPopulatedCorrectlyForMultiples()
    {
        var fixture = new GetCohortCardLinkViewModelTestsFixture();
        var result = fixture.GetCohortCardLinkViewModel();

        fixture.VerifyCohortsInReviewIsCorrectForMultiples(result);
    }

    [Test]
    public void TheCohortsWithTrainingProviderIsPopulatedCorrectly()
    {
        var fixture = new GetCohortCardLinkViewModelTestsFixture();
        var result = fixture.GetCohortCardLinkViewModel();

        fixture.VerifyCohortsWithProviderIsCorrect(result);
    }

    [Test]
    public void TheCohortsWithTransferSenderIsPopulatedCorrectly()
    {
        var fixture = new GetCohortCardLinkViewModelTestsFixture();
        var result = fixture.GetCohortCardLinkViewModel();

        fixture.VerifyCohortsWithTransferSenderIsCorrect(result);
    }

    [TestCase(CohortStatus.Draft)]
    [TestCase(CohortStatus.Review)]
    [TestCase(CohortStatus.WithProvider)]
    [TestCase(CohortStatus.WithTransferSender)]
    public void TheCohortsStatusIsSetCorrectly(CohortStatus selectedCohortStatus)
    {
        var fixture = new GetCohortCardLinkViewModelTestsFixture();
        var result = fixture.GetCohortCardLinkViewModel(selectedCohortStatus);

        GetCohortCardLinkViewModelTestsFixture.VerifySelectedCohortStatusIsCorrect(result, selectedCohortStatus);
    }

    private class GetCohortCardLinkViewModelTestsFixture
    {
        private readonly Fixture _fixture;
        private Mock<IUrlHelper> UrlHelper { get; }

        private CohortSummary[] CohortSummaries { get; set; }

        private static string AccountHashed => "ABC123";

        public GetCohortCardLinkViewModelTestsFixture(bool useSingles = false)
        {
            UrlHelper = new Mock<IUrlHelper>();
            UrlHelper.Setup(x => x.Action(It.IsAny<UrlActionContext>())).Returns<UrlActionContext>((ac) => $"http://{ac.Controller}/{ac.Action}/");
            _fixture = new Fixture();

            CohortSummaries = CreateGetCohortsResponse(useSingles);
        }

        public void VerifyCohortsInDraftIsCorrect(ApprenticeshipRequestsHeaderViewModel result)
        {
            result.CohortsInDraft.Should().NotBeNull();
            result.CohortsInDraft.Count.Should().Be(5);
            result.CohortsInDraft.Description.Should().Be("Drafts");
            UrlHelper.Verify(x => x.Action(It.Is<UrlActionContext>(p => p.Controller == "Cohort" && p.Action == "Draft")));
        }

        public void VerifyCohortsInReviewIsCorrectForMultiples(ApprenticeshipRequestsHeaderViewModel result)
        {
            result.CohortsInDraft.Should().NotBeNull();
            result.CohortsInReview.Count.Should().Be(4);
            result.CohortsInReview.Description.Should().Be("apprentice requests ready for review");
            UrlHelper.Verify(x => x.Action(It.Is<UrlActionContext>(p => p.Controller == "Cohort" && p.Action == "Review")));
        }

        public void VerifyCohortsInReviewIsCorrectForSingles(ApprenticeshipRequestsHeaderViewModel result)
        {
            result.CohortsInDraft.Should().NotBeNull();
            result.CohortsInReview.Count.Should().Be(1);
            result.CohortsInReview.Description.Should().Be("apprentice request ready for review");
            UrlHelper.Verify(x => x.Action(It.Is<UrlActionContext>(p => p.Controller == "Cohort" && p.Action == "Review")));
        }

        public void VerifyCohortsWithProviderIsCorrect(ApprenticeshipRequestsHeaderViewModel result)
        {
            result.CohortsInDraft.Should().NotBeNull();
            result.CohortsWithTrainingProvider.Count.Should().Be(3);
            result.CohortsWithTrainingProvider.Description.Should().Be("With training providers");
            UrlHelper.Verify(x => x.Action(It.Is<UrlActionContext>(p => p.Controller == "Cohort" && p.Action == "WithTrainingProvider")));
        }

        public void VerifyCohortsWithTransferSenderIsCorrect(ApprenticeshipRequestsHeaderViewModel result)
        {
            result.CohortsInDraft.Should().NotBeNull();
            result.CohortsWithTransferSender.Count.Should().Be(2);
            result.CohortsWithTransferSender.Description.Should().Be("With transfer sending employers");
            UrlHelper.Verify(x => x.Action(It.Is<UrlActionContext>(p => p.Controller == "Cohort" && p.Action == "WithTransferSender")));
        }

        public static void VerifySelectedCohortStatusIsCorrect(ApprenticeshipRequestsHeaderViewModel result, CohortStatus expectedCohortStatus)
        {
            result.CohortsWithTransferSender.IsSelected.Should().Be(expectedCohortStatus == CohortStatus.WithTransferSender);
            result.CohortsInDraft.IsSelected.Should().Be(expectedCohortStatus == CohortStatus.Draft);
            result.CohortsInReview.IsSelected.Should().Be(expectedCohortStatus == CohortStatus.Review);
            result.CohortsWithTrainingProvider.IsSelected.Should().Be(expectedCohortStatus == CohortStatus.WithProvider);
        }

        public ApprenticeshipRequestsHeaderViewModel GetCohortCardLinkViewModel(CohortStatus selectedCohortStatus = CohortStatus.Draft)
        {
            return CohortSummaries.GetCohortCardLinkViewModel(UrlHelper.Object, AccountHashed, selectedCohortStatus);
        }

        private static void PopulateWith(List<CohortSummary> list, bool draft, Party withParty)
        {
            foreach (var item in list)
            {
                item.IsDraft = draft;
                item.WithParty = withParty;
            }
        }

        private CohortSummary[] CreateGetCohortsResponse(bool useSingles = false)
        {
            var listInDraft = _fixture.CreateMany<CohortSummary>(5).ToList();
            PopulateWith(listInDraft, true, Party.Employer);

            var listInReview = _fixture.CreateMany<CohortSummary>(useSingles ? 1 : 4).ToList();
            PopulateWith(listInReview, false, Party.Employer);

            var listWithProvider = _fixture.CreateMany<CohortSummary>(3).ToList();
            PopulateWith(listWithProvider, false, Party.Provider);

            var listWithTransferSender = _fixture.CreateMany<CohortSummary>(2).ToList();
            PopulateWith(listWithTransferSender, false, Party.TransferSender);

            var cohorts = listInDraft.Union(listInReview).Union(listWithProvider).Union(listWithTransferSender);

            return cohorts.ToArray();
        }
    }
}