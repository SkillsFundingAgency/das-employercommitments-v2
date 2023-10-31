using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.Authorization.Services;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.EmployerUrlHelper;
using SFA.DAS.Encoding;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.CohortControllerTests
{
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

        [TestCase(Party.Provider)]
        [TestCase(Party.TransferSender)]
        public async Task ThenViewModelIsReadOnlyIfCohortIsNotWithEmployer(Party withParty)
        {
            _fixture.WithParty(withParty);
            await _fixture.GetDetails();
            Assert.IsTrue(_fixture.IsViewModelReadOnly());
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
                    Mock.Of<IAuthorizationService>(),
                    Mock.Of<IEncodingService>(),
                    Mock.Of<IApprovalsApiClient>());
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

                Assert.IsInstanceOf<DetailsViewModel>(viewModel);
                var detailsViewModel = (DetailsViewModel) viewModel;

                Assert.AreEqual(_viewModel, detailsViewModel);

                var expectedTotalCost = _viewModel.Courses?.Sum(g => g.DraftApprenticeships.Sum(a => a.Cost ?? 0)) ?? 0;
                Assert.AreEqual(expectedTotalCost, _viewModel.TotalCost, "The total cost stored in the model is incorrect");
            }

            public bool IsViewModelReadOnly()
            {
                return _viewModel.IsReadOnly;
            }

        }
    }
}
