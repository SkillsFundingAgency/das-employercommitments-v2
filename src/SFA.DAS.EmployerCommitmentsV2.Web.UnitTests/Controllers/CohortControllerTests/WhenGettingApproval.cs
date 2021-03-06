﻿using System.Threading.Tasks;
using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.Authorization.Services;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.EmployerUrlHelper;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.CohortControllerTests
{
    [TestFixture]
    public class WhenGettingApproval
    {
        private WhenGettingApprovalTestFixture _fixture;

        [SetUp]
        public void Arrange()
        {
            _fixture = new WhenGettingApprovalTestFixture();
        }

        [Test]
        public async Task ThenViewModelShouldBeMappedFromRequest()
        {
            await _fixture.GetApproved();
            _fixture.VerifyViewModelIsMappedFromRequest();
        }

        public class WhenGettingApprovalTestFixture
        {
            private readonly ApprovedRequest _request;
            private readonly ApprovedViewModel _viewModel;
            private IActionResult _result;
            private readonly string _linkGeneratorResult;

            public WhenGettingApprovalTestFixture()
            {
                var autoFixture = new Fixture();

                _request = autoFixture.Create<ApprovedRequest>();
                _viewModel = autoFixture.Create<ApprovedViewModel>();
                _viewModel.WithParty = Party.Provider;

                var modelMapper = new Mock<IModelMapper>();
                modelMapper.Setup(x => x.Map<ApprovedViewModel>(It.Is<ApprovedRequest>(r => r == _request)))
                    .ReturnsAsync(_viewModel);

                _linkGeneratorResult = autoFixture.Create<string>();
                var linkGenerator = new Mock<ILinkGenerator>();
                linkGenerator.Setup(x => x.CommitmentsLink(It.IsAny<string>()))
                    .Returns(_linkGeneratorResult);

                CohortController = new CohortController(Mock.Of<ICommitmentsApiClient>(),
                    Mock.Of<ILogger<CohortController>>(),
                    linkGenerator.Object,
                    modelMapper.Object,
                    Mock.Of<IAuthorizationService>());
            }

            public CohortController CohortController { get; set; }

            public WhenGettingApprovalTestFixture WithParty(Party withParty)
            {
                _viewModel.WithParty = withParty;
                return this;
            }

            public async Task GetApproved()
            {
                _result = await CohortController.Approved(_request);
            }

            public void VerifyViewModelIsMappedFromRequest()
            {
                var viewResult = (ViewResult)_result;
                var viewModel = viewResult.Model;

                Assert.IsInstanceOf<ApprovedViewModel>(viewModel);
                var detailsViewModel = (ApprovedViewModel)viewModel;

                Assert.AreEqual(_viewModel, detailsViewModel);
            }
        }
    }
}
