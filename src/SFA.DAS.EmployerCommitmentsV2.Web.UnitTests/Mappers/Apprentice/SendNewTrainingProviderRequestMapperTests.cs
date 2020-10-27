using NUnit.Framework;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Collections.Generic;
using System.Text;
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

            Assert.AreEqual(viewModel.ApprenticeshipHashedId, result.ApprenticeshipHashedId);
        }

        [Test, MoqAutoData]
        public async Task AccountHashedId_IsMapped(EnterNewTrainingProviderViewModel viewModel)
        {
            var result = await _mapper.Map(viewModel);

            Assert.AreEqual(viewModel.AccountHashedId, result.AccountHashedId);
        }

        [Test, MoqAutoData]
        public async Task Ukprn_IsMapped(EnterNewTrainingProviderViewModel viewModel)
        {
            var result = await _mapper.Map(viewModel);

            Assert.AreEqual(viewModel.Ukprn, result.Ukprn);
        }
    }
}
