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
using SFA.DAS.Encoding;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.CohortControllerTests
{
    [TestFixture]
    public class WhenSendingToProvider
        {
            private WhenSendingToProviderTestFixture _fixture;

            [SetUp]
            public void Arrange()
            {
                _fixture = new WhenSendingToProviderTestFixture();
            }

            [Test]
            public async Task ThenViewModelShouldBeMappedFromRequest()
            {
                await _fixture.Sent();
                _fixture.VerifyViewModelIsMappedFromRequest();
            }

            public class WhenSendingToProviderTestFixture
            {
                private readonly SentRequest _request;
                private readonly SentViewModel _viewModel;
                private IActionResult _result;

                public WhenSendingToProviderTestFixture()
                {
                    var autoFixture = new Fixture();

                    _request = autoFixture.Create<SentRequest>();
                    _viewModel = autoFixture.Create<SentViewModel>();

                    var modelMapper = new Mock<IModelMapper>();
                    modelMapper.Setup(x => x.Map<SentViewModel>(It.Is<SentRequest>(r => r == _request)))
                        .ReturnsAsync(_viewModel);

                    CohortController = new CohortController(Mock.Of<ICommitmentsApiClient>(),
                        Mock.Of<ILogger<CohortController>>(),
                        Mock.Of<ILinkGenerator>(),
                        modelMapper.Object,
                        Mock.Of<IAuthorizationService>(),
                        Mock.Of<IEncodingService>());
                }

                public CohortController CohortController { get; set; }

                public async Task Sent()
                {
                    _result = await CohortController.Sent(_request);
                }

                public void VerifyViewModelIsMappedFromRequest()
                {
                    var viewResult = (ViewResult)_result;
                    var viewModel = viewResult.Model;

                    Assert.IsInstanceOf<SentViewModel>(viewModel);
                    var detailsViewModel = (SentViewModel)viewModel;

                    Assert.AreEqual(_viewModel, detailsViewModel);
                }
            }
        }
    }

