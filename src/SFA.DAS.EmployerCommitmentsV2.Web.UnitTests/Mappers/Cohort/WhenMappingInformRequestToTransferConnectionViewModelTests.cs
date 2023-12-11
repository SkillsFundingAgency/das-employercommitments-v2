using SFA.DAS.EAS.Account.Api.Client;
using SFA.DAS.EAS.Account.Api.Types;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.Encoding;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Cohort;

[TestFixture]
public class WhenMappingInformRequestToTransferConnectionViewModelTests
{
    private Mock<IAccountApiClient> _accountApiClient;
    private Mock<IEncodingService> _encodingService;
    private InformRequestToSelectTransferConnectionViewModelMapper _mapper;        
    private InformRequest _informRequest;

    [SetUp]
    public void Arrange()
    {
        var autoFixture = new Fixture();
        _accountApiClient = new Mock<IAccountApiClient>();
        _encodingService = new Mock<IEncodingService>();
        _informRequest = autoFixture.Create<InformRequest>(); 

        _accountApiClient.Setup(x => x.GetTransferConnections(_informRequest.AccountHashedId))
            .ReturnsAsync(new List<TransferConnectionViewModel>
            {
                new()
                {
                    FundingEmployerAccountId = 1234,
                    FundingEmployerAccountName = "FirstAccountName",
                    FundingEmployerHashedAccountId = "FAN"
                },
                new()
                {
                    FundingEmployerAccountId = 1235,
                    FundingEmployerAccountName = "SecondAccountName",
                    FundingEmployerHashedAccountId = "SAN"
                }

            });

        _mapper = new InformRequestToSelectTransferConnectionViewModelMapper(_accountApiClient.Object, _encodingService.Object);
    }

    [Test]
    public async Task Then_AccountHashedId_Is_Mapped()
    {
        //Act
        var result = await _mapper.Map(_informRequest);

        //Assert           
        Assert.That(result.AccountHashedId, Is.EqualTo(_informRequest.AccountHashedId));
    }

    [Test]
    public async Task Then_Non_Empty_List_Of_TransferConnections_Is_Mapped()
    {   
        //Act
        var result = await _mapper.Map(_informRequest);

        //Assert           
        Assert.That(result.TransferConnections, Has.Count.EqualTo(2));
    }

    [Test]
    public async Task Then_Empty_List_Of_TransferConnections_Is_Mapped()
    {
        //Arrange
        _accountApiClient.Setup(x => x.GetTransferConnections(_informRequest.AccountHashedId))
            .ReturnsAsync(new List<TransferConnectionViewModel>());

        //Act
        var result = await _mapper.Map(_informRequest);

        //Assert           
        Assert.That(result.TransferConnections, Is.Empty);
    }

    [Test]
    public async Task Then_GetTransferConnections_Is_Called()
    {
        //Act
        await _mapper.Map(_informRequest);

        //Assert
        _accountApiClient.Verify(x => x.GetTransferConnections(It.Is<String>(c => c == _informRequest.AccountHashedId)),                   
            Times.Once);
    }
}