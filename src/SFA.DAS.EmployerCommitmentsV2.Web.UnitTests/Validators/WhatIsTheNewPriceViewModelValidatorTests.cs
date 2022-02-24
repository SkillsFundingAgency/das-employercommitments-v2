using AutoFixture;
using NUnit.Framework;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.Validators;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Validators
{
    public class WhatIsTheNewPriceViewModelValidatorTests : ValidatorTestBase<WhatIsTheNewPriceViewModel, WhatIsTheNewPriceViewModelValidator>
    {
        private IFixture _autoFixture;

        [SetUp]
        public void Arrange()
        {
            _autoFixture = new Fixture();
            _autoFixture.Customize<WhatIsTheNewPriceViewModel>(c =>
                c.With(m => m.NewPrice, 1500)
                    .With(m => m.NewStartMonthYear, "022020")
                    .With(m => m.NewEndMonthYear, "092020")
                    .With(m => m.NewStartMonth, 1)
                    .With(m => m.NewStartYear, 2020)
                    .With(m => m.NewEndMonth, 9)
                    .With(m => m.NewEndYear, 2020));
        }

        [TestCase("5143541", true)]
        [TestCase(" ", false)]
        [TestCase("", false)]
        [TestCase(null, false)]
        public void WhenValidatingWhatIsThePrice_ValidateTheAccountHashedId(string accountHashedId, bool expectedValid)
        {
            //Arrange
            var viewModel = _autoFixture.Create<WhatIsTheNewPriceViewModel>();
            viewModel.AccountHashedId = accountHashedId;

            //Assert
            AssertValidationResult(x => x.AccountHashedId, viewModel, expectedValid);
        }

        [TestCase("5143541", true)]
        [TestCase(" ", false)]
        [TestCase("", false)]
        [TestCase(null, false)]
        public void WhenValidatingWhatIsThePrice_ValidateTheApprenticeshipHashedId(string apprenticeshipHashedId, bool expectedValid)
        {
            //Arrange
            var viewModel = _autoFixture.Create<WhatIsTheNewPriceViewModel>();
            viewModel.ApprenticeshipHashedId = apprenticeshipHashedId;

            //Assert
            AssertValidationResult(x => x.ApprenticeshipHashedId, viewModel, expectedValid);
        }


        [TestCase(null, false, "Enter the agreed price of completing the training with the new provider")]
        [TestCase(0, false, "The agreed price of completing the training with the new provider must be more than 0")]
        [TestCase(100050, false, "The agreed price of completing the training with the new provider must be less than 100000")]
        [TestCase(1500, true, null)]
        public void WhenValidatingWhatIsThePrice_ValidateTheNewPrice(int? newPrice, bool expectedValid, string errorMessage)
        {
            //Arrange
            var viewModel = _autoFixture.Create<WhatIsTheNewPriceViewModel>();
            viewModel.NewPrice = newPrice;

            //Assert
            AssertValidationResult(x => x.NewPrice, viewModel, expectedValid, errorMessage);
        }

        [TestCase("012021", "062021", true, null)]
        [TestCase("132021", "062021", false, "The specified condition was not met for 'New Start Month Year'.")]
        public void WhenValidatingWhatIsThePrice_ValidateTheNewStartMonthYear(string newStartMonthYear, string newEndMonthYear, bool expectedValid, string errorMessage)
        {
            //Arrange
            var viewModel = _autoFixture.Create<WhatIsTheNewPriceViewModel>();
            viewModel.NewStartMonthYear = newStartMonthYear;
            viewModel.NewEndMonthYear = newEndMonthYear;

            //Assert
            AssertValidationResult(x => x.NewStartMonthYear, viewModel, expectedValid, errorMessage);
        }

        [TestCase("012021", "062021", true, null)]
        [TestCase("012021", "162021", false, "The specified condition was not met for 'New End Month Year'.")]
        public void WhenValidatingWhatIsThePrice_ValidateTheNewEndMonthYear(string newStartMonthYear, string newEndMonthYear, bool expectedValid, string errorMessage)
        {
            //Arrange
            var viewModel = _autoFixture.Create<WhatIsTheNewPriceViewModel>();
            viewModel.NewStartMonthYear = newStartMonthYear;
            viewModel.NewEndMonthYear = newEndMonthYear;

            //Assert
            AssertValidationResult(x => x.NewEndMonthYear, viewModel, expectedValid, errorMessage);
        }

        [Test]
        public void WhenValidatingWhatIsThePrice_ValidateTheNewEndMonthYearIsAfterTheNewStartMonthYear()
        {
            //Arrange
            const string errorMessage = "The new planned training end date must be after  new planned start date";
            var viewModel = _autoFixture.Create<WhatIsTheNewPriceViewModel>();
            viewModel.NewStartMonth = 1;
            viewModel.NewStartYear = 2021;
            viewModel.NewEndMonth = 6;
            viewModel.NewEndYear = 2020;

            //Act
            AssertValidationResult(x => x.NewEndYear, viewModel, false, errorMessage);
        }

    }
}
