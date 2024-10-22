using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Contracts;
using SFA.DAS.EmployerCommitmentsV2.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.EmployerUrlHelper;
using SFA.DAS.Encoding;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.CohortControllerTests;

[TestFixture]
public class WhenSelectTransferConnection
{
    private CohortController _controller;
    private SelectTransferConnectionViewModel _informViewModel;
    private InformRequest _informRequest;        
    private Mock<IModelMapper> _modelMapper;
    private Mock<ILinkGenerator> _linkGenerator;

    [SetUp]
    public void Arrange()
    {
        var autoFixture = new Fixture();
        _modelMapper = new Mock<IModelMapper>();
        _linkGenerator = new Mock<ILinkGenerator>();

        _informRequest = autoFixture.Create<InformRequest>();
        _informViewModel = autoFixture.Create<SelectTransferConnectionViewModel>();
        _modelMapper.Setup(x => x.Map<SelectTransferConnectionViewModel>(It.Is<InformRequest>(r => r == _informRequest)))
            .ReturnsAsync(_informViewModel);

        _controller = new CohortController(Mock.Of<ICommitmentsApiClient>(),
            Mock.Of<ILogger<CohortController>>(),
            _linkGenerator.Object,
            _modelMapper.Object,
            Mock.Of<IEncodingService>(),
            Mock.Of<IApprovalsApiClient>(),
            Mock.Of<ICacheStorageService>());
    }
      
    [TearDown]
    public void TearDown() => _controller?.Dispose();

    [Test]
    public async Task Then_User_Is_Redirected_To_SelectLegalEntity_Page()
    {
        //Arrange          
        _informViewModel.TransferConnections = new List<TransferConnection>();

        //Act
        var result = await _controller.SelectTransferConnection(_informRequest);

        //Assert
        var redirectToActionResult = result as RedirectToActionResult;
        Assert.That(redirectToActionResult.ActionName, Is.EqualTo("SelectLegalEntity"));
    }

    [Test]
    public async Task Then_Verify_ViewModel()
    {
        //Act
        var result = await _controller.SelectTransferConnection(_informRequest);

        //Assert
        var viewResult = result as ViewResult;
        var viewModel = viewResult.Model;
        
        Assert.Multiple(() =>
        {
            Assert.That(viewModel, Is.InstanceOf<SelectTransferConnectionViewModel>());
            Assert.That((SelectTransferConnectionViewModel)viewModel, Is.EqualTo(_informViewModel));
        });
    }

    [Test]
    public void Then_User_Redirected_To_SelectedLegalEntity()
    {
        //Act
        var result = _controller.SetTransferConnection(_informViewModel);

        //Assert
        var redirectToActionResult = result as RedirectToActionResult;
        Assert.That(redirectToActionResult.ActionName, Is.EqualTo("SelectLegalEntity"));
    }
}