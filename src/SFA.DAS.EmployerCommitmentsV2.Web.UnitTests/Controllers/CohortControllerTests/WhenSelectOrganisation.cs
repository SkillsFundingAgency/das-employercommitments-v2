using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EAS.Account.Api.Types;
using SFA.DAS.EmployerCommitmentsV2.Contracts;
using SFA.DAS.EmployerCommitmentsV2.Services;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.EmployerUrlHelper;
using SFA.DAS.Encoding;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.CohortControllerTests;

[TestFixture]
public class WhenSelectOrganisation
{
    private CohortController _controller;
    private SelectLegalEntityViewModel _selectLegalEntityViewModel;
    private SelectLegalEntityRequest _chooseOrganisationRequest;
    private Mock<IModelMapper> _modelMapper;
    private Mock<ILinkGenerator> _linkGenerator;        
    private const string LegalEntityCode = "LCODE";
    private const string LegalEntityName = "LNAME";        
    private const string CohortRefViewModel = "ViewModelCohortReef";

    [SetUp]
    public void Arrange()
    {
        var autoFixture = new Fixture();
        _modelMapper = new Mock<IModelMapper>();
        _linkGenerator = new Mock<ILinkGenerator>();            

        _chooseOrganisationRequest = autoFixture.Create<SelectLegalEntityRequest>();
        _selectLegalEntityViewModel = autoFixture.Create<SelectLegalEntityViewModel>();
        _modelMapper.Setup(x => x.Map<SelectLegalEntityViewModel>(It.Is<SelectLegalEntityRequest>(r => r == _chooseOrganisationRequest)))
            .ReturnsAsync(_selectLegalEntityViewModel);

        _controller = new CohortController(Mock.Of<ICommitmentsApiClient>(),
            Mock.Of<ILogger<CohortController>>(),
            _linkGenerator.Object,
            _modelMapper.Object,
            Mock.Of<IEncodingService>(),
            Mock.Of<IApprovalsApiClient>(), 
            Mock.Of<IReservationsService>());
    }
        
    [TearDown]
    public void TearDown() => _controller?.Dispose();

    [TestCase(true)]
    [TestCase(false)]
    public async Task Then_Verify_ViewModel_Is_Mapped_From_Request(bool isTransfer)
    {
        //Arrange
        _chooseOrganisationRequest.transferConnectionCode = isTransfer ? _chooseOrganisationRequest.transferConnectionCode : string.Empty;

        //Act
        var result = await _controller.SelectLegalEntity(_chooseOrganisationRequest);

        //Assert            
        var viewResult = result as ViewResult;
        var viewModel = viewResult.Model;
        
        Assert.Multiple(() =>
        {
            Assert.That(viewModel, Is.InstanceOf<SelectLegalEntityViewModel>());
            Assert.That((SelectLegalEntityViewModel)viewModel, Is.EqualTo(_selectLegalEntityViewModel));
        });
    }

    [TestCase(true)]
    [TestCase(false)]
    public void Then_Throw_Exception_If_LegalEntity_Is_null(bool isTransfer)
    {
        //Arrange
        _chooseOrganisationRequest.transferConnectionCode = isTransfer ? _chooseOrganisationRequest.transferConnectionCode : string.Empty;
        _modelMapper.Setup(x => x.Map<SelectLegalEntityViewModel>(It.Is<SelectLegalEntityRequest>(r => r == _chooseOrganisationRequest)))
            .ReturnsAsync(new SelectLegalEntityViewModel());
            
        //Assert
        Assert.ThrowsAsync<Exception>(() => _controller.SelectLegalEntity(_chooseOrganisationRequest));
    }

    [TestCase(true)]
    [TestCase(false)]
    public void Then_Throw_Exception_If_LegalEntity_Is_Empty(bool isTransfer)
    {
        //Arrange
        _chooseOrganisationRequest.transferConnectionCode = isTransfer ? _chooseOrganisationRequest.transferConnectionCode : string.Empty;
        _modelMapper.Setup(x => x.Map<SelectLegalEntityViewModel>(It.Is<SelectLegalEntityRequest>(r => r == _chooseOrganisationRequest)))
            .ReturnsAsync(new SelectLegalEntityViewModel { LegalEntities = Enumerable.Empty<LegalEntity>() });

        //Assert
        Assert.ThrowsAsync<Exception>(() => _controller.SelectLegalEntity(_chooseOrganisationRequest));
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
        _modelMapper.Setup(x => x.Map<SelectLegalEntityViewModel>(It.Is<SelectLegalEntityRequest>(r => r == _chooseOrganisationRequest)))
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

        //Act
        _chooseOrganisationRequest.transferConnectionCode = isTransfer ? _chooseOrganisationRequest.transferConnectionCode : string.Empty;
        var result = await _controller.SelectLegalEntity(_chooseOrganisationRequest);

        //Assert
        var redirectToActionResult = result as RedirectToActionResult;
        switch (expectedAction)
        {
            case ExpectedAction.AgreementNotSigned:
                Assert.That(redirectToActionResult.ActionName, Is.EqualTo(expectedAction.ToString()));
                break;
            case ExpectedAction.SelectProvider:
                Assert.That(redirectToActionResult.ActionName, Is.EqualTo(expectedAction.ToString()));
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