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
public class WhenPostingSelectDirectConnection
{
    private CohortController _controller;
    private SelectTransferConnectionViewModel _model;
    private AddApprenticeshipCacheModel _cacheModel;
    private Mock<ICacheStorageService> _cacheStorageService;

    [SetUp]
    public void Arrange()
    {
        var autoFixture = new Fixture();
        _model = autoFixture.Create<SelectTransferConnectionViewModel>();

        _cacheModel = autoFixture.Create<AddApprenticeshipCacheModel>();
        _cacheModel.ApprenticeshipSessionKey = _model.ApprenticeshipSessionKey.Value;

        _cacheStorageService = new Mock<ICacheStorageService>();
        _cacheStorageService
           .Setup(x => x.RetrieveFromCache<AddApprenticeshipCacheModel>(_cacheModel.ApprenticeshipSessionKey))
           .ReturnsAsync(_cacheModel);
        _cacheStorageService
        .Setup(x => x.SaveToCache(It.IsAny<Guid>(), It.IsAny<AddApprenticeshipCacheModel>(), 1))
        .Returns(Task.CompletedTask);

        _controller = new CohortController(Mock.Of<ICommitmentsApiClient>(),
            Mock.Of<ILogger<CohortController>>(),
            Mock.Of<ILinkGenerator>(),
            Mock.Of<IModelMapper>(),
            Mock.Of<IEncodingService>(),
            Mock.Of<IApprovalsApiClient>(),
            _cacheStorageService.Object);
    }
      
    [TearDown]
    public void TearDown() => _controller?.Dispose();

    [Test]
    public async Task Then_User_Is_Redirected_To_SelectProvider_Page()
    {
        //Act
        var result = await _controller.SelectDirectTransferConnection(_model);

        //Assert
        var redirectToActionResult = result as RedirectToActionResult;
        redirectToActionResult.ActionName.Should().Be("SelectProvider");
    }

    [Test]
    public async Task Then_Verify_RouteValues()
    {
        //Act
        var result = await _controller.SelectDirectTransferConnection(_model);

        //Assert
        var redirectToActionResult = result as RedirectToActionResult;
        redirectToActionResult.RouteValues["AccountHashedId"].Should().Be(_model.AccountHashedId);
        redirectToActionResult.RouteValues["ApprenticeshipSessionKey"].Should().Be(_model.ApprenticeshipSessionKey);
    }
}