using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Web.Extensions;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Extensions;

[TestFixture]
public class CohortSummaryExtensionTest
{
    [Test]
    public void CohortSummary_GetStatus_Returns_Correct_Status_For_Review()
    {
        var cohortSummary = new CohortSummary
        {
            CohortId = 1,
            IsDraft = false,
            WithParty = Party.Employer
        };

        var status =  cohortSummary.GetStatus();

        Assert.That(status, Is.EqualTo(CohortStatus.Review));
    }

    [Test]
    public void CohortSummary_GetStatus_Returns_Correct_Status_For_Draft()
    {
        var cohortSummary = new CohortSummary
        {
            CohortId = 1,
            IsDraft = true,
            WithParty = Party.Employer
        };

        var status = cohortSummary.GetStatus();

        Assert.That(status, Is.EqualTo(CohortStatus.Draft));
    }

    [Test]
    public void CohortSummary_GetStatus_Returns_Correct_Status_For_WithTrainingProvider()
    {
        var cohortSummary = new CohortSummary
        {
            CohortId = 1,
            IsDraft = false,
            WithParty = Party.Provider
        };

        var status = cohortSummary.GetStatus();

        Assert.That(status, Is.EqualTo(CohortStatus.WithProvider));
    }

    [Test]
    public void CohortSummary_GetStatus_Returns_Unknown_If_Unable_To_Find_The_Status()
    {
        var cohortSummary = new CohortSummary
        {
            CohortId = 1,
            IsDraft = true,
            WithParty = Party.Provider
        };

        var status = cohortSummary.GetStatus();

        Assert.That(status, Is.EqualTo(CohortStatus.Unknown));
    }
}