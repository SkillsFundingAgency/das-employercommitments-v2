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
            Assert.AreEqual(request.ApprenticeshipId, result.ApprenticeshipId);
        }

        [Test, MoqAutoData]
        public async Task FirstName_IsMapped()
        {
            var result = await mapper.Map(request);

            Assert.AreEqual(request.FirstName, result.FirstName);
        }

        [Test, MoqAutoData]
        public async Task LastName_IsMapped()
        {
            var result = await mapper.Map(request);

            Assert.AreEqual(request.LastName, result.LastName);
        }

        [Test, MoqAutoData]
        public async Task Dob_IsMapped()
        {
            var result = await mapper.Map(request);

            Assert.AreEqual(request.DateOfBirth, result.DateOfBirth);
        }

        [Test, MoqAutoData]
        public async Task StartDate_IsMapped()
        {
            var result = await mapper.Map(request);

            Assert.AreEqual(request.StartDate, result.StartDate);
        }


        [Test, MoqAutoData]
        public async Task EndDate_IsMapped()
        {
            var result = await mapper.Map(request);

            Assert.AreEqual(request.EndDate, result.EndDate);
        }

        [Test, MoqAutoData]
        public async Task Course_IsMapped()
        {
            var result = await mapper.Map(request);

            Assert.AreEqual(request.CourseCode, result.CourseCode);
        }

        [Test, MoqAutoData]
        public async Task Cost_IsMapped()
        {
            var result = await mapper.Map(request);

            Assert.AreEqual(request.Cost, result.Cost);
        }
    }
}
