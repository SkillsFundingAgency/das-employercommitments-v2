﻿using AutoFixture;
using AutoFixture.NUnit3;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.Authorization.Services;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Types.Dtos;
using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.EmployerUrlHelper;
using SFA.DAS.Encoding;
using SFA.DAS.Testing.AutoFixture;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.CohortControllerTests
{
    public class WhenPostingAddDraftApprenticeshipOrRoute
    {
        [Test, MoqAutoData]
        public async Task WithoutEditThenReturnsRedirect(
            CreateCohortRequest request,
            [Frozen] Mock<IModelMapper> mockMapper,
            [Frozen] Mock<ICommitmentsApiClient> commitmentsApiClient)
        {            
            var viewModel = new ApprenticeViewModel();
            var autoFixture = new Fixture();

            var createCohortResponse = autoFixture.Create<CreateCohortResponse>();
            var getDraftApprenticeshipsResponse = autoFixture.Build<GetDraftApprenticeshipsResponse>()
                .With(x => x.DraftApprenticeships, new DraftApprenticeshipDto[1] { autoFixture.Create<DraftApprenticeshipDto>() })
                .Create();

            commitmentsApiClient.Setup(x => x.CreateCohort(It.IsAny<CreateCohortRequest>(), It.IsAny<CancellationToken>()))
                            .ReturnsAsync(createCohortResponse);
            commitmentsApiClient.Setup(x => x.GetDraftApprenticeships(It.IsAny<long>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(getDraftApprenticeshipsResponse);

            mockMapper.Setup(mapper => mapper.Map<CreateCohortRequest>(viewModel))
                .ReturnsAsync(request);

            var controller = new CohortController(
                commitmentsApiClient.Object,
                Mock.Of<ILogger<CohortController>>(),
                Mock.Of<ILinkGenerator>(),
                mockMapper.Object,
                Mock.Of<IAuthorizationService>(),
                Mock.Of<IEncodingService>(),
                Mock.Of<IApprovalsApiClient>());

            var result = await controller.AddDraftApprenticeshipOrRoute("", "", viewModel) as RedirectToActionResult;

            Assert.IsNotNull(result);
            Assert.AreEqual("SelectOption", result.ActionName);
            Assert.AreEqual("DraftApprenticeship", result.ControllerName);
        }

        [Test, MoqAutoData]
        public async Task WithEditCourseThenReturnsRedirect(
            ApprenticeRequest request,
            [Frozen] Mock<IModelMapper> mockMapper,
            [Greedy] CohortController controller)
        {
            var viewModel = new ApprenticeViewModel();
            controller.TempData = new Mock<ITempDataDictionary>().Object;

            mockMapper
                .Setup(mapper => mapper.Map<ApprenticeRequest>(viewModel))
                .ReturnsAsync(request);

            var result = await controller.AddDraftApprenticeshipOrRoute("Edit", "", viewModel) as RedirectToActionResult;

            Assert.IsNotNull(result);
            Assert.AreEqual("SelectCourse", result.ActionName);
        }

        [Test, MoqAutoData]
        public async Task WithEditDeliveryModelThenReturnsRedirect(
            ApprenticeRequest request,
            [Frozen] Mock<IModelMapper> mockMapper,
            [Greedy] CohortController controller)
        {
            var viewModel = new ApprenticeViewModel();
            controller.TempData = new Mock<ITempDataDictionary>().Object;

            mockMapper
                .Setup(mapper => mapper.Map<ApprenticeRequest>(viewModel))
                .ReturnsAsync(request);

            var result = await controller.AddDraftApprenticeshipOrRoute("", "Edit", viewModel) as RedirectToActionResult;

            Assert.IsNotNull(result);
            Assert.AreEqual("SelectDeliveryModel", result.ActionName);
        }
    }
}
