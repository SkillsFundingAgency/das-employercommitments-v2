using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Apprentice;

public class EnterNewProviderToChangeOfProviderRequestMapperTests
{
    private GetProviderResponse _getProviderResponse;
    private Mock<ICommitmentsApiClient> _mockCommitmentsApiClient;
    private EnterNewTrainingProviderToChangeOfProviderRequestMapper _mapper;

    [SetUp]
    public void Arrange()
    {
        _getProviderResponse = new GetProviderResponse { Name = "Test Provider" };
        _mockCommitmentsApiClient = new Mock<ICommitmentsApiClient>();
        _mockCommitmentsApiClient.Setup(c => c.GetProvider(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(_getProviderResponse);

        _mapper = new EnterNewTrainingProviderToChangeOfProviderRequestMapper(_mockCommitmentsApiClient.Object);
    }

    [Test, MoqAutoData]
    public async Task ApprenticeshipHashedId_IsMapped(EnterNewTrainingProviderViewModel viewModel)
    {
        var result = await _mapper.Map(viewModel);

        Assert.That(result.ApprenticeshipHashedId, Is.EqualTo(viewModel.ApprenticeshipHashedId));
    }

    [Test, MoqAutoData]
    public async Task AccountHashedId_IsMapped(EnterNewTrainingProviderViewModel viewModel)
    {
        var result = await _mapper.Map(viewModel);

        Assert.That(result.AccountHashedId, Is.EqualTo(viewModel.AccountHashedId));
    }

    [Test, MoqAutoData]
    public async Task Provider_IsMapped(EnterNewTrainingProviderViewModel viewModel)
    {
        var result = await _mapper.Map(viewModel);

        Assert.That(result.ProviderId, Is.EqualTo(viewModel.ProviderId));
    }

    [Test, MoqAutoData]
    public async Task ProviderName_IsMapped(EnterNewTrainingProviderViewModel viewModel)
    {
        var result = await _mapper.Map(viewModel);

        Assert.That(result.ProviderName, Is.EqualTo(_getProviderResponse.Name));
    }

    [Test, MoqAutoData]
    public async Task EmployerWillAdd_IsMapped(EnterNewTrainingProviderViewModel viewModel)
    {
        var result = await _mapper.Map(viewModel);

        Assert.That(result.EmployerWillAdd, Is.EqualTo(viewModel.EmployerWillAdd));
    }

    [Test, MoqAutoData]
    public async Task NewStartMonth_IsMapped(EnterNewTrainingProviderViewModel viewModel)
    {
        var result = await _mapper.Map(viewModel);

        Assert.That(result.NewStartMonth, Is.EqualTo(viewModel.NewStartMonth));
    }

    [Test, MoqAutoData]
    public async Task NewStartYear_IsMapped(EnterNewTrainingProviderViewModel viewModel)
    {
        var result = await _mapper.Map(viewModel);

        Assert.That(result.NewStartYear, Is.EqualTo(viewModel.NewStartYear));
    }

    [Test, MoqAutoData]
    public async Task NewEndMonth_IsMapped(EnterNewTrainingProviderViewModel viewModel)
    {
        var result = await _mapper.Map(viewModel);

        Assert.That(result.NewEndMonth, Is.EqualTo(viewModel.NewEndMonth));
    }

    [Test, MoqAutoData]
    public async Task NewEndYear_IsMapped(EnterNewTrainingProviderViewModel viewModel)
    {
        var result = await _mapper.Map(viewModel);

        Assert.That(result.NewEndYear, Is.EqualTo(viewModel.NewEndYear));
    }

    [Test, MoqAutoData]
    public async Task NewPrice_IsMapped(EnterNewTrainingProviderViewModel viewModel)
    {
        var result = await _mapper.Map(viewModel);

        Assert.That(result.NewPrice, Is.EqualTo(viewModel.NewPrice));
    }
}