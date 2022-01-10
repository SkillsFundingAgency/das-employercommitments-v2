using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.ApprenticeControllerTests
{
    public class WhenPostingChangeOptionTests : ApprenticeControllerTestBase
    {
        private ChangeOptionViewModel _viewModel;

        [SetUp]
        public void Arrange()
        {
            var fixture = new Fixture();

            _viewModel = fixture.Create<ChangeOptionViewModel>();

            _mockModelMapper = new Mock<IModelMapper>();

            _controller = new ApprenticeController(
                _mockModelMapper.Object,
                Mock.Of<ICookieStorageService<IndexRequest>>(),
                Mock.Of<ICommitmentsApiClient>(),
                Mock.Of<ILogger<ApprenticeController>>());

            _controller.TempData = new TempDataDictionary(new Mock<HttpContext>().Object, new Mock<ITempDataProvider>().Object);
        }

        [Test]
        public async Task Then_MapperIsCalled()
        {
            await _controller.ChangeOption(_viewModel);

            _mockModelMapper.Verify(m => m.Map<EditApprenticeshipRequestViewModel>(_viewModel), Times.Once());
        }

        [Test]
        public async Task Then_RedirectToConfirmChanges()
        {
            var result = await _controller.ChangeOption(_viewModel) as RedirectToActionResult;

            result.ActionName.Should().Be("ConfirmEditApprenticeship");
        }
    }
}
