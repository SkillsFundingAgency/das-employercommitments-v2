using AutoFixture.Dsl;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.CommitmentsV2.Types.Dtos;
using SFA.DAS.EAS.Account.Api.Types;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.Encoding;
using SFA.DAS.Http;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Responses;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Cohort;

[TestFixture]
[Parallelizable(ParallelScope.All)]
public class DetailsViewModelMapperTests
{
    [Test]
    public async Task AccountHashedIdIsMappedCorrectly()
    {
        var fixture = new DetailsViewModelMapperTestsFixture();
        var result = await fixture.Map();
        Assert.That(result.AccountHashedId, Is.EqualTo(fixture.Source.AccountHashedId));
    }

    [Test]
    public async Task WithPartyIsMappedCorrectly()
    {
        var fixture = new DetailsViewModelMapperTestsFixture();
        var result = await fixture.Map();
        Assert.That(result.WithParty, Is.EqualTo(fixture.Cohort.WithParty));
    }

    [Test]
    public async Task LegalEntityNameIsMappedCorrectly()
    {
        var fixture = new DetailsViewModelMapperTestsFixture();
        var result = await fixture.Map();
        Assert.That(result.LegalEntityName, Is.EqualTo(fixture.CohortDetails.LegalEntityName));
    }

    [Test]
    public async Task ProviderNameIsMappedCorrectly()
    {
        var fixture = new DetailsViewModelMapperTestsFixture();
        var result = await fixture.Map();
        Assert.That(result.ProviderName, Is.EqualTo(fixture.CohortDetails.ProviderName));
    }

    [Test]
    public async Task MessageIsMappedCorrectly()
    {
        var fixture = new DetailsViewModelMapperTestsFixture();
        var result = await fixture.Map();
        Assert.That(result.Message, Is.EqualTo(fixture.Cohort.LatestMessageCreatedByProvider));
    }

    [TestCase(true, true, "No, request changes from training provider")]
    [TestCase(true, false, "Request changes from training provider")]
    [TestCase(false, true, "Send to the training provider to review or add details")]
    [TestCase(false, false, "Send to the training provider to review or add details")]
    public async Task SendBackToProviderOptionMessageIsMappedCorrectly(bool isAgreementSigned, bool employerComplete, string expected)
    {
        var fixture = new DetailsViewModelMapperTestsFixture().SetIsAgreementSigned(isAgreementSigned).SetEmployerComplete(employerComplete);
        var result = await fixture.Map();
        Assert.That(result.SendBackToProviderOptionMessage, Is.EqualTo(expected));
    }

    [Test]
    public async Task CohortReferenceIsMappedCorrectly()
    {
        var fixture = new DetailsViewModelMapperTestsFixture();
        var result = await fixture.Map();
        Assert.That(result.CohortReference, Is.EqualTo(fixture.Source.CohortReference));
    }

    [Test]
    public async Task IsApprovedByProviderIsMappedCorrectly()
    {
        var fixture = new DetailsViewModelMapperTestsFixture();
        var result = await fixture.Map();
        Assert.That(result.IsApprovedByProvider, Is.EqualTo(fixture.Cohort.IsApprovedByProvider));
    }

    [Test]
    public async Task TransferSenderHashedIdIsEncodedCorrectlyWhenThereIsAValue()
    {
        var fixture = new DetailsViewModelMapperTestsFixture().SetTransferSenderIdAndItsExpectedHashedValue(123, "X123X");
        var result = await fixture.Map();
        Assert.That(result.TransferSenderHashedId, Is.EqualTo("X123X"));
    }

    [Test]
    public async Task TransferSenderHashedIdIsNullWhenThereIsNoValue()
    {
        var fixture = new DetailsViewModelMapperTestsFixture().SetTransferSenderIdAndItsExpectedHashedValue(null, null);
        var result = await fixture.Map();
        Assert.That(result.TransferSenderHashedId, Is.Null);
    }

    [Test]
    public async Task DraftApprenticeshipTotalCountIsReportedCorrectly()
    {
        var fixture = new DetailsViewModelMapperTestsFixture();
        var result = await fixture.Map();
        Assert.That(result.DraftApprenticeshipsCount, Is.EqualTo(fixture.DraftApprenticeshipsResponse.DraftApprenticeships.Count));
    }

    [Test]
    public async Task DraftApprenticeshipCourseCountIsReportedCorrectly()
    {
        var fixture = new DetailsViewModelMapperTestsFixture();
        var result = await fixture.Map();

        foreach (var course in result.Courses)
        {
            var expectedCount = fixture.DraftApprenticeshipsResponse.DraftApprenticeships
                .Count(a => a.CourseCode == course.CourseCode && a.CourseName == course.CourseName && course.DeliveryModel == a.DeliveryModel);

            Assert.That(course.Count, Is.EqualTo(expectedCount));
        }
    }

    [TestCase(DeliveryModel.Regular, false)]
    [TestCase(DeliveryModel.PortableFlexiJob, true)]
    public async Task DraftApprenticeshipCourseIsPortableFlexiJobIsMappedCorrectly(DeliveryModel deliveryModel, bool isPortableFlexiJob)
    {
        var fixture = new DetailsViewModelMapperTestsFixture()
            .CreateDraftApprenticeship(build => build.With(x => x.DeliveryModel, deliveryModel));
        var result = await fixture.Map();
        Assert.That(result.Courses.First().IsPortableFlexiJob, Is.EqualTo(isPortableFlexiJob));
    }

    [TestCase("2019-11-01", null, "-")]
    [TestCase(null, "2019-11-01", "Nov 2019")]
    [TestCase("2019-11-01", "2019-12-01", "Dec 2019")]
    public async Task DraftApprenticeshipEmploymentDatesAreMappedCorrectly(string startDate, string employmentEndDate, string display)
    {
        var fixture = new DetailsViewModelMapperTestsFixture()
            .CreateDraftApprenticeship(build => build
                .With(x => x.StartDate, TryParseNullableDate(startDate))
                .With(x => x.EmploymentEndDate, TryParseNullableDate(employmentEndDate)));
        var result = await fixture.Map();
        Assert.That(result.Courses.First().DraftApprenticeships.First().DisplayEmploymentDates, Is.EqualTo(display));
    }

    private DateTime? TryParseNullableDate(string startDate)
    {
        return DateTime.TryParse(startDate, out DateTime parsed) ? parsed : (DateTime?)null;
    }

