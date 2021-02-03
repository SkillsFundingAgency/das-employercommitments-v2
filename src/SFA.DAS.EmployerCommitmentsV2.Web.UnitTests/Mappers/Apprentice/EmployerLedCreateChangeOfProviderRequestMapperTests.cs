using AutoFixture;
using NUnit.Framework;
using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using System;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Apprentice
{
    public class EmployerLedCreateChangeOfProviderRequestMapperTests
    {
        private ConfirmDetailsAndSendViewModel _viewModel;
        private CreateChangeOfPartyRequestRequest _result;

        [SetUp]
        public async Task Setup()
        {
            var fixture = new Fixture();

            _viewModel = fixture.Build<ConfirmDetailsAndSendViewModel>()
                .With(x => x.NewStartDate, DateTime.Now)
                .With(x => x.NewEndDate, DateTime.Now.AddYears(1))
                .Create();

            var mapper = new EmployerLedCreateChangeOfProviderRequestMapper();

            _result = await mapper.Map(_viewModel);
        }

        [Test]
        public void NewProviderId_IsMapped()
        {
            Assert.AreEqual(_viewModel.ProviderId, _result.NewPartyId);
        }

        [Test]
        public void NewStartDate_IsMapped()
        {
            Assert.AreEqual(_viewModel.NewStartDate, _result.NewStartDate);
        }

        [Test]
        public void NewEndDate_IsMapped()
        {
            Assert.AreEqual(_viewModel.NewEndDate, _result.NewEndDate);
        }

        [Test]
        public void NewPrice_IsMapped()
        {
            Assert.AreEqual(_viewModel.NewPrice, _result.NewPrice);
        }

        [Test]
        public void ChangeOfPartyRequestType_IsMapped()
        {
            Assert.AreEqual(ChangeOfPartyRequestType.ChangeProvider, _result.ChangeOfPartyRequestType);
        }
    }
}
