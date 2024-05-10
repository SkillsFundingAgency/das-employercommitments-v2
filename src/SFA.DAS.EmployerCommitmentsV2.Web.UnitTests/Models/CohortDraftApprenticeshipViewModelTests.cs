using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;


namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Models;

[TestFixture]
public class CohortDraftApprenticeshipViewModelTests
{
    private CohortDraftApprenticeshipViewModel _viewModel;

    [TestCase("John", "Smith", "John Smith")]
    public void ThenDisplayNameIsCorrect(string firstName, string lastName, string expectedDisplayName)
    {
        _viewModel = new CohortDraftApprenticeshipViewModel {FirstName = firstName, LastName = lastName};
        Assert.That(_viewModel.DisplayName, Is.EqualTo(expectedDisplayName));
    }

    [TestCase(null, "-")]
    [TestCase(123, "£123")]
    [TestCase(123456, "£123,456")]
    public void ThenDisplayCostIsCorrect(int? cost, string expectedDisplayCost)
    {
        _viewModel = new CohortDraftApprenticeshipViewModel { Cost = cost };
        Assert.That(_viewModel.DisplayCost, Is.EqualTo(expectedDisplayCost));
    }

    [TestCase(null, null, "-")]
    [TestCase("2019-01-01", null, "-")]
    [TestCase(null, "2019-02-01", "-")]
    [TestCase("2019-01-01", "2019-02-01", "Jan 2019 to Feb 2019")]
    public void ThenDisplayTrainingDatesIsCorrect(DateTime? startDate, DateTime? endDate,
        string expectedDisplayTrainingDates)
    {
        _viewModel = new CohortDraftApprenticeshipViewModel {StartDate = startDate, ActualStartDate = null, EndDate = endDate};
        Assert.That(_viewModel.DisplayTrainingDates, Is.EqualTo(expectedDisplayTrainingDates));
    }

    [TestCase(null, null, "-")]
    [TestCase("2019-01-13", null, "-")]
    [TestCase(null, "2019-02-01", "-")]
    [TestCase("2019-01-12", "2019-02-03", "12 Jan 2019 to 3 Feb 2019")]
    public void ThenDisplayTrainingDatesIsCorrectForPilotUsers(DateTime? startDate, DateTime? endDate,
        string expectedDisplayTrainingDates)
    {
        _viewModel = new CohortDraftApprenticeshipViewModel {StartDate = null, ActualStartDate = startDate, EndDate = endDate};
        Assert.That(_viewModel.DisplayTrainingDates, Is.EqualTo(expectedDisplayTrainingDates));
    }

    [TestCase("2017-09-01", "1 Sep 2017")]
    [TestCase("2018-10-10", "10 Oct 2018")]
    public void ThenDisplayDateOfBirth(DateTime? dateOfBirth, string expectedDisplayDateOfBirth)
    {
        _viewModel = new CohortDraftApprenticeshipViewModel {DateOfBirth = dateOfBirth};
        Assert.That(_viewModel.DisplayDateOfBirth, Is.EqualTo(expectedDisplayDateOfBirth));
    }
}