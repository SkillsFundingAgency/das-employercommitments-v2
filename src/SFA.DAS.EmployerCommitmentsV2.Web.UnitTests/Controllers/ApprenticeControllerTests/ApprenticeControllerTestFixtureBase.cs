using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
//using NUnit.Framework;
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
        protected readonly Mock<IAuthorizationService> _mockAuthorizationService;

        protected readonly ApprenticeController _controller;

        public ApprenticeControllerTestFixtureBase()
        {
            _autoFixture = new Fixture();

            _mockMapper = new Mock<IModelMapper>();
            _mockAuthorizationService = new Mock<IAuthorizationService>();

            _controller = new ApprenticeController(_mockMapper.Object,
                Mock.Of<ICookieStorageService<IndexRequest>>(),
                Mock.Of<ICommitmentsApiClient>(),
                Mock.Of<ILinkGenerator>(),
                Mock.Of<ILogger<ApprenticeController>>(),
                _mockAuthorizationService.Object);
        }

        //public void VerifyViewModel<T>(IActionResult actionResult, T expectedViewModel)
        //{
        //    var result = actionResult as ViewResult;
        //    var viewModel = result.Model;

        //    Assert.IsInstanceOf<T>(viewModel);

        //    var viewModelResult = (T)viewModel;

        //    Assert.AreEqual(expectedViewModel, viewModelResult);
        //}
    }
}
