using FluentAssertions;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Cohort;

[TestFixture]
public class WhenMappingInformRequestToInformViewModelTests
{
    private InformRequest _informRequest;
    private AccountResponse _accountResponse;
    private InformRequestToInformViewModelMapper _mapper;
    private Mock<ICommitmentsApiClient> _commitmentsClient;

    [SetUp]
    public void Arrange()
    {
        var autoFixture = new Fixture();            
        _informRequest = autoFixture.Create<InformRequest>();
        _accountResponse = autoFixture.Create<AccountResponse>();
        _commitmentsClient = new Mock<ICommitmentsApiClient>();
        _mapper = new InformRequestToInformViewModelMapper(_commitmentsClient.Object);
    }

    [Test]
    public async Task Then_AccountHashedId_Is_Mapped()
    {
        //Act
        var result = await _mapper.Map(_informRequest);

        //Assert           
        result.AccountHashedId.Should().Be(_informRequest.AccountHashedId);
    }

    [TestCase(ApprenticeshipEmployerType.Levy, true)]
    [TestCase(ApprenticeshipEmployerType.NonLevy, false)]
    public async Task Then_LevyStatus_Is_Mapped(ApprenticeshipEmployerType levyStatus, bool expectedIsLevy)
    {
        _accountResponse.LevyStatus = levyStatus;
        _commitmentsClient.Setup(x => x.GetAccount(_informRequest.AccountId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(_accountResponse);
        
        //Act
        var result = await _mapper.Map(_informRequest);

        //Assert           
        result.LevyFunded.Should().Be(expectedIsLevy);
    }
}