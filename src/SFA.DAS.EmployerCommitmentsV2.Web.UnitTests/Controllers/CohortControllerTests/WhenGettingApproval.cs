using System.Threading.Tasks;
using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
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

            public WhenGettingApprovalTestFixture()
            {
                var autoFixture = new Fixture();

                _request = autoFixture.Create<ApprovedRequest>();
                _viewModel = autoFixture.Create<ApprovedViewModel>();
                _viewModel.WithParty = Party.Provider;

                var modelMapper = new Mock<IModelMapper>();
                modelMapper.Setup(x => x.Map<ApprovedViewModel>(It.Is<ApprovedRequest>(r => r == _request)))
                    .ReturnsAsync(_viewModel);

                CohortController = new CohortController(Mock.Of<ICommitmentsApiClient>(),
                    Mock.Of<ILogger<CohortController>>(),
                    Mock.Of<ILinkGenerator>(),
                    modelMapper.Object,
                    Mock.Of<IEncodingService>(),
                    Mock.Of<IApprovalsApiClient>());
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

                Assert.That(viewModel, Is.InstanceOf<ApprovedViewModel>());
                var detailsViewModel = (ApprovedViewModel)viewModel;

                Assert.That(detailsViewModel, Is.EqualTo(_viewModel));
            }
        }
    }
}
