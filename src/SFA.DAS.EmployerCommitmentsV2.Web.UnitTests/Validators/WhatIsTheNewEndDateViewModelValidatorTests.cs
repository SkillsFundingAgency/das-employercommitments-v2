﻿using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.Validators;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Validators;

public class WhatIsTheNewEndDateViewModelValidatorTests : ValidatorTestBase<WhatIsTheNewEndDateViewModel, WhatIsTheNewEndDateViewModelValidator>
{
    [TestCase("5143541", true)]
    [TestCase(" ", false)]
    [TestCase("", false)]
    [TestCase(null, false)]
    public void WhenValidatingWhatIsTheEndDate_ValidateTheAccountHashedId(string accountHashedId, bool expectedValid)
    {
        var viewModel = new WhatIsTheNewEndDateViewModel { AccountHashedId = accountHashedId };

        AssertValidationResult(x => x.AccountHashedId, viewModel, expectedValid);
    }

    [TestCase("5143541", true)]
    [TestCase(" ", false)]
    [TestCase("", false)]
    [TestCase(null, false)]
    public void WhenValidatingWhatIsTheEndDate_ValidateTheApprenticeshipHashedId(string apprenticeshipHashedId, bool expectedValid)
    {
        var viewModel = new WhatIsTheNewEndDateViewModel { ApprenticeshipHashedId = apprenticeshipHashedId };

        AssertValidationResult(x => x.ApprenticeshipHashedId, viewModel, expectedValid);
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
        var viewModel = new WhatIsTheNewEndDateViewModel { NewEndMonth = newEndDateMonth, NewEndYear = newEndDateYear };

        AssertValidationResult(x => x.NewEndDate, viewModel, expectedValid, errorMessage);
    }

    [TestCase(01, 2020, "2020-01-01", false, "The new planned training end date must be after January 2020")]
    [TestCase(01, 2020, "2020-02-01", false, "The new planned training end date must be after February 2020")]
    [TestCase(02, 2020, "2020-01-01", true, null)]
    [TestCase(02, 2021, "2020-02-01", true, null)]
    public void WhenValidatingWhatIsTheEndDate_ValidateTheNewEndDateIsAfterTheNewStartDate(int? newEndDateMonth, int? newEndDateYear, string startDateTime, bool expectedValid, string errorMessage)
    {
        var viewModel = new WhatIsTheNewEndDateViewModel { NewEndMonth = newEndDateMonth, NewEndYear = newEndDateYear, NewStartDate = DateTime.Parse(startDateTime) };

        AssertValidationResult(x => x.NewEndDate, viewModel, expectedValid, errorMessage);
    }
}