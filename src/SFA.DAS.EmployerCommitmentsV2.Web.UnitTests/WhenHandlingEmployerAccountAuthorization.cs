using System.Security.Claims;
using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SFA.DAS.EmployerCommitmentsV2.Authorization;
using SFA.DAS.EmployerCommitmentsV2.Configuration;
using SFA.DAS.EmployerCommitmentsV2.Infrastructure;
using SFA.DAS.EmployerCommitmentsV2.Models.UserAccounts;
using SFA.DAS.EmployerCommitmentsV2.Services;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Responses;
using SFA.DAS.EmployerCommitmentsV2.Web.Authentication;
using SFA.DAS.EmployerCommitmentsV2.Web.Authorization.EmployerAccounts;
using SFA.DAS.EmployerCommitmentsV2.Web.RouteValues;
using SFA.DAS.Testing.AutoFixture;
using EmployerUserAccountItem = SFA.DAS.GovUK.Auth.Employer.EmployerUserAccountItem;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests;

public class WhenHandlingEmployerAccountAuthorization
{
    [Test, MoqAutoData]
    public async Task Then_User_EmployerAccounts_Should_Be_Retrieved_From_AssociatedAccountsService(
        EmployerIdentifier employerIdentifier,
        string userId,
        string email,
        EmployerUserAccountItem serviceResponse,
        EmployerTransactorOwnerAccountRequirement transactorOwnerRolesRequirement,
        [Frozen] Mock<IHttpContextAccessor> httpContextAccessor,
        Mock<ILogger<EmployerAccountAuthorisationHandler>> logger,
        [Frozen] Mock<IAssociatedAccountsService> associatedAccountsService,
        EmployerAccountAuthorisationHandler authHandler
    )
    {
        //Arrange
        serviceResponse.AccountId = serviceResponse.AccountId.ToUpper();
        employerIdentifier.Role = "Viewer";
        employerIdentifier.AccountId = serviceResponse.AccountId;
        
        var claimsPrinciple = new ClaimsPrincipal([
            new ClaimsIdentity([
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.NameIdentifier, userId)
            ])
        ]);
        
        var context = new AuthorizationHandlerContext([transactorOwnerRolesRequirement], claimsPrinciple, null);
        var httpContext = new DefaultHttpContext(new FeatureCollection());
        httpContext.Request.RouteValues.Add(RouteValueKeys.AccountHashedId, employerIdentifier.AccountId);
        httpContextAccessor.Setup(x => x.HttpContext).Returns(httpContext);
        
        var accounts = new List<EmployerUserAccountItem>
        {
            serviceResponse
        };

        var accountsDictionary = accounts.ToDictionary(x => x.AccountId);

        associatedAccountsService.Setup(x => x.GetAccounts(false)).ReturnsAsync(accountsDictionary);

        //Act
        await authHandler.IsEmployerAuthorised(context, EmployerUserRole.Transactor);

