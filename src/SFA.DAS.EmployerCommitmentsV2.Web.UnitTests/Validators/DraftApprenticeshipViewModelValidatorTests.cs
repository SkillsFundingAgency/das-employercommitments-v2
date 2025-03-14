using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Shared;
using SFA.DAS.EmployerCommitmentsV2.Web.Validators;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Validators;

public class DraftApprenticeshipViewModelValidatorTests : ValidatorTestBase<DraftApprenticeshipViewModel, DraftApprenticeshipViewModelValidator>
{
    [TestCase(12, 12, 2000, true)]
    [TestCase(31, 2, 2000, false)]
    [TestCase(null, 2, 2000, false)]
    [TestCase(31, null, 2000, false)]
    [TestCase(31, 1, null, false)]
    [TestCase(null, null, null, true)]
    public void Validate_DoB_ShouldBeValidated(int? day, int? month, int? year, bool expectedValid)
    {
        var model = new DraftApprenticeshipViewModel { BirthDay = day, BirthMonth = month, BirthYear = year };
        AssertValidationResult(request => request.DateOfBirth, model, expectedValid);
    }

    [TestCase(12, 2000, true)]
    [TestCase(13, 2000, false)]
    [TestCase(null, 2000, false)]
    [TestCase(1, null, false)]
    [TestCase(null, null, true)]
    public void Validate_StartDate_ShouldBeValidated(int? month, int? year, bool expectedValid)
    {
        var model = new DraftApprenticeshipViewModel { StartMonth = month, StartYear = year };
        AssertValidationResult(request => request.StartDate, model, expectedValid);
    }

    [TestCase(12, 2000, true)]
    [TestCase(13, 2000, false)]
    [TestCase(null, 2000, false)]
    [TestCase(1, null, false)]
    [TestCase(null, null, true)]
    public void Validate_EndDate_ShouldBeValidated(int? month, int? year, bool expectedValid)
    {
        var model = new DraftApprenticeshipViewModel { EndMonth = month, EndYear = year };
        AssertValidationResult(request => request.EndDate, model, expectedValid);
    }

    [TestCase(DeliveryModel.Regular, true)]
    [TestCase(DeliveryModel.FlexiJobAgency, true)]
    [TestCase(DeliveryModel.PortableFlexiJob, true)]
    [TestCase(null, false)]
    public void Validate_DeliveryModel_ShouldBeValidated(DeliveryModel? deliveryModel, bool expectedValid)
    {
        var model = new DraftApprenticeshipViewModel { DeliveryModel = deliveryModel };
        AssertValidationResult(request => request.DeliveryModel, model, expectedValid);
    }
    
    [TestCase("something", true)]
    [TestCase("", false)]
    [TestCase(null, false)]
    public void Validate_FirstName_ShouldBeValidated(string value, bool expectedValid)
    {
        var model = new DraftApprenticeshipViewModel { FirstName = value };
        AssertValidationResult(request => request.FirstName, model, expectedValid);
    }
    
    [TestCase("something", true)]
    [TestCase("", false)]
    [TestCase(null, false)]
    public void Validate_LastName_ShouldBeValidated(string value, bool expectedValid)
    {
        var model = new DraftApprenticeshipViewModel { LastName = value };
        AssertValidationResult(request => request.LastName, model, expectedValid);
    }
    
    [TestCase("something@email.test", true)]
    [TestCase("something.test", false)]
    [TestCase("@email.test", false)]
    [TestCase("@-email.test", false)]
    [TestCase("", false)]
    [TestCase(null, false)]
    public void Validate_Email_ShouldBeValidated(string value, bool expectedValid)
    {
        var model = new DraftApprenticeshipViewModel { Email = value };
        AssertValidationResult(request => request.Email, model, expectedValid);
    }
}