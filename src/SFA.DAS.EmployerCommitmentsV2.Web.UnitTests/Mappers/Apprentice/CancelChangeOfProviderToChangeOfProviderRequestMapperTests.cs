using AutoFixture;
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
    public class CancelChangeOfProviderToChangeOfProviderRequestMapperTests
    {
        private CancelChangeOfProviderToChangeOfProviderRequestMapper _mapper;

        [SetUp]
        public void Arrange()
        {
            _mapper = new CancelChangeOfProviderToChangeOfProviderRequestMapper();
        }

        [Test, MoqAutoData]
        public async Task ApprenticeshipHashedId_IsMapped(CancelChangeOfProviderRequestViewModel viewModel)
        {
            var result = await _mapper.Map(viewModel);

            Assert.AreEqual(viewModel.ApprenticeshipHashedId, result.ApprenticeshipHashedId);
        }

        [Test, MoqAutoData]
        public async Task AccountHashedId_IsMapped(CancelChangeOfProviderRequestViewModel viewModel)
        {
            var result = await _mapper.Map(viewModel);

            Assert.AreEqual(viewModel.AccountHashedId, result.AccountHashedId);
        }

        [Test, MoqAutoData]
        public async Task Provider_IsMapped(CancelChangeOfProviderRequestViewModel viewModel)
        {
            var result = await _mapper.Map(viewModel);

            Assert.AreEqual(viewModel.ProviderId, result.ProviderId);
        }

        [Test, MoqAutoData]
        public async Task ProviderName_IsMapped(CancelChangeOfProviderRequestViewModel viewModel)
        {
            var result = await _mapper.Map(viewModel);

            Assert.AreEqual(viewModel.ProviderName, result.ProviderName);
        }

        [Test, MoqAutoData]
        public async Task EmployerWillAdd_IsMapped(CancelChangeOfProviderRequestViewModel viewModel)
        {
            var result = await _mapper.Map(viewModel);

            Assert.AreEqual(viewModel.EmployerWillAdd, result.EmployerWillAdd);
        }

        [Test, MoqAutoData]
        public async Task NewStartMonth_IsMapped(CancelChangeOfProviderRequestViewModel viewModel)
        {
            var result = await _mapper.Map(viewModel);

            Assert.AreEqual(viewModel.NewStartMonth, result.NewStartMonth);
        }

        [Test, MoqAutoData]
        public async Task NewStartYear_IsMapped(CancelChangeOfProviderRequestViewModel viewModel)
        {
            var result = await _mapper.Map(viewModel);

            Assert.AreEqual(viewModel.NewStartYear, result.NewStartYear);
        }

        [Test, MoqAutoData]
        public async Task NewEndMonth_IsMapped(CancelChangeOfProviderRequestViewModel viewModel)
        {
            var result = await _mapper.Map(viewModel);

            Assert.AreEqual(viewModel.NewEndMonth, result.NewEndMonth);
        }

        [Test, MoqAutoData]
        public async Task NewEndYear_IsMapped(CancelChangeOfProviderRequestViewModel viewModel)
        {
            var result = await _mapper.Map(viewModel);

            Assert.AreEqual(viewModel.NewEndYear, result.NewEndYear);
        }

        [Test, MoqAutoData]
        public async Task NewPrice_IsMapped(CancelChangeOfProviderRequestViewModel viewModel)
        {
            var result = await _mapper.Map(viewModel);

            Assert.AreEqual(viewModel.NewPrice, result.NewPrice);
        }
    }
}
