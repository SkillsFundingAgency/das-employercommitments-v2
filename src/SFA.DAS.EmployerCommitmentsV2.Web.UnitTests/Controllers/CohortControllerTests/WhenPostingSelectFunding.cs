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
    private AddApprenticeshipCacheModel _cacheModel;
    private Mock<ICacheStorageService> _cacheStorageService;
    private Mock<ILinkGenerator> _linkGenerator;

    [SetUp]
    public void Arrange()
    {
        var autoFixture = new Fixture();
        _model = autoFixture.Create<SelectFundingViewModel>();

        _cacheModel = autoFixture.Create<AddApprenticeshipCacheModel>();
        _cacheModel.ApprenticeshipSessionKey = _model.ApprenticeshipSessionKey.Value;

        _cacheStorageService = new Mock<ICacheStorageService>();
        _cacheStorageService.Setup(x => x.RetrieveFromCache<AddApprenticeshipCacheModel>(_cacheModel.ApprenticeshipSessionKey))
           .ReturnsAsync(_cacheModel);

        _linkGenerator = new Mock<ILinkGenerator>();
        _linkGenerator.Setup(x => x.ReservationsLink(It.IsAny<string>())).Returns("https://reservations.com");

        _controller = new CohortController(Mock.Of<ICommitmentsApiClient>(),
            Mock.Of<ILogger<CohortController>>(),
            _linkGenerator.Object,
            Mock.Of<IModelMapper>(),
            Mock.Of<IEncodingService>(),
            Mock.Of<IApprovalsApiClient>(),
            _cacheStorageService.Object);
    }
      
    [TearDown]
    public void TearDown() => _controller?.Dispose();

    [Test]
    public async Task And_SelectedFunding_Is_Direct_Transfer_Then_Redirect_To_SelectDirectTransferConnection()
    {
        _model.FundingType = FundingType.DirectTransfers;

        var result = await _controller.SelectFundingType(_model);

        var redirectToActionResult = result as RedirectToActionResult;
        redirectToActionResult.ActionName.Should().Be("SelectDirectTransferConnection");
        redirectToActionResult.RouteValues["AccountHashedId"].Should().Be(_cacheModel.AccountHashedId);
        redirectToActionResult.RouteValues["ApprenticeshipSessionKey"].Should().Be(_cacheModel.ApprenticeshipSessionKey);
    }

    [Test]
    public async Task And_SelectedFunding_Is_UnallocatedReservations_Then_Redirect_To_Reservation()
    {
        _model.FundingType = FundingType.UnallocatedReservations;

        var result = await _controller.SelectFundingType(_model);

        _linkGenerator.Verify(x => x.ReservationsLink($"accounts/{_cacheModel.AccountHashedId}/reservations/{_cacheModel.AccountLegalEntityHashedId}/select?" +
                                                      $"&beforeProviderSelected=true" +
                                                      $"&apprenticeshipSessionKey={_cacheModel.ApprenticeshipSessionKey}"));
        var redirectResult = result as RedirectResult;
        redirectResult.Url.Should().Be("https://reservations.com");
    }

    [TestCase(FundingType.CurrentLevy)]
    [TestCase(FundingType.AdditionalReservations)]
    public async Task And_SelectedFunding_Is_AnotherType(FundingType fundingType)
    {
        _model.FundingType = fundingType;

        var result = await _controller.SelectFundingType(_model);

        var redirectToActionResult = result as RedirectToActionResult;
        redirectToActionResult.ActionName.Should().Be("SelectProvider");
        redirectToActionResult.RouteValues["AccountHashedId"].Should().Be(_cacheModel.AccountHashedId);
        redirectToActionResult.RouteValues["ApprenticeshipSessionKey"].Should().Be(_cacheModel.ApprenticeshipSessionKey);
    }
}