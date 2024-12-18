using FluentAssertions;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Models.CreateCohort;

public class WhenCastingIndexViewModelToDictionary
{
    [Test, AutoData]
    public void Then_Adds_AccountHashedId_To_Dictionary(
        IndexViewModel indexViewModel)
    {
        var dictionary = indexViewModel.ToDictionary();

        dictionary.Should().ContainKey(nameof(indexViewModel.AccountHashedId))
            .WhoseValue.Should().Be(indexViewModel.AccountHashedId);
    }

    [Test, AutoData]
    public void And_ApprenticeshipSessionKey_Then_Add_ApprenticeshipSessionKey_To_Dictionary(
        IndexViewModel indexViewModel)
    {
        indexViewModel.ApprenticeshipSessionKey = Guid.NewGuid();
        var dictionary = indexViewModel.ToDictionary();

        dictionary.Should().ContainKey(nameof(IndexViewModel.ApprenticeshipSessionKey));
    }

    [Test, AutoData]
    public void And_No_ApprenticeshipSessionKey_Then_Not_Add_ApprenticeshipSessionKey_To_Dictionary(
        IndexViewModel indexViewModel)
    {
        indexViewModel.ApprenticeshipSessionKey = null;
        var dictionary = indexViewModel.ToDictionary();

        dictionary.Should().NotContainKey(nameof(IndexViewModel.ApprenticeshipSessionKey));
    }
}