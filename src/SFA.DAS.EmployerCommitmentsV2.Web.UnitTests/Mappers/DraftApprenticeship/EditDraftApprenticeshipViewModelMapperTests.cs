using FluentAssertions;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Contracts;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Responses;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.DraftApprenticeship;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.DraftApprenticeship;
using SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Extensions;
using SFA.DAS.Encoding;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.DraftApprenticeship;

[TestFixture]
public class EditDraftApprenticeshipViewModelMapperTests
{
    private EditDraftApprenticeshipViewModelMapper _mapper;
    private EditDraftApprenticeshipRequest _source;
    private EditDraftApprenticeshipViewModel _result;

    private Mock<ICommitmentsApiClient> _commitmentsApiClient;
    private Mock<IApprovalsApiClient> _apiClient;
    private GetEditDraftApprenticeshipResponse _draftApprenticeshipResponse;
    private Mock<IEncodingService> _encodingService;
    private string _encodedApprenticeshipId;
    private string _cohortReference;
    private string _aleHashedId;
    private GetCohortResponse _cohort;
    private List<TrainingProgramme> _allTrainingProgrammes;
    private List<TrainingProgramme> _standardTrainingProgrammes;

    [SetUp]
    public async Task Arrange()
    {
        var autoFixture = new Fixture();

        _allTrainingProgrammes = autoFixture.CreateMany<TrainingProgramme>().ToList();
        _standardTrainingProgrammes = autoFixture.CreateMany<TrainingProgramme>().ToList();
        _encodedApprenticeshipId = autoFixture.Create<string>();
        _cohortReference = autoFixture.Create<string>();
        _aleHashedId = autoFixture.Create<string>();

        _encodingService = new Mock<IEncodingService>();
        _encodingService
            .Setup(x => x.Encode(It.IsAny<long>(), It.Is<EncodingType>(e => e == EncodingType.ApprenticeshipId)))
            .Returns(_encodedApprenticeshipId);
        _encodingService
            .Setup(x => x.Encode(It.IsAny<long>(), It.Is<EncodingType>(e => e == EncodingType.CohortReference)))
            .Returns(_cohortReference);
        _encodingService
            .Setup(x => x.Encode(It.IsAny<long>(), EncodingType.PublicAccountLegalEntityId))
            .Returns(_aleHashedId);

        _draftApprenticeshipResponse = autoFixture.Create<GetEditDraftApprenticeshipResponse>();
        _draftApprenticeshipResponse.IsContinuation = false;
        _commitmentsApiClient = new Mock<ICommitmentsApiClient>();
            
        _commitmentsApiClient
            .Setup(x => x.GetAllTrainingProgrammeStandards(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GetAllTrainingProgrammeStandardsResponse
            {
                TrainingProgrammes = _standardTrainingProgrammes
            });
            
        _commitmentsApiClient
            .Setup(x => x.GetAllTrainingProgrammes(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GetAllTrainingProgrammesResponse
            {
                TrainingProgrammes = _allTrainingProgrammes
            });

        _apiClient = new Mock<IApprovalsApiClient>();
        _apiClient.Setup(x =>
                x.GetEditDraftApprenticeship(It.IsAny<long>(), It.IsAny<long>(), It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(_draftApprenticeshipResponse);

        _cohort = autoFixture.Create<GetCohortResponse>();
        _cohort.WithParty = Party.Employer;
        _cohort.ChangeOfPartyRequestId = null;
        _commitmentsApiClient.Setup(x => x.GetCohort(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(_cohort);

        _source = autoFixture.Create<EditDraftApprenticeshipRequest>();
        _source.Cohort = _cohort;
        _mapper = new EditDraftApprenticeshipViewModelMapper(_commitmentsApiClient.Object, _encodingService.Object, _apiClient.Object);

        _result = await _mapper.Map(TestHelper.Clone(_source)) as EditDraftApprenticeshipViewModel;
    }

    [Test]
    public void DraftApprenticeshipIdIsMappedCorrectly()
    {
        _result.DraftApprenticeshipId.Should().Be(_source.Request.DraftApprenticeshipId);
    }

    [Test]
    public void DraftApprenticeshipHashedIdIsMappedCorrectly()
    {
        _result.DraftApprenticeshipHashedId.Should().Be(_encodedApprenticeshipId);
    }

    [Test]
    public void CohortIdIsMappedCorrectly()
    {
        _result.CohortId.Should().Be(_source.Request.CohortId);
    }

    [Test]
    public void CohortReferenceIsMappedCorrectly()
    {
        _result.CohortReference.Should().Be(_cohortReference);
    }

    [Test]
    public void ReservationIdIsMappedCorrectly()
    {
        _result.ReservationId.Should().Be(_draftApprenticeshipResponse.ReservationId);
    }

    [Test]
    public void FirstNameIsMappedCorrectly()
    {
        _result.FirstName.Should().Be(_draftApprenticeshipResponse.FirstName);
    }

    [Test]
    public void LastNameIsMappedCorrectly()
    {
        _result.LastName.Should().Be(_draftApprenticeshipResponse.LastName);
    }

    [Test]
    public void EmailIsMappedCorrectly()
    {
        _result.Email.Should().Be(_draftApprenticeshipResponse.Email);
    }

    [Test]
    public void EmailAddressConfirmedIsMappedCorrectly()
    {
        _result.EmailAddressConfirmed.Should().Be(_draftApprenticeshipResponse.EmailAddressConfirmed);
    }

    [Test]
    public void DateOfBirthIsMappedCorrectly()
    {
        _result.DateOfBirth.Date.Should().Be(_draftApprenticeshipResponse.DateOfBirth);
    }

    [Test]
    public void UniqueLearnerNumberIsMappedCorrectly()
    {
        _result.Uln.Should().Be(_draftApprenticeshipResponse.Uln);
    }

    [Test]
    public void DeliveryModelIsMappedCorrectly()
    {
        _result.DeliveryModel.Should().Be((DeliveryModel)_draftApprenticeshipResponse.DeliveryModel);
    }

    [Test]
    public void CourseCodeIsMappedCorrectly()
    {
        _result.CourseCode.Should().Be(_draftApprenticeshipResponse.CourseCode);
    }

    [Test]
    public void CostIsMappedCorrectly()
    {
        _result.Cost.Should().Be(_draftApprenticeshipResponse.Cost);
    }

    [Test]
    public void EmploymentPriceIsMappedCorrectly()
    {
        _result.EmploymentPrice.Should().Be(_draftApprenticeshipResponse.EmploymentPrice);
    }

    [Test]
    public void StartDateIsMappedCorrectly()
    {
        _result.StartDate.Date.ToMonthYearString().Should().Be(_draftApprenticeshipResponse.StartDate.ToMonthYearString());
    }

    [Test]
    public void EndDateIsMappedCorrectly()
    {
        _result.EndDate.Date.ToMonthYearString().Should().Be(_draftApprenticeshipResponse.EndDate.ToMonthYearString());
    }

    [Test]
    public void EmploymentEndDateIsMappedCorrectly()
    {
        _result.EmploymentEndDate.Date.ToMonthYearString().Should().Be(_draftApprenticeshipResponse.EmploymentEndDate.ToMonthYearString());
    }

    [Test]
    public void ReferenceIsMappedCorrectly()
    {
        _result.Reference.Should().Be(_draftApprenticeshipResponse.EmployerReference);
    }

    [Test]
    public void AccountHashedIdIsMappedCorrectly()
    {
        _result.AccountHashedId.Should().Be(_source.Request.AccountHashedId);
    }

    [Test]
    public void ProviderNameIsMappedCorrectly()
    {
        _result.ProviderName.Should().Be(_cohort.ProviderName);
    }

    [Test]
    public void ProviderIdsMappedCorrectly()
    {
        _result.ProviderId.Should().Be(_cohort.ProviderId);
    }

    [Test]
    public void LegalEntityNameIsMappedCorrectly()
    {
        _result.LegalEntityName.Should().Be(_source.Cohort.LegalEntityName);
    }

    [TestCase(123, true)]
    [TestCase(null, false)]
    public async Task CoursesAreMappedCorrectlyWhenAccountIsLevy(long? transferSenderId, bool fundedByTransfer)
    {
        _cohort.LevyStatus = ApprenticeshipEmployerType.Levy;
        _cohort.TransferSenderId = transferSenderId;

        _result = await _mapper.Map(_source) as EditDraftApprenticeshipViewModel;

        _result.Courses.Should().BeEquivalentTo(fundedByTransfer
            ? _standardTrainingProgrammes
            : _allTrainingProgrammes);
    }

    [TestCase(123)]
    [TestCase(null)]
    public async Task CoursesAreMappedCorrectlyWhenAccountIsNonLevy(long? transferSenderId)
    {
        _cohort.LevyStatus = ApprenticeshipEmployerType.NonLevy;
        _cohort.TransferSenderId = transferSenderId;

        _result = await _mapper.Map(_source) as EditDraftApprenticeshipViewModel;

        _result.Courses.Should().BeEquivalentTo(_standardTrainingProgrammes);
    }

    [Test]
    public async Task CoursesAreMappedCorrectlyWhenCohortIsChangeOfParty()
    {
        _cohort.LevyStatus = ApprenticeshipEmployerType.NonLevy;
        _draftApprenticeshipResponse.IsContinuation = true;

        _result = await _mapper.Map(_source) as EditDraftApprenticeshipViewModel;

        _result.Courses.Should().BeEquivalentTo(_allTrainingProgrammes);
    }

    [TestCase(true)]
    [TestCase(false)]
    public async Task ThenIsContinuationIsMappedCorrectly(bool isContinuation)
    {
        _draftApprenticeshipResponse.IsContinuation = isContinuation;
        _result = await _mapper.Map(_source) as EditDraftApprenticeshipViewModel;
        _result.IsContinuation.Should().Be(_draftApprenticeshipResponse.IsContinuation);
    }

    [Test]
    public void AccountLegalEntityIdIsMappedCorrectly()
    {
        _result.AccountLegalEntityId.Should().Be(_source.Cohort.AccountLegalEntityId);
    }

    [Test]
    public void AccountLegalEntityHashedIdIsMappedCorrectly()
    {
        _result.AccountLegalEntityHashedId.Should().Be(_aleHashedId);
    }

    [Test]
    public void HasMultipleDeliveryModelOptionsIsMappedCorrectly()
    {
        _result.HasMultipleDeliveryModelOptions.Should().Be(_draftApprenticeshipResponse.HasMultipleDeliveryModelOptions);
    }

    [Test]
    public void IsOnFlexiPaymentsPilotIsMappedCorrectly()
    {
        _result.IsOnFlexiPaymentPilot.Should().Be(_draftApprenticeshipResponse.IsOnFlexiPaymentPilot);
    }

    [Test]
    public void ActualStartDateIsMappedCorrectly()
    {
        _result.ActualStartDate.Should().Be(_draftApprenticeshipResponse.ActualStartDate);
    }

    [Test]
    public void ActualEndDateIsMappedCorrectly()
    {
        _result.ActualEndDate.Should().BeNull();
    }

    [Test]
    public void RecognisePriorLearningIsMappedCorrectly()
    {
        _result.RecognisePriorLearning.Should().Be(_draftApprenticeshipResponse.RecognisePriorLearning);
    }

    [Test]
    public void TrainingTotalHoursIsMappedCorrectly()
    {
        _result.TrainingTotalHours.Should().Be(_draftApprenticeshipResponse.TrainingTotalHours);
    }

    [Test]
    public void DurationReducedByHoursIsMappedCorrectly()
    {
        _result.DurationReducedByHours.Should().Be(_draftApprenticeshipResponse.DurationReducedByHours);
    }

    [Test]
    public void DurationReducedByIsMappedCorrectly()
    {
        _result.DurationReducedBy.Should().Be(_draftApprenticeshipResponse.DurationReducedBy);
    }

    [Test]
    public void PriceReducedByIsMappedCorrectly()
    {
        _result.PriceReducedBy.Should().Be(_draftApprenticeshipResponse.PriceReducedBy);
    }

    [Test]
    public void StandardPageUrlIsNullWhenNoMatch()
    {
        _result.StandardPageUrl.Should().BeNull();
    }

    [Test]
    public async Task StandardPageUrlIMappedWhenMatchExists()
    {
        _standardTrainingProgrammes.Add(new TrainingProgramme
        {
            CourseCode = "123",
            StandardUId = _draftApprenticeshipResponse.StandardUId,
            StandardPageUrl = "https://test123",
            FundingPeriods = new List<TrainingProgrammeFundingPeriod>()
        });

        _result = await _mapper.Map(TestHelper.Clone(_source)) as EditDraftApprenticeshipViewModel;

        _result.StandardPageUrl.Should().Be("https://test123");
    }

    [Test]
    public void FundingBandsIsNullWhenNoMatch()
    {
        _result.FundingBandMax.Should().BeNull();
    }

    [Test]
    public async Task FundingBandsWhenMatchExists()
    {
        _standardTrainingProgrammes.Add(new TrainingProgramme
        {
            CourseCode = "123",
            StandardUId = _draftApprenticeshipResponse.StandardUId,
            StandardPageUrl = "https://test123",
            FundingPeriods = new List<TrainingProgrammeFundingPeriod>() { new TrainingProgrammeFundingPeriod { EffectiveFrom = DateTime.MinValue, EffectiveTo = DateTime.MaxValue, FundingCap = 1234}}
        });

        _result = await _mapper.Map(TestHelper.Clone(_source)) as EditDraftApprenticeshipViewModel;

        _result.FundingBandMax.Should().Be(1234);
    }
}