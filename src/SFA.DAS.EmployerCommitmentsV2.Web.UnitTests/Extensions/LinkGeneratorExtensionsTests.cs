using SFA.DAS.EmployerCommitmentsV2.Web.Extensions;
using SFA.DAS.EmployerUrlHelper;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Extensions;

[TestFixture]
[Parallelizable]
public class LinkGeneratorExtensionsTests
{
    private LinkGeneratorExtensionsTestsFixture _fixture;
    [SetUp]
    public void Arrange()
    {
        _fixture = new LinkGeneratorExtensionsTestsFixture();
    }

    [Test, AutoData]
    public void YourOrganisationsAndAgreements_BuildsPathCorrectly(string accountHashedId)
    {
        var url = _fixture.Sut.YourOrganisationsAndAgreements(accountHashedId);

        Assert.That(url, Is.EqualTo($"{LinkGeneratorExtensionsTestsFixture.AccountsLink}accounts/{accountHashedId}/agreements"));
    }

    [Test, AutoData]
    public void PayeSchemes_BuildsPathCorrectly(string accountHashedId)
    {
        var url = _fixture.Sut.PayeSchemes(accountHashedId);

        Assert.That(url, Is.EqualTo($"{LinkGeneratorExtensionsTestsFixture.AccountsLink}accounts/{accountHashedId}/schemes"));
    }

    [Test, AutoData]
    public void EmployerHome_BuildsPathCorrectly(string accountHashedId)
    {
        var url = _fixture.Sut.EmployerHome(accountHashedId);
        Assert.That(url, Is.EqualTo($"{LinkGeneratorExtensionsTestsFixture.AccountsLink}accounts/{accountHashedId}/teams"));
    }

    public class LinkGeneratorExtensionsTestsFixture
    {
        public Mock<ILinkGenerator> MockILinkGenerator;
        public ILinkGenerator Sut => MockILinkGenerator.Object;
        public static string AccountsLink => "https://accounts.com/";
        public static string UsersLink => "https://users.com/";

        public LinkGeneratorExtensionsTestsFixture()
        {
            MockILinkGenerator = new Mock<ILinkGenerator>();
            MockILinkGenerator.Setup(x => x.AccountsLink(It.IsAny<string>())).Returns((string path) => AccountsLink + path);
            MockILinkGenerator.Setup(x => x.UsersLink(It.IsAny<string>())).Returns((string path) => UsersLink + path);
        }
    }
}