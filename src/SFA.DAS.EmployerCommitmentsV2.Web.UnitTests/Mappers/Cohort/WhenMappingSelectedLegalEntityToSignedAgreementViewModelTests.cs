using SFA.DAS.EmployerCommitmentsV2.Contracts;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Responses;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.Encoding;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Cohort;

public class WhenMappingSelectedLegalEntityToSignedAgreementViewModelTests
{
    private Mock<IApprovalsApiClient> _approvalsApiClient;
    private Mock<IEncodingService> _encodingService;
    private SelectedLegalEntityToSignedAgreementViewModelMapper _mapper;
    private SelectLegalEntityViewModel _selectLegalEntityViewModel;
    private GetLegalEntitiesForAccountResponse _response;
    private const long AccountId = 998829;

    [SetUp]
    public void Arrange()
    {
        var autoFixture = new Fixture();
        var legalEntity = autoFixture.Create<EmployerCommitmentsV2.Services.Approvals.Responses.LegalEntity>();
        _selectLegalEntityViewModel = autoFixture.Create<SelectLegalEntityViewModel>();
        _approvalsApiClient = new Mock<IApprovalsApiClient>();
        _encodingService = new Mock<IEncodingService>();
        _encodingService.Setup(x => x.Decode(_selectLegalEntityViewModel.AccountHashedId, EncodingType.AccountId))
            .Returns(AccountId);

        _response = new GetLegalEntitiesForAccountResponse
        {
            LegalEntities = new List<EmployerCommitmentsV2.Services.Approvals.Responses.LegalEntity>
            {
                legalEntity
            }
        };
       
        legalEntity.LegalEntityId = _selectLegalEntityViewModel.LegalEntityId;
        _approvalsApiClient.Setup(x => x.GetLegalEntitiesForAccount("new", AccountId))
            .ReturnsAsync(_response);

        _mapper = new SelectedLegalEntityToSignedAgreementViewModelMapper(_approvalsApiClient.Object, _encodingService.Object);
    }

    [Test]
    public async Task Then_TransferConnectionCode_Is_Mapped()
    {
        //Act
        var result = await _mapper.Map(_selectLegalEntityViewModel);

        //Assert           
        Assert.That(result.TransferConnectionCode, Is.EqualTo(_selectLegalEntityViewModel.TransferConnectionCode));
    }

    [Test]
    public async Task Then_LegalEntityCode_Is_Mapped()
    {
        //Act
        var result = await _mapper.Map(_selectLegalEntityViewModel);

        //Assert           
        Assert.That(result.AccountLegalEntityId, Is.EqualTo(_selectLegalEntityViewModel.LegalEntityId));
    }

    [Test]
    public async Task Then_CohortRef_Is_Mapped()
    {
        //Act
        var result = await _mapper.Map(_selectLegalEntityViewModel);

        //Assert           
        Assert.That(result.CohortRef, Is.EqualTo(_selectLegalEntityViewModel.CohortRef));
    }


    [Test]
    public async Task Then_GetLegalEntitiesForAccount_Is_Called()
    {
        //Act
        var result = await _mapper.Map(_selectLegalEntityViewModel);

        //Assert
        _approvalsApiClient.Verify(x => x.GetLegalEntitiesForAccount("new", AccountId), Times.Once);
    }
}