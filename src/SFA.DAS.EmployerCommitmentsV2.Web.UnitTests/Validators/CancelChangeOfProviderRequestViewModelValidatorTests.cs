using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.Validators;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Validators;

public class CancelChangeOfProviderRequestViewModelValidatorTests : ValidatorTestBase<CancelChangeOfProviderRequestViewModel, CancelChangeOfProviderRequestViewModelValidator>
{
    private const string SelectAnswerError = "Select if you want to cancel this request";

    [TestCase(true)]
    [TestCase(false)]
    public void WhenYesOrNoIsSelected_ThenValidationPasses(bool cancelRequest)
    {
        var model = new CancelChangeOfProviderRequestViewModel { CancelRequest = cancelRequest };

        AssertValidationResult(x => x.CancelRequest, model, true);
    }

    [Test]
    public void WhenNoAnswerIsSelected_ThenValidationFails()
    {
        var model = new CancelChangeOfProviderRequestViewModel { CancelRequest = null };

        AssertValidationResult(x => x.CancelRequest, model, false, SelectAnswerError);
    }
}