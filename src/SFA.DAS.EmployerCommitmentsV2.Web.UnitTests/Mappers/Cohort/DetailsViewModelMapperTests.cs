using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.Apprenticeships.Api.Client;
using SFA.DAS.Apprenticeships.Api.Types;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.CommitmentsV2.Types.Dtos;
using SFA.DAS.EAS.Account.Api.Client;
using SFA.DAS.EAS.Account.Api.Types;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.Encoding;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Cohort
{
    [TestFixture]
    [Parallelizable(ParallelScope.All)]
    public class DetailsViewModelMapperTests
    {
        [Test]
        public async Task AccountHashedIdIsMappedCorrectly()
        {
            var fixture = new DetailsViewModelMapperTestsFixture();
            var result = await fixture.Map();
            Assert.AreEqual(fixture.Source.AccountHashedId, result.AccountHashedId);
        }

        [Test]
        public async Task WithPartyIsMappedCorrectly()
        {
            var fixture = new DetailsViewModelMapperTestsFixture();
            var result = await fixture.Map();
            Assert.AreEqual(fixture.Cohort.WithParty, result.WithParty);
        }

        [Test]
        public async Task LegalEntityNameIsMappedCorrectly()
        {
            var fixture = new DetailsViewModelMapperTestsFixture();
            var result = await fixture.Map();
            Assert.AreEqual(fixture.Cohort.LegalEntityName, result.LegalEntityName);
        }

        [Test]
        public async Task ProviderNameIsMappedCorrectly()
        {
            var fixture = new DetailsViewModelMapperTestsFixture();
            var result = await fixture.Map();
            Assert.AreEqual(fixture.Cohort.ProviderName, result.ProviderName);
        }

        [Test]
        public async Task MessageIsMappedCorrectly()
        {
            var fixture = new DetailsViewModelMapperTestsFixture();
            var result = await fixture.Map();
            Assert.AreEqual(fixture.Cohort.LatestMessageCreatedByProvider, result.Message);
        }

        [TestCase(true, true, "No, request changes from training provider")]
        [TestCase(true, false, "Request changes from training provider")]
        [TestCase(false, true, "Send to the training provider to review or add details")]
        [TestCase(false, false, "Send to the training provider to review or add details")]
        public async Task SendBackToProviderOptionMessageIsMappedCorrectly(bool isAgreementSigned, bool employerComplete, string expected)
        {
            var fixture = new DetailsViewModelMapperTestsFixture().SetIsAgreementSigned(isAgreementSigned).SetEmployerComplete(employerComplete);
            var result = await fixture.Map();
            Assert.AreEqual(expected, result.SendBackToProviderOptionMessage);
        }

        [Test]
        public async Task CohortReferenceIsMappedCorrectly()
        {
            var fixture = new DetailsViewModelMapperTestsFixture();
            var result = await fixture.Map();
            Assert.AreEqual(fixture.Source.CohortReference, result.CohortReference);
        }

        [Test]
        public async Task IsApprovedByProviderIsMappedCorrectly()
        {
            var fixture = new DetailsViewModelMapperTestsFixture();
            var result = await fixture.Map();
            Assert.AreEqual(fixture.Cohort.IsApprovedByProvider, result.IsApprovedByProvider);
        }

        [Test]
        public async Task TransferSenderHashedIdIsEncodedCorrectlyWhenThereIsAValue()
        {
            var fixture = new DetailsViewModelMapperTestsFixture().SetTransferSenderIdAndItsExpectedHashedValue(123, "X123X");
            var result = await fixture.Map();
            Assert.AreEqual("X123X", result.TransferSenderHashedId);
        }

        [Test]
        public async Task TransferSenderHashedIdIsNullWhenThereIsNoValue()
        {
            var fixture = new DetailsViewModelMapperTestsFixture().SetTransferSenderIdAndItsExpectedHashedValue(null, null);
            var result = await fixture.Map();
            Assert.IsNull(result.TransferSenderHashedId);
        }

        [Test]
        public async Task DraftApprenticeshipTotalCountIsReportedCorrectly()
        {
            var fixture = new DetailsViewModelMapperTestsFixture();
            var result = await fixture.Map();
            Assert.AreEqual(fixture.DraftApprenticeshipsResponse.DraftApprenticeships.Count, result.DraftApprenticeshipsCount);
        }

        [Test]
        public async Task DraftApprenticeshipCourseCountIsReportedCorrectly()
        {
            var fixture = new DetailsViewModelMapperTestsFixture();
            var result = await fixture.Map();

            foreach (var course in result.Courses)
            {
                var expectedCount = fixture.DraftApprenticeshipsResponse.DraftApprenticeships
                    .Count(a => a.CourseCode == course.CourseCode && a.CourseName == course.CourseName);

                Assert.AreEqual(expectedCount, course.Count);
            }
        }

        [Test]
        public async Task DraftApprenticeshipCourseOrderIsByCourseName()
        {
            var fixture = new DetailsViewModelMapperTestsFixture();
            var result = await fixture.Map();

            var expectedSequence = fixture.DraftApprenticeshipsResponse.DraftApprenticeships
                .Select(c => new { c.CourseName, c.CourseCode })
                .Distinct()
                .OrderBy(c => c.CourseName).ThenBy(c => c.CourseCode)
                .ToList();

            var actualSequence = result.Courses
                .Select(c => new { c.CourseName, c.CourseCode })
                .OrderBy(c => c.CourseName).ThenBy(c => c.CourseCode)
                .ToList();

            fixture.AssertSequenceOrder(expectedSequence, actualSequence, (e, a) => e.CourseName == a.CourseName && e.CourseCode == a.CourseCode);
        }

        [Test]
        public async Task DraftApprenticesOrderIsByApprenticeName()
        {
            var fixture = new DetailsViewModelMapperTestsFixture();
            var result = await fixture.Map();

            foreach (var course in result.Courses)
            {
                var expectedSequence = fixture.DraftApprenticeshipsResponse.DraftApprenticeships
                    .Where(a => a.CourseName == course.CourseName && a.CourseCode == course.CourseCode)
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

                Assert.AreEqual(1000, draftApprenticeshipResult.FundingBandCap);
            }
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

                Assert.AreEqual(null, draftApprenticeshipResult.FundingBandCap);
            }
        }

        [Test]
        public async Task FundingBandExcessModelShowsTwoApprenticeshipsExceedingTheBandForCourse1()
        {
            var fixture = new DetailsViewModelMapperTestsFixture();
            var result = await fixture.Map();

            var excessModel = result.Courses.FirstOrDefault(x => x.CourseCode == "C1").FundingBandExcess;
            Assert.AreEqual(2, excessModel.NumberOfApprenticesExceedingFundingBandCap);
        }

        [Test]
        public async Task FundingBandExcessModelShowsOnlyTheFullStopWhenMultipleFundingCapsAreExceeded()
        {
            var fixture = new DetailsViewModelMapperTestsFixture();
            var result = await fixture.Map();

            var excessModel = result.Courses.FirstOrDefault(x => x.CourseCode == "C1").FundingBandExcess;
            Assert.AreEqual(".", excessModel.DisplaySingleFundingBandCap);
        }

        [Test]
        public async Task FundingBandExcessModelShowsOneApprenticeshipExceedingTheBandForCourse2()
        {
            var fixture = new DetailsViewModelMapperTestsFixture();
            var result = await fixture.Map();

            var excessModel = result.Courses.FirstOrDefault(x => x.CourseCode == "C2").FundingBandExcess;
            Assert.AreEqual(1, excessModel.NumberOfApprenticesExceedingFundingBandCap);
        }

        [Test]
        public async Task FundingBandExcessModelShowsTheSingleFundingBandCapExceeded()
        {
            var fixture = new DetailsViewModelMapperTestsFixture();
            var result = await fixture.Map();

            var excessModel = result.Courses.FirstOrDefault(x => x.CourseCode == "C2").FundingBandExcess;
            Assert.AreEqual(" of £1,000.", excessModel.DisplaySingleFundingBandCap);
        }

        [Test]
        public async Task FundingBandExcessModelIsNullForCourse3()
        {
            var fixture = new DetailsViewModelMapperTestsFixture();
            var result = await fixture.Map();

            var excessModel = result.Courses.FirstOrDefault(x => x.CourseCode == "C3").FundingBandExcess;
            Assert.IsNull(excessModel);
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

            Assert.AreEqual(expectedPageTitle, result.PageTitle);
        }

        [TestCase("C2", "1 apprenticeship above funding band maximum")]
        [TestCase("C1", "2 apprenticeships above funding band maximum")]
        public async Task FundingBandCapExcessHeaderIsSetCorrectlyForTheNumberOfApprenticeshipsOverFundingCap(string courseCode, string expectedFundingBandCapExcessHeader)
        {
            var fixture = new DetailsViewModelMapperTestsFixture();
            var result = await fixture.Map();

            Assert.AreEqual(expectedFundingBandCapExcessHeader, result.Courses.FirstOrDefault(x => x.CourseCode == courseCode).FundingBandExcess.FundingBandCapExcessHeader);
        }

        [TestCase("C2", "The price for this apprenticeship is above its")]
        [TestCase("C1", "The price for these apprenticeships is above the")]
        public async Task FundingBandCapExcessLabelIsSetCorrectlyForTheNumberOfApprenticeshipsOverFundingCap(string courseCode, string expectedFundingBandCapExcessLabel)
        {
            var fixture = new DetailsViewModelMapperTestsFixture();
            var result = await fixture.Map();

            Assert.AreEqual(expectedFundingBandCapExcessLabel, result.Courses.FirstOrDefault(x => x.CourseCode == courseCode).FundingBandExcess.FundingBandCapExcessLabel);
        }

        [TestCase(true, true)]
        [TestCase(false, false)]
        public async Task IsAgreementSignedIsMappedCorrectlyWithATransfer(bool isAgreementSigned,  bool expectedIsAgreementSigned)
        {
            var fixture = new DetailsViewModelMapperTestsFixture();
            fixture.SetTransferSender().SetIsAgreementSigned(isAgreementSigned);
            var result = await fixture.Map();
            Assert.AreEqual(expectedIsAgreementSigned, result.IsAgreementSigned);
        }

        [TestCase(true, true)]
        [TestCase(false, false)]
        public async Task IsAgreementSignedIsMappedCorrectlyWithoutATransfer(bool isAgreementSigned, bool expectedIsAgreementSigned)
        {
            var fixture = new DetailsViewModelMapperTestsFixture();
            fixture.SetIsAgreementSigned(isAgreementSigned);
            var result = await fixture.Map();
            Assert.AreEqual(expectedIsAgreementSigned, result.IsAgreementSigned);
        }

        [TestCase(true, "Approve these details?")]
        [TestCase(false, "Choose an option")]
        public async Task OptionsTitleIsMappedCorrectlyWithATransfer(bool isAgreementSigned, string expectedOptionsTitle)
        {
            var fixture = new DetailsViewModelMapperTestsFixture();
            fixture.SetTransferSender().SetIsAgreementSigned(isAgreementSigned);
            var result = await fixture.Map();
            Assert.AreEqual(expectedOptionsTitle, result.OptionsTitle);
        }

        [TestCase(true, "Approve these details?")]
        [TestCase(false, "Choose an option")]
        public async Task OptionsTitleIsMappedCorrectlyWithoutATransfer(bool isAgreementSigned, string expectedOptionsTitle)
        {
            var fixture = new DetailsViewModelMapperTestsFixture();
            fixture.SetIsAgreementSigned(isAgreementSigned);
            var result = await fixture.Map();
            Assert.AreEqual(expectedOptionsTitle, result.OptionsTitle);
        }

        [TestCase(true, false)]
        [TestCase(false, true)]
        public async Task ShowViewAgreementOptionIsMappedCorrectlyWithATransfer(bool isAgreementSigned, bool expectedShowViewAgreementOption)
        {
            var fixture = new DetailsViewModelMapperTestsFixture();
            fixture.SetTransferSender().SetIsAgreementSigned(isAgreementSigned);
            var result = await fixture.Map();
            Assert.AreEqual(expectedShowViewAgreementOption, result.ShowViewAgreementOption);
        }

        [TestCase(true, false)]
        [TestCase(false, true)]
        public async Task ShowViewAgreementOptionIsMappedCorrectlyWithoutATransfer(bool isAgreementSigned, bool expectedShowViewAgreementOption)
        {
            var fixture = new DetailsViewModelMapperTestsFixture();
            fixture.SetIsAgreementSigned(isAgreementSigned);
            var result = await fixture.Map();
            Assert.AreEqual(expectedShowViewAgreementOption, result.ShowViewAgreementOption);
        }

        [TestCase(true, true)]
        [TestCase(false, false)]
        public async Task ShowApprovalOptionIsMappedCorrectlyWithATransfer(bool isAgreementSigned, bool expectedShowApprovalOption)
        {
            var fixture = new DetailsViewModelMapperTestsFixture();
            fixture.SetTransferSender().SetIsAgreementSigned(isAgreementSigned);
            var result = await fixture.Map();
            Assert.AreEqual(expectedShowApprovalOption, result.EmployerCanApprove);
        }

        [TestCase(true, false)]
        [TestCase(false, true)]
        public async Task ShowAddAnotherApprenticeOptionIsMappedCorrectly(bool isChangeOfParty, bool expectedShowAddAnotherOption)
        {
            var fixture = new DetailsViewModelMapperTestsFixture();
            fixture.SetIsChangeOfParty(isChangeOfParty);
            var result = await fixture.Map();
            Assert.AreEqual(expectedShowAddAnotherOption, result.ShowAddAnotherApprenticeOption);
        }

        [TestCase(true, true)]
        [TestCase(false,  false)]
        public async Task ShowApprovalOptionIsMappedCorrectlyWithoutATransfer(bool isAgreementSigned, bool expectedShowApprovalOption)
        {
            var fixture = new DetailsViewModelMapperTestsFixture();
            fixture.SetIsAgreementSigned(isAgreementSigned);
            var result = await fixture.Map();
            Assert.AreEqual(expectedShowApprovalOption, result.EmployerCanApprove);
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
            Assert.AreEqual(expected, result.ShowGotoHomePageOption);
        }

        [TestCase(true, true, true, true)]
        [TestCase(false, false, true, false)]
        [TestCase(true, true, false, false)]
        [TestCase(false, false, false, false)]
        public async Task ShowApprovalOptionMessageIsMappedCorrectlyWithATransfer(bool isAgreementSigned, bool showApprovalOption, 
            bool isApprovedByProvider, bool expectedShowApprovalOptionMessage)
        {
            var fixture = new DetailsViewModelMapperTestsFixture();
            fixture.Cohort.IsApprovedByProvider = isApprovedByProvider;
            fixture.SetTransferSender().SetIsAgreementSigned(isAgreementSigned);
            var result = await fixture.Map();
            Assert.AreEqual(expectedShowApprovalOptionMessage, result.ShowApprovalOptionMessage);
        }

        [TestCase(true, true, true, true)]
        [TestCase(false, false, true, false)]
        [TestCase(true, true, false, false)]
        [TestCase(false, false, false, false)]
        public async Task ShowApprovalOptionMessageIsMappedCorrectlyWithoutATransfer(bool isAgreementSigned, bool showApprovalOption,
            bool isApprovedByProvider, bool expectedShowApprovalOptionMessage)
        {
            var fixture = new DetailsViewModelMapperTestsFixture();
            fixture.Cohort.IsApprovedByProvider = isApprovedByProvider;
            fixture.SetIsAgreementSigned(isAgreementSigned);
            var result = await fixture.Map();
            Assert.AreEqual(expectedShowApprovalOptionMessage, result.ShowApprovalOptionMessage);
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
            Assert.AreEqual(fixture.Cohort.IsCompleteForEmployer, result.IsCompleteForEmployer);
        }
    }

    public class DetailsViewModelMapperTestsFixture
    {
        public DetailsViewModelMapper Mapper;
        public DetailsRequest Source;
        public DetailsViewModel Result;
        public Mock<ICommitmentsApiClient> CommitmentsApiClient;
        public Mock<ITrainingProgrammeApiClient> TrainingProgrammeApiClient;
        public Mock<IAccountApiClient> AccountApiClient;
        public Mock<IEncodingService> EncodingService;
        public GetCohortResponse Cohort;
        public GetDraftApprenticeshipsResponse DraftApprenticeshipsResponse;
        public DateTime DefaultStartDate = new DateTime(2019, 10, 1);
        public AccountLegalEntityResponse AccountLegalEntityResponse;
        public LegalEntityViewModel LegalEntityViewModel;

        private Fixture _autoFixture;
        private ITrainingProgramme _trainingProgramme;
        private List<FundingPeriod> _fundingPeriods;
        private DateTime _startFundingPeriod = new DateTime(2019, 10, 1);
        private DateTime _endFundingPeriod = new DateTime(2019, 10, 30);

        public DetailsViewModelMapperTestsFixture()
        {
            _autoFixture = new Fixture();

            Cohort = _autoFixture.Build<GetCohortResponse>().Without(x => x.TransferSenderId).Create();
            AccountLegalEntityResponse = _autoFixture.Create<AccountLegalEntityResponse>();
            LegalEntityViewModel = _autoFixture.Create<LegalEntityViewModel>();

            var draftApprenticeships = CreateDraftApprenticeshipDtos(_autoFixture);
            _autoFixture.Register(() => draftApprenticeships);
            DraftApprenticeshipsResponse = _autoFixture.Create<GetDraftApprenticeshipsResponse>();

            CommitmentsApiClient = new Mock<ICommitmentsApiClient>();
            CommitmentsApiClient.Setup(x => x.GetCohort(It.IsAny<long>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Cohort);
            CommitmentsApiClient.Setup(x => x.GetDraftApprenticeships(It.IsAny<long>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(DraftApprenticeshipsResponse);
            CommitmentsApiClient.Setup(x => x.GetAccountLegalEntity(It.IsAny<long>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(AccountLegalEntityResponse);

            AccountApiClient = new Mock<IAccountApiClient>();
            AccountApiClient.Setup(x => x.GetLegalEntity(It.IsAny<string>(), It.IsAny<long>())).ReturnsAsync(LegalEntityViewModel);

            _fundingPeriods = new List<FundingPeriod>
            {
                new FundingPeriod{ EffectiveFrom = _startFundingPeriod, EffectiveTo = _endFundingPeriod, FundingCap = 1000},
                new FundingPeriod{ EffectiveFrom = _startFundingPeriod.AddMonths(1), EffectiveTo = _endFundingPeriod.AddMonths(1), FundingCap = 500}
            };
            _trainingProgramme = new Standard { EffectiveFrom = DefaultStartDate, EffectiveTo = DefaultStartDate.AddYears(1), FundingPeriods = _fundingPeriods };

            TrainingProgrammeApiClient = new Mock<ITrainingProgrammeApiClient>();
            TrainingProgrammeApiClient.Setup(x => x.GetTrainingProgramme(It.IsAny<string>()))
                .ReturnsAsync(_trainingProgramme);

            EncodingService = new Mock<IEncodingService>();
            SetEncodingOfApprenticeIds();

            Mapper = new DetailsViewModelMapper(CommitmentsApiClient.Object, EncodingService.Object, TrainingProgrammeApiClient.Object, AccountApiClient.Object);
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

        public Task<DetailsViewModel> Map()
        {
            return Mapper.Map(TestHelper.Clone(Source));
        }

        public void AssertEquality(DraftApprenticeshipDto source, CohortDraftApprenticeshipViewModel result)
        {
            Assert.AreEqual(source.Id, result.Id);
            Assert.AreEqual(source.FirstName, result.FirstName);
            Assert.AreEqual(source.LastName, result.LastName);
            Assert.AreEqual(source.DateOfBirth, result.DateOfBirth);
            Assert.AreEqual(source.Cost, result.Cost);
            Assert.AreEqual(source.StartDate, result.StartDate);
            Assert.AreEqual(source.EndDate, result.EndDate);
            Assert.AreEqual($"X{source.Id}X", result.DraftApprenticeshipHashedId);
        }

        public void AssertSequenceOrder<T>(List<T> expected, List<T> actual, Func<T, T, bool> evaluator)
        {
            Assert.AreEqual(expected.Count, actual.Count, "Expected and actual sequences are different lengths");

            for (int i = 0; i < actual.Count; i++)
            {
                Assert.IsTrue(evaluator(expected[i], actual[i]), "Actual sequence are not in same order as expected");
            }
        }

        private IReadOnlyCollection<DraftApprenticeshipDto> CreateDraftApprenticeshipDtos(Fixture autoFixture)
        {
            var draftApprenticeships = autoFixture.CreateMany<DraftApprenticeshipDto>(6).ToArray();
            SetCourseDetails(draftApprenticeships[0], "Course1", "C1", 1000);
            SetCourseDetails(draftApprenticeships[1], "Course1", "C1", 2100);
            SetCourseDetails(draftApprenticeships[2], "Course1", "C1", 2000, DefaultStartDate.AddMonths(1));

            SetCourseDetails(draftApprenticeships[3], "Course2", "C2", 1500);
            SetCourseDetails(draftApprenticeships[4], "Course2", "C2", null);

            SetCourseDetails(draftApprenticeships[5], "Course3", "C3", null, DefaultStartDate.AddMonths(2));

            return draftApprenticeships;
        }

        private void SetCourseDetails(DraftApprenticeshipDto draftApprenticeship, string courseName, string courseCode, int? cost, DateTime? startDate = null)
        {
            startDate = startDate ?? DefaultStartDate;

            draftApprenticeship.CourseName = courseName;
            draftApprenticeship.CourseCode = courseCode;
            draftApprenticeship.Cost = cost;
            draftApprenticeship.StartDate = startDate;
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

        public DetailsViewModelMapperTestsFixture SetTransferSender()
        {
            Cohort.TransferSenderId = _autoFixture.Create<long>();
            return this;
        }

        public DetailsViewModelMapperTestsFixture SetIsChangeOfParty(bool isChangeOfParty)
        {
            Cohort.ChangeOfPartyRequestId = isChangeOfParty ? _autoFixture.Create<long>() : default(long?);
            return this;
        }
    }
}