        //Assert
        associatedAccountsService.Verify(x => x.GetAccounts(false), Times.Once);
        associatedAccountsService.Verify(x => x.GetAccounts(true), Times.Never);
    }

    [Test, MoqAutoData]
    public async Task Then_User_EmployerAccounts_Should_Be_Retrieved_From_AssociatedAccountsService_With_Force_Refresh_When_AssociatedAccount_Is_Missing(
        EmployerIdentifier employerIdentifier,
        string userId,
        string email,
        EmployerUserAccountItem serviceResponse,
        EmployerTransactorOwnerAccountRequirement transactorOwnerRolesRequirement,
        [Frozen] Mock<IHttpContextAccessor> httpContextAccessor,
        Mock<ILogger<EmployerAccountAuthorisationHandler>> logger,
        [Frozen] Mock<IAssociatedAccountsService> associatedAccountsService,
        EmployerAccountAuthorisationHandler authHandler
    )
    {
        //Arrange
        employerIdentifier.Role = "Viewer";
        employerIdentifier.AccountId = employerIdentifier.AccountId.ToUpper();
        var claimsPrinciple = new ClaimsPrincipal([
            new ClaimsIdentity([
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.NameIdentifier, userId)
            ])
        ]);
        var context = new AuthorizationHandlerContext([transactorOwnerRolesRequirement], claimsPrinciple, null);
        var httpContext = new DefaultHttpContext(new FeatureCollection());
        httpContext.Request.RouteValues.Add(RouteValueKeys.AccountHashedId, employerIdentifier.AccountId);
        httpContextAccessor.Setup(x => x.HttpContext).Returns(httpContext);

        var accounts = new List<EmployerUserAccountItem>
        {
            serviceResponse
        };

        var accountsDictionary = accounts.ToDictionary(x => x.AccountId);

        associatedAccountsService.Setup(x => x.GetAccounts(false)).ReturnsAsync(() => new Dictionary<string, EmployerUserAccountItem>());
        associatedAccountsService.Setup(x => x.GetAccounts(true)).ReturnsAsync(accountsDictionary);

        //Act
        await authHandler.IsEmployerAuthorised(context, EmployerUserRole.Transactor);

        //Assert
        associatedAccountsService.Verify(x => x.GetAccounts(false), Times.Once);
        associatedAccountsService.Verify(x => x.GetAccounts(true), Times.Once);
    }
    
    [Test, MoqAutoData]
    public async Task Then_Viewer_Is_Not_Allowed_For_Transactor_Role(
        EmployerIdentifier employerIdentifier,
        EmployerTransactorOwnerAccountRequirement transactorOwnerRolesRequirement,
        [Frozen] Mock<IHttpContextAccessor> httpContextAccessor,
        EmployerAccountAuthorisationHandler authorizationHandler)
    {
        //Arrange
        employerIdentifier.Role = "Viewer";
        employerIdentifier.AccountId = employerIdentifier.AccountId.ToUpper();
        var employerAccounts = new Dictionary<string, EmployerIdentifier> { { employerIdentifier.AccountId, employerIdentifier } };
        var claim = new Claim(EmployerClaims.AccountsClaimsTypeIdentifier, JsonConvert.SerializeObject(employerAccounts));
        var claimsPrinciple = new ClaimsPrincipal([new ClaimsIdentity([claim])]);
        var context = new AuthorizationHandlerContext([transactorOwnerRolesRequirement], claimsPrinciple, null);
        var httpContext = new DefaultHttpContext(new FeatureCollection());
        httpContext.Request.RouteValues.Add(RouteValueKeys.AccountHashedId, employerIdentifier.AccountId);
        httpContextAccessor.Setup(x => x.HttpContext).Returns(httpContext);

        //Act
        var actual = await authorizationHandler.IsEmployerAuthorised(context, EmployerUserRole.Transactor);

        //Assert
        actual.Should().BeFalse();
    }

    [Test, MoqAutoData]
    public async Task Then_Returns_False_If_Employer_Is_Not_Authorized(
        string accountId,
        EmployerIdentifier employerIdentifier,
        EmployerTransactorOwnerAccountRequirement transactorOwnerRolesRequirement,
        [Frozen] Mock<IHttpContextAccessor> httpContextAccessor,
        EmployerAccountAuthorisationHandler authorizationHandler)
    {
        //Arrange
        employerIdentifier.Role = "Owner";
        employerIdentifier.AccountId = employerIdentifier.AccountId.ToUpper();
        var employerAccounts = new Dictionary<string, EmployerIdentifier> { { employerIdentifier.AccountId, employerIdentifier } };
        var claim = new Claim(EmployerClaims.AccountsClaimsTypeIdentifier, JsonConvert.SerializeObject(employerAccounts));
        var claimsPrinciple = new ClaimsPrincipal([new ClaimsIdentity([claim])]);
        var context = new AuthorizationHandlerContext([transactorOwnerRolesRequirement], claimsPrinciple, null);
        var responseMock = new FeatureCollection();
        var httpContext = new DefaultHttpContext(responseMock);
        httpContext.Request.RouteValues.Add(RouteValueKeys.AccountHashedId, accountId.ToUpper());
        httpContextAccessor.Setup(x => x.HttpContext).Returns(httpContext);

        //Act
        var actual = await authorizationHandler.IsEmployerAuthorised(context, EmployerUserRole.None);

        //Assert
        actual.Should().BeFalse();
    }

    [Test, MoqAutoData]
    public async Task Then_Returns_False_If_Employer_Is_Authorized_But_Has_Invalid_Role_But_Should_Allow_All_known_Roles(
        EmployerIdentifier employerIdentifier,
        EmployerTransactorOwnerAccountRequirement transactorOwnerRolesRequirement,
        [Frozen] Mock<IHttpContextAccessor> httpContextAccessor,
        EmployerAccountAuthorisationHandler authorizationHandler)
    {
        //Arrange
        employerIdentifier.Role = "Viewer-Owner-Transactor";
        employerIdentifier.AccountId = employerIdentifier.AccountId.ToUpper();
        var employerAccounts = new Dictionary<string, EmployerIdentifier> { { employerIdentifier.AccountId, employerIdentifier } };
        var claim = new Claim(EmployerClaims.AccountsClaimsTypeIdentifier, JsonConvert.SerializeObject(employerAccounts));
        var claimsPrinciple = new ClaimsPrincipal([new ClaimsIdentity([claim])]);
        var context = new AuthorizationHandlerContext([transactorOwnerRolesRequirement], claimsPrinciple, null);
        var responseMock = new FeatureCollection();
        var httpContext = new DefaultHttpContext(responseMock);
        httpContext.Request.RouteValues.Add(RouteValueKeys.AccountHashedId, employerIdentifier.AccountId);
        httpContextAccessor.Setup(x => x.HttpContext).Returns(httpContext);

        //Act
        var actual = await authorizationHandler.IsEmployerAuthorised(context, EmployerUserRole.Owner);

        //Assert
        actual.Should().BeFalse();
    }


    [Test, MoqAutoData]
    public async Task Then_Returns_False_If_AccountId_Not_In_Url(
        EmployerIdentifier employerIdentifier,
        EmployerTransactorOwnerAccountRequirement transactorOwnerRolesRequirement,
        [Frozen] Mock<IHttpContextAccessor> httpContextAccessor,
        EmployerAccountAuthorisationHandler authorizationHandler)
    {
        //Arrange
        employerIdentifier.Role = "Owner";
        employerIdentifier.AccountId = employerIdentifier.AccountId.ToUpper();
        var employerAccounts = new Dictionary<string, EmployerIdentifier> { { employerIdentifier.AccountId, employerIdentifier } };
        var claim = new Claim(EmployerClaims.AccountsClaimsTypeIdentifier, JsonConvert.SerializeObject(employerAccounts));
        var claimsPrinciple = new ClaimsPrincipal([new ClaimsIdentity([claim])]);
        var context = new AuthorizationHandlerContext([transactorOwnerRolesRequirement], claimsPrinciple, null);
        var responseMock = new FeatureCollection();
        var httpContext = new DefaultHttpContext(responseMock);
        httpContext.Request.RouteValues.Clear();
        httpContextAccessor.Setup(x => x.HttpContext).Returns(httpContext);

        //Act
        var actual = await authorizationHandler.IsEmployerAuthorised(context, EmployerUserRole.Viewer);

        //Assert
        actual.Should().BeFalse();
    }

    [Test, MoqAutoData]
    public async Task Then_Returns_False_If_No_Matching_AccountIdentifier_Claim_Found(
        EmployerIdentifier employerIdentifier,
        EmployerTransactorOwnerAccountRequirement transactorOwnerRolesRequirement,
        [Frozen] Mock<IHttpContextAccessor> httpContextAccessor,
        EmployerAccountAuthorisationHandler authorizationHandler)
    {
        //Arrange
        employerIdentifier.Role = "Viewer-Owner-Transactor";
        employerIdentifier.AccountId = employerIdentifier.AccountId.ToUpper();
        var employerAccounts = new Dictionary<string, EmployerIdentifier> { { employerIdentifier.AccountId, employerIdentifier } };
        var claim = new Claim("SomeOtherClaim", JsonConvert.SerializeObject(employerAccounts));
        var claimsPrinciple = new ClaimsPrincipal([new ClaimsIdentity([claim])]);
        var context = new AuthorizationHandlerContext([transactorOwnerRolesRequirement], claimsPrinciple, null);
        var responseMock = new FeatureCollection();
        var httpContext = new DefaultHttpContext(responseMock);
        httpContext.Request.RouteValues.Add(RouteValueKeys.AccountHashedId, employerIdentifier.AccountId);
        httpContextAccessor.Setup(x => x.HttpContext).Returns(httpContext);

        //Act
        var actual = await authorizationHandler.IsEmployerAuthorised(context, EmployerUserRole.Viewer);

        //Assert
        actual.Should().BeFalse();
    }

    [Test, MoqAutoData]
    public async Task Then_Returns_False_If_No_Matching_NameIdentifier_Claim_Found_For_GovSignIn(
        EmployerIdentifier employerIdentifier,
        EmployerTransactorOwnerAccountRequirement transactorOwnerRolesRequirement,
        [Frozen] Mock<IHttpContextAccessor> httpContextAccessor,
        [Frozen] Mock<IOptions<EmployerCommitmentsV2Configuration>> forecastingConfiguration,
        EmployerAccountAuthorisationHandler authorizationHandler)
    {
        //Arrange
        employerIdentifier.Role = "Viewer-Owner-Transactor";
        employerIdentifier.AccountId = employerIdentifier.AccountId.ToUpper();
        var employerAccounts = new Dictionary<string, EmployerIdentifier> { { employerIdentifier.AccountId, employerIdentifier } };
        var claim = new Claim("SomeOtherClaim", JsonConvert.SerializeObject(employerAccounts));
        var claimsPrinciple = new ClaimsPrincipal([new ClaimsIdentity([claim])]);
        var context = new AuthorizationHandlerContext([transactorOwnerRolesRequirement], claimsPrinciple, null);
        var responseMock = new FeatureCollection();
        var httpContext = new DefaultHttpContext(responseMock);
        httpContext.Request.RouteValues.Add(RouteValueKeys.AccountHashedId, employerIdentifier.AccountId);
        httpContextAccessor.Setup(x => x.HttpContext).Returns(httpContext);

        //Act
        var actual = await authorizationHandler.IsEmployerAuthorised(context, EmployerUserRole.Viewer);

        //Assert
        actual.Should().BeFalse();
    }

    [Test, MoqAutoData]
    public async Task Then_Returns_False_If_The_Claim_Cannot_Be_Deserialized(
        EmployerIdentifier employerIdentifier,
        EmployerTransactorOwnerAccountRequirement transactorOwnerRolesRequirement,
        [Frozen] Mock<IHttpContextAccessor> httpContextAccessor,
        EmployerAccountAuthorisationHandler authorizationHandler)
    {
        //Arrange
        employerIdentifier.Role = "Owner";
        employerIdentifier.AccountId = employerIdentifier.AccountId.ToUpper();
        var claim = new Claim(EmployerClaims.AccountsClaimsTypeIdentifier, JsonConvert.SerializeObject(employerIdentifier));
        var claimsPrinciple = new ClaimsPrincipal([new ClaimsIdentity([claim])]);
        var context = new AuthorizationHandlerContext([transactorOwnerRolesRequirement], claimsPrinciple, null);
        var responseMock = new FeatureCollection();
        var httpContext = new DefaultHttpContext(responseMock);
        httpContext.Request.RouteValues.Add(RouteValueKeys.AccountHashedId, employerIdentifier.AccountId);
        httpContextAccessor.Setup(x => x.HttpContext).Returns(httpContext);

        //Act
        var actual = await authorizationHandler.IsEmployerAuthorised(context, EmployerUserRole.Viewer);

        //Assert
        actual.Should().BeFalse();
    }
}