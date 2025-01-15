using FluentAssertions;
using Microsoft.AspNetCore.Http;
using SFA.DAS.EmployerCommitmentsV2.Exceptions;
using SFA.DAS.EmployerCommitmentsV2.Web.Middleware;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Middleware;

public class CacheItemNotFoundMiddlewareTests
{
    private CacheItemNotFoundMiddleware _middleware;
    private Mock<RequestDelegate> _nextDelegate;
    private DefaultHttpContext _httpContext;

    [SetUp]
    public void SetUp()
    {
        _nextDelegate = new Mock<RequestDelegate>();
        _middleware = new CacheItemNotFoundMiddleware(_nextDelegate.Object, Mock.Of<ILogger<CacheItemNotFoundMiddleware>>());
        _httpContext = new DefaultHttpContext();
    }

    [Test]
    public async Task InvokeAsync_WhenExceptionThrown_ShouldRedirectToErrorPage()
    {
        // Arrange
        _nextDelegate.Setup(next => next(It.IsAny<HttpContext>()))
            .ThrowsAsync(new CacheItemNotFoundException<AddApprenticeshipCacheModel>());

        // Act
        await _middleware.InvokeAsync(_httpContext);

        // Assert
        _httpContext.Response.Headers.Location.First().Should().BeSameAs("/error/404");
    }

    [Test]
    public async Task InvokeAsync_WhenNoExceptionThrown_ShouldCallNextDelegate()
    {
        // Arrange
        _nextDelegate.Setup(next => next(It.IsAny<HttpContext>()))
            .Returns(Task.CompletedTask);

        // Act
        await _middleware.InvokeAsync(_httpContext);

        // Assert
        _nextDelegate.Verify(next => next(It.IsAny<HttpContext>()), Times.Once);
    }
}
