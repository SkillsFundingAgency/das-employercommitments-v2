using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.Commitments.Shared.Models;
using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using SFA.DAS.EmployerCommitmentsV2.Web.Authentication;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.DraftApprenticeship;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Shared;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers
{
    [TestFixture]
    [Parallelizable]
    public class AttachUserInfoToSaveRequestsTests
    {
        private AttachUserInfoToSaveRequestsTestsFixture _fixture;

        [SetUp]
        public void Arrange()
        {
            _fixture = new AttachUserInfoToSaveRequestsTestsFixture();
        }

        [Test]
        public async Task MapSaveRequest_WhenNoAuthenticatedUser_ThenResultIsNotNullAndUserInfoIsNull()
        {
            var result = await _fixture.SaveRequestMapper.Map(_fixture.InputViewModel);

            Assert.IsNotNull(result);
            Assert.IsNull(result.UserInfo);
        }

        [Test]
        public async Task MapSaveRequest_WhenAuthenticatedUser_ThenUserInfoIsNotNullAndPopulated()
        {
            var result = await _fixture.SetupAuthenticatedUser().SaveRequestMapper.Map(_fixture.InputViewModel);

            Assert.IsNotNull(result.UserInfo);
            Assert.AreEqual("UserId", result.UserInfo.UserId);
            Assert.AreEqual("UserName", result.UserInfo.UserDisplayName);
            Assert.AreEqual("UserEmail", result.UserInfo.UserEmail);
        }

        [Test]
        public async Task MapNonSaveRequest_WhenNoAuthenticatedUser_ThenResultIsNotNull()
        {
            var result = await _fixture.NonSaveRequestMapper.Map(_fixture.InputDetails);

            Assert.IsNotNull(result);
        }

        [Test]
        public async Task MapNonSaveRequest_WhenAuthenticatedUser_ThenResultIsNotNull()
        {
            var result = await _fixture.SetupAuthenticatedUser().NonSaveRequestMapper.Map(_fixture.InputDetails);

            Assert.IsNotNull(result);
        }
    }

    public class AttachUserInfoToSaveRequestsTestsFixture
    {
        public AttachUserInfoToSaveRequests<AddDraftApprenticeshipViewModel, CreateCohortRequest> SaveRequestMapper;
        public AttachUserInfoToSaveRequests<EditDraftApprenticeshipDetails, EditDraftApprenticeshipViewModel> NonSaveRequestMapper;
        public Mock<IAuthenticationService> AuthenticationServiceMock;
        public AddDraftApprenticeshipViewModel InputViewModel;
        public EditDraftApprenticeshipDetails InputDetails;


        public AttachUserInfoToSaveRequestsTestsFixture()
        {
            var autoFixture = new Fixture();

            AuthenticationServiceMock = new Mock<IAuthenticationService>();
            InputViewModel = new AddDraftApprenticeshipViewModel();
            InputDetails = new EditDraftApprenticeshipDetails();

            SaveRequestMapper = new AttachUserInfoToSaveRequests<AddDraftApprenticeshipViewModel, CreateCohortRequest>(new AddDraftApprenticeshipToCreateCohortRequestMapper(), AuthenticationServiceMock.Object);
            NonSaveRequestMapper = new AttachUserInfoToSaveRequests<EditDraftApprenticeshipDetails, EditDraftApprenticeshipViewModel>(new EditDraftApprenticeshipDetailsToViewModelMapper(), AuthenticationServiceMock.Object);
        }

        public AttachUserInfoToSaveRequestsTestsFixture SetupAuthenticatedUser()
        {
            AuthenticationServiceMock.Setup(x => x.IsUserAuthenticated()).Returns(true);
            AuthenticationServiceMock.Setup(x => x.UserId).Returns("UserId");
            AuthenticationServiceMock.Setup(x => x.UserName).Returns("UserName");
            AuthenticationServiceMock.Setup(x => x.UserEmail).Returns("UserEmail");

            return this;
        }
    }
}