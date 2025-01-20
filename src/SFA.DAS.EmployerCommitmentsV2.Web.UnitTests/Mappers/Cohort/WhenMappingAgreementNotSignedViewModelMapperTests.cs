using SFA.DAS.Encoding;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.EmployerCommitmentsV2.Web.Services;
using SFA.DAS.CommitmentsV2.Types;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Cohort;

public class WhenMappingAgreementNotSignedViewModelMapperTests
{
    private Mock<IEmployerAccountsService> _employerAccountsService;
    private Mock<IEncodingService> _encodingService;
    private Account _account;
    private AddApprenticeshipCacheModelToAgreementNotSignedViewModelMapper _mapper;
    private AddApprenticeshipCacheModel _cacheModel;

    [SetUp]
    public void Arrange()
    {
        var autoFixture = new Fixture();
        _employerAccountsService = new Mock<IEmployerAccountsService>();
        _encodingService = new Mock<IEncodingService>();
        _cacheModel = autoFixture.Create<AddApprenticeshipCacheModel>();

        _account = autoFixture.Create<Account>();
        _account.Id = 123;

        _encodingService.Setup(x => x.Decode(It.IsAny<string>(), EncodingType.AccountId)).Returns(123);
        _employerAccountsService.Setup(x => x.GetAccount(123)).ReturnsAsync(_account);            

        _mapper = new AddApprenticeshipCacheModelToAgreementNotSignedViewModelMapper(_employerAccountsService.Object, _encodingService.Object);
    }

    [TestCase(ApprenticeshipEmployerType.Levy , true)]
    [TestCase(ApprenticeshipEmployerType.NonLevy, false)]
    public async Task Then_CanContinueAnyway_Is_Mapped(ApprenticeshipEmployerType apprenticeshipEmployerType, bool canContinue)
    {
        //Arrange
        _account.ApprenticeshipEmployerType = apprenticeshipEmployerType;
            
        //Act
        var result = await _mapper.Map(_cacheModel);

        //Assert           
        Assert.That(result.CanContinueAnyway, Is.EqualTo(canContinue));
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