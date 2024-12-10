using FluentAssertions;
using SFA.DAS.EmployerCommitmentsV2.Contracts;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Responses;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.Encoding;
using LegalEntity = SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Responses.LegalEntity;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Cohort;

public class WhenMappingAddApprenticeshipCacheModelToSelectLegalEntityViewModelTests
{
    private Mock<IApprovalsApiClient> _apiClient;
    private AddApprenticeshipCacheModelToSelectLegalEntityViewModelMapper _mapper;
    private AddApprenticeshipCacheModel _cacheModel;
    private Mock<IEncodingService> _encodingService;
    private LegalEntity _legalEntity;
    private const long AccountId = 998829;

    [SetUp]
    public void Arrange()
    {
        var autoFixture = new Fixture();
        _apiClient = new Mock<IApprovalsApiClient>();
        _encodingService = new Mock<IEncodingService>();
        _cacheModel = autoFixture.Create<AddApprenticeshipCacheModel>();
        _encodingService.Setup(x => x.Decode(_cacheModel.AccountHashedId, EncodingType.AccountId))
            .Returns(AccountId);
        _legalEntity = autoFixture.Create<LegalEntity>();
        _apiClient.Setup(x => x.GetLegalEntitiesForAccount(_cacheModel.CohortRef, AccountId))
            .ReturnsAsync(new GetLegalEntitiesForAccountResponse
            {
                LegalEntities = new List<LegalEntity>
                {
                    _legalEntity
                }
            });

        _mapper = new AddApprenticeshipCacheModelToSelectLegalEntityViewModelMapper(_apiClient.Object, _encodingService.Object);
    }

    [Test]
    public async Task Then_TransferConnectionCode_Is_Mapped()
    {
        //Act
        var result = await _mapper.Map(_cacheModel);

        //Assert          
        result.TransferConnectionCode.Should().Be(_cacheModel.TransferSenderId);
    }

    [Test]
    public async Task Then_CohortRef_Is_Mapped()
    {
        //Act
        var result = await _mapper.Map(_cacheModel);

        //Assert           
        result.CohortRef.Should().Be(_cacheModel.CohortRef);
    }


    [Test]
    public async Task Then_LegalEntity_Is_Mapped()
    {
        //Act
        var result = await _mapper.Map(_cacheModel);

        //Assert     
        result.LegalEntities.Should().HaveCount(1);
    }

    [Test]
    public async Task Then_GetLegalEntitiesForAccount_Is_Called()
    {
        //Act
        var result = await _mapper.Map(_cacheModel);

        //Assert
        _apiClient.Verify(x => x.GetLegalEntitiesForAccount(_cacheModel.CohortRef, AccountId),
            Times.Once);
    }

    [Test]
    public async Task Then_LegalEntity_Agreement_TemplateVersion_IsMapped()
    {
        //Act
        var result = await _mapper.Map(_cacheModel);

        //Assert
        result.LegalEntities.First().Agreements[0].TemplateVersionNumber.Should().Be(_legalEntity.Agreements.First().TemplateVersionNumber);
    }
}