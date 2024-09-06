using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Contracts;
using SFA.DAS.EmployerCommitmentsV2.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.EmployerUrlHelper;
using SFA.DAS.Encoding;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.CohortControllerTests;

public class WhenGettingDetails
{
    private WhenGettingDetailsTestFixture _fixture;

    [SetUp]
    public void Arrange()
    {
        _fixture = new WhenGettingDetailsTestFixture();
    }

    [Test]
    public async Task ThenViewModelShouldBeMappedFromRequest()
    {
        await _fixture.GetDetails();
        _fixture.VerifyViewModelIsMappedFromRequest();
    }

    [Test]
    public async Task ThenViewModelShouldBeStoredInTempData()
    {
        await _fixture.GetDetails();
        _fixture.VerifyViewEmployerAgreementModelIsStoredInTempData();
    }

    [TestCase(Party.Provider)]
    [TestCase(Party.TransferSender)]
    public async Task ThenViewModelIsReadOnlyIfCohortIsNotWithEmployer(Party withParty)
    {
        _fixture.WithParty(withParty);
        await _fixture.GetDetails();
        Assert.That(_fixture.IsViewModelReadOnly(), Is.True);
    }

    public class WhenGettingDetailsTestFixture
    {
        private readonly DetailsRequest _request;
        private readonly DetailsViewModel _viewModel;
        private IActionResult _result;

        public WhenGettingDetailsTestFixture()
        {
            var autoFixture = new Fixture();

            _request = autoFixture.Create<DetailsRequest>();
            _viewModel = autoFixture.Create<DetailsViewModel>();
            _viewModel.WithParty = Party.Employer;

            var modelMapper = new Mock<IModelMapper>();
            modelMapper.Setup(x => x.Map<DetailsViewModel>(It.Is<DetailsRequest>(r => r == _request)))
                .ReturnsAsync(_viewModel);

            CohortController = new CohortController(Mock.Of<ICommitmentsApiClient>(),
                Mock.Of<ILogger<CohortController>>(),
                Mock.Of<ILinkGenerator>(),
                modelMapper.Object,
                Mock.Of<IEncodingService>(),
                Mock.Of<IApprovalsApiClient>(),
                Mock.Of<ICacheStorageService>());

            CohortController.TempData = new TempDataDictionary(new Mock<HttpContext>().Object, new Mock<ITempDataProvider>().Object);

        }

        public CohortController CohortController { get; set; }

        public WhenGettingDetailsTestFixture WithParty(Party withParty)
        {
            _viewModel.WithParty = withParty;
            return this;
        }

        public async Task GetDetails()
        {
            _result = await CohortController.Details(_request);
        }

        public void VerifyViewModelIsMappedFromRequest()
        {
            var viewResult = (ViewResult)_result;
            var viewModel = viewResult.Model;

            Assert.That(viewModel, Is.InstanceOf<DetailsViewModel>());
            var detailsViewModel = (DetailsViewModel) viewModel;

            Assert.That(detailsViewModel, Is.EqualTo(_viewModel));

            var expectedTotalCost = _viewModel.Courses?.Sum(g => g.DraftApprenticeships.Sum(a => a.Cost ?? 0)) ?? 0;
            Assert.That(_viewModel.TotalCost, Is.EqualTo(expectedTotalCost), "The total cost stored in the model is incorrect");
        }

        public void VerifyViewEmployerAgreementModelIsStoredInTempData()
        {
            Assert.That(CohortController.TempData.ContainsKey(nameof(ViewEmployerAgreementModel)), Is.True);
        }

        public bool IsViewModelReadOnly()
        {
            return _viewModel.IsReadOnly;
        }
    }
}