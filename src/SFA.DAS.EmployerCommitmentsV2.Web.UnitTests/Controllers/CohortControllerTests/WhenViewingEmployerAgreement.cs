﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
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
public class WhenViewingEmployerAgreement
{
    private CohortController _controller;
    private ViewEmployerAgreementRequest _viewEmployerAgreementRequest;
    private ViewEmployerAgreementModel _viewEmployerAgreementModel;
    private Mock<IModelMapper> _modelMapper;
    private Mock<ILinkGenerator> _linkGenerator;
    private Mock<ICacheStorageService> _cacheStorageService;
    private string _organisationAgreementsUrl;
    private string _agreementUrl;

    [SetUp]
    public void Arrange()
    {
        var autoFixture = new Fixture();
        _modelMapper = new Mock<IModelMapper>();
        _linkGenerator = new Mock<ILinkGenerator>();
        _cacheStorageService = new Mock<ICacheStorageService>();

        _viewEmployerAgreementModel = autoFixture.Create<ViewEmployerAgreementModel>();
        _viewEmployerAgreementRequest = autoFixture.Create<ViewEmployerAgreementRequest>();
        _modelMapper.Setup(x => x.Map<ViewEmployerAgreementRequest>(It.IsAny<DetailsViewModel>()))
            .ReturnsAsync(_viewEmployerAgreementRequest);

        _organisationAgreementsUrl = $"accounts/{_viewEmployerAgreementRequest.AccountHashedId}/agreements/";
        _agreementUrl = $"accounts/{_viewEmployerAgreementRequest.AccountHashedId}/agreements/{_viewEmployerAgreementRequest.AgreementHashedId}/about-your-agreement";

        _controller = new CohortController(Mock.Of<ICommitmentsApiClient>(),
            Mock.Of<ILogger<CohortController>>(),
            _linkGenerator.Object,
            _modelMapper.Object,
            Mock.Of<IEncodingService>(),
            Mock.Of<IApprovalsApiClient>(),
            _cacheStorageService.Object);

        _controller.TempData = new TempDataDictionary(new Mock<HttpContext>().Object, new Mock<ITempDataProvider>().Object);
    }

    [TearDown]
    public void TearDown() => _controller?.Dispose();

    [Test]
    public async Task Then_User_Is_Redirected_To_View_Organisations_Agreements_When_NoCacheData()
    {
        // Arrange
        _linkGenerator.Setup(linkGen => linkGen.AccountsLink(_organisationAgreementsUrl))
            .Returns(_organisationAgreementsUrl);

        //Act
        var result = await _controller.ViewAgreement(_viewEmployerAgreementRequest.AccountHashedId) as RedirectResult;

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Url, Is.EqualTo(_organisationAgreementsUrl));
    }

    [Test]
    public async Task Then_User_Is_Redirected_To_View_Agreement_If_AgreementID_In_Cache()
    {
        // Arrange         
        SetUpCacheData();

        _linkGenerator.Setup(linkGen =>
                linkGen.AccountsLink(_agreementUrl))
            .Returns(_agreementUrl);

        //Act
        var result = await _controller.ViewAgreement(_viewEmployerAgreementModel.AccountHashedId) as RedirectResult;

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Url, Is.EqualTo(_agreementUrl));
    }

    private void SetUpCacheData()
    {
        _cacheStorageService
            .Setup(d => d.SafeRetrieveFromCache<ViewEmployerAgreementModel>(nameof(ViewEmployerAgreementModel)))
            .ReturnsAsync(_viewEmployerAgreementModel);
    }
}