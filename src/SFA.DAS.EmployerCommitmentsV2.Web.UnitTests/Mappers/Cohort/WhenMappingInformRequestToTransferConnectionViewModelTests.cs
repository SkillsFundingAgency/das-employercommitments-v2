using FluentAssertions;
using SFA.DAS.EmployerCommitmentsV2.Contracts;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Responses;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Cohort;

[TestFixture]
public class WhenMappingInformRequestToTransferConnectionViewModelTests
{
    private Mock<IApprovalsApiClient> _approvalsApiClient;
    private InformRequestToSelectTransferConnectionViewModelMapper _mapper;        
    private InformRequest _informRequest;
    private GetSelectDirectTransferConnectionResponse _response;

    [SetUp]
    public void Arrange()
    {
        var autoFixture = new Fixture();
        _approvalsApiClient = new Mock<IApprovalsApiClient>();
        _informRequest = autoFixture.Create<InformRequest>();
        _response = autoFixture.Create<GetSelectDirectTransferConnectionResponse>();

        _approvalsApiClient.Setup(x => x.GetSelectDirectTransferConnection(_informRequest.AccountId, CancellationToken.None))
            .ReturnsAsync(_response);

        _mapper = new InformRequestToSelectTransferConnectionViewModelMapper(_approvalsApiClient.Object, Mock.Of<ILogger<InformRequestToSelectTransferConnectionViewModelMapper>>());
    }

    [Test]
    public async Task Then_AccountId_Is_Mapped()
    {
        //Act
        var result = await _mapper.Map(_informRequest);

        //Assert
        _approvalsApiClient.Verify(x=>x.GetSelectDirectTransferConnection(_informRequest.AccountId, CancellationToken.None));
    }

    [Test]
    public async Task Then_List_Of_TransferConnections_Is_Mapped()
    {   
        //Act
        var result = await _mapper.Map(_informRequest);

        //Assert           
        result.TransferConnections.Should().BeEquivalentTo(_response.TransferConnections);
    }

    [Test]
    public async Task Then_LevyStatus_Is_Mapped()
    {
        //Act
        var result = await _mapper.Map(_informRequest);

        //Assert           
        result.IsLevyAccount.Should().Be(_response.IsLevyAccount);
    }
}