    [Test]
    public async Task DraftApprenticeshipCourseOrderIsByCourseNameAndDeliveryModel()
    {
        var fixture = new DetailsViewModelMapperTestsFixture();
        var result = await fixture.Map();

        var expectedSequence = fixture.DraftApprenticeshipsResponse.DraftApprenticeships
            .Select(c => new { c.CourseName, c.CourseCode, DeliveryModel = c.DeliveryModel })
            .Distinct()
            .OrderBy(c => c.CourseName).ThenBy(c => c.CourseCode).ThenBy(c => c.DeliveryModel)
            .ToList();

        var actualSequence = result.Courses
            .Select(c => new { c.CourseName, c.CourseCode, c.DeliveryModel })
            .OrderBy(c => c.CourseName).ThenBy(c => c.CourseCode).ThenBy(c => c.DeliveryModel)
            .ToList();

        fixture.AssertSequenceOrder(expectedSequence, actualSequence, (e, a) => e.CourseName == a.CourseName && e.CourseCode == a.CourseCode && e.DeliveryModel == a.DeliveryModel);
    }

    [Test]
    public async Task DraftApprenticesOrderIsByApprenticeName()
    {
        var fixture = new DetailsViewModelMapperTestsFixture();
        var result = await fixture.Map();

        foreach (var course in result.Courses)
        {
            var expectedSequence = fixture.DraftApprenticeshipsResponse.DraftApprenticeships
                .Where(a => a.CourseName == course.CourseName && a.CourseCode == course.CourseCode && a.DeliveryModel == course.DeliveryModel)
                .Select(a => $"{a.FirstName} {a.LastName}")
                .OrderBy(a => a)
                .ToList();

            var actualSequence = course.DraftApprenticeships.Select(a => a.DisplayName).ToList();

            fixture.AssertSequenceOrder(expectedSequence, actualSequence, (e, a) => e == a);
        }
    }

    [Test]
    public async Task DraftApprenticeshipsAreMappedCorrectly()
    {
        var fixture = new DetailsViewModelMapperTestsFixture();
        var result = await fixture.Map();

        foreach (var draftApprenticeship in fixture.DraftApprenticeshipsResponse.DraftApprenticeships)
        {
            var draftApprenticeshipResult =
                result.Courses.SelectMany(c => c.DraftApprenticeships).Single(x => x.Id == draftApprenticeship.Id);

            fixture.AssertEquality(draftApprenticeship, draftApprenticeshipResult);
        }
    }

    [Test]
    public async Task FundingBandCapsAreMappedCorrectlyForCoursesStartingOnDefaultdate()
    {
        var fixture = new DetailsViewModelMapperTestsFixture();
        var result = await fixture.Map();

        foreach (var draftApprenticeship in fixture.DraftApprenticeshipsResponse.DraftApprenticeships.Where(x => x.StartDate == fixture.DefaultStartDate))
        {
            var draftApprenticeshipResult =
                result.Courses.SelectMany(c => c.DraftApprenticeships).Single(x => x.Id == draftApprenticeship.Id);

            Assert.That(draftApprenticeshipResult.FundingBandCap, Is.EqualTo(1000));
        }
    }

    [Test]
    public async Task Then_Funding_Cap_Is_Null_When_No_Course_Found()
    {
        var fixture = new DetailsViewModelMapperTestsFixture().SetNoCourse();

        var result = await fixture.Map();

        foreach (var draftApprenticeship in fixture.DraftApprenticeshipsResponse.DraftApprenticeships)
        {
            var draftApprenticeshipResult =
                result.Courses.SelectMany(c => c.DraftApprenticeships).Single(x => x.Id == draftApprenticeship.Id);

            Assert.That(draftApprenticeshipResult.FundingBandCap, Is.EqualTo(null));
        }
    }

    [Test]
    public async Task Then_Funding_Cap_Is_Null_When_No_Course_Set()
    {
        var fixture = new DetailsViewModelMapperTestsFixture().SetNoCourseSet();

        var result = await fixture.Map();

        foreach (var draftApprenticeship in fixture.DraftApprenticeshipsResponse.DraftApprenticeships)
        {
            var draftApprenticeshipResult =
                result.Courses.SelectMany(c => c.DraftApprenticeships).Single(x => x.Id == draftApprenticeship.Id);

            Assert.That(draftApprenticeshipResult.FundingBandCap, Is.EqualTo(null));
        }
        fixture.CommitmentsApiClient.Verify(x => x.GetTrainingProgramme(It.IsAny<string>(), CancellationToken.None), Times.Never);
    }

    [Test]
    public async Task FundingBandCapsAreNullForCoursesStarting2MonthsAhead()
    {
        var fixture = new DetailsViewModelMapperTestsFixture();
        var result = await fixture.Map();

        foreach (var draftApprenticeship in fixture.DraftApprenticeshipsResponse.DraftApprenticeships.Where(x => x.StartDate == fixture.DefaultStartDate.AddMonths(2)))
        {
            var draftApprenticeshipResult =
                result.Courses.SelectMany(c => c.DraftApprenticeships).Single(x => x.Id == draftApprenticeship.Id);

            Assert.That(draftApprenticeshipResult.FundingBandCap, Is.EqualTo(null));
        }
    }

    [Test]
    public async Task FundingBandExcessModelShowsTwoApprenticeshipsExceedingTheBandForCourse1()
    {
        var fixture = new DetailsViewModelMapperTestsFixture();
        var result = await fixture.Map();

        var excessModel = result.Courses.FirstOrDefault(x => x.CourseCode == "C1").FundingBandExcess;
        Assert.That(excessModel.NumberOfApprenticesExceedingFundingBandCap, Is.EqualTo(2));
    }

    [Test]
    public async Task FundingBandExcessModelShowsOnlyTheFullStopWhenMultipleFundingCapsAreExceeded()
    {
        var fixture = new DetailsViewModelMapperTestsFixture();
        var result = await fixture.Map();

        var excessModel = result.Courses.FirstOrDefault(x => x.CourseCode == "C1").FundingBandExcess;
        Assert.That(excessModel.DisplaySingleFundingBandCap, Is.EqualTo("."));
    }

