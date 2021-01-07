

using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.EmployerUrlHelper;
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

    public class WhenRequestingTheWhatIsTheNewStartDatePageTestFixture
    {
        private readonly WhatIsTheNewStartDateRequest _request;
        private readonly WhatIsTheNewStartDateViewModel _viewModel;

        private readonly Mock<IModelMapper> _mockMapper;

        private readonly ApprenticeController _controller;

        public WhenRequestingTheWhatIsTheNewStartDatePageTestFixture()
        {
            var autoFixture = new Fixture();

            _request = autoFixture.Create<WhatIsTheNewStartDateRequest>();
            _viewModel = autoFixture.Create<WhatIsTheNewStartDateViewModel>();

            _mockMapper = new Mock<IModelMapper>();
            _mockMapper.Setup(m => m.Map<WhatIsTheNewStartDateViewModel>(_request))
                .ReturnsAsync(_viewModel);

            _controller = new ApprenticeController(_mockMapper.Object,
                Mock.Of<ICookieStorageService<IndexRequest>>(),
                Mock.Of<ICommitmentsApiClient>(),
                Mock.Of<ILinkGenerator>(),
                Mock.Of<ILogger<ApprenticeController>>());
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
