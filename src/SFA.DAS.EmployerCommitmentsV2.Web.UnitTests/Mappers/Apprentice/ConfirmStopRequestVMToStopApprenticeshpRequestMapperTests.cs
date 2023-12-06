using NUnit.Framework;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.Testing.AutoFixture;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Apprentice
{
    public class ConfirmStopRequestVMToStopApprenticeshpRequestMapperTests
    {
        [Test, MoqAutoData]
        public async Task AccountId_IsMapped(ConfirmStopRequestViewModel request)
        {
            var mapper = new ConfirmStopRequestVMToStopApprenticeshpRequestMapper();
            var result = await mapper.Map(request);

            Assert.That(result.AccountId, Is.EqualTo(request.AccountId));
        }

        [Test, MoqAutoData]
        public async Task MadeRedundant_IsMapped(ConfirmStopRequestViewModel request)
        {
            var mapper = new ConfirmStopRequestVMToStopApprenticeshpRequestMapper();
            var result = await mapper.Map(request);

            Assert.That(result.MadeRedundant, Is.EqualTo(request.MadeRedundant));
        }

        [Test, MoqAutoData]
        public async Task StopDate_IsMapped(ConfirmStopRequestViewModel request)
        {
            var mapper = new ConfirmStopRequestVMToStopApprenticeshpRequestMapper();
            var result = await mapper.Map(request);

            Assert.That(result.StopDate, Is.EqualTo(request.StopDate));
        }
    }
}