    [Test]
    public async Task FundingBandExcessModelShowsOneApprenticeshipExceedingTheBandForCourse2()
    {
        var fixture = new DetailsViewModelMapperTestsFixture();
        var result = await fixture.Map();

        var excessModel = result.Courses.FirstOrDefault(x => x.CourseCode == "C2").FundingBandExcess;
        Assert.That(excessModel.NumberOfApprenticesExceedingFundingBandCap, Is.EqualTo(1));
    }

    [Test]
    public async Task FundingBandExcessModelShowsTheSingleFundingBandCapExceeded()
    {
        var fixture = new DetailsViewModelMapperTestsFixture();
        var result = await fixture.Map();

        var excessModel = result.Courses.FirstOrDefault(x => x.CourseCode == "C2").FundingBandExcess;
        Assert.That(excessModel.DisplaySingleFundingBandCap, Is.EqualTo(" of £1,000."));
    }

    [Test]
    public async Task FundingBandExcessModelIsNullForCourse3()
    {
        var fixture = new DetailsViewModelMapperTestsFixture();
        var result = await fixture.Map();

        var excessModel = result.Courses.FirstOrDefault(x => x.CourseCode == "C3").FundingBandExcess;
        Assert.That(excessModel, Is.Null);
    }

    [TestCase(0, "Approve 0 apprentices' details", Party.Employer)]
    [TestCase(1, "Approve apprentice details", Party.Employer)]
    [TestCase(2, "Approve 2 apprentices' details", Party.Employer)]
    [TestCase(0, "View 0 apprentices' details", Party.Provider)]
    [TestCase(1, "View apprentice details", Party.Provider)]
    [TestCase(2, "View 2 apprentices' details", Party.Provider)]
    [TestCase(0, "View 0 apprentices' details", Party.TransferSender)]
    [TestCase(1, "View apprentice details", Party.TransferSender)]
    [TestCase(2, "View 2 apprentices' details", Party.TransferSender)]
    public async Task PageTitleIsSetCorrectlyForTheNumberOfApprenticeships(int numberOfApprenticeships, string expectedPageTitle, Party withParty)
    {
        var fixture = new DetailsViewModelMapperTestsFixture().CreateThisNumberOfApprenticeships(numberOfApprenticeships);
        fixture.Cohort.WithParty = withParty;

        var result = await fixture.Map();

        Assert.That(result.PageTitle, Is.EqualTo(expectedPageTitle));
    }

    [TestCase("C2", "1 apprenticeship above funding band maximum")]
    [TestCase("C1", "2 apprenticeships above funding band maximum")]
    public async Task FundingBandCapExcessHeaderIsSetCorrectlyForTheNumberOfApprenticeshipsOverFundingCap(string courseCode, string expectedFundingBandCapExcessHeader)
    {
        var fixture = new DetailsViewModelMapperTestsFixture();
        var result = await fixture.Map();

        Assert.That(result.Courses.FirstOrDefault(x => x.CourseCode == courseCode).FundingBandExcess.FundingBandCapExcessHeader, Is.EqualTo(expectedFundingBandCapExcessHeader));
    }

    [TestCase("C2", "The price for this apprenticeship is above its")]
    [TestCase("C1", "The price for these apprenticeships is above the")]
    public async Task FundingBandCapExcessLabelIsSetCorrectlyForTheNumberOfApprenticeshipsOverFundingCap(string courseCode, string expectedFundingBandCapExcessLabel)
    {
        var fixture = new DetailsViewModelMapperTestsFixture();
        var result = await fixture.Map();

        Assert.That(result.Courses.FirstOrDefault(x => x.CourseCode == courseCode).FundingBandExcess.FundingBandCapExcessLabel, Is.EqualTo(expectedFundingBandCapExcessLabel));
    }

    [TestCase(true, true)]
    [TestCase(false, false)]
    public async Task IsAgreementSignedIsMappedCorrectlyWithATransfer(bool isAgreementSigned, bool expectedIsAgreementSigned)
    {
        var fixture = new DetailsViewModelMapperTestsFixture();
        fixture.SetTransferSender().SetIsAgreementSigned(isAgreementSigned);
        var result = await fixture.Map();
        Assert.That(result.IsAgreementSigned, Is.EqualTo(expectedIsAgreementSigned));
    }

    [TestCase(true, true)]
    [TestCase(false, false)]
    public async Task IsAgreementSignedIsMappedCorrectlyWithoutATransfer(bool isAgreementSigned, bool expectedIsAgreementSigned)
    {
        var fixture = new DetailsViewModelMapperTestsFixture();
        fixture.SetIsAgreementSigned(isAgreementSigned);
        var result = await fixture.Map();
        Assert.That(result.IsAgreementSigned, Is.EqualTo(expectedIsAgreementSigned));
    }

    [TestCase(true, "Approve these details?")]
    [TestCase(false, "Choose an option")]
    public async Task OptionsTitleIsMappedCorrectlyWithATransfer(bool isAgreementSigned, string expectedOptionsTitle)
    {
        var fixture = new DetailsViewModelMapperTestsFixture();
        fixture.SetTransferSender().SetIsAgreementSigned(isAgreementSigned);
        var result = await fixture.Map();
        Assert.That(result.OptionsTitle, Is.EqualTo(expectedOptionsTitle));
    }

    [TestCase(true, "Approve these details?")]
    [TestCase(false, "Choose an option")]
    public async Task OptionsTitleIsMappedCorrectlyWithoutATransfer(bool isAgreementSigned, string expectedOptionsTitle)
    {
        var fixture = new DetailsViewModelMapperTestsFixture();
        fixture.SetIsAgreementSigned(isAgreementSigned);
        var result = await fixture.Map();
        Assert.That(result.OptionsTitle, Is.EqualTo(expectedOptionsTitle));
    }

    [TestCase(true, false)]
    [TestCase(false, true)]
    public async Task ShowViewAgreementOptionIsMappedCorrectlyWithATransfer(bool isAgreementSigned, bool expectedShowViewAgreementOption)
    {
        var fixture = new DetailsViewModelMapperTestsFixture();
        fixture.SetTransferSender().SetIsAgreementSigned(isAgreementSigned);
        var result = await fixture.Map();
        Assert.That(result.ShowViewAgreementOption, Is.EqualTo(expectedShowViewAgreementOption));
    }

