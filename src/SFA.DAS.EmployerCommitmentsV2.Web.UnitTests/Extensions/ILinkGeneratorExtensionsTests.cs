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

            Assert.AreEqual($"{_fixture.AccountsLink}accounts/{accountHashedId}/agreements", url);
        }

        [Test, AutoData]
        public void PayeSchemes_BuildsPathCorrectly(string accountHashedId)
        {
            var url = _fixture.Sut.PayeSchemes(accountHashedId);

            Assert.AreEqual($"{_fixture.AccountsLink}accounts/{accountHashedId}/schemes", url);
        }

        [Test, AutoData]
        public void ViewApprentice_BuildsPathCorrectly(string accountHashedId, string cohortReference, string draftApprenticeshipHashedId)
        {
            var url = _fixture.Sut.ViewApprentice(accountHashedId, cohortReference, draftApprenticeshipHashedId);

            Assert.AreEqual($"{_fixture.CommitmentsLink}accounts/{accountHashedId}/apprentices/{cohortReference}/apprenticeships/{draftApprenticeshipHashedId}/view", url);
        }

        [Test, AutoData]
        public void DeleteApprentice_BuildsPathCorrectly(string accountHashedId, string cohortReference, string draftApprenticeshipHashedId)
        {
            var url = _fixture.Sut.DeleteApprentice(accountHashedId, cohortReference, draftApprenticeshipHashedId);

            Assert.AreEqual($"{_fixture.CommitmentsLink}accounts/{accountHashedId}/apprentices/{cohortReference}/apprenticeships/{draftApprenticeshipHashedId}/delete", url);
        }

        [Test, AutoData]
        public void EmployerHome_BuildsPathCorrectly(string accountHashedId)
        {
            var url = _fixture.Sut.EmployerHome(accountHashedId);
            Assert.AreEqual($"{_fixture.UsersLink}accounts/{accountHashedId}/teams", url);
        }

    }

    public class ILinkGeneratorExtensionsTestsFixture
    {
        public Mock<ILinkGenerator> MockILinkGenerator;
        public ILinkGenerator Sut => MockILinkGenerator.Object;
        public string AccountsLink => "https://accounts.com/";
        public string CommitmentsLink => "https://commitments.com/";
        public string UsersLink => "https://users.com/";

        public ILinkGeneratorExtensionsTestsFixture()
        {
            MockILinkGenerator = new Mock<ILinkGenerator>();
            MockILinkGenerator.Setup(x => x.AccountsLink(It.IsAny<string>())).Returns((string path) => AccountsLink + path);
            MockILinkGenerator.Setup(x => x.CommitmentsLink(It.IsAny<string>())).Returns((string path) => CommitmentsLink + path);
            MockILinkGenerator.Setup(x => x.UsersLink(It.IsAny<string>())).Returns((string path) => UsersLink + path);
        }
    }
}