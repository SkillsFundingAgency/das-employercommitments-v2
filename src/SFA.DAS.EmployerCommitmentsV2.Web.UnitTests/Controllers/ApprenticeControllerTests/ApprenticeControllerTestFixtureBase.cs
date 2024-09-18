using Microsoft.AspNetCore.Mvc.ViewFeatures;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;


namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.ApprenticeControllerTests;

public class ApprenticeControllerTestFixtureBase
{
    protected readonly Fixture AutoFixture;

    protected readonly Mock<IModelMapper> MockMapper;
    protected readonly Mock<ICommitmentsApiClient> MockCommitmentsApiClient;
    protected readonly Mock<IUrlHelper> MockUrlHelper;
    protected readonly Mock<ITempDataDictionary> TempDataDictionary;
    protected readonly Mock<ICacheStorageService> _cacheStorageService;

    protected readonly ApprenticeController Controller;

    public ApprenticeControllerTestFixtureBase()
    {
        AutoFixture = new Fixture();

        MockMapper = new Mock<IModelMapper>();
        MockCommitmentsApiClient = new Mock<ICommitmentsApiClient>();
        _cacheStorageService = new Mock<ICacheStorageService>();
        MockUrlHelper = new Mock<IUrlHelper>();
        TempDataDictionary = new Mock<ITempDataDictionary>();

        Controller = new ApprenticeController(MockMapper.Object,
            Mock.Of<Interfaces.ICookieStorageService<IndexRequest>>(),
            MockCommitmentsApiClient.Object,
            _cacheStorageService.Object,
            Mock.Of<ILogger<ApprenticeController>>());

        Controller.Url = MockUrlHelper.Object;
        Controller.TempData = TempDataDictionary.Object;
    }
}