    [TestCase(true, false)]
    [TestCase(false, true)]
    public async Task ShowViewAgreementOptionIsMappedCorrectlyWithoutATransfer(bool isAgreementSigned, bool expectedShowViewAgreementOption)
    {
        var fixture = new DetailsViewModelMapperTestsFixture();
        fixture.SetIsAgreementSigned(isAgreementSigned);
        var result = await fixture.Map();
        Assert.That(result.ShowViewAgreementOption, Is.EqualTo(expectedShowViewAgreementOption));
    }

    [TestCase(true, true)]
    [TestCase(false, false)]
    public async Task ShowApprovalOptionIsMappedCorrectlyWithATransfer(bool isAgreementSigned, bool expectedShowApprovalOption)
    {
        var fixture = new DetailsViewModelMapperTestsFixture();
        fixture.SetTransferSender().SetIsAgreementSigned(isAgreementSigned);
        var result = await fixture.Map();
        Assert.That(result.EmployerCanApprove, Is.EqualTo(expectedShowApprovalOption));
    }

    [TestCase(true, false)]
    [TestCase(false, true)]
    public async Task ShowAddAnotherApprenticeOptionIsMappedCorrectly(bool isChangeOfParty, bool expectedShowAddAnotherOption)
    {
        var fixture = new DetailsViewModelMapperTestsFixture();
        fixture.SetIsChangeOfParty(isChangeOfParty);
        var result = await fixture.Map();
        Assert.That(result.ShowAddAnotherApprenticeOption, Is.EqualTo(expectedShowAddAnotherOption));
    }

    [TestCase(true, true)]
    [TestCase(false, false)]
    public async Task ShowApprovalOptionIsMappedCorrectlyWithoutATransfer(bool isAgreementSigned, bool expectedShowApprovalOption)
    {
        var fixture = new DetailsViewModelMapperTestsFixture();
        fixture.SetIsAgreementSigned(isAgreementSigned);
        var result = await fixture.Map();
        Assert.That(result.EmployerCanApprove, Is.EqualTo(expectedShowApprovalOption));
    }

    [TestCase(true, true, false)]
    [TestCase(true, false, true)]
    [TestCase(false, true, false)]
    [TestCase(false, false, false)]
    public async Task ShowGotoHomePageOptionIsMappedCorrectly(bool isAgreementSigned, bool isEmployerComplete, bool expected)
    {
        var fixture = new DetailsViewModelMapperTestsFixture();
        fixture.SetIsAgreementSigned(isAgreementSigned);
        fixture.SetEmployerComplete(isEmployerComplete);
        var result = await fixture.Map();
        Assert.That(result.ShowGotoHomePageOption, Is.EqualTo(expected));
    }

    [TestCase(true, true, true, false)]
    [TestCase(false, false, true, false)]
    [TestCase(true, true, true, false)]
    [TestCase(true, true, false, true)]
    [TestCase(false, false, false, false)]
    public async Task ShowApprovalOptionMessageIsMappedCorrectlyWithATransfer(bool isAgreementSigned, bool showApprovalOption,
        bool isApprovedByProvider, bool expectedShowApprovalOptionMessage)
    {
        var fixture = new DetailsViewModelMapperTestsFixture();
        fixture.Cohort.IsApprovedByProvider = isApprovedByProvider;
        fixture.SetTransferSender().SetIsAgreementSigned(isAgreementSigned);
        var result = await fixture.Map();
        Assert.That(result.ShowApprovalOptionMessage, Is.EqualTo(expectedShowApprovalOptionMessage));
    }

    [TestCase(true, true, true, false)]
    [TestCase(false, false, true, false)]
    [TestCase(true, true, true, false)]
    [TestCase(true, true, false, true)]
    [TestCase(false, false, false, false)]
    public async Task ShowApprovalOptionMessageIsMappedCorrectlyWithoutATransfer(bool isAgreementSigned, bool showApprovalOption,
        bool isApprovedByProvider, bool expectedShowApprovalOptionMessage)
    {
        var fixture = new DetailsViewModelMapperTestsFixture();
        fixture.Cohort.IsApprovedByProvider = isApprovedByProvider;
        fixture.SetIsAgreementSigned(isAgreementSigned);
        var result = await fixture.Map();
        Assert.That(result.ShowApprovalOptionMessage, Is.EqualTo(expectedShowApprovalOptionMessage));
    }

    [Test]
    public async Task VerifyIsAgreementSignedEndpointIsCalledWithCorrectParams()
    {
        var fixture = new DetailsViewModelMapperTestsFixture();
        await fixture.Map();
        fixture.CommitmentsApiClient.Verify(x => x.IsAgreementSigned(It.Is<AgreementSignedRequest>(p => p.AccountLegalEntityId == fixture.Cohort.AccountLegalEntityId
            && p.AgreementFeatures == null), It.IsAny<CancellationToken>()));
    }

    [Test]
    public async Task VerifyIsAgreementSignedEndpointIsCalledWithCorrectParamsWhenIsFundedByTransfer()
    {
        var fixture = new DetailsViewModelMapperTestsFixture();
        fixture.SetTransferSender();
        await fixture.Map();
        fixture.CommitmentsApiClient.Verify(x => x.IsAgreementSigned(It.Is<AgreementSignedRequest>(p => p.AccountLegalEntityId == fixture.Cohort.AccountLegalEntityId
            && p.AgreementFeatures.Length == 1 && p.AgreementFeatures[0] == AgreementFeature.Transfers), It.IsAny<CancellationToken>()));
    }

    [Test]
    public async Task EmployerCanApproveIsMappedCorrectly()
    {
        var fixture = new DetailsViewModelMapperTestsFixture();
        var result = await fixture.Map();
        Assert.That(result.IsCompleteForEmployer, Is.EqualTo(fixture.Cohort.IsCompleteForEmployer));
    }

    [Test]
    public async Task FundingCapIsMappedCorrectlyForChangeOfPartyApprentice()
    {
        var result = await new DetailsViewModelMapperTestsFixture()
            .CreateThisNumberOfApprenticeships(1)
            .SetupChangeOfPartyScenario()
            .Map();

        Assert.That(result.Courses.First().DraftApprenticeships.First().FundingBandCap, Is.EqualTo(1000));
        Assert.That(result.Courses.First().DraftApprenticeships.First().ExceedsFundingBandCap, Is.EqualTo(true));
    }

