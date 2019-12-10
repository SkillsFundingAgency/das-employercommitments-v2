using System.Threading.Tasks;
using AutoFixture;
using NUnit.Framework;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Home;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Home;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Home
{
    [TestFixture]
    public class IndexViewModelMapperTests
    {
        private IndexViewModelMapper _mapper;
        private IndexRequest _request;
        private IndexViewModel _result;

        [SetUp]
        public async Task Arrange()
        {
            var autoFixture = new Fixture();

            _mapper = new IndexViewModelMapper();
            _request = autoFixture.Create<IndexRequest>();
            _result = await _mapper.Map(TestHelper.Clone(_request));
        }

        [Test]
        public void AccountHashedIdIsMappedCorrectly()
        {
            Assert.AreEqual(_request.AccountHashedId, _result.AccountHashedId);
        }
    }
}
