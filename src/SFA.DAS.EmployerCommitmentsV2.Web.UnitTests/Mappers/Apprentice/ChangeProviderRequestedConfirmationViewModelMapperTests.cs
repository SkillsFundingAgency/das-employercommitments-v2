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
    public class ChangeProviderRequestedConfirmationViewModelMapperTests
    {
        private ChangeProviderRequestedConfirmationViewModelMapper _mapper;

        [SetUp]
        public void Arrange()
        {
            _mapper = new ChangeProviderRequestedConfirmationViewModelMapper();
        }

        [Test, MoqAutoData]
        public async Task ApprenticeshipHashedId_IsMapped(ChangeProviderRequestedConfirmationRequest request)
        {
            var result = await _mapper.Map(request);

            Assert.AreEqual(request.ApprenticeshipHashedId, result.ApprenticeshipHashedId);
        }

        [Test, MoqAutoData]
        public async Task AccountHashedId_IsMapped(ChangeProviderRequestedConfirmationRequest request)
        {
            var result = await _mapper.Map(request);

            Assert.AreEqual(request.AccountHashedId, result.AccountHashedId);
        }

    }
}