    [Test]
    public async Task EmailOverlapIsMappedCorrectlyToDraftApprenticeshipAndToSummaryLine()
    {
        var f = new DetailsViewModelMapperTestsFixture().WithOneEmailOverlapping();
        var apprenticeshipId = f.EmailOverlapResponse.ApprenticeshipEmailOverlaps.First().Id;

        var result = await f.Map();
        var course = result.Courses.FirstOrDefault(x => x.DraftApprenticeships.Any(y => y.Id == apprenticeshipId));

        Assert.That(course, Is.Not.Null);
        Assert.That(course.EmailOverlaps, Is.Not.Null);
        Assert.That(course.EmailOverlaps.NumberOfEmailOverlaps, Is.EqualTo(1));
        Assert.That(course.DraftApprenticeships.Count(x => x.HasOverlappingEmail), Is.EqualTo(1));
        Assert.That(course.DraftApprenticeships.First(x => x.HasOverlappingEmail).Id, Is.EqualTo(apprenticeshipId));
    }

    [TestCase(true, true)]
    [TestCase(false, false)]
    public async Task ShowHasOverlappingUlnIsMappedCorrectlyWhenOverlap(bool hasOverlap, bool hasUlnOverlap)
    {
        var fixture = new DetailsViewModelMapperTestsFixture();
        fixture.SetUlnOverlap(hasOverlap);
        var result = await fixture.Map();

        Assert.That(result.HasOverlappingUln, Is.EqualTo(hasUlnOverlap));
    }

    [TestCase(true, false)]
    [TestCase(false, true)]
    public async Task ShowApprovalOptionIsMappedCorrectlyWhenOverlap(bool hasOverlap, bool expectedEmployerCanApprove)
    {
        var fixture = new DetailsViewModelMapperTestsFixture();
        fixture.SetIsAgreementSigned(true).SetUlnOverlap(hasOverlap);
        var result = await fixture.Map();

        Assert.That(result.EmployerCanApprove, Is.EqualTo(expectedEmployerCanApprove));
    }

    [Test]
    public async Task HasEmailOverlapsIsMappedCorrectlyWhenThereAreEmailOverlaps()
    {
        var fixture = new DetailsViewModelMapperTestsFixture().WithOneEmailOverlapping();
        var result = await fixture.Map();
        Assert.That(result.HasEmailOverlaps, Is.True);
    }

    [TestCase(nameof(DraftApprenticeshipDto.FirstName))]
    [TestCase(nameof(DraftApprenticeshipDto.LastName))]
    [TestCase(nameof(DraftApprenticeshipDto.CourseName))]
    [TestCase(nameof(DraftApprenticeshipDto.DateOfBirth))]
    [TestCase(nameof(DraftApprenticeshipDto.EndDate))]
    [TestCase(nameof(DraftApprenticeshipDto.Cost))]
    public async Task IsCompleteMappedCorrectlyWhenAMandatoryFieldIsNull(string propertyName)
    {
        var fixture = new DetailsViewModelMapperTestsFixture()
            .CreateDraftApprenticeship()
            .SetValueOfDraftApprenticeshipProperty(propertyName, null);
        var result = await fixture.Map();
        Assert.That(result.Courses.First().DraftApprenticeships.First().IsComplete, Is.False);
    }

    [Test]
    public async Task IsCompleteIsFalseWhenStartDatesAreBothNull()
    {
        var fixture = new DetailsViewModelMapperTestsFixture()
            .CreateDraftApprenticeship()
            .SetValueOfDraftApprenticeshipProperty("StartDate", null)
            .SetValueOfDraftApprenticeshipProperty("ActualStartDate", null);
        var result = await fixture.Map();
        Assert.That(result.Courses.First().DraftApprenticeships.First().IsComplete, Is.False);
    }

    [Test]
    public async Task IsCompleteMappedCorrectlyWhenAllManadatoryFieldArePresent()
    {
        var fixture = new DetailsViewModelMapperTestsFixture().CreateDraftApprenticeship();
        var result = await fixture.Map();
        Assert.That(result.Courses.First().DraftApprenticeships.First().IsComplete, Is.True);
    }

    [Test]
    public async Task Course4Has2CourseLines()
    {
        var fixture = new DetailsViewModelMapperTestsFixture();
        var result = await fixture.Map();
        Assert.That(result.Courses.Count(c => c.CourseCode == "C4"), Is.EqualTo(2));
    }

    [TestCase(DeliveryModel.PortableFlexiJob)]
    [TestCase(DeliveryModel.Regular)]
    public async Task Course4HasCorrectDeployMethod(DeliveryModel dm)
    {
        var fixture = new DetailsViewModelMapperTestsFixture();
        var result = await fixture.Map();
        Assert.That(result.Courses.Count(c => c.CourseCode == "C4" && c.DeliveryModel == dm), Is.EqualTo(1));
    }

    [TestCase(true, true)]
    [TestCase(false, false)]
    public async Task ShowRofjaaRemovalBanner(bool hasUnavailableFlexiJobAgencyDeliveryModel, bool expectShowBanner)
    {
        var fixture = new DetailsViewModelMapperTestsFixture()
            .UnavailableFlexiJobAgencyDeliveryModel(hasUnavailableFlexiJobAgencyDeliveryModel);

        var result = await fixture.Map();

        Assert.That(result.ShowRofjaaRemovalBanner, Is.EqualTo(expectShowBanner));
    }

    [Test]
    public async Task StatusIsMappedCorrectly_When_With_Employer_But_Without_Provider_Approval()
    {
        var fixture = new DetailsViewModelMapperTestsFixture()
            .CreateThisNumberOfApprenticeships(1)
            .SetCohortWithParty(Party.Employer);

        fixture.Cohort.LastAction = LastAction.Amend;

        var result = await fixture.Map();
        Assert.That(result.Status, Is.EqualTo("Ready for review"));
    }

    [Test]
    public async Task StatusIsMappedCorrectly_When_With_Employer_And_Provider_Approval()
    {
        var fixture = new DetailsViewModelMapperTestsFixture()
            .CreateThisNumberOfApprenticeships(1)
            .SetCohortWithParty(Party.Employer);

        fixture.Cohort.LastAction = LastAction.Approve;

        var result = await fixture.Map();
        Assert.That(result.Status, Is.EqualTo("Ready for approval"));
    }

