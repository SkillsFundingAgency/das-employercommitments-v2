using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.CommitmentsV2.Shared.Models;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Web.Exceptions;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.DraftApprenticeship;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.DraftApprenticeship;
using SFA.DAS.Encoding;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.DraftApprenticeship;

[TestFixture]
public class AddDraftApprenticeshipViewModelMapperTests
{
    private AddDraftApprenticeshipViewModelMapper _mapper;
    private AddDraftApprenticeshipRequest _source;
    private AddDraftApprenticeshipViewModel _result;

    private Mock<ICommitmentsApiClient> _commitmentsApiClient;
    private Mock<IEncodingService> _encodingService;
    private string _encodedTransferSenderId;
    private GetCohortResponse _cohort;
    private List<TrainingProgramme> _allTrainingProgrammes;
    private List<TrainingProgramme> _standardTrainingProgrammes;
        
    [SetUp]
    public async Task Arrange()
    {
        var autoFixture = new Fixture();

        _allTrainingProgrammes = autoFixture.CreateMany<TrainingProgramme>().ToList();
        _standardTrainingProgrammes = autoFixture.CreateMany<TrainingProgramme>().ToList();
            
        _cohort = autoFixture.Create<GetCohortResponse>();
        _cohort.WithParty = Party.Employer;
        _commitmentsApiClient = new Mock<ICommitmentsApiClient>();
        _commitmentsApiClient.Setup(x => x.GetCohort(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(_cohort);
            
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

        _encodedTransferSenderId = autoFixture.Create<string>();
        _encodingService = new Mock<IEncodingService>();
        _encodingService
            .Setup(x => x.Encode(It.IsAny<long>(), It.Is<EncodingType>(e => e == EncodingType.PublicAccountId)))
            .Returns(_encodedTransferSenderId);

        _mapper = new AddDraftApprenticeshipViewModelMapper(_commitmentsApiClient.Object, _encodingService.Object);

        _source = autoFixture.Create<AddDraftApprenticeshipRequest>();
        _source.StartMonthYear = "092020";

        _result = await _mapper.Map(TestHelper.Clone(_source));
    }

    [Test]
    public void CourseCodeIsMappedCorrectly()
    {
        Assert.That(_result.CourseCode, Is.EqualTo(_source.CourseCode));
    }

    [Test]
    public void StartDateIsMappedCorrectly()
    {
        Assert.That(_result.StartDate.Date, Is.EqualTo(new MonthYearModel(_source.StartMonthYear).Date));
    }

    [Test]
    public void AccountHashedIdIsMappedCorrectly()
    {
        Assert.That(_result.AccountHashedId, Is.EqualTo(_source.AccountHashedId));
    }

    [Test]
    public void AccountLegalEntityHashedIdIsMappedCorrectly()
    {
        Assert.That(_result.AccountLegalEntityHashedId, Is.EqualTo(_source.AccountLegalEntityHashedId));
    }

    [Test]
    public void AccountLegalEntityIdIsMappedCorrectly()
    {
        Assert.That(_result.AccountLegalEntityId, Is.EqualTo(_source.AccountLegalEntityId));
    }

    [Test]
    public void CohortIdIsMappedCorrectly()
    {
        Assert.That(_result.CohortId, Is.EqualTo(_source.CohortId));
    }

    [Test]
    public void CohortReferenceIsMappedCorrectly()
    {
        Assert.That(_result.CohortReference, Is.EqualTo(_source.CohortReference));
    }

    [Test]
    public void ReservationIdIsMappedCorrectly()
    {
        Assert.That(_result.ReservationId, Is.EqualTo(_source.ReservationId));
    }

    [Test]
    public void AutoCreatedReservationIsMappedCorrectly()
    {
        Assert.That(_result.AutoCreatedReservation, Is.EqualTo(_source.AutoCreated));
    }

    [Test]
    public void TransferSenderHashedIdIsMappedCorrectly()
    {
        Assert.That(_result.TransferSenderHashedId, Is.EqualTo(_encodedTransferSenderId));
    }
       

    [Test]
    public void ProviderNameIsMappedCorrectly()
    {
        Assert.That(_result.ProviderName, Is.EqualTo(_cohort.ProviderName));
    }

    [Test]
    public void LegalEntityNameIsMappedCorrectly()
    {
        Assert.That(_result.LegalEntityName, Is.EqualTo(_cohort.LegalEntityName));
    }

    [Test]
    public void ThrowsWhenCohortNotWithEditingParty()
    {
        _cohort.WithParty = Party.Provider;
        Assert.ThrowsAsync<CohortEmployerUpdateDeniedException>(() => _mapper.Map(_source));
    }

    [Test]
    public void IsOnFlexiPaymentPilotIsFalse()
    {
        Assert.That(_result.IsOnFlexiPaymentPilot, Is.False);
    }
}