using System.Threading.Tasks;
using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.EmployerUrlHelper;
using SFA.DAS.Encoding;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.CohortControllerTests;

public class WhenGettingConfirmDeleteDetails
{
    [Test]
    public async Task ThenViewModelShouldBeMappedFromRequest()
    {
        var f = new WhenGettingConfirmDeleteDetailsTestFixture();
        var result = await f.CohortController.ConfirmDelete(f.DetailsRequest);
        f.VerifyViewModelIsCorrectlyMappedFromRequest(result);
    }

    public class WhenGettingConfirmDeleteDetailsTestFixture
    {
        public DetailsRequest DetailsRequest;
        public ConfirmDeleteViewModel ConfirmDeleteViewModel;
        public CohortController CohortController { get; set; }

        public WhenGettingConfirmDeleteDetailsTestFixture()
        {
            var autoFixture = new Fixture();

            DetailsRequest = autoFixture.Create<DetailsRequest>();
            ConfirmDeleteViewModel = autoFixture.Create<ConfirmDeleteViewModel>();

            var modelMapper = new Mock<IModelMapper>();
            modelMapper.Setup(x => x.Map<ConfirmDeleteViewModel>(It.Is<DetailsRequest>(r => r == DetailsRequest)))
                .ReturnsAsync(ConfirmDeleteViewModel);

            CohortController = new CohortController(Mock.Of<ICommitmentsApiClient>(),
                Mock.Of<ILogger<CohortController>>(),
                Mock.Of<ILinkGenerator>(),
                modelMapper.Object,
                Mock.Of<IEncodingService>(),
                Mock.Of<IApprovalsApiClient>());
        }

        public void VerifyViewModelIsCorrectlyMappedFromRequest(IActionResult result)
        {
            var viewResult = (ViewResult)result;
            var viewModel = viewResult.Model;

            Assert.That(viewModel, Is.InstanceOf<ConfirmDeleteViewModel>());
            var detailsViewModel = (ConfirmDeleteViewModel)viewModel;

            Assert.That(detailsViewModel, Is.EqualTo(ConfirmDeleteViewModel));
        }
    }
}