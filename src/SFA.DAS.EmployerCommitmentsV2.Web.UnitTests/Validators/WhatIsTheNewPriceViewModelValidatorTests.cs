using FluentValidation.TestHelper;
using NUnit.Framework;
using SFA.DAS.CommitmentsV2.Shared.Models;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.Validators;
using System;
using System.Linq.Expressions;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Validators
{
    public class WhatIsTheNewPriceViewModelValidatorTests
    {
        [TestCase(0, false)]
        [TestCase(1, true)]
        public void ThenProviderIdIsValidated(long providerId, bool expectedValid)
        {
            var request = new WhatIsTheNewPriceViewModel { ProviderId = providerId };

            AssertValidationResult(x => x.ProviderId, request, expectedValid);
        }

        [TestCase(null, false)]
        [TestCase("", false)]
        [TestCase(" ", false)]
        [TestCase("XYZ", true)]
        public void Validate_EmployerAccountLegalEntityPublicHashedId_ShouldBeValidated(string accountHashedId, bool expectedValid)
        {
            var model = new WhatIsTheNewPriceViewModel { AccountHashedId = accountHashedId };

            AssertValidationResult(request => request.AccountHashedId, model, expectedValid);
        }

        [TestCase("", false)]
        [TestCase(" ", false)]
        [TestCase("AB76V", true)]
        public void Validate_ApprenticeshipHashedId_ShouldBeValidated(string apprenticeshipHashedId, bool expectedValid)
        {
            var model = new WhatIsTheNewPriceViewModel { ApprenticeshipHashedId = apprenticeshipHashedId };
            AssertValidationResult(request => request.ApprenticeshipHashedId, model, expectedValid);
        }

        //TODO : Test
        //[TestCase("", false)]
        //[TestCase(" ", false)]
        ////[TestCase("XXXXXXX", false)]
        ////[TestCase("1220", false)]
        //[TestCase("122021", true)]
        //[TestCase("012021", true)]
        //public void Validate_StartDate_ShouldBeValidated(string startDate, bool expectedValid)
        //{
        //    var model = new WhatIsTheNewPriceViewModel { NewStartDate = new MonthYearModel(startDate) };
        //    AssertValidationResult(request => request.NewStartDate, model, expectedValid);
        //}

        [TestCase(null, false)]
        [TestCase(0, false)]
        [TestCase(1, true)]
        [TestCase(100000, true)]
        [TestCase(100001, false)]
        public void Validate_Price_ShouldBeValidated(int? price, bool expectedValid)
        {
            var model = new WhatIsTheNewPriceViewModel { NewPrice = price };
            AssertValidationResult(request => request.NewPrice, model, expectedValid);
        }

        private void AssertValidationResult<T>(Expression<Func<WhatIsTheNewPriceViewModel, T>> property, WhatIsTheNewPriceViewModel instance, bool expectedValid)
        {
            var validator = new WhatIsTheNewPriceViewModelValidator();

            if (expectedValid)
            {
                validator.ShouldNotHaveValidationErrorFor(property, instance);
            }
            else
            {
                validator.ShouldHaveValidationErrorFor(property, instance);
            }
        }
    }
}
