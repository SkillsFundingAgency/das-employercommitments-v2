using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.EmployerUrlHelper;
using SFA.DAS.Encoding;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.CohortControllerTests;

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

        private readonly DetailsViewModel _viewModel;
        private readonly long _cohortId;
        private readonly string _accountHashedId;
        private readonly string _accountLegalEntityHashedId;
        private readonly string _linkGeneratorResult;
        private readonly AcknowledgementRequest _acknowledgementRequest;
        private readonly ViewEmployerAgreementRequest _viewEmployerAgreementRequest;
        private readonly Mock<IModelMapper> _modelMapper;

        public WhenPostingDetailsFixture()
        {
            var autoFixture = new Fixture();
                
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

            _acknowledgementRequest = new AcknowledgementRequest();
            _viewEmployerAgreementRequest = new ViewEmployerAgreementRequest
            {
                AccountHashedId = autoFixture.Create<string>(),
                AgreementHashedId = autoFixture.Create<string>()
            };

            _modelMapper = new Mock<IModelMapper>();

            _modelMapper.Setup(x => x.Map<AcknowledgementRequest>(It.Is<DetailsViewModel>(vm => vm == _viewModel)))
                .ReturnsAsync(_acknowledgementRequest);

            _modelMapper.Setup(x =>
                    x.Map<ViewEmployerAgreementRequest>(It.Is<DetailsViewModel>(vm => vm == _viewModel)))
                .ReturnsAsync(_viewEmployerAgreementRequest);

            _linkGeneratorResult = autoFixture.Create<string>();

            linkGenerator.Setup(x => x.AccountsLink(It.IsAny<string>()))
                .Returns(_linkGeneratorResult);

            _controller = new CohortController(Mock.Of<ICommitmentsApiClient>(),
                Mock.Of<ILogger<CohortController>>(),
                linkGenerator.Object,
                _modelMapper.Object,
                Mock.Of<IEncodingService>(),
                Mock.Of<IApprovalsApiClient>());
        }

        public async Task Post(CohortDetailsOptions option)
        {
            _viewModel.Selection = option;
            _result = await _controller.Details(_viewModel);
        }

        public void VerifyCohortSentToProvider()
        {
            _modelMapper.Verify(x => x.Map<AcknowledgementRequest>(It.Is<DetailsViewModel>(vm => vm.Selection == CohortDetailsOptions.Send)),
                Times.Once);
        }

        public void VerifyRedirectedToSendConfirmation()
        {
            Assert.That(_result, Is.InstanceOf<RedirectToActionResult>());
            var redirect = (RedirectToActionResult) _result;
            Assert.That(redirect.ActionName, Is.EqualTo("Sent"));
        }

        public void VerifyCohortApprovedByEmployer()
        {
            _modelMapper.Verify(x => x.Map<AcknowledgementRequest>(It.Is<DetailsViewModel>(vm => vm.Selection == CohortDetailsOptions.Approve)),
                Times.Once);
        }

        public void VerifyRedirectedToApprovalConfirmation()
        {
            Assert.That(_result, Is.InstanceOf<RedirectToActionResult>());
            var redirect = (RedirectToActionResult)_result;
            Assert.That(redirect.ActionName, Is.EqualTo("Approved"));
        }

        public void VerifyRedirectedToViewEmployerAgreement()
        {
            Assert.That(_result, Is.InstanceOf<RedirectResult>());
            var redirect = (RedirectResult) _result;
            Assert.That(redirect.Url, Is.EqualTo(_linkGeneratorResult));
        }

        public void VerifyRedirectedToHomepage()
        {
            Assert.That(_result, Is.InstanceOf<RedirectResult>());
            var redirect = (RedirectResult)_result;
            Assert.That(redirect.Url, Is.EqualTo(_linkGeneratorResult));
        }
    }
}