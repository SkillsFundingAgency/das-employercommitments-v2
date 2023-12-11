using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.DraftApprenticeship;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.DraftApprenticeship;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Shared;
using SFA.DAS.Encoding;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.DraftApprenticeship;

[TestFixture]
public class SelectDeliveryModelViewModelToAddDraftApprenticeshipRequestMapperTests
{
    private SelectDeliveryModelViewModelToAddDraftApprenticeshipRequestMapper _mapper;
    private Mock<ICommitmentsApiClient> _commitmentsApiClient;
    private GetCohortResponse _getCohortResponse;
    private AddDraftApprenticeshipRequest _result;
    private SelectDeliveryModelViewModel _source;
    public Mock<IEncodingService> _encodingService;

    [SetUp]
    public async Task Arrange()
    {
        var autoFixture = new Fixture();

        var providerId = autoFixture.Create<long>();

        _source = autoFixture.Build<SelectDeliveryModelViewModel>()
            .With(x => x.ProviderId, providerId)
            .Without(x => x.DeliveryModels).Create();

        _getCohortResponse = autoFixture.Build<GetCohortResponse>()
            .With(x => x.LevyStatus, ApprenticeshipEmployerType.Levy)
            .With(x => x.WithParty, Party.Employer)
            .With(x => x.ProviderId, providerId)
            .Without(x => x.TransferSenderId)
            .Create();

        _commitmentsApiClient = new Mock<ICommitmentsApiClient>();
        _commitmentsApiClient.Setup(x => x.GetCohort(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(_getCohortResponse);

        _encodingService = new Mock<IEncodingService>();

        _mapper = new SelectDeliveryModelViewModelToAddDraftApprenticeshipRequestMapper(_commitmentsApiClient.Object, _encodingService.Object);

        _result = await _mapper.Map(TestHelper.Clone(_source));
    }

    [Test]
    public void AccountHashedIdIsMappedCorrectly()
    {
        Assert.That(_result.AccountHashedId, Is.EqualTo(_source.AccountHashedId));
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
    public void CourseCodeIsMappedCorrectly()
    {
        Assert.That(_result.CourseCode, Is.EqualTo(_source.CourseCode));
    }

    [Test]
    public void DeliveryModelIsMappedCorrectly()
    {
        Assert.That(_result.DeliveryModel, Is.EqualTo(_source.DeliveryModel));
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
}