using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Web.Extensions;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Extensions;

public class ConfirmationStatusExtensionTests
{
    [TestCase(ConfirmationStatus.Confirmed, "Confirmed")]
    [TestCase(ConfirmationStatus.Overdue, "Overdue")]
    [TestCase(ConfirmationStatus.Unconfirmed, "Unconfirmed")]
    [TestCase(null, "N/A")]
    public void ToDisplayString_Maps_Correctly(ConfirmationStatus? status, string expectedResult)
    {
        //Act
        var actualResult = status.ToDisplayString();

        //Assert
        Assert.That(actualResult, Is.EqualTo(expectedResult));
    }
}