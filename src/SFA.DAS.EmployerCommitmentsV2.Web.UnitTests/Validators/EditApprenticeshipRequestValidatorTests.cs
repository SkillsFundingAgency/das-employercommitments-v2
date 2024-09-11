using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.Validators;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Validators;

[TestFixture]
public class EditApprenticeshipRequestValidatorTests : ValidatorTestBase<EditApprenticeshipRequest, EditApprenticeshipRequestValidator>
{
    [TestCase("00000000-0000-0000-0000-000000000000", false)]
    [TestCase("a7846ec9-7e58-4ccd-98b9-9803dc8bae6d", true)]
    public void ThenValidatesCacheKey(string cacheKey, bool expectedValid)
    {
        var request = new EditApprenticeshipRequest { CacheKey = Guid.Parse(cacheKey) };

        AssertValidationResult(x => x.CacheKey, request, expectedValid);
    }
}
