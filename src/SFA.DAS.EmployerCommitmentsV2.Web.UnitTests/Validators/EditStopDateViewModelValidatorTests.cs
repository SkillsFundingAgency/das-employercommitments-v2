using System;
using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.Validators;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Validators;

public class EditStopDateViewModelValidatorTests : ValidatorTestBase<EditStopDateViewModel, EditStopDateViewModelValidator>
{
    private Fixture _autoFixture;
    private Mock<ICurrentDateTime> _currentDateTime;

    [SetUp]
    public void Arrange()
    {
        _autoFixture = new Fixture();
        _autoFixture.Customize<EditStopDateViewModel>(c =>
            c.With(m => m.ApprenticeshipStartDate, new DateTime(2020, 9, 1))
                .With(m => m.CurrentStopDate, new DateTime(2021, 2, 1))                
                .With(m => m.NewStopDate, new CommitmentsV2.Shared.Models.MonthYearModel("032021")));
        _currentDateTime = new Mock<ICurrentDateTime>();
        _currentDateTime.Setup(x => x.UtcNow).Returns(new DateTime(2021, 3 ,1));
    }

    protected override EditStopDateViewModelValidator ValidatorInitialize()
    {
        return new EditStopDateViewModelValidator(_currentDateTime.Object);
    }


    [TestCase("5143541", true)]
    [TestCase(" ", false)]
    [TestCase("", false)]
    [TestCase(null, false)]
    public void WhenValidatingEditStopDate_ValidateTheAccountHashedId(string accountHashedId, bool expectedValid)
    {
        //Arrange
        var viewModel = _autoFixture.Create<EditStopDateViewModel>();
        viewModel.AccountHashedId = accountHashedId;

        //Assert
        AssertValidationResult(r => r.AccountHashedId, viewModel, expectedValid);
    }

    [TestCase("5143541", true)]
    [TestCase(" ", false)]
    [TestCase("", false)]
    [TestCase(null, false)]
    public void WhenValidatingEditStopDate_ValidateTheApprenticeshipHashedId(string apprenticeshipHashedId, bool expectedValid)
    {
        //Arrange
        var viewModel = _autoFixture.Create<EditStopDateViewModel>();
        viewModel.ApprenticeshipHashedId = apprenticeshipHashedId;

        //Assert
        AssertValidationResult(r => r.ApprenticeshipHashedId, viewModel, expectedValid);
    }

    [TestCase(null, null, false, "Enter the stop date for this apprenticeship")]
    [TestCase(0, 0, false, "Enter the stop date for this apprenticeship")]
    [TestCase(07, 2021, false, "The stop date cannot be in the future")]
    [TestCase(08, 2020, false, "The stop month cannot be before the apprenticeship started")]
    [TestCase(02, 2021, false, "Enter a date that is different to the current stopped date")]
    public void WhenValidatingEditStopDate_ValidateTheNewStopDate(int? newStopMonth, int? newStopYear, bool expectedValid, string errorMessage)
    {
        //Arrange
        var viewModel = _autoFixture.Create<EditStopDateViewModel>();
        viewModel.NewStopMonth = newStopMonth;
        viewModel.NewStopYear = newStopYear;

        //Assert
        AssertValidationResult(r => r.NewStopDate, viewModel, expectedValid);
    }
}