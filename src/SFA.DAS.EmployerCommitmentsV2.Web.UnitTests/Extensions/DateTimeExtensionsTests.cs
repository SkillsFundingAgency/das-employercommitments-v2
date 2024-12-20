using SFA.DAS.EmployerCommitmentsV2.Web.Extensions;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Extensions
{
    [TestFixture]
    public class DateTimeExtensionsTests
    {
        [Test]
        public void TheGdsHumanisedDateIsPopulated()
        {
            var currentDate = DateTime.Now;

            var gdsHumanisedDate = currentDate.ToGdsHumanisedDate();

            Assert.That(gdsHumanisedDate, Is.Not.Empty);
        }

        [Test]
        public void TheGdsHumanisedDateIsPopulatedWithSt()
        {
            var currentDate = new DateTime(2024, 1, 1);

            var gdsHumanisedDate = currentDate.ToGdsHumanisedDate();

            Assert.That(gdsHumanisedDate, Is.EqualTo("1st January 2024"));
        }

        [Test]
        public void TheGdsHumanisedDateIsPopulatedWithNd()
        {
            var currentDate = new DateTime(2024, 1, 22);

            var gdsHumanisedDate = currentDate.ToGdsHumanisedDate();

            Assert.That(gdsHumanisedDate, Is.EqualTo("22nd January 2024"));
        }

        [Test]
        public void TheGdsHumanisedDateIsPopulatedWithrd()
        {
            var currentDate = new DateTime(2024, 1, 3);

            var gdsHumanisedDate = currentDate.ToGdsHumanisedDate();

            Assert.That(gdsHumanisedDate, Is.EqualTo("3rd January 2024"));
        }
    }
}