using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.ApprenticeControllerTests
{
    public class WhenCallingGetWhoWillEnterTheDetailsTests
    {
        WhenCallingGetWhoWillEnterTheDetailsTestsFixture _fixture;

        [SetUp]
        public void Arrange()
        {
            _fixture = new WhenCallingGetWhoWillEnterTheDetailsTestsFixture();
        }

        [Test]
        public async Task ThenTheCorrectViewIsReturned()
        {
            var result = await _fixture.WhoWillEnterTheDetails();

            _fixture.VerifyViewModel(result as ViewResult);
        }
    }

    public class WhenCallingGetWhoWillEnterTheDetailsTestsFixture : ApprenticeControllerTestFixtureBase
    {
        private readonly ChangeOfProviderRequest _request;
        private readonly WhoWillEnterTheDetailsViewModel _viewModel;

        public WhenCallingGetWhoWillEnterTheDetailsTestsFixture() : base()
        {
            _request = _autoFixture.Create<ChangeOfProviderRequest>();
            _viewModel = _autoFixture.Create<WhoWillEnterTheDetailsViewModel>();

            _mockMapper.Setup(m => m.Map<WhoWillEnterTheDetailsViewModel>(_request))
                .ReturnsAsync(_viewModel);
        }

        public async Task<IActionResult> WhoWillEnterTheDetails()
        {
            return await _controller.WhoWillEnterTheDetails(_request);
        }

        public void VerifyViewModel(ViewResult viewResult)
        {
            var viewModel = viewResult.Model as WhoWillEnterTheDetailsViewModel;

            Assert.That(viewModel, Is.InstanceOf<WhoWillEnterTheDetailsViewModel>());
            Assert.That(viewModel, Is.EqualTo(_viewModel));
        }
    }
}
