using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using SFA.DAS.EmployerCommitmentsV2.Services;
using SFA.DAS.GovUK.Auth.Employer;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerCommitmentsV2.UnitTests.Services.AssociatedAccounts;

public class WhenGettingAssociatedAccounts
{
    [Test, MoqAutoData]
    public async Task Then_User_EmployerAccounts_Should_Be_Retrieved_From_Claims_When_Populated_And_ForceRefresh_Is_False(
        string userId,
        string email,
        [Frozen] Mock<IHttpContextAccessor> httpContextAccessor,
        Mock<ILogger<AccountClaimsService>> logger,
        Mock<IGovAuthEmployerAccountService> userAccountService,
        Dictionary<string, EmployerUserAccountItem> accountData
    )
    {
        //Arrange
        var serialisedAccounts = JsonConvert.SerializeObject(accountData);

        var claimsPrinciple = new ClaimsPrincipal([
            new ClaimsIdentity([
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim(EmployerClaims.AccountsClaimsTypeIdentifier, serialisedAccounts),
            ])
        ]);

        var httpContext = new DefaultHttpContext(new FeatureCollection())
        {
            User = claimsPrinciple
        };

        httpContextAccessor.Setup(x => x.HttpContext).Returns(httpContext);

        var helper = new AccountClaimsService(userAccountService.Object, httpContextAccessor.Object, logger.Object)
        {
            MaxPermittedNumberOfAccountsOnClaim = accountData.Count
        };

        //Act
        var result = await helper.GetAssociatedAccounts(forceRefresh: false);

        //Assert
        userAccountService.Verify(x => x.GetUserAccounts(userId, email), Times.Never);
        claimsPrinciple.Claims.Should().Contain(c => c.Type.Equals(EmployerClaims.AccountsClaimsTypeIdentifier));

        var actualClaimValue = claimsPrinciple.Claims.First(c => c.Type.Equals(EmployerClaims.AccountsClaimsTypeIdentifier)).Value;
        actualClaimValue.Should().Be(serialisedAccounts);

        result.Should().BeEquivalentTo(accountData);
    }
    
    [Test]
    [MoqInlineAutoData(true)]
    [MoqInlineAutoData(false)]
    public async Task Then_User_EmployerAccounts_Should_Be_Retrieved_From_UserService_When_Claims_Are_Populated_But_Empty(
        bool forceRefresh,
        string userId,
        string email,
        [Frozen] Mock<IHttpContextAccessor> httpContextAccessor,
        Mock<ILogger<AccountClaimsService>> logger,
        Mock<IGovAuthEmployerAccountService> userAccountService,
        EmployerUserAccounts updatedAccountData
    )
    {
        //Arrange
        var claimsPrinciple = new ClaimsPrincipal([
            new ClaimsIdentity([
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim(EmployerClaims.AccountsClaimsTypeIdentifier, JsonConvert.SerializeObject(new Dictionary<string, EmployerUserAccountItem>())),
            ])
        ]);

        var httpContext = new DefaultHttpContext(new FeatureCollection())
        {
            User = claimsPrinciple
        };

        httpContextAccessor.Setup(x => x.HttpContext).Returns(httpContext);
        userAccountService.Setup(x => x.GetUserAccounts(userId, email)).ReturnsAsync(updatedAccountData);

        var associatedAccountsService = new AccountClaimsService(userAccountService.Object, httpContextAccessor.Object, logger.Object)
        {
            MaxPermittedNumberOfAccountsOnClaim = updatedAccountData.EmployerAccounts.Count()
        };

        //Act
        var result = await associatedAccountsService.GetAssociatedAccounts(forceRefresh);

        //Assert
        userAccountService.Verify(x => x.GetUserAccounts(userId, email), Times.Once);
        
        if (forceRefresh)
        {
            claimsPrinciple.Claims.Should().Contain(c => c.Type.Equals(EmployerClaims.AccountsClaimsTypeIdentifier));
            
            var actualClaimValue = claimsPrinciple.Claims.First(c => c.Type.Equals(EmployerClaims.AccountsClaimsTypeIdentifier)).Value;
            var expectedClaimValue = JsonConvert.SerializeObject(updatedAccountData.EmployerAccounts.ToDictionary(x => x.AccountId));
            actualClaimValue.Should().Be(expectedClaimValue);
        }
      
        result.Should().BeEquivalentTo(updatedAccountData.EmployerAccounts.ToDictionary(x => x.AccountId));
    }

    [Test, MoqAutoData]
    public async Task Then_User_EmployerAccounts_Should_Be_Retrieved_From_UserService_When_Claims_Are_Populated_And_ForceRefresh_Is_True(
        string userId,
        string email,
        [Frozen] Mock<IHttpContextAccessor> httpContextAccessor,
        Mock<ILogger<AccountClaimsService>> logger,
        Mock<IGovAuthEmployerAccountService> userAccountService,
        Dictionary<string, EmployerUserAccountItem> existingAccountData,
        EmployerUserAccounts updatedAccountData
    )
    {
        //Arrange
        var claimsPrinciple = new ClaimsPrincipal([
            new ClaimsIdentity([
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim(EmployerClaims.AccountsClaimsTypeIdentifier, JsonConvert.SerializeObject(existingAccountData)),
            ])
        ]);

        var httpContext = new DefaultHttpContext(new FeatureCollection())
        {
            User = claimsPrinciple
        };

        httpContextAccessor.Setup(x => x.HttpContext).Returns(httpContext);
        userAccountService.Setup(x => x.GetUserAccounts(userId, email)).ReturnsAsync(updatedAccountData);

        var helper = new AccountClaimsService(userAccountService.Object, httpContextAccessor.Object, logger.Object)
        {
            MaxPermittedNumberOfAccountsOnClaim = existingAccountData.Count
        };

        //Act
        var result = await helper.GetAssociatedAccounts(forceRefresh: true);

        //Assert
        userAccountService.Verify(x => x.GetUserAccounts(userId, email), Times.Once);
        claimsPrinciple.Claims.Should().Contain(c => c.Type.Equals(EmployerClaims.AccountsClaimsTypeIdentifier));

        var actualClaimValue = claimsPrinciple.Claims.First(c => c.Type.Equals(EmployerClaims.AccountsClaimsTypeIdentifier)).Value;
        var expectedClaimValue = JsonConvert.SerializeObject(updatedAccountData.EmployerAccounts.ToDictionary(x => x.AccountId));
        actualClaimValue.Should().Be(expectedClaimValue);
        
        result.Should().BeEquivalentTo(updatedAccountData.EmployerAccounts.ToDictionary(x => x.AccountId));
    }

