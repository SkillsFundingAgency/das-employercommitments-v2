using FluentAssertions;
using SFA.DAS.EmployerCommitmentsV2.Contracts;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Responses;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Cohort;

[TestFixture]
public class WhenMappingBaseSelectProviderRequestToTransferConnectionViewModelTests
{
    private Mock<IApprovalsApiClient> _approvalsApiClient;
    private BaseSelectProviderRequestToSelectTransferConnectionViewModelMapper _mapper;        
    private BaseSelectProviderRequest _request;
    private GetSelectDirectTransferConnectionResponse _response;

    [SetUp]
    public void Arrange()
    {
        var autoFixture = new Fixture();
        _approvalsApiClient = new Mock<IApprovalsApiClient>();
        _request = autoFixture.Create<BaseSelectProviderRequest>();
        _response = autoFixture.Create<GetSelectDirectTransferConnectionResponse>();

        _approvalsApiClient.Setup(x => x.GetSelectDirectTransferConnection(_request.AccountId, CancellationToken.None))
            .ReturnsAsync(_response);

        _mapper = new BaseSelectProviderRequestToSelectTransferConnectionViewModelMapper(_approvalsApiClient.Object);
    }

    [Test]
    public async Task Then_AccountId_Is_Mapped()
    {
        //Act
        var result = await _mapper.Map(_request);

        //Assert
        _approvalsApiClient.Verify(x=>x.GetSelectDirectTransferConnection(_request.AccountId, CancellationToken.None));
    }

    [Test]
    public async Task Then_List_Of_TransferConnections_Is_Mapped()
    {   
        //Act
        var result = await _mapper.Map(_request);

        //Assert           
        result.TransferConnections.Should().BeEquivalentTo(_response.TransferConnections);
    }

    [Test]
    public async Task Then_LevyStatus_Is_Mapped()
    {
        //Act
        var result = await _mapper.Map(_request);

        //Assert           
        result.IsLevyAccount.Should().Be(_response.IsLevyAccount);
    }
}