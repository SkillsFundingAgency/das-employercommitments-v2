using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.Apprenticeships.Api.Client;
using SFA.DAS.Commitments.Shared.Interfaces;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.CommitmentsV2.Api.Types.Validation;
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
                CommitmentsApiClientMock = new Mock<ICommitmentsApiClient>();
                ErrorDetail = new ErrorDetail("field1", "error message");

                MapperResult = new CreateCohortWithOtherPartyRequest();
                ModelMapperMock = new Mock<IModelMapper>();
                ModelMapperMock.Setup(x => x.Map<CreateCohortWithOtherPartyRequest>(It.IsAny<object>()))
                    .ReturnsAsync(() => MapperResult);

                Sut = new CohortController(
                    CommitmentsApiClientMock.Object, Mock.Of<ILogger<CohortController>>(),
                    Mock.Of<ILinkGenerator>(),
                    ModelMapperMock.Object
                );
            }

            public Mock<ICommitmentsApiClient> CommitmentsApiClientMock { get; }
            public Mock<IModelMapper> ModelMapperMock { get; }
            public CreateCohortWithOtherPartyRequest MapperResult { get; }
            public CohortController Sut { get; }
            public ErrorDetail ErrorDetail { get; }

            public CreateCohortWithOtherPartyControllerTestFixture WithProviderName(string name)
            {
                CommitmentsApiClientMock.Setup(x => x.GetProvider(It.IsAny<long>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(new GetProviderResponse {Name = name});
                return this;
            }

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

            public CreateCohortWithOtherPartyControllerTestFixture VerifyViewModelIsMappedCorrectly(
                MessageRequest request, MessageViewModel model)
            {
                Assert.AreEqual(request.AccountHashedId, model.AccountHashedId);
                Assert.AreEqual(request.CourseCode, model.CourseCode);
                Assert.AreEqual(request.StartMonthYear, model.StartMonthYear);
                Assert.AreEqual(request.ProviderId, model.ProviderId);
                Assert.AreEqual(request.AccountLegalEntityHashedId, model.AccountLegalEntityHashedId);
                Assert.AreEqual(request.ReservationId, model.ReservationId);

                return this;
            }
        }
    }
}