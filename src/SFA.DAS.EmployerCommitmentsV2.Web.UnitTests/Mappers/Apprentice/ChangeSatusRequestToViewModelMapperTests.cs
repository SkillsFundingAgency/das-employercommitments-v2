using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.Common.Domain.Types;
using SFA.DAS.EmployerCommitmentsV2.Contracts;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Responses;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.Encoding;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Apprentice;

[TestFixture]
public class WhenMapping_ChangeSatusRequestToViewModelMapperTests
{
    private ChangeStatusRequestToViewModelMapper _mapper;
    private Mock<IApprovalsApiClient> _mockApprovalsApiClient;
    private Mock<IEncodingService> _mockEncodingService;
     
    [SetUp]
    public void Arrange()
    {
        _mockApprovalsApiClient = new Mock<IApprovalsApiClient>();
        _mockEncodingService = new Mock<IEncodingService>();

        _mapper = new ChangeStatusRequestToViewModelMapper(_mockApprovalsApiClient.Object, _mockEncodingService.Object);

        _mockEncodingService.Setup(e => e.Decode(It.IsAny<string>(), It.IsAny<EncodingType>()))
            .Returns(12345);

        _mockApprovalsApiClient.Setup(a => a.GetEditApprenticeship(It.IsAny<long>(), It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GetEditApprenticeshipResponse
            {
                Status = ApprenticeshipStatus.Completed,
                LearningType = Common.Domain.Types.LearningType.Apprenticeship
            });
    }

    [Test, MoqAutoData]
    public async Task ThenApprenticeshipHashedIdIsMappedCorrectly(ChangeStatusRequest request)
    {
        var result = await _mapper.Map(request);

        Assert.That(result.ApprenticeshipHashedId, Is.EqualTo(request.ApprenticeshipHashedId));
    }

    [Test, MoqAutoData]
    public async Task ThenAccountHashedIdIsMappedCorrectly(ChangeStatusRequest request)
    {
        var result = await _mapper.Map(request);

        Assert.That(result.AccountHashedId, Is.EqualTo(request.AccountHashedId));
    }

    [Test, MoqAutoData]
    public async Task ThenCurrentStatusIsMapped(ChangeStatusRequest request)
    {
        var result = await _mapper.Map(request);

        Assert.That(result.CurrentStatus, Is.EqualTo(ApprenticeshipStatus.Completed));
    }
}