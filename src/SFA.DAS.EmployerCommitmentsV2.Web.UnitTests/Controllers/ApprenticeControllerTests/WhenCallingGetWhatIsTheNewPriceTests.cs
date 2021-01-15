using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.Authorization.Services;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.EmployerUrlHelper;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.ApprenticeControllerTests
{
    public class WhenCallingGetWhatIsTheNewPriceTests
    {
        WhenCallingGetWhatIsTheNewPriceTestsFixture _fixture;

        [SetUp]
        public void Arrange()
        {
            _fixture = new WhenCallingGetWhatIsTheNewPriceTestsFixture();
        }

        [Test]
        public async Task ThenTheCorrectViewIsReturned()
        {
            var result = await _fixture.WhatIsTheNewPrice();

            _fixture.VerifyViewModel(result as ViewResult);
        }
    }

    public class WhenCallingGetWhatIsTheNewPriceTestsFixture : ApprenticeControllerTestFixtureBase
    {
        private readonly ChangeOfProviderRequest _request;
        private readonly WhatIsTheNewPriceViewModel _viewModel;


        public WhenCallingGetWhatIsTheNewPriceTestsFixture() : base()
        {
            _request = _autoFixture.Create<ChangeOfProviderRequest>();
            _viewModel = _autoFixture.Create<WhatIsTheNewPriceViewModel>();

            _mockMapper.Setup(m => m.Map<WhatIsTheNewPriceViewModel>(_request))
                .ReturnsAsync(_viewModel);
        }

        public async Task<IActionResult> WhatIsTheNewPrice()
        {
            return await _controller.WhatIsTheNewPrice(_request);
        }

        public void VerifyViewModel(ViewResult viewResult)
        {
            var viewModel = viewResult.Model as WhatIsTheNewPriceViewModel;

            Assert.IsInstanceOf<WhatIsTheNewPriceViewModel>(viewModel);
            Assert.AreEqual(_viewModel, viewModel);
        }
    }
}
