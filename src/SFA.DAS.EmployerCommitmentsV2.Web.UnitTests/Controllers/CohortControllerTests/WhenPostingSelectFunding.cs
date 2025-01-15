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
public class WhenPostingSelectFunding
{
    private CohortController _controller;
    private SelectFundingViewModel _model;
    private Mock<ICacheStorageService> _cacheStorageService;
    private AddApprenticeshipCacheModel _cacheModel;

    [SetUp]
    public void Arrange()
    {
        var autoFixture = new Fixture();
        _model = autoFixture.Create<SelectFundingViewModel>();
        _cacheModel = autoFixture.Create<AddApprenticeshipCacheModel>();
        _cacheModel.ApprenticeshipSessionKey = _model.ApprenticeshipSessionKey.Value;
        _cacheModel.AccountHashedId = _model.AccountHashedId;

        _cacheStorageService = new Mock<ICacheStorageService>();

        _cacheStorageService
          .Setup(x => x.RetrieveFromCache<AddApprenticeshipCacheModel>(_cacheModel.ApprenticeshipSessionKey))
          .ReturnsAsync(_cacheModel);

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

    [TestCase(FundingType.AdditionalReservations)]
    [TestCase(FundingType.UnallocatedReservations)]
    [TestCase(FundingType.CurrentLevy)]
    public async Task And_This_Funding_Option_Is_Selected_Then_User_Is_Redirected_To_SelectProvider_Page(FundingType fundingType)
    {
        _model.FundingType = fundingType;
        //Act
        var result = await _controller.SelectFundingType(_model);

        //Assert
        var redirectToActionResult = result as RedirectToActionResult;
        redirectToActionResult.ActionName.Should().Be("SelectProvider");
    }

    [Test]
    public async Task And_DirectTransfers_Is_Selected_Then_User_Is_Redirected_To_SelectDirectTransferConnection_Page()
    {
        _model.FundingType = FundingType.DirectTransfers;
        //Act
        var result = await _controller.SelectFundingType(_model);

        //Assert
        var redirectToActionResult = result as RedirectToActionResult;
        redirectToActionResult.ActionName.Should().Be("SelectDirectTransferConnection");
    }

    [Test]
    public async Task And_LtmTransfers_Is_Selected_Then_User_Is_Redirected_To_SelectLtmTransfers_Page()
    {
        _model.FundingType = FundingType.LtmTransfers;
        //Act
        var result = await _controller.SelectFundingType(_model);

        //Assert
        var redirectToActionResult = result as RedirectToActionResult;
        redirectToActionResult.ActionName.Should().Be("SelectAcceptedLevyTransferConnection");
        redirectToActionResult.RouteValues["AccountHashedId"].Should().Be(_model.AccountHashedId);
        redirectToActionResult.RouteValues["ApprenticeshipSessionKey"].Should().Be(_model.ApprenticeshipSessionKey);
    }

}