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

            Assert.That(result.ApprenticeshipId, Is.EqualTo(request.ApprenticeshipId));
        }

        [Test, MoqAutoData]
        public async Task EndDate_IsMapped(
         EditEndDateViewModel request,
         IAuthenticationService authenticationService)
        {
            var mapper = new EditEndDateRequestMapper(authenticationService);
            var result = await mapper.Map(request);

            Assert.That(result.EndDate, Is.EqualTo(request.EndDate.Date));
        }

        [Test, MoqAutoData]
        public async Task UserInfo_IsMapped(
        EditEndDateViewModel request,
        IAuthenticationService authenticationService)
        {
            var mapper = new EditEndDateRequestMapper(authenticationService);
            var result = await mapper.Map(request);

            Assert.That(result.UserInfo.UserEmail, Is.EqualTo(authenticationService.UserInfo.UserEmail));
            Assert.That(result.UserInfo.UserId, Is.EqualTo(authenticationService.UserInfo.UserId));
            Assert.That(result.UserInfo.UserDisplayName, Is.EqualTo(authenticationService.UserInfo.UserDisplayName));
        }
    }
}
