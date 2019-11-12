﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.NUnit3;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using NUnit.Framework;
using SFA.DAS.Authorization.Services;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.CommitmentsV2.Api.Types.Validation;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.CommitmentsV2.Shared.Models;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Features;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Exceptions;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.DraftApprenticeship;
using SFA.DAS.EmployerUrlHelper;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.DraftApprenticeshipControllerTests
{
    [TestFixture]
    public class DeleteDraftApprenticeshipTests
    {

        [Test, MoqAutoData]
        public async Task WhenGettingDelete_ThenCallsMapper(
            [Frozen] Mock<IModelMapper> mockMapper,
            DeleteApprenticeshipRequest request,
            DraftApprenticeshipController controller)
        {
            mockMapper
                .Setup(x => x.Map<DeleteDraftApprenticeshipViewModel>(request))
                .ReturnsAsync(new DeleteDraftApprenticeshipViewModel());

            await controller.DeleteDraftApprenticeship(request);

            mockMapper.Verify(x => x.Map<DeleteDraftApprenticeshipViewModel>(request));
        }

        [Test, MoqAutoData]
        public async Task WhenGettingDelete_AndMapperThrowsError_ThenRedirectsToOrigin(
            [Frozen] Mock<IModelMapper> mockMapper,
            DeleteApprenticeshipRequest request,
            DraftApprenticeshipController controller)
        {
            mockMapper
                .Setup(x => x.Map<DeleteDraftApprenticeshipViewModel>(request))
                .ThrowsAsync(new CohortEmployerUpdateDeniedException($"Cohort {request.CohortId} is not With the Employer"));

            await controller.DeleteDraftApprenticeship(request);

            mockMapper.Verify(x => x.Map<DeleteDraftApprenticeshipViewModel>(request));
        }

        [Test]
        public async Task PostDeleteApprenticeshpViewModel_WithValidModel_WithEnhancedApproval_ShouldDeleteDraftApprenticeshipAndRedirectToCohortDetailsV2Page()
        {
            var fixture = new DeleteDraftApprenticeshipTestsFixture().WithEnhancedApproval().WithCohortDetailsLink("cohortPage");

            var result =  await fixture.Sut.DeleteDraftApprenticeship(new DeleteDraftApprenticeshipViewModel { AccountHashedId = fixture.AccountHashedId, CohortId = fixture.CohortId, DraftApprenticeshipId = fixture.DraftApprenticeshipId, Origin = Enums.Origin.EditDraftApprenticeship, ConfirmDelete = true, DraftApprenticeshipHashedId = fixture.DraftApprenticeshipHashedId, CohortReference = fixture.CohortReference });

            fixture.CommitmentApiClient.Verify(cApiClient => cApiClient.DeleteDraftApprenticeship(fixture.CohortId, fixture.DraftApprenticeshipId, It.IsAny<DeleteDraftApprenticeshipRequest>(), It.IsAny<System.Threading.CancellationToken>()), Times.Once);
            var redirect = result.VerifyReturnsRedirect();
            Assert.AreEqual("cohortPage", redirect.Url);
        }
    }

    public class DeleteDraftApprenticeshipTestsFixture
    {
        public Mock<ICommitmentsApiClient> CommitmentApiClient { get; }
        public Mock<ILinkGenerator> LinkGeneratorMock { get; }
        public string AccountHashedId => "ACHID";
        public long CohortId => 1;
        public string CohortReference => "CHREF";
        public long DraftApprenticeshipId => 99;
        public string DraftApprenticeshipHashedId => "DAHID";
        //public List<ErrorDetail> ApiErrors { get; }
        public Mock<IModelMapper> ModelMapperMock { get; }
        public Mock<IAuthorizationService> AuthorizationServiceMock { get; }
        public DraftApprenticeshipController Sut { get; }

        public DeleteDraftApprenticeshipTestsFixture()
        {
            var fixture = new AutoFixture.Fixture();
            CommitmentApiClient = new Mock<ICommitmentsApiClient>();
            LinkGeneratorMock = new Mock<ILinkGenerator>();

            var deleteDraftApprenticeshipViewModel = new DeleteDraftApprenticeshipViewModel
            {
                FirstName = "John",
                LastName = "Jack",
                CohortId = CohortId,
                DraftApprenticeshipHashedId = DraftApprenticeshipHashedId,
                AccountHashedId = AccountHashedId,
            };

            ModelMapperMock = new Mock<IModelMapper>();
            ModelMapperMock.Setup(x => x.Map<DeleteDraftApprenticeshipViewModel>(It.IsAny<DeleteApprenticeshipRequest>()))
                .ReturnsAsync(deleteDraftApprenticeshipViewModel);

            AuthorizationServiceMock = new Mock<IAuthorizationService>();

            Sut = new DraftApprenticeshipController(Mock.Of<ICommitmentsService>(),
                LinkGeneratorMock.Object,
                ModelMapperMock.Object,
                CommitmentApiClient.Object,
                AuthorizationServiceMock.Object);
            Sut.TempData = new Mock<ITempDataDictionary>().Object;
        }

        public DeleteDraftApprenticeshipTestsFixture WithEnhancedApproval()
        {
            AuthorizationServiceMock.Setup(x => x.IsAuthorized(EmployerFeature.EnhancedApproval)).Returns(true);
            return this;
        }

        public DeleteDraftApprenticeshipTestsFixture WithCohortDetailsLink(string url)
        {
            LinkGeneratorMock
                .Setup(lg => lg.CommitmentsV2Link(It.IsAny<string>()))
                .Returns(url);

            return this;
        }
    }

}
