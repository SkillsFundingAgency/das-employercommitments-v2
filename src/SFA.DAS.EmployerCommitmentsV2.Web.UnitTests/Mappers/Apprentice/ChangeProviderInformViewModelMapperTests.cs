using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.Encoding;
using SFA.DAS.Testing.AutoFixture;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Apprentice;

public class ChangeProviderInformViewModelMapperTests
{
    private Mock<ICommitmentsApiClient> _mockCommitmentsApiClient;
    private Mock<IEncodingService> _mockEncodingService;
    private GetApprenticeshipResponse _apprenticeshipResponse;

    private ChangeProviderInformViewModelMapper _mapper;

    private const long ApprenticeshipId = 10000000;

    [SetUp]
    public void Arrange()
    {
        var autoFixture = new Fixture();

        _apprenticeshipResponse = autoFixture.Build<GetApprenticeshipResponse>()
            .With(a => a.Status, ApprenticeshipStatus.Stopped)
            .Create();

        _mockCommitmentsApiClient = new Mock<ICommitmentsApiClient>();
        _mockCommitmentsApiClient.Setup(a => a.GetApprenticeship(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(_apprenticeshipResponse);

        _mockEncodingService = new Mock<IEncodingService>();
        _mockEncodingService.Setup(d => d.Decode(It.IsAny<string>(), EncodingType.ApprenticeshipId))
            .Returns(ApprenticeshipId);

        _mapper = new ChangeProviderInformViewModelMapper(_mockCommitmentsApiClient.Object, _mockEncodingService.Object);
    }

    [Test, MoqAutoData]
    public async Task ApprenticeshipHashedId_IsMapped(ChangeProviderInformRequest request)
    {
        var result = await _mapper.Map(request);

        Assert.That(result.ApprenticeshipHashedId, Is.EqualTo(request.ApprenticeshipHashedId));
    }

    [Test, MoqAutoData]
    public async Task AccountHashedId_IsMapped(ChangeProviderInformRequest request)
    {
        var result = await _mapper.Map(request);

        Assert.That(result.AccountHashedId, Is.EqualTo(request.AccountHashedId));
    }

    [Test, MoqAutoData]
    public async Task ApprenticeshipStatus_IsMapped(ChangeProviderInformRequest request)
    {
        var result = await _mapper.Map(request);

        Assert.That(result.ApprenticeshipStatus, Is.EqualTo(ApprenticeshipStatus.Stopped));
    }

    [Test, MoqAutoData]
    public async Task WhenRequestingChangeProviderInformPage_ThenHashedApprenticeshipIdIsDecoded(ChangeProviderInformRequest request)
    {
        var result = await _mapper.Map(request);

        _mockEncodingService.Verify(a => a.Decode(request.ApprenticeshipHashedId, EncodingType.ApprenticeshipId), Times.Once);
    }

    [Test, MoqAutoData]
    public async Task WhenRequestingChangeProviderInformPage_ThenGetApprenticeshipIsCalled(ChangeProviderInformRequest request)
    {
        var result = await _mapper.Map(request);

        _mockCommitmentsApiClient.Verify(a => a.GetApprenticeship(ApprenticeshipId, It.IsAny<CancellationToken>()), Times.Once);
    }
}