using AutoFixture;
using NUnit.Framework;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using System.Threading.Tasks;
using Moq;
using SFA.DAS.Authorization.Services;
using SFA.DAS.EmployerCommitmentsV2.Features;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Cohort
{
    [TestFixture]
    public class WhenMappingInformRequestToInformViewModelTests
    {
        private InformRequest _informRequest;
        private InformRequestToInformViewModelMapper _mapper;
        private Mock<IAuthorizationService> _mockAuthorizationService;

        [SetUp]
        public void Arrange()
        {
            var autoFixture = new Fixture();            
            _informRequest = autoFixture.Create<InformRequest>();
            _mockAuthorizationService = new Mock<IAuthorizationService>();
            _mapper = new InformRequestToInformViewModelMapper(_mockAuthorizationService.Object);
        }

        [Test]
        public async Task Then_AccountHashedId_Is_Mapped()
        {
            //Act
            var result = await _mapper.Map(_informRequest);

            //Assert           
            Assert.AreEqual(_informRequest.AccountHashedId, result.AccountHashedId);
        }

        [TestCase(true)]
        [TestCase(false)]
        public async Task Then_HasApprenticeEmail_Is_Mapped(bool expected)
        {
            _mockAuthorizationService.Setup(x => x.IsAuthorizedAsync(EmployerFeature.ApprenticeEmail))
                .ReturnsAsync(expected);

            //Act
            var result = await _mapper.Map(_informRequest);

            //Assert           
            Assert.AreEqual(expected, result.HasApprenticeEmail);
        }
    }
}
