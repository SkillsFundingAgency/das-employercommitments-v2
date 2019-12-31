using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using NUnit.Framework;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Exceptions;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.DraftApprenticeship;
using SFA.DAS.EmployerUrlHelper;
using SFA.DAS.Http;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.DraftApprenticeshipControllerTests
{
    [TestFixture]
    public class DeleteDraftApprenticeshipTests
    {

        [Test]
        public async Task WhenGettingDelete_ThenRequestIsMapped()
        {
            var fixture = new DeleteDraftApprenticeshipTestsFixture()
                .WithDeleteDraftApprenticeshipRequest(DeleteDraftApprenticeshipOrigin.CohortDetails);

            await fixture.DeleteDraftApprenticeshipGet();
            
            fixture.Verify_Mapper_IsCalled_Once();
        }

        [Test]
        public async Task WhenGettingDelete_ThenViewIsReturned()
        {
            var fixture = new DeleteDraftApprenticeshipTestsFixture()
                .WithDeleteDraftApprenticeshipRequest(DeleteDraftApprenticeshipOrigin.CohortDetails);

            var result = await fixture.DeleteDraftApprenticeshipGet();

            result.VerifyReturnsViewModel().WithModel<DeleteDraftApprenticeshipViewModel>();
        }

        [Test]
        public async Task WhenGettingDelete_OriginIsEditDraftApprenticeship_AndCohortEmployerUpdateDeniedExceptionIsThrown_ThenGeneratesRedirectUrl()
        {
            var fixture = new DeleteDraftApprenticeshipTestsFixture()
                .WithDeleteDraftApprenticeshipRequest(DeleteDraftApprenticeshipOrigin.EditDraftApprenticeship)
                .WithMapperThrowingCohortEmployerUpdateDeniedException();

            var result = await fixture.DeleteDraftApprenticeshipGet();
            var redirect = result.VerifyReturnsRedirectToActionResult();
            Assert.AreEqual("Details", redirect.ActionName);
            Assert.AreEqual("Cohort", redirect.ControllerName);
        }

        [Test]
        public async Task WhenGettingDelete_OriginIsCohortDetails_AndCohortEmployerUpdateDeniedExceptionIsThrown_ThenRedirectsOrigin()
        {
            var fixture = new DeleteDraftApprenticeshipTestsFixture()
                .WithDeleteDraftApprenticeshipRequest(DeleteDraftApprenticeshipOrigin.CohortDetails)
                .WithMapperThrowingCohortEmployerUpdateDeniedException();

            var result = await fixture.DeleteDraftApprenticeshipGet();

            var redirect = result.VerifyReturnsRedirectToActionResult();
            Assert.AreEqual("Details", redirect.ActionName);
            Assert.AreEqual("Cohort", redirect.ControllerName);
        }

        [Test]
        public async Task WhenGettingDelete_OriginIsCohortDetails_AndDraftApprenticeshipNotFoundExceptionIsThrown_ThenRedirectsOrigin()
        {
            var fixture = new DeleteDraftApprenticeshipTestsFixture()
                .WithDeleteDraftApprenticeshipRequest(DeleteDraftApprenticeshipOrigin.CohortDetails)
                .WithMapperThrowingDraftApprenticeshipNotFoundException();

            var result = await fixture.DeleteDraftApprenticeshipGet();

            var redirect = result.VerifyReturnsRedirectToActionResult();
            Assert.AreEqual("Details", redirect.ActionName);
            Assert.AreEqual("Cohort", redirect.ControllerName);
        }

        [Test]
        public async Task PostDeleteApprenticeshipViewModel_WithValidModel_WithConfirmDeleteTrue_ShouldDeleteDraftApprenticeshipAndRedirectToCohortDetailsV2Page()
        {
            var fixture = new DeleteDraftApprenticeshipTestsFixture()
                                .WithDeleteDraftApprenticeshipViewModel(confirmDelete: true);

            var result = await fixture.DeleteDraftApprenticeship();

            fixture.Verify_CommitmentApiClient_DeleteApprenticeShip_IsCalled_OnlyOnce();
            var redirect = result.VerifyReturnsRedirectToActionResult();
            Assert.AreEqual("Details", redirect.ActionName);
            Assert.AreEqual("Cohort", redirect.ControllerName);
        }

        [Test]
        public async Task PostDeleteApprenticeshipViewModel_WithValidModel_WithConfirmDeleteTrue_ShouldDeleteDraftApprenticeshipAndTheCohortAndRedirectToBingoPage()
        {
            var fixture = new DeleteDraftApprenticeshipTestsFixture()
                .WithNoCohortFoundAfterDeletion()
                .WithDeleteDraftApprenticeshipViewModel(confirmDelete: true);

            var result = await fixture.DeleteDraftApprenticeship();

            fixture.Verify_CommitmentApiClient_DeleteApprenticeShip_IsCalled_OnlyOnce();
            var redirect = result.VerifyReturnsRedirectToActionResult();
            Assert.AreEqual("Cohorts", redirect.ActionName);
            Assert.AreEqual("Cohort", redirect.ControllerName);
        }
 
        [Test]
        public async Task PostDeleteApprenticeshipViewModel_WithValidModel_WithConfirmDeleteFalse_ShouldNotDeleteDraftApprenticeshipAndRedirectToOrigin()
        {
            var fixture = new DeleteDraftApprenticeshipTestsFixture()
                               .WithDeleteDraftApprenticeshipViewModel(confirmDelete: false);
                             
            var result = await fixture.DeleteDraftApprenticeship();

            fixture.Verify_CommitmentApiClient_DeleteApprenticeShip_Is_NeverCalled();
            var redirect = result.VerifyReturnsRedirectToActionResult();
            Assert.AreEqual("Details", redirect.ActionName);
            Assert.AreEqual(null, redirect.ControllerName);
        }
    }

    public class DeleteDraftApprenticeshipTestsFixture
    {
        public Mock<ICommitmentsApiClient> CommitmentApiClient { get; }
        public string AccountHashedId => "ACHID";
        public long CohortId => 1;
        public string CohortReference => "CHREF";
        public long DraftApprenticeshipId => 99;
        public string DraftApprenticeshipHashedId => "DAHID";

        public DeleteDraftApprenticeshipViewModel DeleteDraftApprenticeshipViewModel { get; set; }
        public DeleteApprenticeshipRequest DeleteDraftApprenticeshipRequest { get; set; }
        public Mock<IModelMapper> ModelMapperMock { get; }
        public DraftApprenticeshipController Sut { get; }

        public DeleteDraftApprenticeshipTestsFixture()
        {
            var fixture = new AutoFixture.Fixture();
            CommitmentApiClient = new Mock<ICommitmentsApiClient>();
            CommitmentApiClient.Setup(x => x.GetCohort(It.IsAny<long>(), It.IsAny<CancellationToken>())).ReturnsAsync(new GetCohortResponse());

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

            Sut = new DraftApprenticeshipController(Mock.Of<ICommitmentsService>(),
                ModelMapperMock.Object,
                CommitmentApiClient.Object);
            Sut.TempData = new Mock<ITempDataDictionary>().Object;
        }

        public DeleteDraftApprenticeshipTestsFixture WithDeleteDraftApprenticeshipRequest(DeleteDraftApprenticeshipOrigin origin)
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
            DeleteDraftApprenticeshipViewModel = new DeleteDraftApprenticeshipViewModel { AccountHashedId = AccountHashedId, CohortId = CohortId, DraftApprenticeshipId = DraftApprenticeshipId, Origin = DeleteDraftApprenticeshipOrigin.EditDraftApprenticeship, ConfirmDelete = confirmDelete, DraftApprenticeshipHashedId = DraftApprenticeshipHashedId, CohortReference = CohortReference };
            return this;
        }

        public DeleteDraftApprenticeshipTestsFixture WithNoCohortFoundAfterDeletion()
        {
            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.NotFound);
            httpResponseMessage.RequestMessage = new HttpRequestMessage();
            var restException = new RestHttpClientException(httpResponseMessage, "");
            CommitmentApiClient.Setup(x => x.GetCohort(CohortId, It.IsAny<CancellationToken>())).Throws(restException);

            return this;
        }

        public DeleteDraftApprenticeshipTestsFixture WithMapperThrowingCohortEmployerUpdateDeniedException()
        {
            ModelMapperMock
                .Setup(x => x.Map<DeleteDraftApprenticeshipViewModel>(DeleteDraftApprenticeshipRequest))
                .ThrowsAsync(new CohortEmployerUpdateDeniedException($"Cohort {DeleteDraftApprenticeshipRequest.CohortId} is not With the Employer"));
            return this;
        }

        public DeleteDraftApprenticeshipTestsFixture WithMapperThrowingDraftApprenticeshipNotFoundException()
        {
            ModelMapperMock
                .Setup(x => x.Map<DeleteDraftApprenticeshipViewModel>(DeleteDraftApprenticeshipRequest))
                .ThrowsAsync(new DraftApprenticeshipNotFoundException());
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
    }
}
