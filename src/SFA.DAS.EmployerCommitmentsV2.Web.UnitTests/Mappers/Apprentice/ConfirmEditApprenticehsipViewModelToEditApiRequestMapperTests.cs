using AutoFixture;
using NUnit.Framework;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.Testing.AutoFixture;
using System.Threading.Tasks;
using System;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Apprentice
{
    public class ConfirmEditApprenticehsipViewModelToEditApiRequestMapperTests
    {
        private ConfirmEditApprenticehsipViewModelToEditApiRequestMapper mapper;
        ConfirmEditApprenticeshipViewModel request;

        [SetUp]
        public void SetUp()
        {
            var fixture = new Fixture();

           request = fixture.Build<ConfirmEditApprenticeshipViewModel>()
                .With(x => x.StartMonth, DateTime.Now.Month)
                .With(x => x.StartYear, DateTime.Now.Year)
                .With(x => x.EndMonth, DateTime.Now.Month)
                .With(x => x.EndYear, DateTime.Now.Year)
                .With(x => x.EmploymentEndMonth, DateTime.Now.Month)
                .With(x => x.EmploymentEndYear, DateTime.Now.Year)
                .With(x => x.BirthMonth, DateTime.Now.Month)
                .With(x => x.BirthYear, DateTime.Now.Year)
                .With(x => x.BirthDay, DateTime.Now.Day)
                .Create();


            mapper = new ConfirmEditApprenticehsipViewModelToEditApiRequestMapper();
        }

        [Test, MoqAutoData]
        public async Task ApprenticeshipId_IsMapped()
        {
            var result = await mapper.Map(request);
            Assert.That(result.ApprenticeshipId, Is.EqualTo(request.ApprenticeshipId));
        }

        [Test, MoqAutoData]
        public async Task FirstName_IsMapped()
        {
            var result = await mapper.Map(request);

            Assert.That(result.FirstName, Is.EqualTo(request.FirstName));
        }

        [Test, MoqAutoData]
        public async Task LastName_IsMapped()
        {
            var result = await mapper.Map(request);

            Assert.That(result.LastName, Is.EqualTo(request.LastName));
        }

        [Test, MoqAutoData]
        public async Task Dob_IsMapped()
        {
            var result = await mapper.Map(request);

            Assert.That(result.DateOfBirth, Is.EqualTo(request.DateOfBirth));
        }

        [Test, MoqAutoData]
        public async Task StartDate_IsMapped()
        {
            var result = await mapper.Map(request);

            Assert.That(result.StartDate, Is.EqualTo(request.StartDate));
        }


        [Test, MoqAutoData]
        public async Task EndDate_IsMapped()
        {
            var result = await mapper.Map(request);

            Assert.That(result.EndDate, Is.EqualTo(request.EndDate));
        }

        [Test, MoqAutoData]
        public async Task DeliveryModel_IsMapped()
        {
            var result = await mapper.Map(request);

            Assert.That(result.DeliveryModel, Is.EqualTo(request.DeliveryModel));
        }

        [Test, MoqAutoData]
        public async Task EmploymentEndDate_IsMapped()
        {
            var result = await mapper.Map(request);

            Assert.That(result.EmploymentEndDate, Is.EqualTo(request.EmploymentEndDate));
        }

        [Test, MoqAutoData]
        public async Task EmploymentPrice_IsMapped()
        {
            var result = await mapper.Map(request);

            Assert.That(result.EmploymentPrice, Is.EqualTo(request.EmploymentPrice));
        }

        [Test, MoqAutoData]
        public async Task Course_IsMapped()
        {
            var result = await mapper.Map(request);

            Assert.That(result.CourseCode, Is.EqualTo(request.CourseCode));
        }

        [Test, MoqAutoData]
        public async Task Version_IsMapped()
        {
            var result = await mapper.Map(request);

            Assert.That(result.Version, Is.EqualTo(request.Version));
        }

        [Test, MoqAutoData]
        public async Task Option_IsMapped()
        {
            var result = await mapper.Map(request);

            Assert.That(result.Option, Is.EqualTo(request.Option));
        }

        [Test, MoqAutoData]
        public async Task Cost_IsMapped()
        {
            var result = await mapper.Map(request);

            Assert.That(result.Cost, Is.EqualTo(request.Cost));
        }
    }
}
