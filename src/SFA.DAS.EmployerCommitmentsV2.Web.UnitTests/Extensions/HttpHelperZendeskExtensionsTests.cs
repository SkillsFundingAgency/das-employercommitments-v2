using Microsoft.AspNetCore.Mvc.Rendering;
using SFA.DAS.EmployerCommitmentsV2.Web.Extensions;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Extensions;

[TestFixture]
[Parallelizable]
public class HttpHelperZendeskExtensionsTests
{
    private HttpHelperZendeskExtensionsTestsFixture _fixture;
    [SetUp]
    public void Arrange()
    {
        _fixture = new HttpHelperZendeskExtensionsTestsFixture();
    }

    [Test]
    public void SetZendeskSuggestion_IsCreatedCorrectly()
    {
        const string suggestion = "iowoiwueoiwue";
        var htmlSnippet = _fixture.Sut.SetZendeskSuggestion(suggestion);

        HttpHelperZendeskExtensionsTestsFixture.ExpectedSuggestionJavaScriptSnippet(suggestion, htmlSnippet.ToString());
    }

    [Test]
    public void SetZendeskSuggestion_IsCreatedCorrectlyWithApostrophesEscaped()
    {
        const string suggestion = "'help's";
        var htmlSnippet = _fixture.Sut.SetZendeskSuggestion(suggestion);

        HttpHelperZendeskExtensionsTestsFixture.ExpectedSuggestionJavaScriptSnippet(@"\'help\'s", htmlSnippet.ToString());
    }

    [Test]
    public void SetZendeskLabels_IsCreatedCorrectlyForNoItems()
    {
        var labels = Array.Empty<string>();
        var htmlSnippet = _fixture.Sut.SetZendeskLabels(labels);

        HttpHelperZendeskExtensionsTestsFixture.ExpectedLabelJavaScriptSnippet("", htmlSnippet.ToString());
    }

    [Test]
    public void SetZendeskLabels_IsCreatedCorrectlyForOneItem()
    {
        var labels = new [] { "one" };
        var htmlSnippet = _fixture.Sut.SetZendeskLabels(labels);

        HttpHelperZendeskExtensionsTestsFixture.ExpectedLabelJavaScriptSnippet("'one'", htmlSnippet.ToString());
    }

    [Test]
    public void SetZendeskLabels_IsCreatedCorrectlyForTwoItems()
    {
        var labels = new[] { "one", "two" };
        var htmlSnippet = _fixture.Sut.SetZendeskLabels(labels);

        HttpHelperZendeskExtensionsTestsFixture.ExpectedLabelJavaScriptSnippet("'one','two'", htmlSnippet.ToString());
    }

    [Test]
    public void SetZendeskLabels_IsCreatedCorrectlyForOneItemWithApostrophesEscaped()
    {
        var labels = new[] { "one's" };
        var htmlSnippet = _fixture.Sut.SetZendeskLabels(labels);

        HttpHelperZendeskExtensionsTestsFixture.ExpectedLabelJavaScriptSnippet(@"'one\'s'", htmlSnippet.ToString());
    }
}

public class HttpHelperZendeskExtensionsTestsFixture
{
    public Mock<IHtmlHelper> MockHtmlHelper;
    public IHtmlHelper Sut => MockHtmlHelper.Object;

    private const string StartLabelSnipet = "<script type=\"text/javascript\">zE('webWidget', 'helpCenter:setSuggestions', { labels: [";
    private const string EndLabelSnipet = "] });</script>";

    public HttpHelperZendeskExtensionsTestsFixture()
    {
        MockHtmlHelper = new Mock<IHtmlHelper>();
    }

    public static void ExpectedSuggestionJavaScriptSnippet(string suggestion, string snippet)
    {
        Assert.That(snippet, Is.EqualTo($"<script type=\"text/javascript\">zE('webWidget', 'helpCenter:setSuggestions', {{ search: '{suggestion}' }});</script>"));
    }

    public static void ExpectedLabelJavaScriptSnippet(string labels, string snippet)
    {
        var expected = StartLabelSnipet + labels + EndLabelSnipet;
        Assert.That(snippet, Is.EqualTo(expected));
    }
}