    [Test]
    public async Task StatusIsMappedCorrectly_When_WithProvider_And_Cohort_Not_Approved_By_Employer()
    {
        var fixture = new DetailsViewModelMapperTestsFixture()
            .CreateThisNumberOfApprenticeships(1)
            .SetCohortWithParty(Party.Provider);

        fixture.Cohort.LastAction = LastAction.Amend;

        var result = await fixture.Map();
        Assert.That(result.Status, Is.EqualTo("Under review with provider"));
    }

    [Test]
    public async Task StatusIsMappedCorrectly_When_WithProvider_And_Cohort_Approved_By_Employer()
    {
        var fixture = new DetailsViewModelMapperTestsFixture()
            .CreateThisNumberOfApprenticeships(1)
            .SetCohortWithParty(Party.Provider);

        fixture.Cohort.LastAction = LastAction.Approve;

        var result = await fixture.Map();
        Assert.That(result.Status, Is.EqualTo("With provider for approval"));
    }

    [Test]
    public async Task StatusIsMappedCorrectly_When_WithEmployer_And_New_Cohort()
    {
        var fixture = new DetailsViewModelMapperTestsFixture()
            .CreateThisNumberOfApprenticeships(1)
            .SetCohortWithParty(Party.Employer);

        fixture.Cohort.LastAction = LastAction.None;

        var result = await fixture.Map();
        Assert.That(result.Status, Is.EqualTo("New request"));
    }

    [Test]
    public async Task StatusIsMappedCorrectly_When_PendingApproval_From_TransferSender()
    {
        var fixture = new DetailsViewModelMapperTestsFixture()
            .CreateThisNumberOfApprenticeships(1)
            .SetTransferSender()
            .SetCohortWithParty(Party.TransferSender);

        fixture.Cohort.IsApprovedByEmployer = fixture.Cohort.IsApprovedByProvider = true;
        fixture.Cohort.TransferApprovalStatus = TransferApprovalStatus.Pending;

        var result = await fixture.Map();
        Assert.That(result.Status, Is.EqualTo("Pending - with funding employer"));
    }

    [Test]
    public async Task StatusIsMappedCorrectly_When_rejected_From_TransferSender()
    {
        var fixture = new DetailsViewModelMapperTestsFixture()
            .CreateThisNumberOfApprenticeships(1)
            .SetTransferSender()
            .SetCohortWithParty(Party.TransferSender)
            .SetTransferApprovalStatus(TransferApprovalStatus.Rejected)
            .SetCohortApprovedStatus(true);

        var result = await fixture.Map();
        Assert.That(result.Status, Is.EqualTo("Rejected by transfer sending employer"));
    }

    [Test]
    public async Task StatusIsMappedCorrectly_When_WithEmployer_And_Provider_AmendRejected_Cohort()
    {
        var fixture = new DetailsViewModelMapperTestsFixture()
            .CreateThisNumberOfApprenticeships(1)
            .SetCohortWithParty(Party.Employer);

        fixture.Cohort.LastAction = LastAction.AmendAfterRejected;

        var result = await fixture.Map();
        Assert.That(result.Status, Is.EqualTo("Ready for review"));
    }

    [Test]
    public async Task StatusIsMappedCorrectly_When_WithProvider_And_Employer_AmendRejected_Cohort()
    {
        var fixture = new DetailsViewModelMapperTestsFixture()
            .CreateThisNumberOfApprenticeships(1)
            .SetCohortWithParty(Party.Provider);

        fixture.Cohort.LastAction = LastAction.AmendAfterRejected;

        var result = await fixture.Map();
        Assert.That(result.Status, Is.EqualTo("Under review with provider"));
    }
}

public class DetailsViewModelMapperTestsFixture
{
    public DetailsViewModelMapper Mapper;
    public DetailsRequest Source;
    public DetailsViewModel Result;
    public Mock<ICommitmentsApiClient> CommitmentsApiClient;
    public Mock<IApprovalsApiClient> ApprovalsApiClient;
    public Mock<IEncodingService> EncodingService;
    public GetCohortResponse Cohort;
    public GetCohortDetailsResponse CohortDetails;
    public GetDraftApprenticeshipsResponse DraftApprenticeshipsResponse;
    public DateTime DefaultStartDate = new DateTime(2019, 10, 1);
    public AccountLegalEntityResponse AccountLegalEntityResponse;
    public LegalEntityViewModel LegalEntityViewModel;
    public GetEmailOverlapsResponse EmailOverlapResponse;

    private Fixture _autoFixture;
    private TrainingProgramme _trainingProgramme;
    private List<TrainingProgrammeFundingPeriod> _fundingPeriods;
    private DateTime _startFundingPeriod = new DateTime(2019, 10, 1);
    private DateTime _endFundingPeriod = new DateTime(2019, 10, 30);

