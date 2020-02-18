using AutoFixture.NUnit3;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerCommitmentsV2.Web.Authentication;
using SFA.DAS.EmployerCommitmentsV2.Web.Filters;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Shared;
using SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Customisations;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Filters
{
    [TestFixture]
    public class WhenAddingGoogleAnalyticsInformation
    {
        [Test, MoqAutoData]
        public async Task Then_If_Employer_Adds_The_AccountId_And_UserId_To_The_ViewBag_Data(
            Guid userId,
            long accountId,
            [ArrangeActionContext] ActionExecutingContext context,
            [Frozen] Mock<ActionExecutionDelegate> nextMethod,
            GoogleAnalyticsFilter filter)
        {
            //Arrange
            var claim = new Claim(EmployeeClaims.Id, userId.ToString());
            context.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new[] { claim }));
            context.RouteData.Values.Add("AccountHashedId", accountId);

            //Act
            await filter.OnActionExecutionAsync(context, nextMethod.Object);

            //Assert
            var actualController = context.Controller as Controller;
            Assert.IsNotNull(actualController);
            var viewBagData = actualController.ViewBag.GaData as GaData;
            Assert.IsNotNull(viewBagData);
            Assert.AreEqual(accountId.ToString(), viewBagData.Acc);
            Assert.AreEqual(userId.ToString(), viewBagData.UserId);
        }

        [Test, MoqAutoData]
        public async Task And_Context_Is_Non_Controller_Then_No_Data_Is_Added_To_ViewBag(
            Guid userId,
            long accountId,
            [ArrangeActionContext] ActionExecutingContext context,
            [Frozen] Mock<ActionExecutionDelegate> nextMethod,
            GoogleAnalyticsFilter filter)
        {
            //Arrange
            var claim = new Claim(EmployeeClaims.Id, userId.ToString());
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
}