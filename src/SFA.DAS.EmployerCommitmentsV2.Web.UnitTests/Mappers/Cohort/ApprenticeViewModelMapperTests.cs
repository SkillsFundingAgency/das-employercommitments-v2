using FluentAssertions;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.CommitmentsV2.Shared.Models;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Cohort;

[TestFixture]
public class ApprenticeViewModelMapperTests
{
    private ApprenticeViewModelMapper _mapper;
    private Mock<ICommitmentsApiClient> _commitmentsApiClient;
    private GetProviderResponse _providerResponse;
    private AccountLegalEntityResponse _accountLegalEntityResponse;
    private AddApprenticeshipCacheModel _source;
    private ApprenticeViewModel _result;
    private TrainingProgramme _courseStandard;
    private TrainingProgramme _course;
    private List<TrainingProgramme> _allTrainingProgrammes;
    private List<TrainingProgramme> _standardTrainingProgrammes;

    [SetUp]
    public async Task Arrange()
    {
        var autoFixture = new Fixture();

        _course = autoFixture.Create<TrainingProgramme>();
        _courseStandard = autoFixture.Create<TrainingProgramme>();
        _providerResponse = autoFixture.Create<GetProviderResponse>();
        _accountLegalEntityResponse = autoFixture.Build<AccountLegalEntityResponse>().With(x => x.LevyStatus, ApprenticeshipEmployerType.Levy).Create();
        _source = autoFixture.Create<AddApprenticeshipCacheModel>();
        _source.StartMonthYear = "062020";
        _source.StartMonth = 6;
        _source.StartYear = 2020;

        _source.TransferSenderId = string.Empty;
        _source.AccountId = 12345;

        _commitmentsApiClient = new Mock<ICommitmentsApiClient>();
        _commitmentsApiClient.Setup(x => x.GetProvider(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(_providerResponse);
        _commitmentsApiClient.Setup(x => x.GetAccountLegalEntity(_source.AccountLegalEntityId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(_accountLegalEntityResponse);
        _standardTrainingProgrammes = new List<TrainingProgramme> { _courseStandard };
        _commitmentsApiClient
            .Setup(x => x.GetAllTrainingProgrammeStandards(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GetAllTrainingProgrammeStandardsResponse
            {
                TrainingProgrammes = _standardTrainingProgrammes
            });
        _allTrainingProgrammes = new List<TrainingProgramme> { _courseStandard, _course };
        _commitmentsApiClient
            .Setup(x => x.GetAllTrainingProgrammes(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GetAllTrainingProgrammesResponse
            {
                TrainingProgrammes = _allTrainingProgrammes
            });

        _mapper = new ApprenticeViewModelMapper(_commitmentsApiClient.Object);

        _result = await _mapper.Map(TestHelper.Clone(_source));
    }

    [Test]
    public void AccountLegalEntityIdIsMappedCorrectly()
    {
        _result.AccountLegalEntityId.Should().Be(_source.AccountLegalEntityId);
    }

    [Test]
    public void AccountLegalEntityHashedIdIsMappedCorrectly()
    {
        _result.AccountLegalEntityHashedId.Should().Be(_source.AccountLegalEntityHashedId);
    }

    [Test]
    public void AccountLegalEntityNameIsMappedCorrectly()
    {
        _result.LegalEntityName.Should().Be(_accountLegalEntityResponse.LegalEntityName);
    }

    [Test]
    public void StartDateIsMappedCorrectly()
    {
        _result.StartDate.Date.Should().Be(new MonthYearModel(_source.StartMonthYear).Date);
    }

    [Test]
    public void ReservationIdIsMappedCorrectly()
    {
        _result.ReservationId.Should().Be(_source.ReservationId);
    }

    [Test]
    public void CourseCodeIsMappedCorrectly()
    {
        _result.CourseCode.Should().Be(_source.CourseCode);
    }

    [Test]
    public void ProviderIdIsMappedCorrectly()
    {
        _result.ProviderId.Should().Be(_source.ProviderId);
    }

    [Test]
    public void ProviderNameIsMappedCorrectly()
    {
        _result.ProviderName.Should().Be(_providerResponse.Name);
    }

    [Test]
    public void TransferSenderIdIsMappedCorrectly()
    {
        _result.TransferSenderId.Should().Be(_source.TransferSenderId);
    }

    [Test]
    public void EncodedPledgeApplicationIdIsMappedCorrectly()
    {
        _result.EncodedPledgeApplicationId.Should().Be(_source.EncodedPledgeApplicationId);
    }
}