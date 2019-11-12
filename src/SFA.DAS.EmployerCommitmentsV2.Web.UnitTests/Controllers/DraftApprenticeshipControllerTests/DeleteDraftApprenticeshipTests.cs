using System;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Exceptions;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.DraftApprenticeship;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.DraftApprenticeshipControllerTests
{
    [TestFixture]
    public class DeleteDraftApprenticeshipTests
    {

        [Test, MoqAutoData]
        public async Task WhenGettingDelete_ThenCallsMapper(
            [Frozen] Mock<IModelMapper> mockMapper,
            DeleteApprenticeshipRequest request,
            DraftApprenticeshipController controller)
        {
            mockMapper
                .Setup(x => x.Map<DeleteDraftApprenticeshipViewModel>(request))
                .ReturnsAsync(new DeleteDraftApprenticeshipViewModel());

            await controller.DeleteDraftApprenticeship(request);

            mockMapper.Verify(x => x.Map<DeleteDraftApprenticeshipViewModel>(request));
        }

        [Test, MoqAutoData]
        public async Task WhenGettingDelete_AndMapperThrowsError_ThenRedirectsToOrigin(
            [Frozen] Mock<IModelMapper> mockMapper,
            DeleteApprenticeshipRequest request,
            DraftApprenticeshipController controller)
        {
            mockMapper
                .Setup(x => x.Map<DeleteDraftApprenticeshipViewModel>(request))
                .ThrowsAsync(new CohortEmployerUpdateDeniedException($"Cohort {request.CohortId} is not With the Employer"));

            await controller.DeleteDraftApprenticeship(request);

            mockMapper.Verify(x => x.Map<DeleteDraftApprenticeshipViewModel>(request));
        }

    }
}
