﻿using NUnit.Framework;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.Testing.AutoFixture;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Apprentice
{
    public class EditEndDateViewModelMapperTests
    {
        [Test, MoqAutoData]
        public async Task ApprenticeshipHashedId_IsMapped(
            EditEndDateRequest request)
        {
            var mapper = new EditEndDateViewModelMapper();
            var result = await mapper.Map(request);

            Assert.AreEqual(request.ApprenticeshipHashedId, result.ApprenticeshipHashedId);
        }

        [Test, MoqAutoData]
        public async Task AccountHashedId_IsMapped(
            EditEndDateRequest request)
        {
            var mapper = new EditEndDateViewModelMapper();
            var result = await mapper.Map(request);

            Assert.AreEqual(request.AccountHashedId, result.AccountHashedId);
        }
    }
}
