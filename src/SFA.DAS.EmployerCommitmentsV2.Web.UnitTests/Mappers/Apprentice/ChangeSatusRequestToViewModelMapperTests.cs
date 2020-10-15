using AutoFixture;
using NUnit.Framework;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Apprentice
{
    [TestFixture]
    public class WhenMapping_ChangeSatusRequestToViewModelMapperTests
    {

        private ChangeStatusRequestToViewModelMapper _mapper;
        private ChangeStatusRequest _source;

        [SetUp]
        public void Arrange()
        {
            var fixture = new Fixture();

            _mapper = new ChangeStatusRequestToViewModelMapper();
            _source = fixture.Build<ChangeStatusRequest>().Create();

        }

        [Test]
        public async Task ThenApprenticeshipHashedIdIsMappedCorrectly()
        {
            var result = await _mapper.Map(_source);

            Assert.AreEqual(_source.ApprenticeshipHashedId, result.ApprenticeshipHashedId);
        }

        [Test]
        public async Task ThenAccountHashedIdIsMappedCorrectly()
        {
            var result = await _mapper.Map(_source);

            Assert.AreEqual(_source.AccountHashedId, result.AccountHashedId);
        }
    }
}
