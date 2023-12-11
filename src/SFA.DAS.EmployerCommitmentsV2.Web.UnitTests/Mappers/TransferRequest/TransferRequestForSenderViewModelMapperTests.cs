using System.Text.RegularExpressions;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Responses;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.TransferRequest;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.TransferRequest;
using SFA.DAS.Encoding;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.TransferRequest;

public class TransferRequestForSenderViewModelMapperTests
{
    private Mock<ICommitmentsApiClient> _mockCommitmentsApiClient;
    private Mock<IApprovalsApiClient> _mockApprovalsApiClient;
    private Mock<IEncodingService> _mockEncodingService;
        
    private GetTransferRequestResponse _getTransferRequestResponse;
    private TransferRequestRequest _request;

    private GetPledgeApplicationResponse _getPledgeApplicationResponse;
    private int _getPledgeApplicationId;
        
    private TransferRequestForSenderViewModelMapper _mapper;

    private const long AccountIdFirst = 12;
    private const long TransferRequestIdFirst = 34;
    private const long ReceivingEmployerAccountIdFirst = 56;

    [SetUp]
    public void SetUp()
    {
        //Arrange
        var autoFixture = new Fixture();
        _request = autoFixture.Build<TransferRequestRequest>()
            .With(x => x.AccountHashedId, $"A{AccountIdFirst}")
            .With(x => x.TransferRequestHashedId, $"A{TransferRequestIdFirst}")
            .Create();

        _getPledgeApplicationId = autoFixture.Create<int>();

        _getTransferRequestResponse = autoFixture.Build<GetTransferRequestResponse>()
            .With(x => x.ReceivingEmployerAccountId, ReceivingEmployerAccountIdFirst)
            .With(x => x.TransferRequestId, TransferRequestIdFirst)
            .With(x => x.PledgeApplicationId, _getPledgeApplicationId)
            .Create();
            
        _mockCommitmentsApiClient = new Mock<ICommitmentsApiClient>();
        _mockCommitmentsApiClient.Setup(r => r.GetTransferRequestForSender(It.IsAny<long>(), It.IsAny<long>(), CancellationToken.None))
            .ReturnsAsync(_getTransferRequestResponse);

        _getPledgeApplicationResponse = autoFixture.Create<GetPledgeApplicationResponse>();

        _mockApprovalsApiClient = new Mock<IApprovalsApiClient>();
        _mockApprovalsApiClient.Setup(r => r.GetPledgeApplication(_getPledgeApplicationId, CancellationToken.None))
            .ReturnsAsync(_getPledgeApplicationResponse);

        _mockEncodingService = new Mock<IEncodingService>();
            
        _mockEncodingService.Setup(t => t.Encode(It.IsAny<long>(), EncodingType.AccountId))
            .Returns((long value, EncodingType _) => $"A{value}");
        _mockEncodingService.Setup(t => t.Encode(It.IsAny<long>(), EncodingType.PublicAccountId))
            .Returns((long value, EncodingType _) => $"P{value}");
        _mockEncodingService.Setup(t => t.Encode(It.IsAny<long>(), EncodingType.TransferRequestId))
            .Returns((long value, EncodingType _) => $"T{value}");
        _mockEncodingService.Setup(t => t.Encode(It.IsAny<long>(), EncodingType.CohortReference))
            .Returns((long value, EncodingType _) => $"C{value}");
        _mockEncodingService.Setup(t => t.Encode(It.IsAny<long>(), EncodingType.PledgeId))
            .Returns((long value, EncodingType _) => $"PL{value}");
        _mockEncodingService.Setup(t => t.Encode(It.IsAny<long>(), EncodingType.PledgeApplicationId))
            .Returns((long value, EncodingType _) => $"PA{value}");
        _mockEncodingService.Setup(t => t.Decode(It.IsAny<string>(), It.IsAny<EncodingType>()))
            .Returns((string value, EncodingType _) => long.Parse(Regex.Replace(value, "[A-Za-z ]", "")));

        _mapper = new TransferRequestForSenderViewModelMapper(_mockCommitmentsApiClient.Object, _mockApprovalsApiClient.Object, _mockEncodingService.Object);
    }

    [Test]
    public async Task GetTransferRequestForSenderIsCalled()
    {
        //Act
        await _mapper.Map(_request);

        //Assert
        _mockCommitmentsApiClient.Verify(t => t.GetTransferRequestForSender(_request.AccountId, _request.TransferRequestId, It.IsAny<CancellationToken>()), Times.Once());
    }

    [Test]
    public async Task TransferReceiverHashedAccountId_IsMapped()
    {
        //Act
        var result = await _mapper.Map(_request);

        //Assert
        Assert.That(result.TransferReceiverPublicHashedAccountId, Is.EqualTo($"P{_getTransferRequestResponse.ReceivingEmployerAccountId}"));
    }

    [Test]
    public async Task TransferSenderHashedAccountId_IsMapped()
    {
        //Act
        var result = await _mapper.Map(_request);

        //Assert
        Assert.That(result.TransferSenderHashedAccountId, Is.EqualTo($"A{_getTransferRequestResponse.SendingEmployerAccountId}"));
    }

    [Test]
    public async Task TransferSenderName_IsMapped()
    {
        //Act
        var result = await _mapper.Map(_request);

        //Assert
        Assert.That(result.TransferReceiverName, Is.EqualTo(_getTransferRequestResponse.LegalEntityName));
    }

