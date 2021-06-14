using System.Linq;
using NUnit.Framework;
using SFA.DAS.EmployerCommitmentsV2.Web.Validators;
using AutoFixture;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Validators
{
    public class SelectTransferConnectionViewModelValidatorTests
    {
        private SelectTransferConnectionViewModelValidator _validator;
        private Fixture _autoFixture;

        [SetUp]
        public void Arrange()
        {
            _autoFixture = new Fixture();
            _autoFixture.Customize<SelectTransferConnectionViewModel>(c =>
                c.With(m => m.AccountHashedId, "VNR9P")
                 .With(m => m.TransferConnectionCode, "1234"));                  

            _validator = new SelectTransferConnectionViewModelValidator();
        }

        [TestCase("5143541", true)]
        [TestCase(" ", false)]
        [TestCase("", false)]
        [TestCase(null, false)]
        public void WhenValidatingSelectTransferConnection_ValidateTheAccountHashedId(string accountHashedId, bool expectedValid)
        {
            //Arrange
            var viewModel = _autoFixture.Create<SelectTransferConnectionViewModel>();
            viewModel.AccountHashedId = accountHashedId;

            //Act
            var result = _validator.Validate(viewModel);

            //Assert
            Assert.AreEqual(expectedValid, result.IsValid);
        }
        
        [TestCase(" ", false, "Please choose an option")]
        [TestCase("", false, "Please choose an option")]        
        public void WhenValidatingSelectTransferConnection_ValidateTransferConnectionCode(string transferConnectionCode, bool expectedValid, string errorMessage)
        {
            //Arrange
            var viewModel = _autoFixture.Create<SelectTransferConnectionViewModel>();
            viewModel.TransferConnectionCode = transferConnectionCode;

            //Act
            var result = _validator.Validate(viewModel);

            //Assert
            Assert.AreEqual(expectedValid, result.IsValid);
            if (errorMessage != null) Assert.IsTrue(result.Errors.Any(e => e.ErrorMessage == errorMessage));
        }

    }
}
