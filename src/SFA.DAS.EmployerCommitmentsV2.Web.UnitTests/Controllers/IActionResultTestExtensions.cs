using System.Net;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests
{
    internal static class IActionResultTestExtensions
    {

        public static ViewResult VerifyReturnsViewModel(this IActionResult result)
        {
            return result.VerifyResponseObjectType<ViewResult>();
        }

        public static RedirectResult VerifyReturnsRedirect(this IActionResult result)
        {
            return result.VerifyResponseObjectType<RedirectResult>();
        }

        public static RedirectToActionResult VerifyReturnsRedirectToActionResult(this IActionResult result)
        {
            return result.VerifyResponseObjectType<RedirectToActionResult>();
        }


        public static IActionResult VerifyReturnsBadRequest(this IActionResult result)
        {
            var badRequest = result.VerifyResponseObjectType<BadRequestResult>();

            result.VerifyReturnsSpecifiedStatusCode(HttpStatusCode.BadRequest);

            return badRequest;
        }

        public static ObjectResult VerifyReturnsSpecifiedStatusCode(this IActionResult result, HttpStatusCode expectedStatusCode)
        {
            var objectResult = result
                .VerifyResponseObjectType<ObjectResult>();

            Assert.AreEqual(expectedStatusCode, objectResult.StatusCode);

            return objectResult;
        }

        public static TExpectedResponseType VerifyResponseObjectType<TExpectedResponseType>(this IActionResult result) where TExpectedResponseType : IActionResult
        {
            Assert.IsTrue(result is TExpectedResponseType, $"Expected response type {typeof(TExpectedResponseType)} but got {result.GetType()}");
            return (TExpectedResponseType) result;
        }

        public static RedirectResult WithUrl(this RedirectResult result, string expectedUrl)
        {
            Assert.AreEqual(expectedUrl, result.Url);
            return result;
        }

        public static TExpectedModel WithModel<TExpectedModel>(this ViewResult result) where TExpectedModel : class
        {
            Assert.IsInstanceOf<TExpectedModel>(result.Model);
            return result.Model as TExpectedModel;
        }
    }
}