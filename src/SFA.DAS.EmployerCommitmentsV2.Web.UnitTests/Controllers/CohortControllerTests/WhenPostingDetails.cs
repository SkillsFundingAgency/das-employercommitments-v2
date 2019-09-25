using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.Authorization.Services;
using SFA.DAS.Commitments.Shared.Interfaces;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.EmployerUrlHelper;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.CohortControllerTests
{
    [TestFixture]
    public class WhenPostingDetails
    {
        private WhenPostingDetailsFixture _fixture;

        [SetUp]
        public void Arrange()
        {
            _fixture = new WhenPostingDetailsFixture();
        }

        [Test]
        public async Task And_User_Selected_Send_Then_Cohort_Is_Sent_To_Provider()
        {
            await _fixture.Post(CohortDetailsOptions.Send);
            _fixture.VerifyCohortSentToProvider();
        }

        [Test]
        public async Task And_User_Selected_Send_Then_User_Is_Redirected_To_Confirmation_Page()
        {
            await _fixture.Post(CohortDetailsOptions.Send);
            _fixture.VerifyRedirectedToSendConfirmation();
        }

        public class WhenPostingDetailsFixture
        {
            private readonly CohortController _controller;
            private IActionResult _result;
            private readonly Mock<ICommitmentsApiClient> _commitmentsApiClient;

            private readonly DetailsViewModel _viewModel;
            private readonly long _cohortId;
            private readonly SendCohortRequest _apiRequest;

            public WhenPostingDetailsFixture()
            {
                var autoFixture = new Fixture();

                var modelMapper = new Mock<IModelMapper>();
                _commitmentsApiClient = new Mock<ICommitmentsApiClient>();

                _cohortId = autoFixture.Create<long>();

                _viewModel = new DetailsViewModel
                {
                    CohortId = _cohortId
                };

                _apiRequest = new SendCohortRequest();

                modelMapper.Setup(x => x.Map<SendCohortRequest>(It.Is<DetailsViewModel>(vm => vm == _viewModel)))
                    .ReturnsAsync(_apiRequest);

                _commitmentsApiClient.Setup(x => x.SendCohort(It.Is<long>(c => c == _cohortId),
                        It.Is<SendCohortRequest>(r => r == _apiRequest), It.IsAny<CancellationToken>()))
                    .Returns(Task.CompletedTask);

                _controller = new CohortController(_commitmentsApiClient.Object,
                    Mock.Of<ILogger<CohortController>>(),
                    Mock.Of<ILinkGenerator>(),
                    modelMapper.Object,
                    Mock.Of<IAuthorizationService>());
            }

            public async Task Post(CohortDetailsOptions option)
            {
                _viewModel.Selection = option;
                _result = await _controller.Details(_viewModel);
            }

            public void VerifyCohortSentToProvider()
            {
                _commitmentsApiClient.Verify(x => x.SendCohort(It.Is<long>(c => c == _cohortId),
                        It.Is<SendCohortRequest>(r => r == _apiRequest),
                        It.IsAny<CancellationToken>()),
                    Times.Once);
            }

            public void VerifyRedirectedToSendConfirmation()
            {
                Assert.IsInstanceOf<RedirectToActionResult>(_result);
                var redirect = (RedirectToActionResult) _result;
                Assert.AreEqual("Sent", redirect.ActionName);
            }
        }
    }
}