    public DetailsViewModelMapperTestsFixture()
    {
        _autoFixture = new Fixture();

        CohortDetails = _autoFixture.Build<GetCohortDetailsResponse>()
            .With(x => x.HasUnavailableFlexiJobAgencyDeliveryModel, false).Create();
        Cohort = _autoFixture.Build<GetCohortResponse>().Without(x => x.TransferSenderId).Without(x => x.ChangeOfPartyRequestId).Create();
        AccountLegalEntityResponse = _autoFixture.Create<AccountLegalEntityResponse>();
        LegalEntityViewModel = _autoFixture.Create<LegalEntityViewModel>();
        EmailOverlapResponse = new GetEmailOverlapsResponse { ApprenticeshipEmailOverlaps = new List<ApprenticeshipEmailOverlap>() };

        var draftApprenticeships = CreateDraftApprenticeshipDtos(_autoFixture);
        _autoFixture.Register(() => draftApprenticeships);
        DraftApprenticeshipsResponse = _autoFixture.Create<GetDraftApprenticeshipsResponse>();

        ApprovalsApiClient = new Mock<IApprovalsApiClient>();
        ApprovalsApiClient.Setup(x =>
                x.GetCohortDetails(It.IsAny<long>(), It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(CohortDetails);

        CommitmentsApiClient = new Mock<ICommitmentsApiClient>();
        CommitmentsApiClient.Setup(x => x.GetCohort(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Cohort);
        CommitmentsApiClient.Setup(x => x.GetDraftApprenticeships(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(() => DraftApprenticeshipsResponse);
        CommitmentsApiClient.Setup(x => x.GetAccountLegalEntity(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(AccountLegalEntityResponse);
        CommitmentsApiClient.Setup(x => x.GetEmailOverlapChecks(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(EmailOverlapResponse);

        _fundingPeriods = new List<TrainingProgrammeFundingPeriod>
        {
            new TrainingProgrammeFundingPeriod{ EffectiveFrom = _startFundingPeriod, EffectiveTo = _endFundingPeriod, FundingCap = 1000},
            new TrainingProgrammeFundingPeriod{ EffectiveFrom = _startFundingPeriod.AddMonths(1), EffectiveTo = _endFundingPeriod.AddMonths(1), FundingCap = 500}
        };
        _trainingProgramme = new TrainingProgramme { EffectiveFrom = DefaultStartDate, EffectiveTo = DefaultStartDate.AddYears(1), FundingPeriods = _fundingPeriods };

        CommitmentsApiClient.Setup(x => x.GetTrainingProgramme(It.Is<string>(c => !string.IsNullOrEmpty(c)), CancellationToken.None))
            .ReturnsAsync(new GetTrainingProgrammeResponse { TrainingProgramme = _trainingProgramme });
        CommitmentsApiClient.Setup(x => x.GetTrainingProgramme("no-course", CancellationToken.None))
            .ThrowsAsync(new RestHttpClientException(new HttpResponseMessage(HttpStatusCode.NotFound)
            {
                RequestMessage = new HttpRequestMessage(),
                ReasonPhrase = "Url not found"
            }, "Course not found"));
        CommitmentsApiClient.Setup(x => x.ValidateUlnOverlap(It.IsAny<ValidateUlnOverlapRequest>(), CancellationToken.None))
            .ReturnsAsync(new ValidateUlnOverlapResult { HasOverlappingEndDate = false, HasOverlappingStartDate = false });

        EncodingService = new Mock<IEncodingService>();
        SetEncodingOfApprenticeIds();

        Mapper = new DetailsViewModelMapper(CommitmentsApiClient.Object, EncodingService.Object, ApprovalsApiClient.Object);
        Source = _autoFixture.Create<DetailsRequest>();
    }

    public DetailsViewModelMapperTestsFixture SetCohortWithParty(Party party)
    {
        Cohort.WithParty = party;
        return this;
    }

    public DetailsViewModelMapperTestsFixture SetTransferSenderIdAndItsExpectedHashedValue(long? transferSenderId, string expectedHashedId)
    {
        Cohort.TransferSenderId = transferSenderId;
        if (transferSenderId.HasValue)
        {
            EncodingService.Setup(x => x.Encode(transferSenderId.Value, EncodingType.PublicAccountId))
                .Returns(expectedHashedId);
        }

        return this;
    }

    public DetailsViewModelMapperTestsFixture SetEncodingOfApprenticeIds()
    {
        EncodingService.Setup(x => x.Encode(It.IsAny<long>(), EncodingType.ApprenticeshipId))
            .Returns((long p, EncodingType t) => $"X{p}X");

        return this;
    }

    public DetailsViewModelMapperTestsFixture CreateThisNumberOfApprenticeships(int numberOfApprenticeships)
    {
        var draftApprenticeships = _autoFixture.CreateMany<DraftApprenticeshipDto>(numberOfApprenticeships).ToArray();
        DraftApprenticeshipsResponse.DraftApprenticeships = draftApprenticeships;
        return this;
    }

    public DetailsViewModelMapperTestsFixture WithOneEmailOverlapping()
    {
        var first = DraftApprenticeshipsResponse.DraftApprenticeships.First();
        var emailOverlap = _autoFixture.Build<ApprenticeshipEmailOverlap>().With(x => x.Id, first.Id).Create();
        EmailOverlapResponse.ApprenticeshipEmailOverlaps = new List<ApprenticeshipEmailOverlap> { emailOverlap };

        return this;
    }

    public DetailsViewModelMapperTestsFixture SetValueOfDraftApprenticeshipProperty(string propertyName, object value)
    {
        var draftApprenticeship = DraftApprenticeshipsResponse.DraftApprenticeships.First();
        if (!string.IsNullOrWhiteSpace(propertyName))
        {
            PropertyInfo propertyInfo = draftApprenticeship.GetType().GetProperty(propertyName);
            // make sure object has the property we are after
            if (propertyInfo != null)
            {
                propertyInfo.SetValue(draftApprenticeship, value, null);
            }
        }

        return this;
    }

    public DetailsViewModelMapperTestsFixture CreateDraftApprenticeship(
        Func<IPostprocessComposer<DraftApprenticeshipDto>, IPostprocessComposer<DraftApprenticeshipDto>> build = null)
    {
        build ??= x => x;
        var draftApprenticeship = build(_autoFixture.Build<DraftApprenticeshipDto>()).Create();

        DraftApprenticeshipsResponse.DraftApprenticeships = new List<DraftApprenticeshipDto>() { draftApprenticeship };
        return this;
    }

    public DetailsViewModelMapperTestsFixture WithTwoEmailOverlappingOnSameCourse()
    {
        var draftApprenticeships = _autoFixture.CreateMany<DraftApprenticeshipDto>(5).ToArray();
        foreach (var draftApprenticeship in draftApprenticeships)
        {
            draftApprenticeship.CourseCode = "ABC";
            draftApprenticeship.CourseName = "ABC Name";
            draftApprenticeship.DeliveryModel = DeliveryModel.Regular;
        }
        DraftApprenticeshipsResponse.DraftApprenticeships = draftApprenticeships;
        var first = DraftApprenticeshipsResponse.DraftApprenticeships.First();
        var last = DraftApprenticeshipsResponse.DraftApprenticeships.Last();
        var emailOverlap1 = _autoFixture.Build<ApprenticeshipEmailOverlap>().With(x => x.Id, first.Id).Create();
        var emailOverlap2 = _autoFixture.Build<ApprenticeshipEmailOverlap>().With(x => x.Id, last.Id).Create();
        EmailOverlapResponse.ApprenticeshipEmailOverlaps = new List<ApprenticeshipEmailOverlap> { emailOverlap1, emailOverlap2 };

        return this;
    }

    public Task<DetailsViewModel> Map()
    {
        return Mapper.Map(TestHelper.Clone(Source));
    }

    public void AssertEquality(DraftApprenticeshipDto source, CohortDraftApprenticeshipViewModel result)
    {
        Assert.That(result.Id, Is.EqualTo(source.Id));
        Assert.That(result.FirstName, Is.EqualTo(source.FirstName));
        Assert.That(result.LastName, Is.EqualTo(source.LastName));
        Assert.That(result.DateOfBirth, Is.EqualTo(source.DateOfBirth));
        Assert.That(result.Cost, Is.EqualTo(source.Cost));
        Assert.That(result.StartDate, Is.EqualTo(source.StartDate));
        Assert.That(result.EndDate, Is.EqualTo(source.EndDate));
        Assert.That(result.DraftApprenticeshipHashedId, Is.EqualTo($"X{source.Id}X"));
    }

    public void AssertSequenceOrder<T>(List<T> expected, List<T> actual, Func<T, T, bool> evaluator)
    {
        Assert.That(actual.Count, Is.EqualTo(expected.Count), "Expected and actual sequences are different lengths");

        for (int i = 0; i < actual.Count; i++)
        {
            Assert.That(evaluator(expected[i], actual[i]), Is.True, "Actual sequence are not in same order as expected");
        }
    }

    private IReadOnlyCollection<DraftApprenticeshipDto> CreateDraftApprenticeshipDtos(Fixture autoFixture)
    {
        var draftApprenticeships = autoFixture.CreateMany<DraftApprenticeshipDto>(8).ToArray();
        SetCourseDetails(draftApprenticeships[0], "Course1", "C1", 1000);
        SetCourseDetails(draftApprenticeships[1], "Course1", "C1", 2100);
        SetCourseDetails(draftApprenticeships[2], "Course1", "C1", 2000, DefaultStartDate.AddMonths(1));

        SetCourseDetails(draftApprenticeships[3], "Course2", "C2", 1500);
        SetCourseDetails(draftApprenticeships[4], "Course2", "C2", null);

        SetCourseDetails(draftApprenticeships[5], "Course3", "C3", null, DefaultStartDate.AddMonths(2));

        SetCourseDetails(draftApprenticeships[6], "Course4", "C4", null, null, null, DeliveryModel.PortableFlexiJob);
        SetCourseDetails(draftApprenticeships[7], "Course4", "C4", null, null, null, DeliveryModel.Regular);

        return draftApprenticeships;
    }

    private void SetCourseDetails(DraftApprenticeshipDto draftApprenticeship, string courseName, string courseCode, int? cost, DateTime? startDate = null, DateTime? originalStartDate = null, DeliveryModel dm = DeliveryModel.Regular)
    {
        startDate = startDate ?? DefaultStartDate;

        draftApprenticeship.CourseName = courseName;
        draftApprenticeship.CourseCode = courseCode;
        draftApprenticeship.Cost = cost;
        draftApprenticeship.StartDate = startDate;
        draftApprenticeship.OriginalStartDate = originalStartDate;
        draftApprenticeship.DeliveryModel = dm;
    }

    public DetailsViewModelMapperTestsFixture SetIsAgreementSigned(bool isAgreementSigned)
    {
        CommitmentsApiClient
            .Setup(x => x.IsAgreementSigned(It.IsAny<AgreementSignedRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(isAgreementSigned);

        return this;
    }

    public DetailsViewModelMapperTestsFixture SetEmployerComplete(bool employerComplete)
    {
        Cohort.IsCompleteForEmployer = employerComplete;
        return this;
    }

    public DetailsViewModelMapperTestsFixture SetTransferApprovalStatus(TransferApprovalStatus transferApprovalStatus)
    {
        Cohort.TransferApprovalStatus = transferApprovalStatus; ;
        return this;
    }

    public DetailsViewModelMapperTestsFixture SetCohortApprovedStatus(bool isApproved)
    {
        Cohort.IsApprovedByEmployer = Cohort.IsApprovedByProvider = isApproved; ;
        return this;
    }
       
    public DetailsViewModelMapperTestsFixture SetTransferSender()
    {
        Cohort.TransferSenderId = _autoFixture.Create<long>();
        return this;
    }

    public DetailsViewModelMapperTestsFixture SetupChangeOfPartyScenario()
    {
        Cohort.ChangeOfPartyRequestId = 1;
        var draftApprenticeship = DraftApprenticeshipsResponse.DraftApprenticeships.First();
        draftApprenticeship.OriginalStartDate = _fundingPeriods.First().EffectiveFrom;
        draftApprenticeship.StartDate = _fundingPeriods.Last().EffectiveFrom;
        draftApprenticeship.Cost = _fundingPeriods.First().FundingCap + 500;
        return this;
    }

    public DetailsViewModelMapperTestsFixture SetIsChangeOfParty(bool isChangeOfParty)
    {
        Cohort.ChangeOfPartyRequestId = isChangeOfParty ? _autoFixture.Create<long>() : default(long?);

        return this;
    }

    public DetailsViewModelMapperTestsFixture SetNoCourse()
    {
        DraftApprenticeshipsResponse.DraftApprenticeships = DraftApprenticeshipsResponse.DraftApprenticeships.Select(c =>
        {
            c.CourseCode = "no-course";
            return c;
        }).ToList();
        return this;
    }

    public DetailsViewModelMapperTestsFixture SetNoCourseSet()
    {
        DraftApprenticeshipsResponse.DraftApprenticeships = DraftApprenticeshipsResponse.DraftApprenticeships.Select(c =>
        {
            c.CourseCode = "";
            return c;
        }).ToList();
        return this;
    }

    internal DetailsViewModelMapperTestsFixture SetUlnOverlap(bool hasOverlap)
    {
        CommitmentsApiClient.Setup(x => x.ValidateUlnOverlap(It.IsAny<ValidateUlnOverlapRequest>(), CancellationToken.None))
            .ReturnsAsync(new ValidateUlnOverlapResult { HasOverlappingEndDate = hasOverlap, HasOverlappingStartDate = hasOverlap });

        return this;
    }

    public DetailsViewModelMapperTestsFixture UnavailableFlexiJobAgencyDeliveryModel(bool hasUnavailableFlexiJobAgencyDeliveryModel)
    {
        CohortDetails.HasUnavailableFlexiJobAgencyDeliveryModel = hasUnavailableFlexiJobAgencyDeliveryModel;
        return this;
    }
}