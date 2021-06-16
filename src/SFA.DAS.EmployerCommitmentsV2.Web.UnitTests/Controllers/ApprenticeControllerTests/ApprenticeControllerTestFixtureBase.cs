using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SFA.DAS.Authorization.Services;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.EmployerUrlHelper;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.ApprenticeControllerTests
{
    public class ApprenticeControllerTestFixtureBase
    {
        protected Fixture _autoFixture;

        protected readonly Mock<IModelMapper> _mockMapper;
        protected readonly Mock<ICommitmentsApiClient> _mockCommitmentsApiClient;
        protected readonly Mock<ILinkGenerator> _mockLinkGenerator;
        protected readonly Mock<IAuthorizationService> _mockAuthorizationService;
        protected readonly Mock<IUrlHelper> _mockUrlHelper;
        
        protected readonly ApprenticeController _controller;

        public ApprenticeControllerTestFixtureBase()
        {
            _autoFixture = new Fixture();

            _mockMapper = new Mock<IModelMapper>();
            _mockCommitmentsApiClient = new Mock<ICommitmentsApiClient>();
            _mockLinkGenerator = new Mock<ILinkGenerator>();
            _mockAuthorizationService = new Mock<IAuthorizationService>();
            _mockUrlHelper = new Mock<IUrlHelper>();

            _controller = new ApprenticeController(_mockMapper.Object,
                Mock.Of<ICookieStorageService<IndexRequest>>(),
                _mockCommitmentsApiClient.Object,
                _mockLinkGenerator.Object,
                Mock.Of<ILogger<ApprenticeController>>());

            _controller.Url = _mockUrlHelper.Object;
        }
    }
}
