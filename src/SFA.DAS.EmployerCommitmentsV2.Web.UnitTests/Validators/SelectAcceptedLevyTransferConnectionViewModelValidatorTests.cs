using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.EmployerCommitmentsV2.Web.Validators;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Validators;

public class SelectAcceptedLevyTransferConnectionViewModelValidatorTests : ValidatorTestBase<SelectAcceptedLevyTransferConnectionViewModel, SelectAcceptedLevyTransferConnectionViewModelValidator>
{
    [Test]
    [InlineAutoData("5143541", true)]
    [InlineAutoData(" ", false)]
    [InlineAutoData("", false)]
    [InlineAutoData(null, false)]
    public void WhenValidatingSelectAcceptedLevyTransferConnection_ValidateTheAccountHashedId(string accountHashedId, bool expectedValid, SelectAcceptedLevyTransferConnectionViewModel viewModel)
    {
        //Arrange
        viewModel.AccountHashedId = accountHashedId;

        //Assert
        AssertValidationResult(x => x.AccountHashedId, viewModel, expectedValid);
    }

    [Test]
    [InlineAutoData(" ", false, "Select a transfer")]
    [InlineAutoData("", false, "Select a transfer")]        
    public void WhenValidatingSelectTransferConnection_ValidateTransferConnectionCode(string selectedCode, bool expectedValid, string errorMessage, SelectAcceptedLevyTransferConnectionViewModel viewModel)
    {
        //Arrange
        viewModel.ApplicationAndSenderHashedId = selectedCode;

        //Assert
        AssertValidationResult(x => x.ApplicationAndSenderHashedId, viewModel, expectedValid, errorMessage);
    }
}