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
public class WhenPostingSelectLegalEntity
{
    private Mock<IModelMapper> _mockMapper;
    private CohortController _controller;
    private Fixture _fixture;
    private AddApprenticeshipCacheModel _cacheModel;
    private Mock<ICacheStorageService> _cacheStorageService;
    private SelectLegalEntityViewModel _selectLegalEntityViewModel;

    [SetUp]
    public void SetUp()
    {
        _fixture = new Fixture();

        _selectLegalEntityViewModel = _fixture.Create<SelectLegalEntityViewModel>();

        _cacheModel = _fixture.Create<AddApprenticeshipCacheModel>();
        _cacheModel.ApprenticeshipSessionKey = _selectLegalEntityViewModel.ApprenticeshipSessionKey.Value;

        _cacheStorageService = new Mock<ICacheStorageService>();
        _cacheStorageService
           .Setup(x => x.RetrieveFromCache<AddApprenticeshipCacheModel>(_cacheModel.ApprenticeshipSessionKey))
           .ReturnsAsync(_cacheModel);
        _cacheStorageService
        .Setup(x => x.SaveToCache(It.IsAny<Guid>(), It.IsAny<AddApprenticeshipCacheModel>(), 1))
        .Returns(Task.CompletedTask);

        _mockMapper = new Mock<IModelMapper>();
        _controller = new CohortController(Mock.Of<ICommitmentsApiClient>(),
            Mock.Of<ILogger<CohortController>>(),
            Mock.Of<ILinkGenerator>(),
            _mockMapper.Object,
            Mock.Of<IEncodingService>(),
            Mock.Of<IApprovalsApiClient>(),
            _cacheStorageService.Object);
    }

    [TearDown]
    public void TearDown() => _controller?.Dispose();

    [Test]
    public async Task SetLegalEntity_WhenAgreementSigned_ShouldRedirectToSelectProvider()
    {
        // Arrange
        var response = _fixture.Build<LegalEntitySignedAgreementViewModel>()
                               .With(x => x.HasSignedMinimumRequiredAgreementVersion, true)
                               .Create();

        _mockMapper.Setup(x => x.Map<LegalEntitySignedAgreementViewModel>(_selectLegalEntityViewModel))
                   .ReturnsAsync(response);

        // Act
        var result = await _controller.SetLegalEntity(_selectLegalEntityViewModel);

        // Assert
        var redirectResult = result.Should().BeOfType<RedirectToActionResult>().Subject;
        redirectResult.ActionName.Should().Be("SelectProvider");
        redirectResult.RouteValues["AccountHashedId"].Should().Be(_cacheModel.AccountHashedId);
        redirectResult.RouteValues["ApprenticeshipSessionKey"].Should().Be(_cacheModel.ApprenticeshipSessionKey);
    }

    [Test]
    public async Task SetLegalEntity_WhenAgreementNotSigned_ShouldRedirectToAgreementNotSigned()
    {
        // Arrange
        var response = _fixture.Build<LegalEntitySignedAgreementViewModel>()
                               .With(x => x.HasSignedMinimumRequiredAgreementVersion, false)
                               .Create();

        _mockMapper.Setup(x => x.Map<LegalEntitySignedAgreementViewModel>(_selectLegalEntityViewModel))
                   .ReturnsAsync(response);

        // Act
        var result = await _controller.SetLegalEntity(_selectLegalEntityViewModel) as RedirectToActionResult;

        // Assert
        result.ActionName.Should().Be("AgreementNotSigned");
        result.RouteValues.Should().NotBeEmpty();

        result.RouteValues["AccountHashedId"].Should().Be(_selectLegalEntityViewModel.AccountHashedId);
        result.RouteValues["CohortRef"].Should().Be(_selectLegalEntityViewModel.CohortRef);
        result.RouteValues["LegalEntityName"].Should().Be(response.LegalEntityName);
        result.RouteValues["AccountLegalEntityHashedId"].Should().Be(response.AccountLegalEntityHashedId);
        result.RouteValues["EncodedPledgeApplicationId"].Should().Be(_selectLegalEntityViewModel.EncodedPledgeApplicationId);
    }
}
