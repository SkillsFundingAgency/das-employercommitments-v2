﻿using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Contracts;
using SFA.DAS.EmployerCommitmentsV2.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.EmployerUrlHelper;
using SFA.DAS.Encoding;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.CohortControllerTests;

[TestFixture]
public class WhenGettingInform
{
    private InformRequest _request;
    private InformViewModel _viewModel;
    private CohortController _controller;
    private Mock<IModelMapper> _modelMapper;
    private Mock<ICacheStorageService> _cacheStorageService;

    [SetUp]
    public void Arrange()
    {
        var autoFixture = new Fixture();
        _request = autoFixture.Create<InformRequest>();
        _viewModel = autoFixture.Create<InformViewModel>();

        _cacheStorageService = new Mock<ICacheStorageService>();
        _cacheStorageService
            .Setup(x => x.SaveToCache(It.IsAny<Guid>(), It.IsAny<AddApprenticeshipCacheModel>(), 1))
            .Returns(Task.CompletedTask);

        _modelMapper = new Mock<IModelMapper>();
        _modelMapper.Setup(x => x.Map<InformViewModel>(It.Is<InformRequest>(r => r == _request)))
            .ReturnsAsync(_viewModel);

        _controller = new CohortController(Mock.Of<ICommitmentsApiClient>(),
            Mock.Of<ILogger<CohortController>>(),
            Mock.Of<ILinkGenerator>(),
            _modelMapper.Object,
            Mock.Of<IEncodingService>(),
            Mock.Of<IApprovalsApiClient>(),
            Mock.Of<ICacheStorageService>());
    }

    [TearDown]
    public void TearDown() => _controller?.Dispose();

    [Test]
    public async Task Then_The_Correct_ViewModel_Is_Returned()
    {
        //Act
        var result = await _controller.Inform(_request) as ViewResult;

        //Assert
        Assert.That(result.Model, Is.InstanceOf<InformViewModel>());
    }
}