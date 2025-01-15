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

internal class WhenGettingApprentice
{
    private ApprenticeRequest _apprenticeRequest;
    private CohortController _controller;
    private Fixture _fixture;
    private AddApprenticeshipCacheModel _cacheModel;
    private Mock<ICacheStorageService> _cacheStorageService;

    [SetUp]
    public void SetUp()
    {
        _fixture = new Fixture();
        _apprenticeRequest = _fixture.Build<ApprenticeRequest>()
            .With(x => x.StartMonthYear, "012025")
            .Create();

        _cacheModel = _fixture.Create<AddApprenticeshipCacheModel>();
        _apprenticeRequest.ApprenticeshipSessionKey = _cacheModel.ApprenticeshipSessionKey;

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

    [Test]
    public async Task Apprentice_ShouldUpdateCacheAndRedirectToSelectCourse()
    {
        // Act
        var result = await _controller.Apprentice(_apprenticeRequest);

        // Assert
        _cacheStorageService.Verify(x => x.RetrieveFromCache<AddApprenticeshipCacheModel>(_apprenticeRequest.ApprenticeshipSessionKey.Value), Times.Once);
        _cacheStorageService.Verify(x => x.SaveToCache(_cacheModel.ApprenticeshipSessionKey, It.Is<AddApprenticeshipCacheModel>(m =>
            m.ReservationId == _apprenticeRequest.ReservationId &&
            m.CourseCode == _apprenticeRequest.CourseCode &&
            m.StartMonthYear == _apprenticeRequest.StartMonthYear), 1), Times.Once);

        var redirectResult = result.Should().BeOfType<RedirectToActionResult>().Subject;
        redirectResult.ActionName.Should().Be(nameof(CohortController.SelectCourse));
        redirectResult.RouteValues["AccountHashedId"].Should().Be(_cacheModel.AccountHashedId);
        redirectResult.RouteValues["ApprenticeshipSessionKey"].Should().Be(_cacheModel.ApprenticeshipSessionKey);
    }

    [TearDown]
    public void TearDown() => _controller?.Dispose();

}