    [Test]
    public async Task HashedCohortReference_IsMapped()
    {
        //Act
        var result = await _mapper.Map(_request);

        //Assert
        Assert.That(result.HashedCohortReference, Is.EqualTo($"C{_getTransferRequestResponse.CommitmentId}"));
    }

    [Test]
    public async Task TransferApprovalStatusDesc_IsMapped()
    {
        //Act
        var result = await _mapper.Map(_request);

        //Assert
        Assert.That(result.TransferApprovalStatusDesc, Is.EqualTo(_getTransferRequestResponse.Status.ToString()));
    }

    [Test]
    public async Task TransferApprovalSetBy_IsMapped()
    {
        //Act
        var result = await _mapper.Map(_request);

        //Assert
        Assert.That(result.TransferApprovalSetBy, Is.EqualTo(_getTransferRequestResponse.ApprovedOrRejectedByUserName));
    }

    [Test]
    public async Task TransferApprovalSetOn_IsMapped()
    {
        //Act
        var result = await _mapper.Map(_request);

        //Assert
        Assert.That(result.TransferApprovalSetOn, Is.EqualTo(_getTransferRequestResponse.ApprovedOrRejectedOn));
    }

    [Test]
    public async Task TotalCost_IsMapped()
    {
        //Act
        var result = await _mapper.Map(_request);

        //Assert
        Assert.That(result.TotalCost, Is.EqualTo(_getTransferRequestResponse.TransferCost));
    }

    [Test]
    public async Task FundingCap_IsMapped()
    {
        //Act
        var result = await _mapper.Map(_request);

        //Assert
        Assert.That(result.FundingCap, Is.EqualTo(_getTransferRequestResponse.FundingCap));
    }

    [Test]
    public async Task AutoApproval_IsMapped()
    {
        //Act
        var result = await _mapper.Map(_request);

        //Assert
        Assert.That(result.AutoApprovalEnabled, Is.EqualTo(_getTransferRequestResponse.AutoApproval));
    }

    [Test]
    public async Task PledgeId_IsMapped()
    {
        //Act
        var result = await _mapper.Map(_request);

        //Assert
        Assert.That(result.HashedPledgeId, Is.EqualTo($"PL{_getPledgeApplicationResponse.PledgeId}"));
    }

    [TestCase(TransferApprovalStatus.Pending, 1500.0, 12000, true)]
    [TestCase(TransferApprovalStatus.Approved, 1500.0, 12000, true)]
    [TestCase(TransferApprovalStatus.Pending, 12000.0, 12000, false)]
    [TestCase(TransferApprovalStatus.Approved, 12000.0, 12000, false)]
    [TestCase(TransferApprovalStatus.Pending, 13000.0, 12000, false)]
    [TestCase(TransferApprovalStatus.Approved, 13000.0, 12000, false)]
    [TestCase(TransferApprovalStatus.Rejected, 1500.0, 12000, false)]
    [TestCase(TransferApprovalStatus.Rejected, 12000.0, 12000, false)]
    [TestCase(TransferApprovalStatus.Rejected, 13000.0, 12000, false)]
    public async Task TrainingName_IsMapped(TransferApprovalStatus transferApprovalStatus, decimal transferCost, int fundingCap, bool showFundingCapWarning)
    {
        // Arrange
        _getTransferRequestResponse.Status = transferApprovalStatus;
        _getTransferRequestResponse.TransferCost = transferCost;
        _getTransferRequestResponse.FundingCap = fundingCap;

        //Act
        var result = await _mapper.Map(_request);

        //Assert
        Assert.That(result.ShowFundingCapWarning, Is.EqualTo(showFundingCapWarning));
    }

    [Test]
    public async Task PledgeApplicationId_IsMapped()
    {
        //Act
        var result = await _mapper.Map(_request);

        //Assert
        Assert.That(result.HashedPledgeApplicationId, Is.EqualTo($"PA{_getTransferRequestResponse.PledgeApplicationId}"));
    }

    [Test]
    public async Task When_PledgeApplication_Is_Null_Then_It_Is_Not_Mapped()
    {
        var autoFixture = new Fixture();
        _getTransferRequestResponse = autoFixture.Build<GetTransferRequestResponse>()
            .With(x => x.ReceivingEmployerAccountId, ReceivingEmployerAccountIdFirst)
            .With(x => x.TransferRequestId, TransferRequestIdFirst)
            .With(x => x.PledgeApplicationId, default(int?))
            .Create();

        _mockCommitmentsApiClient = new Mock<ICommitmentsApiClient>();
        _mockCommitmentsApiClient.Setup(r => r.GetTransferRequestForSender(It.IsAny<long>(), It.IsAny<long>(), CancellationToken.None))
            .ReturnsAsync(_getTransferRequestResponse);

        _mapper = new TransferRequestForSenderViewModelMapper(_mockCommitmentsApiClient.Object, _mockApprovalsApiClient.Object, _mockEncodingService.Object);

        var result = await _mapper.Map(_request);

        Assert.Multiple(() =>
        {
            Assert.That(result.HashedPledgeApplicationId, Is.EqualTo(string.Empty));
            Assert.That(result.HashedPledgeId, Is.EqualTo(string.Empty));
        });
    }
}