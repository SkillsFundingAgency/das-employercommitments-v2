using FluentAssertions;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Contracts;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Responses;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Cohort;

[TestFixture]
[Parallelizable(ParallelScope.All)]
public class FinishedViewModelMapperTests
{
    [Test]
    public async Task AccountHashedIdIsMappedCorrectly()
    {
        var fixture = new FinishedViewModelMapperTestsFixture();
        var result = await fixture.Map();
        result.AccountHashedId.Should().Be(fixture.Source.AccountHashedId);
    }

    [Test]
    public async Task LegalEntityNameIsMappedCorrectly()
    {
        var fixture = new FinishedViewModelMapperTestsFixture();
        var result = await fixture.Map();
        result.LegalEntityName.Should().Be(fixture.Cohort.LegalEntityName);
    }

    [Test]
    public async Task ProviderNameIsMappedCorrectly()
    {
        var fixture = new FinishedViewModelMapperTestsFixture();
        var result = await fixture.Map();
        result?.ProviderName.Should().Be(fixture.Cohort.ProviderName);
    }

    [Test]
    public async Task MessageIsMappedCorrectly()
    {
        var fixture = new FinishedViewModelMapperTestsFixture();
        var result = await fixture.Map();
        result.Message.Should().Be(fixture.Cohort.LatestMessageCreatedByEmployer);
    }

    [Test]
    public async Task CohortReferenceIsMappedCorrectly()
    {
        var fixture = new FinishedViewModelMapperTestsFixture();
        var result = await fixture.Map();
        result.CohortReference.Should().Be(fixture.Source.CohortReference);
    }
}

public class FinishedViewModelMapperTestsFixture
{
    public FinishedRequestToFinishedViewModelMapper Mapper;
    public FinishedRequest Source;
    public FinishedViewModel Result;
    public Mock<IApprovalsApiClient> approvalsApiClient;
    public GetCohortDetailsResponse Cohort;
    public GetSelectFundingOptionsResponse selectFundingOptions;
    private Fixture _autoFixture;

    public FinishedViewModelMapperTestsFixture()
    {
        _autoFixture = new Fixture();

        Source = _autoFixture.Create<FinishedRequest>();

        Cohort = _autoFixture.Create<GetCohortDetailsResponse>();
        selectFundingOptions = _autoFixture.Create<GetSelectFundingOptionsResponse>();

        approvalsApiClient = new Mock<IApprovalsApiClient>();
        approvalsApiClient.Setup(x => x.GetCohortDetails(It.Is<long>(t => t == Source.AccountId), It.Is<long>(t => t == Source.CohortId)))
            .ReturnsAsync(Cohort);

        approvalsApiClient.Setup(x => x.GetSelectFundingOptions(It.Is<long>(t => t == Source.AccountId)))
            .ReturnsAsync(selectFundingOptions);

        Mapper = new FinishedRequestToFinishedViewModelMapper(approvalsApiClient.Object);
    }

    public FinishedViewModelMapperTestsFixture SetCohortWithParty(Party party)
    {
        Cohort.WithParty = party;
        return this;
    }

    public Task<FinishedViewModel> Map()
    {
        return Mapper.Map(TestHelper.Clone(Source));
    }
}