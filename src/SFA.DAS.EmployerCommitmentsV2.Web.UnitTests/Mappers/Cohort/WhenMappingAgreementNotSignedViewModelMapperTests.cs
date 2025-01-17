using FluentAssertions;
using SFA.DAS.Encoding;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.EmployerCommitmentsV2.Contracts;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Responses;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Cohort;

public class WhenMappingAgreementNotSignedViewModelMapperTests
{
    private Mock<IApprovalsApiClient> _client;
    private Mock<IEncodingService> _encodingService;
    private GetAgreementNotSignedResponse _response;
    private AddApprenticeshipCacheModelToAgreementNotSignedViewModelMapper _mapper;
    private AddApprenticeshipCacheModel _cacheModel;

    [SetUp]
    public void Arrange()
    {
        var autoFixture = new Fixture();
        _client = new Mock<IApprovalsApiClient>();
        _encodingService = new Mock<IEncodingService>();
        _cacheModel = autoFixture.Create<AddApprenticeshipCacheModel>();

        _response = autoFixture.Create<GetAgreementNotSignedResponse>();

        _encodingService.Setup(x => x.Decode(It.IsAny<string>(), EncodingType.AccountId)).Returns(123);
        _client.Setup(x => x.GetAgreementNotSigned(123, It.IsAny<CancellationToken>())).ReturnsAsync(_response);            

        _mapper = new AddApprenticeshipCacheModelToAgreementNotSignedViewModelMapper(_client.Object, _encodingService.Object);
    }

    [TestCase(true)]
    [TestCase(false)]
    public async Task Then_CanContinueAnyway_Is_Mapped(bool isLevy)
    {
        //Arrange
        _response.IsLevyAccount = isLevy;
            
        //Act
        var result = await _mapper.Map(_cacheModel);

        //Assert           
        result.CanContinueAnyway.Should().Be(isLevy);
    }

    [Test]
    public async Task Then_AccountLegalEntityPublicHashedId_Is_Mapped()
    {          
        //Act
        var result = await _mapper.Map(_cacheModel);

        //Assert           
        Assert.That(result.AccountLegalEntityHashedId, Is.EqualTo(_cacheModel.AccountLegalEntityHashedId));
    }

    [Test]
    public async Task Then_LegalEntityName_Is_Mapped()
    {
        //Act
        var result = await _mapper.Map(_cacheModel);

        //Assert           
        Assert.That(result.LegalEntityName, Is.EqualTo(_cacheModel.LegalEntityName));
    }

    [Test]
    public async Task Then_LegalEntityCode_Is_Mapped()
    {
        //Act
        var result = await _mapper.Map(_cacheModel);

        //Assert           
        Assert.That(result.AccountLegalEntityId, Is.EqualTo(_cacheModel.AccountLegalEntityId));
    }

    [Test]
    public async Task Then_TransferConnectionCode_Is_Mapped()
    {
        //Act
        var result = await _mapper.Map(_cacheModel);

        //Assert           
        Assert.That(result.TransferConnectionCode, Is.EqualTo(_cacheModel.TransferSenderId));
    }
}