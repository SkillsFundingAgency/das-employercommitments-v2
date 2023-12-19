using SFA.DAS.CommitmentsV2.Shared.Models;
using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Contracts;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Requests;
using SFA.DAS.EmployerCommitmentsV2.Web.Authentication;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.DraftApprenticeship;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Cohort;

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

        Assert.That(result, Is.Not.Null);
        Assert.That(result.UserInfo, Is.Null);
    }

    [Test]
    public async Task MapSaveRequest_WhenAuthenticatedUser_ThenUserInfoIsSet()
    {
        var result = await _fixture.SetupAuthenticatedUser().SaveRequestMapper.Map(_fixture.InputViewModel);

        Assert.That(result.UserInfo, Is.Not.Null);
        Assert.That(result.UserInfo, Is.EqualTo(_fixture.UserInfo));
    }

    [Test]
    public async Task MapNonSaveRequest_WhenNoAuthenticatedUser_ThenResultIsNotNull()
    {
        var result = await _fixture.NonSaveRequestMapper.Map(_fixture.InputDetails);

        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public async Task MapNonSaveRequest_WhenAuthenticatedUser_ThenResultIsNotNull()
    {
        var result = await _fixture.SetupAuthenticatedUser().NonSaveRequestMapper.Map(_fixture.InputDetails);

        Assert.That(result, Is.Not.Null);
    }
}

public class AttachUserInfoToSaveRequestsTestsFixture
{
    public AttachUserInfoToSaveRequests<MessageViewModel, CreateCohortWithOtherPartyRequest> SaveRequestMapper;
    public AttachUserInfoToSaveRequests<EditDraftApprenticeshipDetails, EditDraftApprenticeshipViewModel> NonSaveRequestMapper;
    public Mock<IAuthenticationService> AuthenticationServiceMock;
    public MessageViewModel InputViewModel;
    public EditDraftApprenticeshipDetails InputDetails;
    public UserInfo UserInfo;


    public AttachUserInfoToSaveRequestsTestsFixture()
    {
        var autoFixture = new Fixture();

        AuthenticationServiceMock = new Mock<IAuthenticationService>();
        InputViewModel = new MessageViewModel();
        InputDetails = new EditDraftApprenticeshipDetails();
        UserInfo = new UserInfo();

        SaveRequestMapper = new AttachUserInfoToSaveRequests<MessageViewModel, CreateCohortWithOtherPartyRequest>(new CreateCohortWithOtherPartyRequestMapper(), AuthenticationServiceMock.Object);
        NonSaveRequestMapper = new AttachUserInfoToSaveRequests<EditDraftApprenticeshipDetails, EditDraftApprenticeshipViewModel>(new EditDraftApprenticeshipViewModelMapper(), AuthenticationServiceMock.Object);
    }

    public AttachUserInfoToSaveRequestsTestsFixture SetupAuthenticatedUser()
    {
        AuthenticationServiceMock.Setup(x => x.IsUserAuthenticated()).Returns(true);
        AuthenticationServiceMock.Setup(x => x.UserId).Returns("UserId");
        AuthenticationServiceMock.Setup(x => x.UserName).Returns("UserName");
        AuthenticationServiceMock.Setup(x => x.UserEmail).Returns("UserEmail");
        AuthenticationServiceMock.Setup(x => x.UserInfo).Returns(UserInfo);

        return this;
    }
}