using NUnit.Framework;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.Testing.AutoFixture;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Apprentice
{
    public class WhoWillEnterTheDetailsToSendNewTrainingProviderRequestMapperTests
    {
        private WhoWillEnterTheDetailsToSendNewTrainingProviderRequestMapper _mapper;

        [SetUp]
        public void Arrange()
        {
            _mapper = new WhoWillEnterTheDetailsToSendNewTrainingProviderRequestMapper();
        }

        [Test, MoqAutoData]
        public async Task ApprenticeshipHashedId_IsMapped(WhoWillEnterTheDetailsViewModel viewModel)
        {
            var result = await _mapper.Map(viewModel);

            Assert.AreEqual(viewModel.ApprenticeshipHashedId, result.ApprenticeshipHashedId);
        }

        [Test, MoqAutoData]
        public async Task AccountHashedId_IsMapped(WhoWillEnterTheDetailsViewModel viewModel)
        {
            var result = await _mapper.Map(viewModel);

            Assert.AreEqual(viewModel.AccountHashedId, result.AccountHashedId);
        }

        [Test, MoqAutoData]
        public async Task Ukprn_IsMapped(WhoWillEnterTheDetailsViewModel viewModel)
        {
            var result = await _mapper.Map(viewModel);

            Assert.AreEqual(viewModel.ProviderId, result.ProviderId);
        }
    }
}
