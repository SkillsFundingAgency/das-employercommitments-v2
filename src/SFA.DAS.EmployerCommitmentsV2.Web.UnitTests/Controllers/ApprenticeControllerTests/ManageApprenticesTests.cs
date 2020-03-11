using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.ApprenticeControllerTests
{
    [TestFixture]
    public class ManageApprenticesTests
    {
        private ApprenticeController _controller;

        [SetUp]
        public void Setup()
        {
            _controller = new ApprenticeController();
        }

        [Test]
        public void AndIsCalledWithInvalidModelStateThenBadResponseShouldBeReturned()
        {
            //Arrange
            _controller.ModelState.AddModelError("test", "test");
            //Act
            var result = _controller.Index("");
            //Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public void ThenTheAccountHashedIdIsPassedToTheViewModel()
        {
            //Arrange
            var expectedAccountHashedId = "TEST";
            //Act
            var result = _controller.Index(expectedAccountHashedId);
            var view = ((ViewResult) result).Model as ManageApprenticesViewModel;
            //Assert
            Assert.AreEqual(expectedAccountHashedId, view.AccountHashedId);
        }
    }
}