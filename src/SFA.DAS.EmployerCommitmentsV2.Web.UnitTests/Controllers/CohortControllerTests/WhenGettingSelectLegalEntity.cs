using FluentAssertions;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EAS.Account.Api.Types;
using SFA.DAS.EmployerCommitmentsV2.Contracts;
using SFA.DAS.EmployerCommitmentsV2.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.EmployerUrlHelper;
using SFA.DAS.Encoding;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.CohortControllerTests;

[TestFixture]
public class WhenGettingSelectLegalEntity
{
    private CohortController _controller;
    private SelectLegalEntityViewModel _selectLegalEntityViewModel;
    private Mock<IModelMapper> _modelMapper;
    private const string LegalEntityCode = "LCODE";
    private const string LegalEntityName = "LNAME";
    private const string CohortRefViewModel = "ViewModelCohortReef";
    private const string EncodedPledgeApplicationId = "PLDG";
    private const string TransferConnectionCode = "TRNSCD";
    private const string AccountHashedId = "ACCNTID";

    private AddApprenticeshipCacheModel _cacheModel;
    private Mock<ICacheStorageService> _cacheStorageService;

    [SetUp]
    public void Arrange()
    {
        var autoFixture = new Fixture();
        _modelMapper = new Mock<IModelMapper>();

        _cacheModel = autoFixture.Create<AddApprenticeshipCacheModel>();

        _cacheStorageService = new Mock<ICacheStorageService>();
        _cacheStorageService
           .Setup(x => x.RetrieveFromCache<AddApprenticeshipCacheModel>(_cacheModel.ApprenticeshipSessionKey))
           .ReturnsAsync(_cacheModel);

        _cacheStorageService
        .Setup(x => x.SaveToCache(It.IsAny<Guid>(), It.IsAny<AddApprenticeshipCacheModel>(), 1))
        .Returns(Task.CompletedTask);

        _selectLegalEntityViewModel = autoFixture.Create<SelectLegalEntityViewModel>();
        _modelMapper.Setup(x => x.Map<SelectLegalEntityViewModel>(
            It.Is<AddApprenticeshipCacheModel>(r => r == _cacheModel)))
            .ReturnsAsync(_selectLegalEntityViewModel);

        _controller = new CohortController(Mock.Of<ICommitmentsApiClient>(),
            Mock.Of<ILogger<CohortController>>(),
            Mock.Of<ILinkGenerator>(),
            _modelMapper.Object,
            Mock.Of<IEncodingService>(),
            Mock.Of<IApprovalsApiClient>(),
            _cacheStorageService.Object);
    }

    [TearDown]
    public void TearDown() => _controller?.Dispose();

    [TestCase(true)]
    [TestCase(false)]
    public async Task Then_Verify_ViewModel_Is_Mapped_From_CacheModel(bool isTransfer)
    {
        //Arrange

        //Act
        var result = await _controller.SelectLegalEntity(
            AccountHashedId,
            _cacheModel.ApprenticeshipSessionKey,
            isTransfer ? TransferConnectionCode : null,
            isTransfer ? EncodedPledgeApplicationId : null);

        //Assert            
        var viewResult = result as ViewResult;
        var viewModel = viewResult.Model;

        viewModel.Should().BeOfType<SelectLegalEntityViewModel>();
        viewModel.As<SelectLegalEntityViewModel>().Should().BeEquivalentTo(_selectLegalEntityViewModel);
    }

    [TestCase(true)]
    [TestCase(false)]
    public async Task Then_Throw_Exception_If_LegalEntity_Is_null(bool isTransfer)
    {
        //Arrange
        _modelMapper.Setup(x => x.Map<SelectLegalEntityViewModel>(It.Is<AddApprenticeshipCacheModel>(r => r == _cacheModel)))
            .ReturnsAsync(new SelectLegalEntityViewModel());

        //Assert
        await _controller.Invoking(c => c.SelectLegalEntity(
            AccountHashedId,
            _cacheModel.ApprenticeshipSessionKey,
            isTransfer ? TransferConnectionCode : null,
            isTransfer ? EncodedPledgeApplicationId : null))
        .Should().ThrowAsync<Exception>();
    }

