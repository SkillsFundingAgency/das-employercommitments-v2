using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.Testing.AutoFixture;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Apprentice;

public class EnterNewTrainingProviderViewModelMapperTests
{
    private Mock<ICommitmentsApiClient> _mockCommitmentsApiClient;

    private GetApprenticeshipResponse _apprenticeshipResponse;

    private EnterNewTrainingProviderViewModelMapper _mapper;

    [SetUp]
    public void Arrange()
    {
        var _autoFixture = new Fixture();

        _apprenticeshipResponse = _autoFixture.Build<GetApprenticeshipResponse>().Create();

        _mockCommitmentsApiClient = new Mock<ICommitmentsApiClient>();

        _mockCommitmentsApiClient.Setup(m => m.GetAllProviders(It.IsAny<CancellationToken>()))
            .ReturnsAsync(MockGetAllProvidersResponse());

        _mockCommitmentsApiClient.Setup(m => m.GetApprenticeship(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(_apprenticeshipResponse);

        _mapper = new EnterNewTrainingProviderViewModelMapper(_mockCommitmentsApiClient.Object);
    }

    [Test, MoqAutoData]
    public async Task ApprenticeshipHashedId_IsMapped(ChangeOfProviderRequest request)
    {
        var result = await _mapper.Map(request);

        Assert.That(result.ApprenticeshipHashedId, Is.EqualTo(request.ApprenticeshipHashedId));
    }

    [Test, MoqAutoData]
    public async Task AccountHashedId_IsMapped(ChangeOfProviderRequest request)
    {
        var result = await _mapper.Map(request);

        Assert.That(result.AccountHashedId, Is.EqualTo(request.AccountHashedId));
    }

    [Test, MoqAutoData]
    public async Task NewProviderId_IsMapped(ChangeOfProviderRequest request)
    {
        var result = await _mapper.Map(request);

        Assert.That(result.ProviderId, Is.EqualTo(request.ProviderId));
    }

    [Test, MoqAutoData]
    public async Task WhenRequestingEnterNewTrainingProvider_ThenListOfGetAllProvidersCalled(ChangeOfProviderRequest request)
    {
        var result = await _mapper.Map(request);

        _mockCommitmentsApiClient.Verify(m => m.GetAllProviders(It.IsAny<CancellationToken>()), Times.Once());
    }

    [Test, MoqAutoData]
    public async Task WhenRequestingEnterNewTrainingProvider_ThenListOfTrainingProvidersIsMapped(ChangeOfProviderRequest request)
    {
        var result = await _mapper.Map(request);

        Assert.That(result.Providers.Count, Is.EqualTo(3));
    }

    [Test, MoqAutoData]
    public async Task WhenRequestingEnterNewTrainingProvider_ThenCurrentProviderIsMapped(ChangeOfProviderRequest request)
    {
        var result = await _mapper.Map(request);

        _mockCommitmentsApiClient.Verify(c => c.GetApprenticeship(It.Is<long>(id => id == request.ApprenticeshipId), It.IsAny<CancellationToken>()), Times.Once);
        Assert.That(result.CurrentProviderId, Is.EqualTo(_apprenticeshipResponse.ProviderId));
    }
    private GetAllProvidersResponse MockGetAllProvidersResponse()
    {
        return new GetAllProvidersResponse
        {
            Providers = new List<Provider>
            {
                new Provider { Ukprn = 10000001, Name = "Provider 1" },
                new Provider { Ukprn = 10000002, Name = "Provider 2" },
                new Provider { Ukprn = 10000003, Name = "Provider 3" }
            }
        };
    }
}