    [Test]
    [MoqInlineAutoData(true)]
    [MoqInlineAutoData(false)]
    public async Task Then_User_EmployerAccounts_Should_Be_Retrieved_From_AccountsService_And_Stored_When_Claim_Value_Is_Empty_And_Within_Max_Number_Of_Accounts(
        bool forceRefresh,
        string userId,
        string email,
        [Frozen] Mock<IHttpContextAccessor> httpContextAccessor,
        Mock<ILogger<AccountClaimsService>> logger,
        Mock<IGovAuthEmployerAccountService> userAccountService,
        EmployerUserAccounts accountData
    )
    {
        //Arrange
        var claimsPrinciple = new ClaimsPrincipal([
            new ClaimsIdentity([
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.NameIdentifier, userId)
            ])
        ]);

        var httpContext = new DefaultHttpContext(new FeatureCollection())
        {
            User = claimsPrinciple
        };

        httpContextAccessor.Setup(x => x.HttpContext).Returns(httpContext);
        userAccountService.Setup(x => x.GetUserAccounts(userId, email)).ReturnsAsync(accountData);

        var associatedAccountsService = new AccountClaimsService(userAccountService.Object, httpContextAccessor.Object, logger.Object)
        {
            MaxPermittedNumberOfAccountsOnClaim = accountData.EmployerAccounts.Count()
        };

        //Act
        var result = await associatedAccountsService.GetAssociatedAccounts(forceRefresh);

        //Assert
        userAccountService.Verify(x => x.GetUserAccounts(userId, email), Times.Once);
        if (forceRefresh)
        {
            claimsPrinciple.Claims.Should().Contain(c => c.Type.Equals(EmployerClaims.AccountsClaimsTypeIdentifier));
            
            var actualClaimValue = claimsPrinciple.Claims.First(c => c.Type.Equals(EmployerClaims.AccountsClaimsTypeIdentifier)).Value;
        
            var action = () => JsonConvert.DeserializeObject<Dictionary<string, EmployerUserAccountItem>>(actualClaimValue)
                .Select(x => x.Value)
                .ToList();
        
            action.Should().NotThrow();
        }
        else
        {
            claimsPrinciple.Claims.Should().NotContain(c => c.Type.Equals(EmployerClaims.AccountsClaimsTypeIdentifier));
        }
        
        result.Should().BeEquivalentTo(accountData.EmployerAccounts.ToDictionary(x=> x.AccountId));
    }

    [Test]
    [MoqInlineAutoData(true)]
    [MoqInlineAutoData(false)]
    public async Task Then_User_EmployerAccounts_Should_Be_Retrieved_From_AccountsService_When_Claim_Value_Is_Empty_And_Above_Max_Number_Of_Accounts(
        bool forceRefresh,
        string userId,
        string email,
        [Frozen] Mock<IHttpContextAccessor> httpContextAccessor,
        Mock<ILogger<AccountClaimsService>> logger,
        Mock<IGovAuthEmployerAccountService> userAccountService,
        EmployerUserAccounts accountData
    )
    {
        //Arrange
        var claimsPrinciple = new ClaimsPrincipal([
            new ClaimsIdentity([
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.NameIdentifier, userId)
            ])
        ]);

        var httpContext = new DefaultHttpContext(new FeatureCollection())
        {
            User = claimsPrinciple
        };

        httpContextAccessor.Setup(x => x.HttpContext).Returns(httpContext);
        userAccountService.Setup(x => x.GetUserAccounts(userId, email)).ReturnsAsync(accountData);

        var associatedAccountsService = new AccountClaimsService(userAccountService.Object, httpContextAccessor.Object, logger.Object)
        {
            MaxPermittedNumberOfAccountsOnClaim = accountData.EmployerAccounts.Count() - 1
        };

        //Act
        var result = await associatedAccountsService.GetAssociatedAccounts(forceRefresh);

        //Assert
        userAccountService.Verify(x => x.GetUserAccounts(userId, email), Times.Once);

        if (forceRefresh)
        {
            claimsPrinciple.Claims.Should().Contain(c => c.Type.Equals(EmployerClaims.AccountsClaimsTypeIdentifier));
            
            var actualClaimValue = claimsPrinciple.Claims.First(c => c.Type.Equals(EmployerClaims.AccountsClaimsTypeIdentifier)).Value;
        
            var action = () => JsonConvert.DeserializeObject<Dictionary<string, EmployerUserAccountItem>>(actualClaimValue)
                .Select(x => x.Value)
                .ToList();
        
            action.Should().NotThrow();
        }
        else
        {
            claimsPrinciple.Claims.Should().NotContain(c => c.Type.Equals(EmployerClaims.AccountsClaimsTypeIdentifier));
        }
        
        result.Should().BeEquivalentTo(accountData.EmployerAccounts.ToDictionary(x=> x.AccountId));
    }
}