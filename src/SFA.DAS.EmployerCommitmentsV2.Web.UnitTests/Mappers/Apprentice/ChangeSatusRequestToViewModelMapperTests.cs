using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.Testing.AutoFixture;
using System.Text;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Apprentice;

[TestFixture]
public class WhenMapping_ChangeSatusRequestToViewModelMapperTests
{
    private ChangeStatusRequestToViewModelMapper _mapper;
    private Mock<ICommitmentsApiClient> _mockCommitmentsApiClient;

    [SetUp]
    public void Arrange()
    {
            

        _mockCommitmentsApiClient = new Mock<ICommitmentsApiClient>();
            
        _mapper = new ChangeStatusRequestToViewModelMapper(_mockCommitmentsApiClient.Object);
            
        _mockCommitmentsApiClient.Setup(a => a.GetApprenticeship(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GetApprenticeshipResponse
            {
                Status = ApprenticeshipStatus.Completed
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