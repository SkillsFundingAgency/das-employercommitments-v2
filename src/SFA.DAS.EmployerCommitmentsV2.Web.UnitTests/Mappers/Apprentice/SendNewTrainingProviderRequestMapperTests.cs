using NUnit.Framework;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.Testing.AutoFixture;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Apprentice
{
    public class SendNewTrainingProviderRequestMapperTests
    {
        private SendNewTrainingProviderRequestMapper _mapper;

        [SetUp]
        public void Arrange()
        {
            _mapper = new SendNewTrainingProviderRequestMapper();
        }

        [Test, MoqAutoData]
        public async Task ApprenticeshipHashedId_IsMapped(EnterNewTrainingProviderViewModel viewModel)
        {
            var result = await _mapper.Map(viewModel);

            Assert.That(result.ApprenticeshipHashedId, Is.EqualTo(viewModel.ApprenticeshipHashedId));
        }

        [Test, MoqAutoData]
        public async Task AccountHashedId_IsMapped(EnterNewTrainingProviderViewModel viewModel)
        {
            var result = await _mapper.Map(viewModel);

            Assert.That(result.AccountHashedId, Is.EqualTo(viewModel.AccountHashedId));
        }

        [Test, MoqAutoData]
        public async Task Ukprn_IsMapped(EnterNewTrainingProviderViewModel viewModel)
        {
            var result = await _mapper.Map(viewModel);

            Assert.That(result.ProviderId, Is.EqualTo(viewModel.ProviderId));
        }
    }
}
