using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.CreateCohort;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers
{
    public class WhenMappingAssignRequestToViewModel
    {
        [Test, AutoData]
        public void Then_Maps_AccountHashedId(
            AssignRequest request,
            AssignViewModelMapper mapper)
        {
            var viewModel = mapper.Map(request);

            viewModel.AccountHashedId.Should().Be(request.AccountHashedId);
        }
    }
}