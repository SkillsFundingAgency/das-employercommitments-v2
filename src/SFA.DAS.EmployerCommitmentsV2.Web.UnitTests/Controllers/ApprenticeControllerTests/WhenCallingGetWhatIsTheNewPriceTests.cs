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

    public class WhenCallingGetWhatIsTheNewPriceTestsFixture
    {
        private readonly EmployerLedChangeOfProviderRequest _request;
        private readonly WhatIsTheNewPriceViewModel _viewModel;

        private readonly ApprenticeController _controller;

        public WhenCallingGetWhatIsTheNewPriceTestsFixture()
        {
            var autoFixture = new Fixture();
            var mockMapper = new Mock<IModelMapper>();

            _request = autoFixture.Create<EmployerLedChangeOfProviderRequest>();
            _viewModel = autoFixture.Create<WhatIsTheNewPriceViewModel>();

            mockMapper.Setup(m => m.Map<WhatIsTheNewPriceViewModel>(_request))
                .ReturnsAsync(_viewModel);

            _controller = new ApprenticeController(mockMapper.Object,
                Mock.Of<ICookieStorageService<IndexRequest>>(),
                Mock.Of<ICommitmentsApiClient>(),
                Mock.Of<ILinkGenerator>(),
                Mock.Of<ILogger<ApprenticeController>>(),
                Mock.Of<IAuthorizationService>());
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
