using System.Threading.Tasks;
using AutoFixture;
using NUnit.Framework;
using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Cohort
{
    [TestFixture]
    public class ApproveCohortRequestMapperTests
    {
        private ApproveCohortRequestMapper _mapper;
        private ApproveCohortRequest _result;
        private DetailsViewModel _source;

        [SetUp]
        public async Task Arrange()
        {
            var autoFixture = new Fixture();

            _source = autoFixture.Build<DetailsViewModel>().Without(x=>x.Courses).Create();
            
            _mapper = new ApproveCohortRequestMapper();

            _result = await _mapper.Map(TestHelper.Clone(_source));
        }

        [Test]
        public void MessageIsMappedCorrectly()
        {
            Assert.That(_result.Message, Is.EqualTo(_source.ApproveMessage));
        }
    }
}
