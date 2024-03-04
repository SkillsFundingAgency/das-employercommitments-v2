using SFA.DAS.EmployerCommitmentsV2.Web.Models.DraftApprenticeship;
using SFA.DAS.EmployerCommitmentsV2.Web.Validators;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Validators;

[TestFixture]
public class DeleteDraftApprenticeshipViewModelValidatorTests : ValidatorTestBase<DeleteDraftApprenticeshipViewModel, DeleteDraftApprenticeshipViewModelValidator>
{
    [TestCase(true,true)]
    [TestCase(false, true)]
    [TestCase(null, false)]
    public void ThenConfirmDeleteIsValidated(bool? confirmDeleteValue, bool expectedValid)
    {
        var model = new DeleteDraftApprenticeshipViewModel() {ConfirmDelete = confirmDeleteValue};
        AssertValidationResult(x => x.ConfirmDelete, model, expectedValid);
    }
}