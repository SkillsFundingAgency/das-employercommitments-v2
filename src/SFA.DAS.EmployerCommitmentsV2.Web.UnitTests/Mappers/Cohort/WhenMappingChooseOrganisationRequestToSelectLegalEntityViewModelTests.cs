using SFA.DAS.EmployerCommitmentsV2.Contracts;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Responses;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.Encoding;
using LegalEntity = SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Responses.LegalEntity;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Cohort;

public class WhenMappingChooseOrganisationRequestToSelectLegalEntityViewModelTests
{       
    private Mock<IApprovalsApiClient> _apiClient;
    private SelectLegalEntityRequestToSelectLegalEntityViewModelMapper _mapper;
    private SelectLegalEntityRequest _chooseOrganisationRequest;
    private Mock<IEncodingService> _encodingService;
    private const long AccountId = 998829;

    [SetUp]
    public void Arrange()
    {
        var autoFixture = new Fixture();
        _apiClient = new Mock<IApprovalsApiClient>();
        _encodingService = new Mock<IEncodingService>();
        _chooseOrganisationRequest = autoFixture.Create<SelectLegalEntityRequest>();
        _encodingService.Setup(x => x.Decode(_chooseOrganisationRequest.AccountHashedId, EncodingType.AccountId))
            .Returns(AccountId);
        var legalEntity = autoFixture.Create<LegalEntity>();
        _apiClient.Setup(x => x.GetLegalEntitiesForAccount(_chooseOrganisationRequest.cohortRef, AccountId))
            .ReturnsAsync(new GetLegalEntitiesForAccountResponse
            {
                LegalEntities = new List<LegalEntity>
                {
                    legalEntity
                }
            });
            
        _mapper = new SelectLegalEntityRequestToSelectLegalEntityViewModelMapper(_apiClient.Object, _encodingService.Object);
    }

    [Test]
    public async Task Then_TransferConnectionCode_Is_Mapped()
    {
        //Act
        var result = await _mapper.Map(_chooseOrganisationRequest);

        //Assert           
        Assert.That(result.TransferConnectionCode, Is.EqualTo(_chooseOrganisationRequest.transferConnectionCode));
    }

    [Test]
    public async Task Then_CohortRef_Is_Mapped()
    {
        //Act
        var result = await _mapper.Map(_chooseOrganisationRequest);

        //Assert           
        Assert.That(result.CohortRef, Is.EqualTo(_chooseOrganisationRequest.cohortRef));
    }


    [Test]
    public async Task Then_LegalEntitiy_Is_Mapped()
    {
        //Act
        var result = await _mapper.Map(_chooseOrganisationRequest);

        //Assert           
        Assert.That(result.LegalEntities.Count(), Is.EqualTo(1));
    }

    [Test]
    public async Task Then_GetLegalEntitiesForAccount_Is_Called()
    {
        //Act
        var result = await _mapper.Map(_chooseOrganisationRequest);

        //Assert
        _apiClient.Verify(x => x.GetLegalEntitiesForAccount(_chooseOrganisationRequest.cohortRef, AccountId),
            Times.Once);
    }
}