using System.Linq;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.CommitmentsV2.Types.Dtos;
using SFA.DAS.EmployerCommitmentsV2.Web.Exceptions;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.DraftApprenticeship;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.DraftApprenticeship;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.DraftApprenticeship;

[TestFixture]
public class DeleteDraftApprenticeshipViewModelMapperTests 
{
    [Test]
    public async Task WhenCohortExistsAndWithCorrectPartyAndApprenticeshipFound_ThenCallsCommitmentsApiToGetCohort()
    {
        var f = new DeleteDraftApprenticeshipViewModelMapperTestsFixture().WithParty(Party.Employer).ApprenticeshipsExists();

        await f.Sut.Map(f.DeleteApprenticeshipRequest);

        f.VerifyGetCohortIsCalledCorrectly();
    }

    [Test]
    public async Task WhenCohortExistsAndWithCorrectPartyAndApprenticeshipFound_ThenCallsCommitmentsApiToGetDraftApprenticeships()
    {
        var f = new DeleteDraftApprenticeshipViewModelMapperTestsFixture().WithParty(Party.Employer).ApprenticeshipsExists();

        await f.Sut.Map(f.DeleteApprenticeshipRequest);

        f.VerifyGetDraftApprenticeshipsIsCalledCorrectly();
    }

    [Test]
    public async Task WhenCohortExistsAndWithCorrectPartyAndApprenticeshipFound_ThenIsLastApprenticeshipIsFalse()
    {
        var f = new DeleteDraftApprenticeshipViewModelMapperTestsFixture().WithParty(Party.Employer).ApprenticeshipsExists();

        var result = await f.Sut.Map(f.DeleteApprenticeshipRequest);

        Assert.That(result.IsLastApprenticeshipInCohort, Is.False);
    }

    [Test]
    public async Task WhenCohortExistsAndWithCorrectPartyAndThisIsTheLastApprenticeship_ThenMarksModelAsIsLastApprenticeship()
    {
        var f = new DeleteDraftApprenticeshipViewModelMapperTestsFixture().WithParty(Party.Employer).WithSingleApprenticeship();

        var result = await f.Sut.Map(f.DeleteApprenticeshipRequest);

        Assert.That(result.IsLastApprenticeshipInCohort, Is.True);
    }

    [Test]
    public void WhenCohortIsWithTheProvider_ThenThrowsException()
    {
        var f = new DeleteDraftApprenticeshipViewModelMapperTestsFixture().WithParty(Party.Provider).ApprenticeshipsExists();

        Assert.ThrowsAsync<CohortEmployerUpdateDeniedException>(async () => await f.Sut.Map(f.DeleteApprenticeshipRequest));
    }

    [Test]
    public void WhenCohortExistsAndWithCorrectPartyButNoMatchingApprenticeship_ThenThrowsException()
    {
        var f = new DeleteDraftApprenticeshipViewModelMapperTestsFixture().WithParty(Party.Employer).WithNoMatchingApprentices();

        Assert.ThrowsAsync<DraftApprenticeshipNotFoundException>(async () => await f.Sut.Map(f.DeleteApprenticeshipRequest));
    }

    [Test, MoqAutoData]
    public async Task ThenMapsRequestValuesToViewModel()
    {
        var f = new DeleteDraftApprenticeshipViewModelMapperTestsFixture().WithParty(Party.Employer).WithSingleApprenticeship();

        var result = await f.Sut.Map(f.DeleteApprenticeshipRequest);

        Assert.That(result.FirstName, Is.EqualTo(f.SingleApprenticeship.FirstName));
        Assert.That(result.LastName, Is.EqualTo(f.SingleApprenticeship.LastName));
        Assert.That(result.FullName, Is.EqualTo(f.SingleApprenticeship.FirstName + " " + f.SingleApprenticeship.LastName));
        Assert.That(result.AccountHashedId, Is.EqualTo(f.DeleteApprenticeshipRequest.AccountHashedId));
        Assert.That(result.DraftApprenticeshipHashedId, Is.EqualTo(f.DeleteApprenticeshipRequest.DraftApprenticeshipHashedId));
        Assert.That(result.AccountHashedId, Is.EqualTo(f.DeleteApprenticeshipRequest.AccountHashedId));
        Assert.That(result.Origin, Is.EqualTo(f.DeleteApprenticeshipRequest.Origin));
        Assert.That(result.CohortReference, Is.EqualTo(f.DeleteApprenticeshipRequest.CohortReference));
        Assert.That(result.LegalEntityName, Is.EqualTo(f.GetCohortResponse.LegalEntityName));
        Assert.That(result.IsLastApprenticeshipInCohort, Is.True);
    }

