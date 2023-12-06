using AutoFixture;
using NUnit.Framework;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Cohort
{
    [TestFixture]
    public class WhenMappingInformRequestToInformViewModelTests
    {
        private InformRequest _informRequest;
        private InformRequestToInformViewModelMapper _mapper;

        [SetUp]
        public void Arrange()
        {
            var autoFixture = new Fixture();            
            _informRequest = autoFixture.Create<InformRequest>();
            _mapper = new InformRequestToInformViewModelMapper();
        }

        [Test]
        public async Task Then_AccountHashedId_Is_Mapped()
        {
            //Act
            var result = await _mapper.Map(_informRequest);

            //Assert           
            Assert.That(result.AccountHashedId, Is.EqualTo(_informRequest.AccountHashedId));
        }
    }
}
