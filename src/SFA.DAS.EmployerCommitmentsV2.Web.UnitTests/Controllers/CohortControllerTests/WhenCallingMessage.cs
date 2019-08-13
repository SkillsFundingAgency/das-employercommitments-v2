using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.Commitments.Shared.Interfaces;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.CommitmentsV2.Api.Types.Validation;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.CreateCohort;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers
{
    [TestFixture]
    [Parallelizable]
    public class CreateCohortWithOtherPartyControllerTests
    {
        private CreateCohortWithOtherPartyControllerTestFixture _fixture;

        [SetUp]
        public void Arrange()
        {
            _fixture = new CreateCohortWithOtherPartyControllerTestFixture();
        }

        [Test, AutoData]
        public async Task GetMessage_ValidModel_ShouldCallGetProvider(MessageRequest request)
        {
            _fixture.WithProviderName("ProviderName");

            await _fixture.Sut.Message(request);

            _fixture.CommitmentsApiClientMock.Verify(x => x.GetProvider(request.ProviderId, It.IsAny<CancellationToken>()));
        }

        [Test, AutoData]
        public async Task GetMessage_ValidModel_ShouldReturnMessageViewModelWithMappedValues(MessageRequest request)
        {
            _fixture.WithProviderName("ProviderName");

            var result = await _fixture.Sut.Message(request);

            var model = result.VerifyReturnsViewModel().WithModel<MessageViewModel>();
            _fixture.VerifyViewModelIsMappedCorrectly(request, model);
        }

        [Test, AutoData]
        public async Task PostMessage_WithInvalidRequest_ShouldReturnViewModelAndNotCallCreateCohort(MessageViewModel model)
        {
            _fixture.WithProviderName("ProviderName").WithInvalidModel();

            var result = await _fixture.Sut.Message(model);

            result.VerifyReturnsViewModel().WithModel<MessageViewModel>();
            _fixture.CommitmentsApiClientMock.Verify(x=>x.CreateCohort(It.IsAny<CreateCohortWithOtherPartyRequest>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Test, AutoData]
        public async Task PostMessage_WithValidRequest_ShouldAddCohortAndReturnRedirectResult(MessageViewModel model, CreateCohortResponse createCohortResponse)
        {
            _fixture.WithProviderName("ProviderName").SetCreateCohortResponse(createCohortResponse);

            var result = await _fixture.Sut.Message(model);

            _fixture.VerifyAddCohortIsCalledWithCorrectMappedValues(model);


            var redirect = result.VerifyReturnsRedirectToActionResult();
            Assert.AreEqual(redirect.ActionName, "Finished");
            Assert.AreEqual(redirect.RouteValues["AccountHashedId"], model.AccountHashedId);
            Assert.AreEqual(redirect.RouteValues["CohortReference"], createCohortResponse.CohortReference);
        }

        [Test, AutoData]
        public async Task PostMessage_WithValidRequestButApiErrorOccurs_ShouldSetModelState(MessageViewModel model)
        {
            _fixture.WithProviderName("ProviderName").WithCreateCohortApiError();

            await _fixture.Sut.Message(model);

            Assert.AreEqual(1, _fixture.Sut.ModelState.Count);
        }

        [Test, AutoData]
        public async Task PostMessage_WithValidRequestButApiErrorOccurs_ShouldReturnViewModel(MessageViewModel model)
        {
            _fixture.WithProviderName("ProviderName").WithCreateCohortApiError();

            var result = await _fixture.Sut.Message(model);
            result.VerifyReturnsViewModel().WithModel<MessageViewModel>();
        }

        [Test, AutoData]
        public async Task GetFinished_ValidModel_ShouldReturnFinishedViewModelWithMappedValues(FinishedRequest request, GetCohortResponse getCohortResponse)
        {
            _fixture.SetGetCohortResponse(getCohortResponse);

            var response = await _fixture.Sut.Finished(request);
            var model = response.VerifyReturnsViewModel().WithModel<FinishedViewModel>();
            
            Assert.AreEqual(model.CohortReference, request.CohortReference);
            Assert.AreEqual(model.LegalEntityName, getCohortResponse.LegalEntityName);
            Assert.AreEqual(model.ProviderName, getCohortResponse.ProviderName);
            Assert.AreEqual(model.Message, getCohortResponse.LatestMessageCreatedByEmployer);
        }

        [Test]
        public async Task GetFinished_InvalidModel_ShouldReturnBadRequestResponse()
        {
            _fixture.WithInvalidModel();
            
            var response = await _fixture.Sut.Finished(null);
            
            Assert.IsTrue(response is BadRequestObjectResult);
        }
    }

    public class CreateCohortWithOtherPartyControllerTestFixture
    {
        public CreateCohortWithOtherPartyControllerTestFixture()
        {
            RequestMapper = new MessageViewModelToCreateCohortWithOtherPartyRequestMapper();
            CommitmentsApiClientMock = new Mock<ICommitmentsApiClient>();
            ErrorDetail = new ErrorDetail("field1", "error message");

            Sut = new CohortController(
                Mock.Of<IMapper<IndexRequest, IndexViewModel>>(),
                Mock.Of<IMapper<SelectProviderRequest, SelectProviderViewModel>>(),
                Mock.Of<IMapper<SelectProviderViewModel, ConfirmProviderRequest>>(),
                Mock.Of<IMapper<ConfirmProviderRequest, ConfirmProviderViewModel>>(),
                Mock.Of<IMapper<ConfirmProviderViewModel, SelectProviderViewModel>>(),
                Mock.Of<IMapper<ConfirmProviderViewModel, AssignRequest>>(),
                Mock.Of<IMapper<AssignRequest, AssignViewModel>>(),
                CommitmentsApiClientMock.Object, Mock.Of<ILogger<CohortController>>(),
                RequestMapper);
        }

        public Mock<ICommitmentsApiClient> CommitmentsApiClientMock { get; }
        public IMapper<MessageViewModel, CreateCohortWithOtherPartyRequest> RequestMapper { get; }
        public CohortController Sut { get; }
        public ErrorDetail ErrorDetail { get; }

        public CreateCohortWithOtherPartyControllerTestFixture WithProviderName(string name)
        {
            CommitmentsApiClientMock.Setup(x => x.GetProvider(It.IsAny<long>(), It.IsAny<CancellationToken>())).ReturnsAsync(new GetProviderResponse { Name = name});
            return this;
        }

        public CreateCohortWithOtherPartyControllerTestFixture WithInvalidModel()
        {
            Sut.ModelState.AddModelError("AKey", "Some Error");
            return this;
        }

        public CreateCohortWithOtherPartyControllerTestFixture SetCreateCohortResponse(CreateCohortResponse createCohortResponse)
        {
            CommitmentsApiClientMock.Setup(c => c.CreateCohort(It.IsAny<CreateCohortWithOtherPartyRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(createCohortResponse);

            return this;
        }

        public CreateCohortWithOtherPartyControllerTestFixture SetGetCohortResponse(GetCohortResponse getCohortResponse)
        {
            CommitmentsApiClientMock.Setup(c => c.GetCohort(It.IsAny<long>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(getCohortResponse);
            
            return this;
        }

        public CreateCohortWithOtherPartyControllerTestFixture VerifyAddCohortIsCalledWithCorrectMappedValues(MessageViewModel model)
        {
            CommitmentsApiClientMock.Verify(
                x => x.CreateCohort(
                    It.Is<CreateCohortWithOtherPartyRequest>(p =>
                        p.ProviderId == model.ProviderId && p.AccountLegalEntityId == model.AccountLegalEntityId &&
                        p.Message == model.Message), It.IsAny<CancellationToken>()));

            return this;
        }

        public CreateCohortWithOtherPartyControllerTestFixture WithCreateCohortApiError()
        {
            CommitmentsApiClientMock.Setup(
                    x => x.CreateCohort(It.IsAny<CreateCohortWithOtherPartyRequest>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new CommitmentsApiModelException(new List<ErrorDetail>{ErrorDetail}));

            return this;
        }

        public CreateCohortWithOtherPartyControllerTestFixture VerifyViewModelIsMappedCorrectly(MessageRequest request, MessageViewModel model)
        {
            Assert.AreEqual(request.AccountHashedId, model.AccountHashedId);
            Assert.AreEqual(request.CourseCode, model.CourseCode);
            Assert.AreEqual(request.StartMonthYear, model.StartMonthYear);
            Assert.AreEqual(request.ProviderId, model.ProviderId);
            Assert.AreEqual(request.EmployerAccountLegalEntityPublicHashedId, model.AccountLegalEntityHashedId);
            Assert.AreEqual(request.ReservationId, model.ReservationId);

            return this;
        }
    }
}