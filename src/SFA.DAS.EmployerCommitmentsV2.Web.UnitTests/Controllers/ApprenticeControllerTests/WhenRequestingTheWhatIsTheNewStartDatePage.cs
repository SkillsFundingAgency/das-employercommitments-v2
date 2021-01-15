using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.ApprenticeControllerTests
{
    public class WhenRequestingTheWhatIsTheNewStartDatePage 
    {
        private WhenRequestingTheWhatIsTheNewStartDatePageTestFixture _fixture;

        [SetUp]
        public void Arrange()
        {
            _fixture = new WhenRequestingTheWhatIsTheNewStartDatePageTestFixture();
        }

        [Test]
        public async Task ThenTheCorrectViewIsReturned()
        {
            var result = await _fixture.WhatIsTheNewStartDate();

            _fixture.VerifyViewModel(result as ViewResult);
        }
    }

    public class WhenRequestingTheWhatIsTheNewStartDatePageTestFixture : ApprenticeControllerTestFixtureBase
    {
        private readonly ChangeOfProviderRequest _request;
        private readonly WhatIsTheNewStartDateViewModel _viewModel;

        public WhenRequestingTheWhatIsTheNewStartDatePageTestFixture() : base()
        {
            _request = _autoFixture.Create<ChangeOfProviderRequest>();
            _viewModel = _autoFixture.Create<WhatIsTheNewStartDateViewModel>();

            _mockMapper.Setup(m => m.Map<WhatIsTheNewStartDateViewModel>(_request))
                .ReturnsAsync(_viewModel);
        }

        public async Task<IActionResult> WhatIsTheNewStartDate()
        {
            return await _controller.WhatIsTheNewStartDate(_request);
        }

        public void VerifyViewModel(ViewResult viewResult)
        {
            var viewModel = viewResult.Model as WhatIsTheNewStartDateViewModel;

            Assert.IsInstanceOf<WhatIsTheNewStartDateViewModel>(viewModel);
            Assert.AreEqual(_viewModel, viewModel);
        }
    }
}
