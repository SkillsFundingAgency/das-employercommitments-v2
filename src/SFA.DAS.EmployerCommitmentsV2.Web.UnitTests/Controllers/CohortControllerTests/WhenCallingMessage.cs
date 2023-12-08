using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.NUnit3;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.CommitmentsV2.Api.Types.Validation;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.EmployerUrlHelper;
using SFA.DAS.Encoding;

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
            _fixture.WithLegalEntityNameStoredInTempData();

            var result = await _fixture.Sut.Message(request);

            var model = result.VerifyReturnsViewModel().WithModel<MessageViewModel>();
            _fixture.VerifyViewModelIsMappedCorrectly(model);
        }

        [Test, AutoData]
        public async Task GetMessage_ValidModel_ForProvider_ShouldPopulateLegalEntityNameFromTempData(
            [Frozen] MessageRequest request)
        {
            request.LegalEntityName = null;
            _fixture.WithLegalEntityNameStoredInTempData();

            var result = await _fixture.Sut.Message(request);

            var model = result.VerifyReturnsViewModel().WithModel<MessageViewModel>();
            _fixture.VerifyLegalEntityNamePopulatedFromTempData(model);
        }

        [Test, AutoData]
        public async Task PostMessage_WithValidRequest_ShouldAddCohortAndReturnRedirectResult(MessageViewModel model, CreateCohortResponse createCohortResponse)
        {
            _fixture.SetCreateCohortResponse(createCohortResponse);

            var result = await _fixture.Sut.Message(model);

            _fixture.VerifyAddCohortIsCalledWithCorrectMappedValues(model);


            var redirect = result.VerifyReturnsRedirectToActionResult();
            Assert.That("Finished", Is.EqualTo(redirect.ActionName));
            Assert.That(model.AccountHashedId, Is.EqualTo(redirect.RouteValues["AccountHashedId"]));
            Assert.That(createCohortResponse.CohortReference, Is.EqualTo(redirect.RouteValues["CohortReference"]));
        }

        [Test, AutoData]
        public async Task GetFinished_ValidModel_ShouldReturnFinishedViewModelWithMappedValues(FinishedRequest request, GetCohortResponse getCohortResponse)
        {
            _fixture.SetGetCohortResponse(getCohortResponse);

            var response = await _fixture.Sut.Finished(request);
            var model = response.VerifyReturnsViewModel().WithModel<FinishedViewModel>();

            Assert.That(request.CohortReference, Is.EqualTo(model.CohortReference));
            Assert.That(getCohortResponse.LegalEntityName, Is.EqualTo(model.LegalEntityName));
            Assert.That(getCohortResponse.ProviderName, Is.EqualTo(model.ProviderName));
            Assert.That(getCohortResponse.LatestMessageCreatedByEmployer, Is.EqualTo(model.Message));
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
                ModelMapperMock
                    .Setup(x => x.Map<CreateCohortWithOtherPartyRequest>(It.IsAny<object>()))
                    .ReturnsAsync(() => MapperResult);

                MessageViewModel = autoFixture.Create<MessageViewModel>();
                ModelMapperMock.Setup(x => x.Map<MessageViewModel>(It.IsAny<object>()))
                    .ReturnsAsync(() => MessageViewModel);

                Sut = new CohortController(
                    CommitmentsApiClientMock.Object, Mock.Of<ILogger<CohortController>>(),
                    Mock.Of<ILinkGenerator>(),
                    ModelMapperMock.Object,
                    Mock.Of<IEncodingService>(),
                    Mock.Of<IApprovalsApiClient>()
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

            public CreateCohortWithOtherPartyControllerTestFixture WithLegalEntityNameStoredInTempData()
            {
                var tempData = new TempDataDictionary(new DefaultHttpContext(), Mock.Of<ITempDataProvider>());
                tempData[nameof(MessageRequest.LegalEntityName)] = MessageViewModel.LegalEntityName;
                Sut.TempData = tempData;
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
                Assert.That(model.AccountHashedId, Is.EqualTo(MessageViewModel.AccountHashedId));
                Assert.That(model.CourseCode, Is.EqualTo(MessageViewModel.CourseCode));
                Assert.That(model.StartMonthYear, Is.EqualTo(MessageViewModel.StartMonthYear));
                Assert.That(model.ProviderId, Is.EqualTo(MessageViewModel.ProviderId));
                Assert.That(model.AccountLegalEntityHashedId, Is.EqualTo(MessageViewModel.AccountLegalEntityHashedId));
                Assert.That(model.ReservationId, Is.EqualTo(MessageViewModel.ReservationId));

                return this;
            }

            public CreateCohortWithOtherPartyControllerTestFixture VerifyLegalEntityNamePopulatedFromTempData(MessageViewModel model)
            {
                Assert.That(model.LegalEntityName, Is.EqualTo(MessageViewModel.LegalEntityName));
                return this;
            }
        }
    }
}