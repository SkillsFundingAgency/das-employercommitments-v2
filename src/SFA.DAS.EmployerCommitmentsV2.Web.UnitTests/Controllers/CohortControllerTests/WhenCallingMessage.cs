using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.NUnit3;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.Authorization.Services;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.CommitmentsV2.Api.Types.Validation;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.EmployerUrlHelper;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.CohortControllerTests
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
        public async Task GetMessage_ValidModel_ShouldReturnMessageViewModelWithMappedValues(MessageRequest request)
        {
            var result = await _fixture.Sut.Message(request);

            var model = result.VerifyReturnsViewModel().WithModel<MessageViewModel>();
            _fixture.VerifyViewModelIsMappedCorrectly(model);
        }

        [Test, AutoData]
        public async Task PostMessage_WithValidRequest_ShouldAddCohortAndReturnRedirectResult(MessageViewModel model, CreateCohortResponse createCohortResponse)
        {
            _fixture.SetCreateCohortResponse(createCohortResponse);

            var result = await _fixture.Sut.Message(model);

            _fixture.VerifyAddCohortIsCalledWithCorrectMappedValues(model);


            var redirect = result.VerifyReturnsRedirectToActionResult();
            Assert.AreEqual(redirect.ActionName, "Finished");
            Assert.AreEqual(redirect.RouteValues["AccountHashedId"], model.AccountHashedId);
            Assert.AreEqual(redirect.RouteValues["CohortReference"], createCohortResponse.CohortReference);
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

        public class CreateCohortWithOtherPartyControllerTestFixture
        {
            public CreateCohortWithOtherPartyControllerTestFixture()
            {
                var autoFixture = new Fixture();

                CommitmentsApiClientMock = new Mock<ICommitmentsApiClient>();
                ErrorDetail = new ErrorDetail("field1", "error message");

                MapperResult = new CreateCohortWithOtherPartyRequest();
                ModelMapperMock = new Mock<IModelMapper>();
                ModelMapperMock.Setup(x => x.Map<CreateCohortWithOtherPartyRequest>(It.IsAny<object>()))
                    .ReturnsAsync(() => MapperResult);

                MessageViewModel = autoFixture.Create<MessageViewModel>();
                ModelMapperMock.Setup(x => x.Map<MessageViewModel>(It.IsAny<object>()))
                    .ReturnsAsync(() => MessageViewModel);

                Sut = new CohortController(
                    CommitmentsApiClientMock.Object, Mock.Of<ILogger<CohortController>>(),
                    Mock.Of<ILinkGenerator>(),
                    ModelMapperMock.Object,
                    Mock.Of<IAuthorizationService>()
                );
            }

            public Mock<ICommitmentsApiClient> CommitmentsApiClientMock { get; }
            public Mock<IModelMapper> ModelMapperMock { get; }
            public CreateCohortWithOtherPartyRequest MapperResult { get; }
            public MessageViewModel MessageViewModel { get; }
            public CohortController Sut { get; }
            public ErrorDetail ErrorDetail { get; }

            public CreateCohortWithOtherPartyControllerTestFixture WithInvalidModel()
            {
                Sut.ModelState.AddModelError("AKey", "Some Error");
                return this;
            }

            public CreateCohortWithOtherPartyControllerTestFixture SetCreateCohortResponse(
                CreateCohortResponse createCohortResponse)
            {
                CommitmentsApiClientMock.Setup(c =>
                        c.CreateCohort(It.IsAny<CreateCohortWithOtherPartyRequest>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(createCohortResponse);

                return this;
            }

            public CreateCohortWithOtherPartyControllerTestFixture SetGetCohortResponse(
                GetCohortResponse getCohortResponse)
            {
                CommitmentsApiClientMock.Setup(c => c.GetCohort(It.IsAny<long>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(getCohortResponse);

                return this;
            }

            public CreateCohortWithOtherPartyControllerTestFixture VerifyAddCohortIsCalledWithCorrectMappedValues(
                MessageViewModel model)
            {
                CommitmentsApiClientMock.Verify(
                    x => x.CreateCohort(
                        It.Is<CreateCohortWithOtherPartyRequest>(p => p == MapperResult),
                        It.IsAny<CancellationToken>()));

                return this;
            }

            public CreateCohortWithOtherPartyControllerTestFixture WithCreateCohortApiError()
            {
                CommitmentsApiClientMock.Setup(
                        x => x.CreateCohort(It.IsAny<CreateCohortWithOtherPartyRequest>(),
                            It.IsAny<CancellationToken>()))
                    .ThrowsAsync(new CommitmentsApiModelException(new List<ErrorDetail> {ErrorDetail}));

                return this;
            }

            public CreateCohortWithOtherPartyControllerTestFixture VerifyViewModelIsMappedCorrectly(MessageViewModel model)
            {
                Assert.AreEqual(MessageViewModel.AccountHashedId, model.AccountHashedId);
                Assert.AreEqual(MessageViewModel.CourseCode, model.CourseCode);
                Assert.AreEqual(MessageViewModel.StartMonthYear, model.StartMonthYear);
                Assert.AreEqual(MessageViewModel.ProviderId, model.ProviderId);
                Assert.AreEqual(MessageViewModel.AccountLegalEntityHashedId, model.AccountLegalEntityHashedId);
                Assert.AreEqual(MessageViewModel.ReservationId, model.ReservationId);

                return this;
            }
        }
    }
}