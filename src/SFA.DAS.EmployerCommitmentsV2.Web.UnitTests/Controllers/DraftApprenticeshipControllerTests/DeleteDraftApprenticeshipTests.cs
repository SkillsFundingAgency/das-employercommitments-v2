using System.Threading.Tasks;
using AutoFixture.NUnit3;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using NUnit.Framework;
using SFA.DAS.Authorization.Services;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Features;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Enums;
using SFA.DAS.EmployerCommitmentsV2.Web.Exceptions;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.DraftApprenticeship;
using SFA.DAS.EmployerUrlHelper;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.DraftApprenticeshipControllerTests
{
    [TestFixture]
    public class DeleteDraftApprenticeshipTests
    {

        [Test]
        public async Task WhenGettingDelete_ThenRequestIsMapped()
        {
            var fixture = new DeleteDraftApprenticeshipTestsFixture()
                .WithEnhancedApproval()
                .WithDeleteDraftApprenticeshipRequest(Origin.CohortDetails);

            var result = await fixture.DeleteDraftApprenticeshipGet();
            
            fixture.Verify_Mapper_IsCalled_Once();
        }

        [Test]
        public async Task WhenGettingDelete_ThenViewIsReturned()
        {
            var fixture = new DeleteDraftApprenticeshipTestsFixture()
                .WithEnhancedApproval()
                .WithDeleteDraftApprenticeshipRequest(Origin.CohortDetails);

            var result = await fixture.DeleteDraftApprenticeshipGet();

            result.VerifyReturnsViewModel().WithModel<DeleteDraftApprenticeshipViewModel>();
        }

        [Test]
        public async Task WhenGettingDelete_AndCohortEmployerUpdateDeniedExceptionIsThrown_ThenGeneratesRedirectUrl()
        {
            var fixture = new DeleteDraftApprenticeshipTestsFixture()
                .WithEnhancedApproval()
                .WithDeleteDraftApprenticeshipRequest(Origin.CohortDetails)
                .WithMapperThrowingCohortEmployerUpdateDeniedException()
                .WithCohortDetailsLink("CohortDetails");

            var result = await fixture.DeleteDraftApprenticeshipGet();

            fixture.Verify_LinkGenerator_IsCalled_Once();
        }

        [Test]
        public async Task WhenGettingDelete_AndCohortEmployerUpdateDeniedExceptionIsThrown_ThenRedirectsOrigin()
        {
            var fixture = new DeleteDraftApprenticeshipTestsFixture()
                .WithEnhancedApproval()
                .WithDeleteDraftApprenticeshipRequest(Origin.CohortDetails)
                .WithMapperThrowingCohortEmployerUpdateDeniedException()
                .WithCohortDetailsLink("CohortDetails");

            var result = await fixture.DeleteDraftApprenticeshipGet();

            var redirect = result.VerifyReturnsRedirect();
            Assert.AreEqual("CohortDetails", redirect.Url);
            fixture.Verify_LinkGenerator_IsCalled_Once();
        }

        [Test, MoqAutoData]
        public async Task WhenGettingDelete_AndOriginIsCohortDetails_WhenCohortEmployerUpdateDeniedExceptionIsThrown_ThenRedirectsToCohortDetails(
            [Frozen] Mock<IModelMapper> mockMapper,
            [Frozen] Mock<ILinkGenerator> mockLinkGenerator,
            DeleteApprenticeshipRequest request,
            DraftApprenticeshipController controller)
        {
            request.Origin = Origin.CohortDetails;
            var cohortDetailsUrl = "TestUrl.com";
            var unexpectedUrl = "unexpectedUrl";
            mockLinkGenerator
                .Setup(x => x.CommitmentsV2Link($"{request.AccountHashedId}/unapproved/{request.CohortReference}"))
                .Returns(cohortDetailsUrl);
            mockLinkGenerator
                .Setup(x => x.CommitmentsV2Link($"{request.AccountHashedId}/unapproved/{request.CohortReference}/apprentices/{request.DraftApprenticeshipHashedId}/edit"))
                .Returns("unexpectedUrl");
            mockMapper
                .Setup(x => x.Map<DeleteDraftApprenticeshipViewModel>(request))
                .ThrowsAsync(new CohortEmployerUpdateDeniedException($"Cohort {request.CohortId} is not With the Employer"));

            var result = await controller.DeleteDraftApprenticeship(request) as RedirectResult;

            Assert.NotNull(result);
            Assert.AreEqual(cohortDetailsUrl, result.Url);
            Assert.False(result.Url.Equals(unexpectedUrl));
            mockLinkGenerator.Verify(x =>
                x.CommitmentsV2Link($"{request.AccountHashedId}/unapproved/{request.CohortReference}"), Times.Once);
            mockLinkGenerator.Verify(x =>
                x.CommitmentsV2Link($"{request.AccountHashedId}/unapproved/{request.CohortReference}/apprentices/{request.DraftApprenticeshipHashedId}/edit"), Times.Never);
        }

        [Test, MoqAutoData]
        public async Task WhenGettingDelete_AndOriginIsEditPage_WhenCohortEmployerUpdateDeniedExceptionIsThrown_ThenRedirectsToEditPage(
            [Frozen] Mock<IModelMapper> mockMapper,
            [Frozen] Mock<ILinkGenerator> mockLinkGenerator,
            DeleteApprenticeshipRequest request,
            DraftApprenticeshipController controller)
        {
            request.Origin = Origin.EditDraftApprenticeship;
            var EditDraftApprenticeshipUrl = "TestUrl.com";
            var unexpectedUrl = "unexpectedUrl";
            mockLinkGenerator
                .Setup(x => x.CommitmentsV2Link($"{request.AccountHashedId}/unapproved/{request.CohortReference}"))
                .Returns(unexpectedUrl);
            mockLinkGenerator
                .Setup(x => x.CommitmentsV2Link($"{request.AccountHashedId}/unapproved/{request.CohortReference}/apprentices/{request.DraftApprenticeshipHashedId}/edit"))
                .Returns(EditDraftApprenticeshipUrl);
            mockMapper
                .Setup(x => x.Map<DeleteDraftApprenticeshipViewModel>(request))
                .ThrowsAsync(new CohortEmployerUpdateDeniedException($"Cohort {request.CohortId} is not With the Employer"));

            var result = await controller.DeleteDraftApprenticeship(request) as RedirectResult;

            Assert.NotNull(result);
            Assert.AreEqual(EditDraftApprenticeshipUrl, result.Url);
            Assert.False(result.Url.Equals(unexpectedUrl));
            mockLinkGenerator.Verify(x =>
                x.CommitmentsV2Link($"{request.AccountHashedId}/unapproved/{request.CohortReference}"), Times.Never);
            mockLinkGenerator.Verify(x =>
                x.CommitmentsV2Link($"{request.AccountHashedId}/unapproved/{request.CohortReference}/apprentices/{request.DraftApprenticeshipHashedId}/edit"), Times.Once);
        }

        [Test]
        public async Task PostDeleteApprenticeshipViewModel_WithValidModel_WithConfirmDeleteTrue_ShouldDeleteDraftApprenticeshipAndRedirectToCohortDetailsV2Page()
        {
            var fixture = new DeleteDraftApprenticeshipTestsFixture()
                                .WithEnhancedApproval()
                                .WithDeleteDraftApprenticeshipViewModel(confirmDelete: true)
                                .WithCohortDetailsLink("CohortPage");

            var result = await fixture.DeleteDraftApprenticeship();

            fixture.Verify_CommitmentApiClient_DeleteApprenticeShip_IsCalled_OnlyOnce();
            var redirect = result.VerifyReturnsRedirectToActionResult();
            Assert.AreEqual("Details", redirect.ActionName);
            Assert.AreEqual("Cohort", redirect.ControllerName);
        }

        [Test]
        public async Task PostDeleteApprenticeshipViewModel_WithValidModel_WithConfirmDeleteFalse_ShouldNotDeleteDraftApprenticeshipAndRedirectToOrigin()
        {
            var fixture = new DeleteDraftApprenticeshipTestsFixture()
                               .WithEnhancedApproval()
                               .WithDeleteDraftApprenticeshipViewModel(confirmDelete: false)
                               .WithCohortDetailsLink("CohortPage");

            var result = await fixture.DeleteDraftApprenticeship();

            fixture.Verify_CommitmentApiClient_DeleteApprenticeShip_Is_NeverCalled();
            var redirect = result.VerifyReturnsRedirect();
            Assert.AreEqual("CohortPage", redirect.Url);
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
        

        public DeleteDraftApprenticeshipViewModel DeleteDraftApprenticeshipViewModel { get; set; }
        public DeleteApprenticeshipRequest DeleteDraftApprenticeshipRequest { get; set; }

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

        public DeleteDraftApprenticeshipTestsFixture WithDeleteDraftApprenticeshipRequest(Origin origin)
        {
            DeleteDraftApprenticeshipRequest = new DeleteApprenticeshipRequest
            {
                AccountHashedId = AccountHashedId,
                CohortId = CohortId,
                DraftApprenticeshipId = DraftApprenticeshipId,
                Origin = origin,
                CohortReference = CohortReference,
                DraftApprenticeshipHashedId = DraftApprenticeshipHashedId
            };
            return this;
        }

        public DeleteDraftApprenticeshipTestsFixture WithDeleteDraftApprenticeshipViewModel(bool confirmDelete)
        {
            DeleteDraftApprenticeshipViewModel = new DeleteDraftApprenticeshipViewModel { AccountHashedId = AccountHashedId, CohortId = CohortId, DraftApprenticeshipId = DraftApprenticeshipId, Origin = Enums.Origin.EditDraftApprenticeship, ConfirmDelete = confirmDelete, DraftApprenticeshipHashedId = DraftApprenticeshipHashedId, CohortReference = CohortReference };
            return this;
        }

        public DeleteDraftApprenticeshipTestsFixture WithMapperThrowingCohortEmployerUpdateDeniedException()
        {
            ModelMapperMock
                .Setup(x => x.Map<DeleteDraftApprenticeshipViewModel>(DeleteDraftApprenticeshipRequest))
                .ThrowsAsync(new CohortEmployerUpdateDeniedException($"Cohort {DeleteDraftApprenticeshipRequest.CohortId} is not With the Employer"));
            return this;
        }

        public async Task<IActionResult> DeleteDraftApprenticeship()
        {
            return await Sut.DeleteDraftApprenticeship(DeleteDraftApprenticeshipViewModel);
        }
        public async Task<IActionResult> DeleteDraftApprenticeshipGet()
        {
            return await Sut.DeleteDraftApprenticeship(DeleteDraftApprenticeshipRequest);
        }

        public void Verify_CommitmentApiClient_DeleteApprenticeShip_IsCalled_OnlyOnce()
        {
            CommitmentApiClient.Verify(cApiClient => cApiClient.DeleteDraftApprenticeship(CohortId, DraftApprenticeshipId, It.IsAny<DeleteDraftApprenticeshipRequest>(), It.IsAny<System.Threading.CancellationToken>()), Times.Once);
        }

        public void Verify_CommitmentApiClient_DeleteApprenticeShip_Is_NeverCalled()
        {
            CommitmentApiClient.Verify(cApiClient => cApiClient.DeleteDraftApprenticeship(It.IsAny<long>(), It.IsAny<long>(), It.IsAny<DeleteDraftApprenticeshipRequest>(), It.IsAny<System.Threading.CancellationToken>()), Times.Never);
        }

        public void Verify_Mapper_IsCalled_Once()
        {
            ModelMapperMock.Verify(x => x.Map<DeleteDraftApprenticeshipViewModel>(DeleteDraftApprenticeshipRequest));
        }

        public void Verify_LinkGenerator_IsCalled_Once()
        {
            LinkGeneratorMock.Verify(x => x.CommitmentsV2Link(It.IsAny<string>()));
        }
        
    }
}
