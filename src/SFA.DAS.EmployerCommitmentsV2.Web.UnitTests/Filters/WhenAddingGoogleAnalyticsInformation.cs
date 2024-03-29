﻿using Microsoft.AspNetCore.Mvc.Filters;
using SFA.DAS.EmployerCommitmentsV2.Web.Authentication;
using SFA.DAS.EmployerCommitmentsV2.Web.Filters;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Shared;
using SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Customisations;
using SFA.DAS.Testing.AutoFixture;
using System.Security.Claims;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Filters;

[TestFixture]
public class WhenAddingGoogleAnalyticsInformation
{
    [Test, DomainAutoData]
    public async Task Then_If_Employer_Adds_The_AccountId_And_UserId_To_The_ViewBag_Data(
        Guid userId,
        long accountId,
        [ArrangeActionContext] ActionExecutingContext context,
        [Frozen] Mock<ActionExecutionDelegate> nextMethod,
        GoogleAnalyticsFilter filter)
    {
        //Arrange
        var claim = new Claim(EmployeeClaims.IdamsUserIdClaimTypeIdentifier, userId.ToString());
        context.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new[] { claim }));
        context.RouteData.Values.Add("AccountHashedId", accountId);

        //Act
        await filter.OnActionExecutionAsync(context, nextMethod.Object);

        //Assert
        var actualController = context.Controller as Controller;
        
        Assert.That(actualController, Is.Not.Null);
        
        var viewBagData = actualController.ViewBag.GaData as GaData;
        
        Assert.That(viewBagData, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(viewBagData.Acc, Is.EqualTo(accountId.ToString()));
            Assert.That(viewBagData.UserId, Is.EqualTo(userId.ToString()));
        });
    }

    [Test, DomainAutoData]
    public async Task And_Context_Is_Non_Controller_Then_No_Data_Is_Added_To_ViewBag(
        Guid userId,
        long accountId,
        [ArrangeActionContext] ActionExecutingContext context,
        [Frozen] Mock<ActionExecutionDelegate> nextMethod,
        GoogleAnalyticsFilter filter)
    {
        //Arrange
        var claim = new Claim(EmployeeClaims.IdamsUserIdClaimTypeIdentifier, userId.ToString());
        context.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new[] { claim }));
        context.RouteData.Values.Add("employerAccountId", accountId);

        var contextWithoutController = new ActionExecutingContext(
            new ActionContext(context.HttpContext, context.RouteData, context.ActionDescriptor),
            context.Filters,
            context.ActionArguments,
            "");

        //Act
        await filter.OnActionExecutionAsync(contextWithoutController, nextMethod.Object);

        //Assert
        Assert.DoesNotThrowAsync(() => filter.OnActionExecutionAsync(contextWithoutController, nextMethod.Object));
    }
}