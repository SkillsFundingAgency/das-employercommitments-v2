using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Contracts;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.EmployerUrlHelper;
using SFA.DAS.Encoding;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.CohortControllerTests;

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
                Mock.Of<IEncodingService>(),
                Mock.Of<IApprovalsApiClient>());
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

            Assert.That(viewModel, Is.InstanceOf<SentViewModel>());
            var detailsViewModel = (SentViewModel)viewModel;

            Assert.That(detailsViewModel, Is.EqualTo(_viewModel));
        }
    }
}