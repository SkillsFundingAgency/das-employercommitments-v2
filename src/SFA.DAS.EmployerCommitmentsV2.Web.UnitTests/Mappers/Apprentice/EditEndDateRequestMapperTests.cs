using NUnit.Framework;
using SFA.DAS.EmployerCommitmentsV2.Web.Authentication;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.Testing.AutoFixture;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Apprentice
{
    public class EditEndDateRequestMapperTests
    {
        [Test, MoqAutoData]
        public async Task ApprenticeshipId_IsMapped(
          EditEndDateViewModel request,
          IAuthenticationService authenticationService)
        {
            var mapper = new EditEndDateRequestMapper(authenticationService);
            var result = await mapper.Map(request);

            Assert.AreEqual(request.ApprenticeshipId, result.ApprenticeshipId);
        }

        [Test, MoqAutoData]
        public async Task EndDate_IsMapped(
         EditEndDateViewModel request,
         IAuthenticationService authenticationService)
        {
            var mapper = new EditEndDateRequestMapper(authenticationService);
            var result = await mapper.Map(request);

            Assert.AreEqual(request.EndDate.Date, result.EndDate);
        }

        [Test, MoqAutoData]
        public async Task UserInfo_IsMapped(
        EditEndDateViewModel request,
        IAuthenticationService authenticationService)
        {
            var mapper = new EditEndDateRequestMapper(authenticationService);
            var result = await mapper.Map(request);

            Assert.AreEqual(authenticationService.UserInfo.UserEmail, result.UserInfo.UserEmail);
            Assert.AreEqual(authenticationService.UserInfo.UserId, result.UserInfo.UserId);
            Assert.AreEqual(authenticationService.UserInfo.UserDisplayName, result.UserInfo.UserDisplayName);
        }
    }
}
