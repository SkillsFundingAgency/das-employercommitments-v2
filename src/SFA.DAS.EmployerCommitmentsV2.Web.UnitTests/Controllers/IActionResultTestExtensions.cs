using System.Net;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests
{
    internal static class IActionResultTestExtensions
    {

        public static ViewResult VerifyReturnsViewModel(this IActionResult result)
        {
            return result.VerifyResponseObjectType<ViewResult>();
        }

        public static RedirectToActionResult VerifyReturnsRedirect(this IActionResult result)
        {
            return result.VerifyResponseObjectType<RedirectToActionResult>();
        }

        public static RedirectToActionResult VerifyReturnsRedirectToActionResult(this IActionResult result)
        {
            return result.VerifyResponseObjectType<RedirectToActionResult>();
        }

        public static RedirectToActionResult WithActionName(this RedirectToActionResult result, string expectedName)
        {
            Assert.AreEqual(expectedName, result.ActionName);
            return result;
        }

        public static IActionResult VerifyReturnsBadRequest(this IActionResult result)
        {
            var badRequest = result.VerifyResponseObjectType<BadRequestResult>();

            result.VerifyReturnsSpecifiedStatusCode(HttpStatusCode.BadRequest);

            return badRequest;
        }

        public static IActionResult VerifyReturnsBadRequestObject(this IActionResult result)
        {
            var badRequest = result.VerifyResponseObjectType<BadRequestObjectResult>();

            result.VerifyReturnsSpecifiedStatusCode(HttpStatusCode.BadRequest);

            return badRequest;
        }


        public static ObjectResult VerifyReturnsSpecifiedStatusCode(this IActionResult result, HttpStatusCode expectedStatusCode)
        {
            var objectResult = result
                .VerifyResponseObjectType<ObjectResult>();

            Assert.AreEqual((int?)expectedStatusCode, objectResult.StatusCode);

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