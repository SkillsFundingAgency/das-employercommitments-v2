using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Responses;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.DraftApprenticeship;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.DraftApprenticeship;
using SFA.DAS.Encoding;
using DeliveryModel = SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Types.DeliveryModel;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.DraftApprenticeship
{
    [TestFixture]
    public class ViewDraftApprenticeshipViewModelMapperTests
    {
        private ViewDraftApprenticeshipViewModelMapper _mapper;
        private Mock<ICommitmentsApiClient> _commitmentsApiClient;
        private Mock<IApprovalsApiClient> _approvalsApiClient;
        private ViewDraftApprenticeshipRequest _request;
        private ViewDraftApprenticeshipViewModel _result;
        private GetViewDraftApprenticeshipResponse _draftApprenticeship;
        private TrainingProgramme _course;
        private Mock<IEncodingService> _encodingService; 

        [SetUp]
        public async Task Arrange()
        {
            var autoFixture = new Fixture();
            _request = autoFixture.Create<ViewDraftApprenticeshipRequest>();

            _draftApprenticeship = autoFixture.Create<GetViewDraftApprenticeshipResponse>();
            _course = autoFixture.Create<TrainingProgramme>();
            
            _commitmentsApiClient = new Mock<ICommitmentsApiClient>();
            _commitmentsApiClient
                .Setup(x => x.GetTrainingProgramme(_draftApprenticeship.CourseCode, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GetTrainingProgrammeResponse
                {
                    TrainingProgramme = _course
                });

            _approvalsApiClient = new Mock<IApprovalsApiClient>();

            _approvalsApiClient.Setup(x =>
                    x.GetViewDraftApprenticeship(It.IsAny<long>(), It.IsAny<long>(), It.IsAny<long>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(_draftApprenticeship);

            _encodingService = new Mock<IEncodingService>(); ;
            _encodingService.Setup(t => t.Decode(It.IsAny<string>(), It.IsAny<EncodingType>())).Returns(123);

            _mapper = new ViewDraftApprenticeshipViewModelMapper(_commitmentsApiClient.Object, _approvalsApiClient.Object, _encodingService.Object);

            _result = await _mapper.Map(_request) as ViewDraftApprenticeshipViewModel;
        }

        [Test]
        public void ThenAccountHashedIdIsMappedCorrectly()
        {
            Assert.AreEqual(_request.Request.AccountHashedId, _result.AccountHashedId);
        }

        [Test]
        public void ThenCohortReferenceIsMappedCorrectly()
        {
            Assert.AreEqual(_request.Request.CohortReference, _result.CohortReference);
        }

        [Test]
        public void ThenFirstNameIsMappedCorrectly()
        {
            Assert.AreEqual(_draftApprenticeship.FirstName, _result.FirstName);
        }

        [Test]
        public void ThenLastNameIsMappedCorrectly()
        {
            Assert.AreEqual(_draftApprenticeship.LastName, _result.LastName);
        }

        [Test]
        public void ThenEmailIsMappedCorrectly()
        {
            Assert.AreEqual(_draftApprenticeship.Email, _result.Email);
        }

        [Test]
        public void ThenUlnIsMappedCorrectly()
        {
            Assert.AreEqual(_draftApprenticeship.Uln, _result.Uln);
        }

        [Test]
        public void DateOfBirthIsMappedCorrectly()
        {
            Assert.AreEqual(_draftApprenticeship.DateOfBirth, _result.DateOfBirth);
        }

        [Test]
        public void ThenCostIsMappedCorrectly()
        {
            Assert.AreEqual(_draftApprenticeship.Cost, _result.Cost);
        }

        [Test]
        public void ThenEmploymentPriceIsMappedCorrectly()
        {
            Assert.AreEqual(_draftApprenticeship.EmploymentPrice, _result.EmploymentPrice);
        }

        [Test]
        public void ThenStartDateIsMappedCorrectly()
        {
            Assert.AreEqual(_draftApprenticeship.StartDate, _result.StartDate);
        }

        [Test]
        public void ThenEndDateIsMappedCorrectly()
        {
            Assert.AreEqual(_draftApprenticeship.EndDate, _result.EndDate);
        }

        [Test]
        public void ThenEmploymentEndDateIsMappedCorrectly()
        {
            Assert.AreEqual(_draftApprenticeship.EmploymentEndDate, _result.EmploymentEndDate);
        }

        [Test]
        public void ThenReferenceIsMappedCorrectly()
        {
            Assert.AreEqual(_draftApprenticeship.Reference, _result.Reference);
        }

        [Test]
        public void ThenLegalEntityNameIsMappedCorrectly()
        {
            Assert.AreEqual(_request.Cohort.LegalEntityName, _result.LegalEntityName);
        }

        [TestCase(DeliveryModel.Regular, DeliveryModel.Regular)]
        [TestCase(DeliveryModel.PortableFlexiJob, DeliveryModel.PortableFlexiJob)]
        public async Task ThenDeliveryModelIsMappedCorrectly(DeliveryModel delivery, DeliveryModel display)
        {
            _draftApprenticeship.DeliveryModel = delivery;
            _result = await _mapper.Map(_request) as ViewDraftApprenticeshipViewModel;
            Assert.AreEqual(display, _result.DeliveryModel);
        }

        [Test]
        public void ThenTrainingCourseIsMappedCorrectly()
        {
            Assert.AreEqual(_course.Name, _result.TrainingCourse);
        }

        [Test]
        public void ThenVersionIsMappedCorrectly()
        {
            Assert.AreEqual(_draftApprenticeship.TrainingCourseVersion, _result.Version);
        }

        [Test]
        public void ThenHasStandardOptionsIsMappedCorrectly()
        {
            Assert.AreEqual(_draftApprenticeship.HasStandardOptions, _result.HasStandardOptions);
        }

        [Test]
        public async Task And_TrainingCourseOptionIsNull_Then_CourseOptionIsMappedToEmptyString()
        {
            _draftApprenticeship.TrainingCourseOption = null;

            _result = await _mapper.Map(_request) as ViewDraftApprenticeshipViewModel;

            Assert.AreEqual(string.Empty, _result.CourseOption);
        }

        [Test]
        public async Task And_TrainingCourseOptionIsEmptyString_Then_CourseOptionIsMappedToToBeConfirmed()
        {
            _draftApprenticeship.TrainingCourseOption = "";

            _result = await _mapper.Map(_request) as ViewDraftApprenticeshipViewModel;

            Assert.AreEqual("To be confirmed", _result.CourseOption);
        }

        [Test]
        public void Then_CourseOptionIsMappedCorrectly()
        {
            Assert.AreEqual(_draftApprenticeship.TrainingCourseOption, _result.CourseOption);
        }

        [Test]
        public void Then_RecognisePriorLearningIsMappedCorrectly()
        {
            Assert.AreEqual(_draftApprenticeship.RecognisePriorLearning, _result.RecognisePriorLearning);
        }

        [Test]
        public void Then_DurationReducedByIsMappedCorrectly()
        {
            Assert.AreEqual(_draftApprenticeship.DurationReducedBy, _result.DurationReducedBy);
        }

        [Test]
        public void Then_PriceReducedByIsMappedCorrectly()
        {
            Assert.AreEqual(_draftApprenticeship.PriceReducedBy, _result.PriceReducedBy);
        }

        [Test]
        public void Then_IsOnFlexiPaymentPilotIsMappedCorrectly()
        {
            Assert.AreEqual(_draftApprenticeship.IsOnFlexiPaymentPilot, _result.IsOnFlexiPaymentPilot);
        }

        [Test]
        public void Then_ActualStartDateByIsMappedCorrectly()
        {
            Assert.AreEqual(_draftApprenticeship.ActualStartDate, _result.ActualStartDate);
        }

        [Test]
        public void Then_TrainingTotalHoursIsMappedCorrectly()
        {
            Assert.AreEqual(_draftApprenticeship.TrainingTotalHours, _result.TrainingTotalHours);
        }

        [Test]
        public void Then_DurationReducedByHoursIsMappedCorrectly()
        {
            Assert.AreEqual(_draftApprenticeship.DurationReducedByHours, _result.DurationReducedByHours);
        }
    }
}
