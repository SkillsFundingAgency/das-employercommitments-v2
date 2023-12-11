using SFA.DAS.EmployerCommitmentsV2.Web.Models.DraftApprenticeship;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Models;

[TestFixture]
public class EditDraftApprenticeshipViewModelTests
{
    private EditDraftApprenticeshipViewModel _editDraftApprenticeshipViewModel;

    [SetUp]
    public void Setup()
    {
        // Arrange
        var fixture = new Fixture();
        var baseDate = DateTime.Now;
        var startDate = baseDate;
        var endDate = baseDate.AddYears(2);
        var dateOfBirth = baseDate.AddYears(-18);

        _editDraftApprenticeshipViewModel = fixture.Build<EditDraftApprenticeshipViewModel>()
            .Do(x =>
            {
                var constructorInfo = x.GetType().GetConstructor(new[] { typeof(DateTime?), typeof(DateTime?), typeof(DateTime?) });
                if (constructorInfo != null)
                {
                    constructorInfo.Invoke(x, new object[] { dateOfBirth, startDate, endDate });
                }
            })
            .Without(x => x.StartDate)
            .Create();
    }

    [Test]
    public void CloneBaseValues_Should_Not_Include_Excluded_Properties()
    {           
        // Act
        var clonedViewModel = _editDraftApprenticeshipViewModel.CloneBaseValues();

        // Assert
        Assert.That(clonedViewModel, Is.Not.Null);
        Assert.That(clonedViewModel.ProviderId, Is.EqualTo(_editDraftApprenticeshipViewModel.ProviderId));
        Assert.That(clonedViewModel.ProviderName, Is.EqualTo(_editDraftApprenticeshipViewModel.ProviderName));
        Assert.That(clonedViewModel, Does.Not.Property("AccountLegalEntityId"));
        Assert.That(clonedViewModel, Does.Not.Property("CohortId"));
    }   
}