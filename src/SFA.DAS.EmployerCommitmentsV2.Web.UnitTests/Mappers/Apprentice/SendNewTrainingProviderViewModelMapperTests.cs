using AutoFixture;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Apprentice;

public class SendNewTrainingProviderViewModelMapperTests
{
    private Mock<ICommitmentsApiClient> _commitmentApiClient;
    private GetApprenticeshipResponse _apprenticeshipResponse;
    private GetProviderResponse _providerResponse;
    private SendNewTrainingProviderViewModelMapper _sut;
    private SendNewTrainingProviderRequest _request;

    [SetUp]
    public void Arrange()
    {
        var autoFixture = new Fixture();

        _apprenticeshipResponse = autoFixture.Build<GetApprenticeshipResponse>()
            .Create();
        _providerResponse = autoFixture.Build<GetProviderResponse>()
            .Create();
        _request = autoFixture.Build<SendNewTrainingProviderRequest>()
            .Create();


        _commitmentApiClient = new Mock<ICommitmentsApiClient>();
        _commitmentApiClient.Setup(a => a.GetApprenticeship(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(_apprenticeshipResponse);

        _commitmentApiClient.Setup(a => a.GetProvider(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(_providerResponse);

        _sut = new SendNewTrainingProviderViewModelMapper(_commitmentApiClient.Object, Mock.Of<ILogger<SendNewTrainingProviderViewModelMapper>>());
    }

    [Test]
    public async Task ApprenticeshipHashedId_IsMapped()
    {
        var result = await _sut.Map(_request);

        Assert.That(result.ApprenticeshipHashedId, Is.EqualTo(_request.ApprenticeshipHashedId));
    }

    [Test]
    public async Task AccountHashedId_IsMapped()
    {
        var result = await _sut.Map(_request);

        Assert.That(result.AccountHashedId, Is.EqualTo(_request.AccountHashedId));
    }

    [Test]
    public async Task EmployerName_IsMapped()
    {
        var result = await _sut.Map(_request);

        Assert.That(result.EmployerName, Is.EqualTo(_apprenticeshipResponse.EmployerName));
    }

    [Test]
    public async Task ApprenticeName_IsMapped()
    {
        var result = await _sut.Map(_request);

        Assert.That(result.ApprenticeName, Is.EqualTo($"{_apprenticeshipResponse.FirstName} {_apprenticeshipResponse.LastName}"));
    }

    [Test]
    public async Task OldProviderName_IsMapped()
    {
        var result = await _sut.Map(_request);

        Assert.That(result.OldProviderName, Is.EqualTo(_apprenticeshipResponse.ProviderName));
    }

    [Test]
    public async Task NewProviderName_IsMapped()
    {
        var result = await _sut.Map(_request);

        Assert.That(result.NewProviderName, Is.EqualTo(_providerResponse.Name));
    }

    [Test]
    public async Task ProviderId_IsMapped()
    {
        var result = await _sut.Map(_request);

        Assert.That(result.ProviderId, Is.EqualTo(_request.ProviderId));
    }

    [Test]
    public async Task ApprenticeshipStatus_IsMapped()
    {
        var result = await _sut.Map(_request);

        Assert.That(result.ApprenticeshipStatus, Is.EqualTo(_apprenticeshipResponse.Status));
    }
}