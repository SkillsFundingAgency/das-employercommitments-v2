using FluentAssertions;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.CommitmentsV2.Shared.Models;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.DraftApprenticeship;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Cohort;

[TestFixture]
public class CreateCohortWithDraftApprenticeshipRequestToAddDraftApprenticeshipViewModelMapperTests
{
    private AddDraftApprenticeshipViewModelMapper _mapper;
    private Mock<ICommitmentsApiClient> _commitmentsApiClient;
    private GetProviderResponse _providerResponse;
    private AccountLegalEntityResponse _accountLegalEntityResponse;
    private ApprenticeRequest _source;
    private AddDraftApprenticeshipViewModel _result;
    private List<TrainingProgramme> _standardTrainingProgrammes;
    private List<TrainingProgramme> _allTrainingProgrammes;

    [SetUp]
    public async Task Arrange()
    {
        var autoFixture = new Fixture();

        _standardTrainingProgrammes = autoFixture.CreateMany<TrainingProgramme>().ToList();
        _allTrainingProgrammes = autoFixture.CreateMany<TrainingProgramme>().ToList();
        _providerResponse = autoFixture.Create<GetProviderResponse>();
        _accountLegalEntityResponse = autoFixture.Build<AccountLegalEntityResponse>().With(x => x.LevyStatus, ApprenticeshipEmployerType.Levy).Create();

        _source = autoFixture.Build<ApprenticeRequest>()
            .With(x=>x.StartMonthYear, "062020")
            .With(x=>x.AccountId, 12345)
            .Without(x=>x.TransferSenderId).Create();

        _commitmentsApiClient = new Mock<ICommitmentsApiClient>();
        _commitmentsApiClient.Setup(x => x.GetProvider(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(_providerResponse);
        _commitmentsApiClient.Setup(x => x.GetAccountLegalEntity(_source.AccountLegalEntityId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(_accountLegalEntityResponse);
        _commitmentsApiClient
            .Setup(x => x.GetAllTrainingProgrammeStandards(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GetAllTrainingProgrammeStandardsResponse()
            {
                TrainingProgrammes = _standardTrainingProgrammes
            });
        _commitmentsApiClient
            .Setup(x => x.GetAllTrainingProgrammes(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GetAllTrainingProgrammesResponse
            {
                TrainingProgrammes = _allTrainingProgrammes
            });

        _mapper = new AddDraftApprenticeshipViewModelMapper(_commitmentsApiClient.Object);

        _result = await _mapper.Map(TestHelper.Clone(_source));
    }

    [Test]
    public void AccountLegalEntityIdIsMappedCorrectly()
    {
        Assert.That(_result.AccountLegalEntityId, Is.EqualTo(_source.AccountLegalEntityId));
    }

    [Test]
    public void AccountLegalEntityHashedIdIsMappedCorrectly()
    {
        Assert.That(_result.AccountLegalEntityHashedId, Is.EqualTo(_source.AccountLegalEntityHashedId));
    }

    [Test]
    public void StartDateIsMappedCorrectly()
    {
        Assert.That(_result.StartDate.Date, Is.EqualTo(new MonthYearModel(_source.StartMonthYear).Date));
    }

    [Test]
    public void ReservationIdIsMappedCorrectly()
    {
        Assert.That(_result.ReservationId, Is.EqualTo(_source.ReservationId));
    }

    [Test]
    public void CourseCodeIsMappedCorrectly()
    {
        Assert.That(_result.CourseCode, Is.EqualTo(_source.CourseCode));
    }

    [Test]
    public void ProviderIdIsMappedCorrectly()
    {
        Assert.That(_result.ProviderId, Is.EqualTo(_source.ProviderId));
    }

    [Test]
    public void ProviderNameIsMappedCorrectly()
    {
        Assert.That(_result.ProviderName, Is.EqualTo(_providerResponse.Name));
    }

    [Test]
    public void CoursesAreMappedCorrectly()
    {
        Assert.That(_result.Courses, Is.EqualTo(_allTrainingProgrammes));
    }

    [Test]
    public async Task TransferFundedCohortsAllowStandardCoursesOnlyWhenEmployerIsLevy()
    {
        _source.TransferSenderId = "test";
        _result = await _mapper.Map(TestHelper.Clone(_source));
        _result.Courses.Should().BeEquivalentTo(_standardTrainingProgrammes);
    }

    [TestCase("12345")]
    [TestCase(null)]
    public async Task NonLevyCohortsAllowStandardCoursesOnlyRegardlessOfTransferStatus(string transferSenderId)
    {
        _source.TransferSenderId = transferSenderId;
        _accountLegalEntityResponse.LevyStatus = ApprenticeshipEmployerType.NonLevy;
            
            
        _result = await _mapper.Map(TestHelper.Clone(_source));
        _result.Courses.Should().BeEquivalentTo(_standardTrainingProgrammes);
    }

}