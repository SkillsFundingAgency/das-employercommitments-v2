using System;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.DraftApprenticeship;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.DraftApprenticeship;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.DraftApprenticeship
{
    [TestFixture]
    public class IDraftApprenticeshipDetailsViewModelMapperTests
    {
        private IDraftApprenticeshipDetailsViewModelMapper _mapper;
        private Mock<ICommitmentsApiClient> _apiClient;
        private Mock<IModelMapper> _modelMapper;
        private GetCohortResponse _cohort;
        private DetailsRequest _request;

        [SetUp]
        public void Arrange()
        {
            var autoFixture = new Fixture();
            _cohort = autoFixture.Create<GetCohortResponse>();

            _request = autoFixture.Create<DetailsRequest>();

            _apiClient = new Mock<ICommitmentsApiClient>();

            _apiClient.Setup(x => x.GetCohort(It.IsAny<long>(), It.IsAny<CancellationToken>())).ReturnsAsync(_cohort);

            _modelMapper = new Mock<IModelMapper>();

            _modelMapper.Setup(x => x.Map<IDraftApprenticeshipViewModel>(It.IsAny<EditDraftApprenticeshipRequest>()))
                .ReturnsAsync(new EditDraftApprenticeshipViewModel());

            _modelMapper.Setup(x => x.Map<IDraftApprenticeshipViewModel>(It.IsAny<ViewDraftApprenticeshipRequest>()))
                .ReturnsAsync(new ViewDraftApprenticeshipViewModel());

            _mapper = new IDraftApprenticeshipDetailsViewModelMapper(_apiClient.Object, _modelMapper.Object);
        }

        [TestCase(Party.Employer, typeof(EditDraftApprenticeshipRequest))]
        [TestCase(Party.Provider, typeof(ViewDraftApprenticeshipRequest))]
        [TestCase(Party.TransferSender, typeof(ViewDraftApprenticeshipRequest))]
        public async Task When_Mapping_The_Mapping_Request_Is_Passed_On_To_The_Appropriate_Mapper(Party withParty, Type expectedMappingRequestType)
        {
            _cohort.WithParty = withParty;
            await _mapper.Map(_request);

            _modelMapper.Verify(
                x => x.Map<IDraftApprenticeshipViewModel>(It.Is<IDraftApprenticeshipRequest>(
                    r => r.GetType() == expectedMappingRequestType
                         && r.Request == _request
                         && r.Cohort == _cohort)),
                Times.Once);
        }
    }
}
