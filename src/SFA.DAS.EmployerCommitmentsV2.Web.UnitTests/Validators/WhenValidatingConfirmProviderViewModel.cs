using System.Linq;
using NUnit.Framework;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.EmployerCommitmentsV2.Web.Validators;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Validators
{
    public class WhenValidatingConfirmProviderViewModel
    {

        [Test, MoqAutoData]
        public void And_The_ViewModel_Is_Populated_Correctly_Then_The_Validator_Returns_Valid(
            ConfirmProviderViewModel viewModel,
            long providerId,
            string employerAccountLegalEntityPublicHashedId,
            string providerName,
            ConfirmProviderViewModelValidator validator)
        {
            viewModel.UseThisProvider = true;
         
            var result = validator.Validate(viewModel);

            Assert.True(result.IsValid);
        }

        [Test, MoqAutoData]
        public void And_The_UseThisProvider_Has_Not_Been_Set_Then_Invalid_Is_Returned(
            ConfirmProviderViewModel viewModel,
            long providerId,
            string employerAccountLegalEntityPublicHashedId,
            string providerName,
            ConfirmProviderViewModelValidator validator)
        {
            viewModel.ProviderId = providerId;
            viewModel.ProviderName = providerName;
            viewModel.UseThisProvider = null;
            viewModel.AccountLegalEntityHashedId = employerAccountLegalEntityPublicHashedId;

            var result = validator.Validate(viewModel);

            Assert.False(result.IsValid);
            Assert.AreEqual("Select a training provider", result.Errors.First().ErrorMessage);
        }
    }
}