    [TestCase(true)]
    [TestCase(false)]
    public async Task Then_Throw_Exception_If_LegalEntity_Is_Empty(bool isTransfer)
    {
        //Arrange
        _modelMapper.Setup(x => x.Map<SelectLegalEntityViewModel>(It.Is<AddApprenticeshipCacheModel>(r => r == _cacheModel)))
            .ReturnsAsync(new SelectLegalEntityViewModel { LegalEntities = Enumerable.Empty<LegalEntity>() });

        //Assert
        await _controller.Invoking(c => c.SelectLegalEntity(
            AccountHashedId,
            _cacheModel.ApprenticeshipSessionKey,
            isTransfer ? TransferConnectionCode : null,
            isTransfer ? EncodedPledgeApplicationId : null))
            .Should().ThrowAsync<Exception>();
    }

    [TestCase(true, EmployerAgreementStatus.Signed, 1, ExpectedAction.AgreementNotSigned)]
    [TestCase(true, EmployerAgreementStatus.Signed, 2, ExpectedAction.SelectProvider)]
    [TestCase(true, EmployerAgreementStatus.Pending, 0, ExpectedAction.AgreementNotSigned)]
    [TestCase(true, EmployerAgreementStatus.Expired, 0, ExpectedAction.AgreementNotSigned)]
    [TestCase(true, EmployerAgreementStatus.Removed, 0, ExpectedAction.AgreementNotSigned)]
    [TestCase(true, EmployerAgreementStatus.Superseded, 0, ExpectedAction.AgreementNotSigned)]
    [TestCase(false, EmployerAgreementStatus.Signed, 1, ExpectedAction.SelectProvider)]
    [TestCase(false, EmployerAgreementStatus.Pending, 1, ExpectedAction.AgreementNotSigned)]
    [TestCase(false, EmployerAgreementStatus.Expired, 1, ExpectedAction.AgreementNotSigned)]
    [TestCase(false, EmployerAgreementStatus.Removed, 1, ExpectedAction.AgreementNotSigned)]
    [TestCase(false, EmployerAgreementStatus.Expired, 1, ExpectedAction.AgreementNotSigned)]
    public async Task Then_with_single_legal_entity_then_redirects_correctly(bool isTransfer, EmployerAgreementStatus status, int templateVersionNumber, ExpectedAction expectedAction)
    {
        //Arrange                        
        _modelMapper.Setup(x => x.Map<SelectLegalEntityViewModel>(It.Is<AddApprenticeshipCacheModel>(r => r == _cacheModel)))
            .ReturnsAsync(new SelectLegalEntityViewModel
            {
                LegalEntities = new[] { new LegalEntity
                {
                    Agreements = new List<Agreement> {new Agreement
                    {
                        Status = status,
                        TemplateVersionNumber = templateVersionNumber
                    }},
                    Code = LegalEntityCode,
                    Name = LegalEntityName
                }},
                CohortRef = CohortRefViewModel
            });

        if (!isTransfer)
        {
            _cacheModel.TransferSenderId = null;
            _cacheStorageService
           .Setup(x => x.RetrieveFromCache<AddApprenticeshipCacheModel>(_cacheModel.ApprenticeshipSessionKey))
           .ReturnsAsync(_cacheModel);
        }
        //Act
        var result = await _controller.SelectLegalEntity(
            AccountHashedId,
            _cacheModel.ApprenticeshipSessionKey,
            isTransfer ? TransferConnectionCode : null,
            isTransfer ? EncodedPledgeApplicationId : null);

        //Assert
        var redirectToActionResult = result as RedirectToActionResult;
        redirectToActionResult.Should().NotBeNull();

        switch (expectedAction)
        {
            case ExpectedAction.AgreementNotSigned:
                redirectToActionResult.ActionName.Should().Be(expectedAction.ToString());
                break;
            case ExpectedAction.SelectProvider:
                redirectToActionResult.ActionName.Should().Be(expectedAction.ToString());
                break;
            default:
                throw new NotImplementedException();
        }
    }

    public enum ExpectedAction
    {
        AgreementNotSigned,
        SelectProvider
    }
}