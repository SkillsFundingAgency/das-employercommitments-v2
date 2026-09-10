using FluentAssertions;
using SFA.DAS.EmployerCommitmentsV2.Web.Extensions;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Extensions;

[TestFixture]
public class ChangeHistoryTypeExtensionTests
{
    [Test]
    public void GetDisplayClass_WhenAutoRejected_ReturnsRedTag()
    {
        LearningChangeType.AutoRejected.GetDisplayClass().Should().Be("govuk-tag--red");
    }

    [Test]
    public void GetEnumDescription_WhenAutoRejected_ReturnsAutoRejectedLabel()
    {
        LearningChangeType.AutoRejected.GetEnumDescription().Should().Be("Auto-rejected");
    }
}
