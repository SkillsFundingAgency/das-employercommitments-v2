using FluentAssertions;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Contracts;
using SFA.DAS.EmployerCommitmentsV2.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.EmployerUrlHelper;
using SFA.DAS.Encoding;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.CohortControllerTests;

[TestFixture]
public class WhenSelectTransferConnection
{
    private CohortController _controller;
    private SelectTransferConnectionViewModel _selectTransferConnectionViewModel;
    private AddApprenticeshipCacheModel _cacheModel;
    private Mock<IModelMapper> _modelMapper;
    private Mock<ICacheStorageService> _cacheStorageService;

    [SetUp]
    public void Arrange()
    {
        var autoFixture = new Fixture();
        _modelMapper = new Mock<IModelMapper>();

        _cacheModel = autoFixture.Create<AddApprenticeshipCacheModel>();

        _selectTransferConnectionViewModel = autoFixture.Create<SelectTransferConnectionViewModel>();
        _selectTransferConnectionViewModel.ApprenticeshipSessionKey = _cacheModel.ApprenticeshipSessionKey;
        _selectTransferConnectionViewModel.AccountHashedId = _cacheModel.AccountHashedId;

        _modelMapper.Setup(x => x.Map<SelectTransferConnectionViewModel>(
            It.Is<AddApprenticeshipCacheModel>(r => r == _cacheModel)))
            .ReturnsAsync(_selectTransferConnectionViewModel);

        _cacheStorageService = new Mock<ICacheStorageService>();
        _cacheStorageService.Setup(x => x.RetrieveFromCache<AddApprenticeshipCacheModel>(_cacheModel.ApprenticeshipSessionKey))
           .ReturnsAsync(_cacheModel);
        _cacheStorageService.Setup(x => x.SaveToCache(It.IsAny<Guid>(), It.IsAny<AddApprenticeshipCacheModel>(), 1))
         .Returns(Task.CompletedTask);

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

    [Test]
    public async Task Then_User_Is_Redirected_To_SelectLegalEntity_Page()
    {
        //Arrange          
        _selectTransferConnectionViewModel.TransferConnections = new List<TransferConnection>();

        //Act
        var result = await _controller.SelectTransferConnection(_cacheModel.ApprenticeshipSessionKey);

        //Assert
        var redirectToActionResult = result as RedirectToActionResult;
        redirectToActionResult.ActionName.Should().Be("SelectLegalEntity");
    }

    [Test]
    public async Task Then_Verify_ViewModel()
    {
        //Act
        var result = await _controller.SelectTransferConnection(_cacheModel.ApprenticeshipSessionKey);

        //Assert
        var viewResult = result as ViewResult;
        var viewModel = viewResult.Model;

        viewModel.Should().BeOfType<SelectTransferConnectionViewModel>();
        viewModel.As<SelectTransferConnectionViewModel>().Should().BeEquivalentTo(_selectTransferConnectionViewModel);
    }

    [Test]
    public async Task And_No_TransferConnections_Redirect_To_SelectLegalEntity()
    {
        _selectTransferConnectionViewModel.TransferConnections = [];

        _modelMapper.Setup(x => x.Map<SelectTransferConnectionViewModel>(
           It.Is<AddApprenticeshipCacheModel>(r => r == _cacheModel)))
           .ReturnsAsync(_selectTransferConnectionViewModel);

        //Act
        var result = await _controller.SelectTransferConnection(_cacheModel.ApprenticeshipSessionKey) as RedirectToActionResult;

        result.ActionName.Should().Be("SelectLegalEntity");
        result.RouteValues.Should().NotBeEmpty();
        result.RouteValues["AccountHashedId"].Should().Be(_selectTransferConnectionViewModel.AccountHashedId);
        result.RouteValues["ApprenticeshipSessionKey"].Should().Be(_selectTransferConnectionViewModel.ApprenticeshipSessionKey);
    }

    [Test]
    public async Task Then_User_Posts_and_Redirected_To_SelectedLegalEntity()
    {
        //Act
        var result = await _controller.SetTransferConnection(_selectTransferConnectionViewModel) as RedirectToActionResult;

        //Assert
        result.ActionName.Should().Be("SelectLegalEntity");
        result.RouteValues.Should().NotBeEmpty();
        result.RouteValues["AccountHashedId"].Should().Be(_selectTransferConnectionViewModel.AccountHashedId);
        result.RouteValues["ApprenticeshipSessionKey"].Should().Be(_selectTransferConnectionViewModel.ApprenticeshipSessionKey);
    }
}