using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.Authorization.Services;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
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

        [Test]
        public async Task And_User_Selected_Approve_Then_Cohort_Is_Approved_By_Employer()
        {
            await _fixture.Post(CohortDetailsOptions.Approve);
            _fixture.VerifyCohortApprovedByEmployer();
        }

        [Test]
        public async Task And_User_Selected_Approve_Then_User_Is_Redirected_To_Confirmation_Page()
        {
            await _fixture.Post(CohortDetailsOptions.Approve);
            _fixture.VerifyRedirectedToApprovalConfirmation();
        }

        [Test]
        public async Task And_User_Selected_View_Employer_Agreement_Then_User_Is_Redirected_To_Agreements_Page()
        {
            await _fixture.Post(CohortDetailsOptions.ViewEmployerAgreement);
            _fixture.VerifyRedirectedToViewEmployerAgreement();
        }

        [Test]
        public async Task And_User_Selected_Homepage_Then_User_Is_Redirected_To_Homepage()
        {
            await _fixture.Post(CohortDetailsOptions.Homepage);
            _fixture.VerifyRedirectedToHomepage();
        }

        public class WhenPostingDetailsFixture
        {
            private readonly CohortController _controller;
            private IActionResult _result;
            private readonly Mock<ICommitmentsApiClient> _commitmentsApiClient;

            private readonly DetailsViewModel _viewModel;
            private readonly long _cohortId;
            private readonly string _accountHashedId;
            private readonly string _accountLegalEntityHashedId;
            private readonly string _linkGeneratorResult;
            private readonly SendCohortRequest _sendCohortApiRequest;
            private readonly ApproveCohortRequest _approveCohortApiRequest;
            private readonly ViewEmployerAgreementRequest _viewEmployerAgreementRequest;


            public WhenPostingDetailsFixture()
            {
                var autoFixture = new Fixture();

                var modelMapper = new Mock<IModelMapper>();
                _commitmentsApiClient = new Mock<ICommitmentsApiClient>();
                var linkGenerator = new Mock<ILinkGenerator>();

                _cohortId = autoFixture.Create<long>();
                _accountHashedId = autoFixture.Create<string>();
                _accountLegalEntityHashedId = autoFixture.Create<string>();

                _viewModel = new DetailsViewModel
                {
                    CohortId = _cohortId,
                    AccountHashedId = _accountHashedId,
                    AccountLegalEntityHashedId = _accountLegalEntityHashedId
                };

                _sendCohortApiRequest = new SendCohortRequest();
                _approveCohortApiRequest = new ApproveCohortRequest();
                _viewEmployerAgreementRequest = new ViewEmployerAgreementRequest
                {
                    AccountHashedId = autoFixture.Create<string>(),
                    AccountLegalEntityHashedId = autoFixture.Create<string>()
                };

                modelMapper.Setup(x => x.Map<SendCohortRequest>(It.Is<DetailsViewModel>(vm => vm == _viewModel)))
                    .ReturnsAsync(_sendCohortApiRequest);

                modelMapper.Setup(x => x.Map<ApproveCohortRequest>(It.Is<DetailsViewModel>(vm => vm == _viewModel)))
                    .ReturnsAsync(_approveCohortApiRequest);

                modelMapper.Setup(x =>
                        x.Map<ViewEmployerAgreementRequest>(It.Is<DetailsViewModel>(vm => vm == _viewModel)))
                    .ReturnsAsync(_viewEmployerAgreementRequest);

                _commitmentsApiClient.Setup(x => x.SendCohort(It.Is<long>(c => c == _cohortId),
                        It.Is<SendCohortRequest>(r => r == _sendCohortApiRequest), It.IsAny<CancellationToken>()))
                    .Returns(Task.CompletedTask);

                _commitmentsApiClient.Setup(x => x.ApproveCohort(It.Is<long>(c => c == _cohortId),
                        It.Is<ApproveCohortRequest>(r => r == _approveCohortApiRequest), It.IsAny<CancellationToken>()))
                    .Returns(Task.CompletedTask);

                _linkGeneratorResult = autoFixture.Create<string>();

                linkGenerator.Setup(x => x.AccountsLink(It.IsAny<string>()))
                    .Returns(_linkGeneratorResult);

                linkGenerator.Setup(x => x.CommitmentsLink(It.IsAny<string>()))
                    .Returns(_linkGeneratorResult);

                _controller = new CohortController(_commitmentsApiClient.Object,
                    Mock.Of<ILogger<CohortController>>(),
                    linkGenerator.Object,
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
                        It.Is<SendCohortRequest>(r => r == _sendCohortApiRequest),
                        It.IsAny<CancellationToken>()),
                    Times.Once);
            }

            public void VerifyRedirectedToSendConfirmation()
            {
                Assert.IsInstanceOf<RedirectToActionResult>(_result);
                var redirect = (RedirectToActionResult) _result;
                Assert.AreEqual("Sent", redirect.ActionName);
            }

            public void VerifyCohortApprovedByEmployer()
            {
                _commitmentsApiClient.Verify(x => x.ApproveCohort(It.Is<long>(c => c == _cohortId),
                        It.Is<ApproveCohortRequest>(r => r == _approveCohortApiRequest),
                        It.IsAny<CancellationToken>()),
                    Times.Once);
            }

            public void VerifyRedirectedToApprovalConfirmation()
            {
                Assert.IsInstanceOf<RedirectToActionResult>(_result);
                var redirect = (RedirectToActionResult)_result;
                Assert.AreEqual("Approved", redirect.ActionName);
            }

            public void VerifyRedirectedToViewEmployerAgreement()
            {
                Assert.IsInstanceOf<RedirectResult>(_result);
                var redirect = (RedirectResult) _result;
                Assert.AreEqual(_linkGeneratorResult, redirect.Url);
            }

            public void VerifyRedirectedToHomepage()
            {
                Assert.IsInstanceOf<RedirectResult>(_result);
                var redirect = (RedirectResult)_result;
                Assert.AreEqual(_linkGeneratorResult, redirect.Url);
            }
        }
    }
}
