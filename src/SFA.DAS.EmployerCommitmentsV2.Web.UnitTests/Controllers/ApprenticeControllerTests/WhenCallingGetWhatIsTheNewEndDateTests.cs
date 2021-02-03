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
    public class WhenCallingGetWhatIsTheNewEndDateTests
    {
        WhenCallingGetWhatIsTheNewEndDateTestsFixture _fixture;

        [SetUp]
        public void Arrange()
        {
            _fixture = new WhenCallingGetWhatIsTheNewEndDateTestsFixture();
        }

        [Test]
        public async Task ThenTheCorrectViewIsReturned()
        {
            var result = await _fixture.WhatIsTheNewEndDate();

            _fixture.VerifyViewModel(result as ViewResult);
        }
    }

    public class WhenCallingGetWhatIsTheNewEndDateTestsFixture : ApprenticeControllerTestFixtureBase
    {
        private readonly ChangeOfProviderRequest _request;
        private readonly WhatIsTheNewEndDateViewModel _viewModel;
        
        public WhenCallingGetWhatIsTheNewEndDateTestsFixture() : base()
        {
            _request = _autoFixture.Create<ChangeOfProviderRequest>();
            _viewModel = _autoFixture.Create<WhatIsTheNewEndDateViewModel>();

            _mockMapper.Setup(m => m.Map<WhatIsTheNewEndDateViewModel>(_request))
                .ReturnsAsync(_viewModel);
        }

        public async Task<IActionResult> WhatIsTheNewEndDate()
        {
            return await _controller.WhatIsTheNewEndDate(_request);
        }

        public void VerifyViewModel(ViewResult viewResult)
        {
            var viewModel = viewResult.Model as WhatIsTheNewEndDateViewModel;

            Assert.IsInstanceOf<WhatIsTheNewEndDateViewModel>(viewModel);
            Assert.AreEqual(_viewModel, viewModel);
        }
    }
}
