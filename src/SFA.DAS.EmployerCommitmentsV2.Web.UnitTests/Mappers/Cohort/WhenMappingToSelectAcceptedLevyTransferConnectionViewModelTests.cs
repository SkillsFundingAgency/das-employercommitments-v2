using FluentAssertions;
using SFA.DAS.EmployerCommitmentsV2.Contracts;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Responses;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.Encoding;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Cohort;

[TestFixture]
public class WhenMappingToSelectAcceptedLevyTransferConnectionViewModelTests
{
    private Mock<IApprovalsApiClient> _approvalsApiClient;
    private Mock<IEncodingService> _encodingService;
    private SelectAcceptedLevyTransferConnectionViewModelMapper _mapper;
    private AddApprenticeshipCacheModel _request;
    private GetSelectLevyTransferConnectionResponse _response;

    [SetUp]
    public void Arrange()
    {
        var autoFixture = new Fixture();
        _approvalsApiClient = new Mock<IApprovalsApiClient>();
        _encodingService = new Mock<IEncodingService>();
        _request = autoFixture.Create<AddApprenticeshipCacheModel>();
        _response = autoFixture.Create<GetSelectLevyTransferConnectionResponse>();

        _approvalsApiClient.Setup(x => x.GetSelectLevyTransferConnection(_request.AccountId, CancellationToken.None))
            .ReturnsAsync(_response);
        _encodingService.Setup(x => x.Encode(It.IsAny<long>(), EncodingType.PledgeApplicationId)).Returns((long input, EncodingType _) => input.ToString() + "AppId");
        _encodingService.Setup(x => x.Encode(It.IsAny<long>(), EncodingType.PledgeId)).Returns((long input, EncodingType _) => input.ToString() + "PledgeId");
        _encodingService.Setup(x => x.Encode(It.IsAny<long>(), EncodingType.PublicAccountId)).Returns((long input, EncodingType _) => input.ToString() + "PublicId");

        _mapper = new SelectAcceptedLevyTransferConnectionViewModelMapper(_approvalsApiClient.Object, _encodingService.Object);
    }

    [Test]
    public async Task Then_AccountId_Is_Mapped()
    {
        //Act
        var result = await _mapper.Map(_request);

        //Assert
        _approvalsApiClient.Verify(x => x.GetSelectLevyTransferConnection(_request.AccountId, CancellationToken.None));
    }

    [Test]
    public async Task Then_AccountHashedId_Is_Mapped()
    {
        //Act
        var result = await _mapper.Map(_request);

        //Assert           
        result.AccountHashedId.Should().Be(_request.AccountHashedId);
    }
    
    [Test]
    public async Task Then_ApprenticeshipSessionKey_Is_Mapped()
    {
        //Act
        var result = await _mapper.Map(_request);

        //Assert           
        result.ApprenticeshipSessionKey.Should().Be(_request.ApprenticeshipSessionKey);
    }

    [Test]
    public async Task Then_List_Of_Applications_Is_Mapped()
    {
        //Act
        var result = await _mapper.Map(_request);

        //Assert           
        result.Applications.Should().BeEquivalentTo(_response.Applications.Select(x => new LevyTransferDisplayConnection()
        {
            Id = x.Id,
            ApplicationHashedId = x.Id + "AppId",
            SendingEmployerPublicHashedId = x.SenderEmployerAccountId + "PublicId",
            OpportunityHashedId = x.OpportunityId + "PledgeId",
            ApplicationAndSenderHashedId = x.Id + "AppId" + "|" + x.SenderEmployerAccountId + "PublicId",
            DisplayName = (x.IsNamePublic ? x.SenderEmployerAccountName : "Opportunity") + $" ({x.OpportunityId + "PledgeId"}) - £{x.TotalAmount.ToString("N")}"
        }));
    }

}