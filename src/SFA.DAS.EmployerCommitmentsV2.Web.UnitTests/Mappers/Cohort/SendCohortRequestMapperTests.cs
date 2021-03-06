﻿using System.Threading.Tasks;
using AutoFixture;
using NUnit.Framework;
using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Cohort
{
    [TestFixture]
    public class SendCohortRequestMapperTests
    {
        private SendCohortRequestMapper _mapper;
        private SendCohortRequest _result;
        private DetailsViewModel _source;

        [SetUp]
        public async Task Arrange()
        {
            var autoFixture = new Fixture();

            _source = autoFixture.Build<DetailsViewModel>().Without(x=>x.Courses).Create();
            
            _mapper = new SendCohortRequestMapper();

            _result = await _mapper.Map(TestHelper.Clone(_source));
        }

        [Test]
        public void MessageIsMappedCorrectly()
        {
            Assert.AreEqual(_source.SendMessage, _result.Message);
        }
    }
}
