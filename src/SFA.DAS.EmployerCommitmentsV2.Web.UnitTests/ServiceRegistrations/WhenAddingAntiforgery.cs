using FluentAssertions;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;
using SFA.DAS.EmployerCommitmentsV2.Web.ServiceRegistrations;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.ServiceRegistrations;

public class WhenAddingAntiforgery
{
    private ServiceProvider _serviceProvider;
    private IAntiforgery _antiforgery;
    private AntiforgeryOptions _options;

    [SetUp]
    public void SetUp()
    {
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddDataProtection().UseEphemeralDataProtectionProvider();
        services.AddDasMvc();

        _serviceProvider = services.BuildServiceProvider();
        _antiforgery = _serviceProvider.GetRequiredService<IAntiforgery>();
        _options = _serviceProvider.GetRequiredService<IOptions<AntiforgeryOptions>>().Value;
    }

    [TearDown]
    public void TearDown()
    {
        _serviceProvider.Dispose();
    }

    [Test]
    public void Then_The_Antiforgery_Cookie_Is_Secure_HttpOnly_And_Essential()
    {
        _options.Cookie.SecurePolicy.Should().Be(CookieSecurePolicy.Always);
        _options.Cookie.HttpOnly.Should().BeTrue();
        _options.Cookie.IsEssential.Should().BeTrue();
        _options.Cookie.SameSite.Should().Be(Microsoft.AspNetCore.Http.SameSiteMode.Strict);
    }

    [Test]
    public void Then_The_Response_Cookie_Includes_Secure_And_Preserves_Existing_Flags()
    {
        var context = CreateHttpsContext();

        _antiforgery.GetAndStoreTokens(context);

        var cookie = SetCookieHeaderValue.Parse(context.Response.Headers.SetCookie.Single());
        cookie.Name.Value.Should().StartWith(".AspNetCore.Antiforgery.");
        cookie.Secure.Should().BeTrue();
        cookie.HttpOnly.Should().BeTrue();
        cookie.SameSite.Should().Be(Microsoft.Net.Http.Headers.SameSiteMode.Strict);
        cookie.Path.Value.Should().Be("/");
    }

    [TestCase(true)]
    [TestCase(false)]
    public async Task Then_A_Post_With_Matching_Cookie_And_Request_Token_Is_Accepted(bool useFormToken)
    {
        var context = CreateHttpsContext();
        var tokens = _antiforgery.GetAndStoreTokens(context);
        var postContext = CreatePostContext(tokens);
        if (useFormToken)
        {
            postContext.Request.ContentType = "application/x-www-form-urlencoded";
            postContext.Request.Form = new FormCollection(new Dictionary<string, StringValues>
            {
                { _options.FormFieldName, tokens.RequestToken }
            });
        }
        else
        {
            postContext.Request.Headers[_options.HeaderName] = tokens.RequestToken;
        }

        var result = await ValidatePost(postContext);

        result.Should().BeNull();
    }

    [TestCase(null)]
    [TestCase("invalid-token")]
    public async Task Then_A_Post_Without_A_Valid_Request_Token_Is_Rejected(string requestToken)
    {
        var context = CreateHttpsContext();
        var tokens = _antiforgery.GetAndStoreTokens(context);
        var postContext = CreatePostContext(tokens);
        if (requestToken != null)
        {
            postContext.Request.Headers[_options.HeaderName] = requestToken;
        }

        var result = await ValidatePost(postContext);

        result.Should().BeOfType<AntiforgeryValidationFailedResult>();
        ((AntiforgeryValidationFailedResult)result).StatusCode.Should().Be(StatusCodes.Status400BadRequest);
    }

    [Test]
    public void Then_An_Antiforgery_Token_Cannot_Be_Issued_Over_Http()
    {
        var context = CreateHttpsContext();
        context.Request.Scheme = "http";

        Action action = () => _antiforgery.GetAndStoreTokens(context);

        action.Should().Throw<InvalidOperationException>();
    }

    private DefaultHttpContext CreateHttpsContext()
    {
        var context = new DefaultHttpContext { RequestServices = _serviceProvider };
        context.Request.Scheme = "https";
        return context;
    }

    private DefaultHttpContext CreatePostContext(AntiforgeryTokenSet tokens)
    {
        var context = CreateHttpsContext();
        context.Request.Method = HttpMethods.Post;
        context.Request.Headers.Cookie = $"{_options.Cookie.Name}={tokens.CookieToken}";
        return context;
    }

    private async Task<IActionResult> ValidatePost(HttpContext context)
    {
        var mvcOptions = _serviceProvider.GetRequiredService<IOptions<MvcOptions>>().Value;
        var attribute = mvcOptions.Filters.OfType<AutoValidateAntiforgeryTokenAttribute>().Single();
        var filter = (IAsyncAuthorizationFilter)attribute.CreateInstance(_serviceProvider);
        var authorizationContext = new AuthorizationFilterContext(
            new ActionContext(context, new RouteData(), new ActionDescriptor()),
            new List<IFilterMetadata> { filter });

        await filter.OnAuthorizationAsync(authorizationContext);

        return authorizationContext.Result;
    }
}
