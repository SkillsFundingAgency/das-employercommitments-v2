using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerCommitmentsV2.Web.Extensions;
using SFA.DAS.EmployerUrlHelper;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Extensions
{
    [TestFixture]
    [Parallelizable]
    public class ILinkGeneratorExtensionsTests
    {
        private ILinkGeneratorExtensionsTestsFixture _fixture;
        [SetUp]
        public void Arrange()
        {
            _fixture = new ILinkGeneratorExtensionsTestsFixture();
        }

        [Test, AutoData]
        public void YourOrganisationsAndAgreements_BuildsPathCorrectly(string accountHashedId)
        {
            var url = _fixture.Sut.YourOrganisationsAndAgreements(accountHashedId);

            Assert.That(url, Is.EqualTo($"{_fixture.AccountsLink}accounts/{accountHashedId}/agreements"));
        }

        [Test, AutoData]
        public void PayeSchemes_BuildsPathCorrectly(string accountHashedId)
        {
            var url = _fixture.Sut.PayeSchemes(accountHashedId);

            Assert.That(url, Is.EqualTo($"{_fixture.AccountsLink}accounts/{accountHashedId}/schemes"));
        }

        [Test, AutoData]
        public void EmployerHome_BuildsPathCorrectly(string accountHashedId)
        {
            var url = _fixture.Sut.EmployerHome(accountHashedId);
            Assert.That(url, Is.EqualTo($"{_fixture.AccountsLink}accounts/{accountHashedId}/teams"));
        }

        public class ILinkGeneratorExtensionsTestsFixture
        {
            public Mock<ILinkGenerator> MockILinkGenerator;
            public ILinkGenerator Sut => MockILinkGenerator.Object;
            public string AccountsLink => "https://accounts.com/";
            public string UsersLink => "https://users.com/";

            public ILinkGeneratorExtensionsTestsFixture()
            {
                MockILinkGenerator = new Mock<ILinkGenerator>();
                MockILinkGenerator.Setup(x => x.AccountsLink(It.IsAny<string>())).Returns((string path) => AccountsLink + path);
                MockILinkGenerator.Setup(x => x.UsersLink(It.IsAny<string>())).Returns((string path) => UsersLink + path);
            }
        }
    }
}