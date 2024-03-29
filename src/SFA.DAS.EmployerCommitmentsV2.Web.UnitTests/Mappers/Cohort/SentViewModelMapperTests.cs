﻿using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.Encoding;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Cohort;

[TestFixture]
[Parallelizable(ParallelScope.All)]
public class SentViewModelMapperTests
{
    [Test]
    public async Task AccountHashedIdIsMappedCorrectly()
    {
        var fixture = new SentViewModelMapperTestsFixture();
        var result = await fixture.Map();
        Assert.That(result.AccountHashedId, Is.EqualTo(fixture.Source.AccountHashedId));
    }

    [Test]
    public async Task LegalEntityNameIsMappedCorrectly()
    {
        var fixture = new SentViewModelMapperTestsFixture();
        var result = await fixture.Map();
        Assert.That(result.LegalEntityName, Is.EqualTo(fixture.Cohort.LegalEntityName));
    }

    [Test]
    public async Task ProviderNameIsMappedCorrectly()
    {
        var fixture = new SentViewModelMapperTestsFixture();
        var result = await fixture.Map();
        Assert.That(result.ProviderName, Is.EqualTo(fixture.Cohort.ProviderName));
    }

    [Test]
    public async Task CohortReferenceIsMappedCorrectly()
    {
        var fixture = new SentViewModelMapperTestsFixture();
        var result = await fixture.Map();
        Assert.That(result.CohortReference, Is.EqualTo(fixture.Source.CohortReference));
    }

    [Test]
    public async Task CohortIdIsMappedCorrectly()
    {
        var fixture = new SentViewModelMapperTestsFixture();
        var result = await fixture.Map();
        Assert.That(result.CohortId, Is.EqualTo(fixture.Source.CohortId));
    }
}

public class SentViewModelMapperTestsFixture
{
    public SentViewModelMapper Mapper;
    public SentRequest Source;
    public SentViewModel Result;
    public Mock<ICommitmentsApiClient> CommitmentsApiClient;
    public GetCohortResponse Cohort;
    public GetDraftApprenticeshipsResponse DraftApprenticeshipsResponse;
    private Fixture _autoFixture;

    public SentViewModelMapperTestsFixture()
    {
        _autoFixture = new Fixture();

        Cohort = _autoFixture.Create<GetCohortResponse>();

        DraftApprenticeshipsResponse = _autoFixture.Create<GetDraftApprenticeshipsResponse>();

        CommitmentsApiClient = new Mock<ICommitmentsApiClient>();
        CommitmentsApiClient.Setup(x => x.GetCohort(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Cohort);

        CommitmentsApiClient.Setup(x => x.GetDraftApprenticeships(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(DraftApprenticeshipsResponse);

        Mapper = new SentViewModelMapper(CommitmentsApiClient.Object);
        Source = _autoFixture.Create<SentRequest>();
    }

    public Task<SentViewModel> Map()
    {
        return Mapper.Map(TestHelper.Clone(Source));
    }
}