using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.EmployerUrlHelper;
using System;
using System.Collections.Generic;
using System.Text;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.ApprenticeControllerTests
{
    public class WhenCallingChangeProviderRequestedConfirmationPageTests
    {
        private WhenCallingChangeProviderRequestedConfirmationPageTestsFixture _fixture;

        [SetUp]
        public void Arrange()
        {
            _fixture = new WhenCallingChangeProviderRequestedConfirmationPageTestsFixture();

        }

        [Test]
        public void ThenViewIsReturned()
        {

        }

        [Test]
        public void ThenGetCohortWillBeCalled()
        {

        }

        [Test]
        public void ThenViewModelIsPopulatedWithApprenticeAndProviderDetails()
        {

        }
    }

    public class WhenCallingChangeProviderRequestedConfirmationPageTestsFixture
    {
        private ChangeProviderRequestedConfirmationRequest _request;
        private ChangeProviderRequestedConfirmationViewModel _viewModel;

        private Mock<IModelMapper> _mockMapper;
        private Mock<ICommitmentsApiClient> _mockCommitmentsApiClient;

        private ApprenticeController _controller;

        public WhenCallingChangeProviderRequestedConfirmationPageTestsFixture()
        {
            var autoFixture = new Fixture();
            _request = autoFixture.Create<ChangeProviderRequestedConfirmationRequest>();
            _viewModel = autoFixture.Create<ChangeProviderRequestedConfirmationViewModel>();

            _mockMapper = new Mock<IModelMapper>();
            _mockMapper.Setup(m => m.Map<ChangeProviderRequestedConfirmationViewModel>(_request))
                .ReturnsAsync(_viewModel);

            _mockCommitmentsApiClient = new Mock<ICommitmentsApiClient>();

            var controller = new ApprenticeController(_mockMapper.Object,
                Mock.Of<ICookieStorageService<IndexRequest>>(),
                _mockCommitmentsApiClient.Object,
                Mock.Of<ILinkGenerator>());
        }
    }
}