    public class DeleteDraftApprenticeshipViewModelMapperTestsFixture
    {
        public Mock<ICommitmentsApiClient> CommitmentsApiClient { get; }
        public GetCohortResponse GetCohortResponse { get; }
        public GetDraftApprenticeshipsResponse GetDraftApprenticeshipsResponse { get; }
        public DeleteApprenticeshipRequest DeleteApprenticeshipRequest { get; private set; }
        public DeleteDraftApprenticeshipViewModelMapper Sut { get; }
        public DraftApprenticeshipDto SingleApprenticeship { get; private set; }

        private readonly Fixture _autoFixture;

        public DeleteDraftApprenticeshipViewModelMapperTestsFixture()
        {
            _autoFixture = new Fixture();

            GetCohortResponse = _autoFixture.Create<GetCohortResponse>();
            GetDraftApprenticeshipsResponse = _autoFixture.Create<GetDraftApprenticeshipsResponse>();
            CommitmentsApiClient = new Mock<ICommitmentsApiClient>();
            CommitmentsApiClient.Setup(x => x.GetCohort(It.IsAny<long>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(GetCohortResponse);
            CommitmentsApiClient.Setup(x => x.GetDraftApprenticeships(It.IsAny<long>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(GetDraftApprenticeshipsResponse);

            Sut = new DeleteDraftApprenticeshipViewModelMapper(CommitmentsApiClient.Object);
        }

        public DeleteDraftApprenticeshipViewModelMapperTestsFixture WithParty(Party party)
        {
            GetCohortResponse.WithParty = party;
            return this;
        }

        public DeleteDraftApprenticeshipViewModelMapperTestsFixture ApprenticeshipsExists()
        {
            var apprenticeId = GetDraftApprenticeshipsResponse.DraftApprenticeships.ToArray()[0].Id;

            DeleteApprenticeshipRequest = _autoFixture.Build<DeleteApprenticeshipRequest>()
                .With(x => x.DraftApprenticeshipId, apprenticeId).Create();
            return this;
        }

        public DeleteDraftApprenticeshipViewModelMapperTestsFixture WithSingleApprenticeship()
        {
            SingleApprenticeship = _autoFixture.Create<DraftApprenticeshipDto>();
            GetDraftApprenticeshipsResponse.DraftApprenticeships = new List<DraftApprenticeshipDto> {SingleApprenticeship};

            CommitmentsApiClient.Setup(x => x.GetDraftApprenticeships(It.IsAny<long>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(GetDraftApprenticeshipsResponse);

            DeleteApprenticeshipRequest = _autoFixture.Build<DeleteApprenticeshipRequest>()
                .With(x => x.DraftApprenticeshipId, SingleApprenticeship.Id).Create();
            return this;
        }

        public DeleteDraftApprenticeshipViewModelMapperTestsFixture WithNoMatchingApprentices()
        {
            CommitmentsApiClient.Setup(x => x.GetDraftApprenticeships(It.IsAny<long>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(GetDraftApprenticeshipsResponse);
            DeleteApprenticeshipRequest = _autoFixture.Create<DeleteApprenticeshipRequest>();
            return this;
        }

        public void VerifyGetCohortIsCalledCorrectly()
        {
            CommitmentsApiClient.Verify(x => x.GetCohort(DeleteApprenticeshipRequest.CohortId, CancellationToken.None), Times.Once);
        }

        public void VerifyGetDraftApprenticeshipsIsCalledCorrectly()
        {
            CommitmentsApiClient.Verify(x => x.GetDraftApprenticeships(DeleteApprenticeshipRequest.CohortId, CancellationToken.None), Times.Once);
        }
    }
}