using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Shared;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Cohort;

[TestFixture]
public class AddApprenticeshipCacheModelToSelectCourseViewModelMapperTests
{
    private AddApprenticeshipCacheModelToSelectCourseViewModelMapper _mapper;
    private AddApprenticeshipCacheModel _source;
    private Mock<ICommitmentsApiClient> _commitmentsApiClient;
    private List<TrainingProgramme> _standardTrainingProgrammes;
    private SelectCourseViewModel _result;

    [SetUp]
    public async Task Arrange()
    {
        var autoFixture = new Fixture();

        _standardTrainingProgrammes = autoFixture.CreateMany<TrainingProgramme>().ToList();

        _source = autoFixture.Build<AddApprenticeshipCacheModel>()
            .With(x => x.StartMonthYear, "062020")
            .With(x => x.AccountId, 12345)
            .With(x => x.CourseCode, "Course1")
            .With(x => x.DeliveryModel, DeliveryModel.PortableFlexiJob)
            .Without(x => x.TransferSenderId).Create();

        _commitmentsApiClient = new Mock<ICommitmentsApiClient>();
        _commitmentsApiClient
            .Setup(x => x.GetAllTrainingProgrammeStandards(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GetAllTrainingProgrammeStandardsResponse()
            {
                TrainingProgrammes = _standardTrainingProgrammes
            });

        _mapper = new AddApprenticeshipCacheModelToSelectCourseViewModelMapper(_commitmentsApiClient.Object);

        _result = await _mapper.Map(TestHelper.Clone(_source));
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
    public void ReservationIdIsMappedCorrectly()
    {
        Assert.That(_result.ReservationId, Is.EqualTo(_source.ReservationId));
    }

    [Test]
    public void StartDateIsMappedCorrectly()
    {
        Assert.That(_result.StartMonthYear, Is.EqualTo(_source.StartMonthYear));
    }

    [Test]
    public void DeliveryIsMappedCorrectly()
    {
        Assert.That(_result.DeliveryModel, Is.EqualTo(_source.DeliveryModel));
    }

    [Test]
    public void TransferSenderIdIsMappedCorrectly()
    {
        Assert.That(_result.TransferSenderId, Is.EqualTo(_source.TransferSenderId));
    }
}