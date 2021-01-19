using System;
using System.Linq;
using NUnit.Framework;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.Validators;
using AutoFixture;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Validators
{
    public class WhatIsTheNewEndDateViewModelValidatorTests
    {
        private WhatIsTheNewEndDateViewModelValidator _validator;
        private Fixture _autoFixture;

        [SetUp]
        public void Arrange()
        {
            _autoFixture = new Fixture();
            _autoFixture.Customize<WhatIsTheNewEndDateViewModel>(c =>
                c.With(m => m.NewEndMonth, 01)
                    .With(m => m.NewEndYear, 2020)
                    .With(m => m.NewStartDate, DateTime.MinValue));

            _validator = new WhatIsTheNewEndDateViewModelValidator();
        }

        [TestCase("5143541", true)]
        [TestCase(" ", false)]
        [TestCase("", false)]
        [TestCase(null, false)]
        public void WhenValidatingWhatIsTheEndDate_ValidateTheAccountHashedId(string accountHashedId, bool expectedValid)
        {
            var viewModel = _autoFixture.Create<WhatIsTheNewEndDateViewModel>();
            viewModel.AccountHashedId = accountHashedId;

            var result = _validator.Validate(viewModel);
            Assert.AreEqual(expectedValid, result.IsValid);
        }

        [TestCase("5143541", true)]
        [TestCase(" ", false)]
        [TestCase("", false)]
        [TestCase(null, false)]
        public void WhenValidatingWhatIsTheEndDate_ValidateTheApprenticeshipHashedId(string apprenticeshipHashedId, bool expectedValid)
        {
            var viewModel = _autoFixture.Create<WhatIsTheNewEndDateViewModel>();
            viewModel.ApprenticeshipHashedId = apprenticeshipHashedId;

            var result = _validator.Validate(viewModel);
            Assert.AreEqual(expectedValid, result.IsValid);
        }

        [TestCase(null, null, false, "Enter the new planned training end date with the new training provider")]
        [TestCase(null, 2020, false, "The new planned training end date must include a month")]
        [TestCase(null, 2020, false, "The new planned training end date must include a month")]
        [TestCase(01, null, false, "The new planned training end date must include a year")]
        [TestCase(01, null, false, "The new planned training end date must include a year")]
        [TestCase(01, 2020, true, null)]
        [TestCase(13, 999, false, "The new planned training end date must be a real date")]
        public void WhenValidatingWhatIsTheEndDate_ValidateTheNewEndDate(int? newEndDateMonth, int? newEndDateYear, bool expectedValid, string errorMessage)
        {
            var viewModel = _autoFixture.Create<WhatIsTheNewEndDateViewModel>();
            viewModel.NewEndMonth = newEndDateMonth;
            viewModel.NewEndYear = newEndDateYear;

            var result = _validator.Validate(viewModel);
            Assert.AreEqual(expectedValid, result.IsValid);

            if(errorMessage != null) Assert.AreEqual(errorMessage, result.Errors.Single().ErrorMessage);
        }

        [TestCase(01, 2020, "2020-01-01", false, "The new planned training end date must be after January 2020")]
        [TestCase(01, 2020, "2020-02-01", false, "The new planned training end date must be after February 2020")]
        [TestCase(02, 2020, "2020-01-01", true, null)]
        [TestCase(02, 2021, "2020-02-01", true, null)]
        public void WhenValidatingWhatIsTheEndDate_ValidateTheNewEndDateIsAfterTheNewStartDate(int? newEndDateMonth, int? newEndDateYear, string startDateTime, bool expectedValid, string errorMessage)
        {
            var viewModel = _autoFixture.Create<WhatIsTheNewEndDateViewModel>();
            viewModel.NewEndMonth = newEndDateMonth;
            viewModel.NewEndYear = newEndDateYear;
            viewModel.NewStartDate = DateTime.Parse(startDateTime);

            var result = _validator.Validate(viewModel);
            Assert.AreEqual(expectedValid, result.IsValid);

            if (errorMessage != null) Assert.AreEqual(errorMessage, result.Errors.Single().ErrorMessage);
        }
    }
}
