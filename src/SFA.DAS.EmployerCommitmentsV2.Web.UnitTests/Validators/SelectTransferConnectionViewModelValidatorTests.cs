﻿using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.EmployerCommitmentsV2.Web.Validators;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Validators;

public class SelectTransferConnectionViewModelValidatorTests : ValidatorTestBase<SelectTransferConnectionViewModel, SelectTransferConnectionViewModelValidator>
{
    [Test]
    [InlineAutoData("5143541", true)]
    [InlineAutoData(" ", false)]
    [InlineAutoData("", false)]
    [InlineAutoData(null, false)]
    public void WhenValidatingSelectTransferConnection_ValidateTheAccountHashedId(string accountHashedId, bool expectedValid, SelectTransferConnectionViewModel viewModel)
    {
        //Arrange
        viewModel.AccountHashedId = accountHashedId;

        //Assert
        AssertValidationResult(x => x.AccountHashedId, viewModel, expectedValid);
    }

    [Test]
    [InlineAutoData(" ", false, "Select a connection")]
    [InlineAutoData("", false, "Select a connection")]        
    [InlineAutoData(null, false, "Select a connection")]        
    [InlineAutoData("CCC", true, null)]        
    public void WhenValidatingSelectTransferConnection_ValidateTransferConnectionCode(string transferConnectionCode, bool expectedValid, string errorMessage, SelectTransferConnectionViewModel viewModel)
    {
        //Arrange
        viewModel.TransferConnectionCode = transferConnectionCode;

        //Assert
        AssertValidationResult(x => x.TransferConnectionCode, viewModel, expectedValid, errorMessage);
    }

}