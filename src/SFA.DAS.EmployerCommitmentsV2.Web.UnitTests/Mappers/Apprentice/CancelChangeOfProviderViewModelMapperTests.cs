using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Apprentice;

public class CancelChangeOfProviderViewModelMapperTests
{
    private Mock<ICommitmentsApiClient> _mockCommitmentsApiClient;

    private ChangeOfProviderRequest _request;
    private GetApprenticeshipResponse _apprenticeshipResponse;

    private CancelChangeOfProviderRequestViewModelMapper _mapper;

    [SetUp]
    public void Arrange()
    {
        var autoFixture = new Fixture();

        _request = autoFixture.Create<ChangeOfProviderRequest>();
        _apprenticeshipResponse = autoFixture.Create<GetApprenticeshipResponse>();

        _mockCommitmentsApiClient = new Mock<ICommitmentsApiClient>();
        _mockCommitmentsApiClient.Setup(m => m.GetApprenticeship(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(_apprenticeshipResponse);

        _mapper = new CancelChangeOfProviderRequestViewModelMapper(_mockCommitmentsApiClient.Object);
    }

    [Test]
    public async Task ApprenticeshipHashedId_IsMapped()
    {
        var result = await _mapper.Map(_request);

        Assert.That(result.ApprenticeshipHashedId, Is.EqualTo(_request.ApprenticeshipHashedId));
    }

    [Test]
    public async Task AccountHashedId_IsMapped()
    {
        var result = await _mapper.Map(_request);

        Assert.That(result.AccountHashedId, Is.EqualTo(_request.AccountHashedId));
    }

    [Test]
    public async Task ProviderId_IsMapped()
    {
        var result = await _mapper.Map(_request);

        Assert.That(result.ProviderId, Is.EqualTo(_request.ProviderId));
    }

    [Test]
    public async Task ProviderName_IsMapped()
    {
        var result = await _mapper.Map(_request);

        Assert.That(result.ProviderName, Is.EqualTo(_request.ProviderName));
    }

    [Test]
    public async Task NewStartDate_IsMapped()
    {
        var result = await _mapper.Map(_request);

        Assert.That(result.NewStartMonth, Is.EqualTo(_request.NewStartMonth));
        Assert.That(result.NewStartYear, Is.EqualTo(_request.NewStartYear));
    }

    [Test]
    public async Task NewEndDate_IsMapped()
    {
        var result = await _mapper.Map(_request);

        Assert.That(result.NewEndMonth, Is.EqualTo(_request.NewEndMonth));
        Assert.That(result.NewEndYear, Is.EqualTo(_request.NewEndYear));
    }

    [Test]
    public async Task NewPrice_IsMapped()
    {
        var result = await _mapper.Map(_request);

        Assert.That(result.NewPrice, Is.EqualTo(_request.NewPrice));
    }

    [Test]
    public async Task EmployerWillAdd_IsMapped()
    {
        var result = await _mapper.Map(_request);

        Assert.That(result.EmployerWillAdd, Is.EqualTo(_request.EmployerWillAdd));
    }

    [Test]
    public async Task ApprenticeName_IsMapped()
    {
        var result = await _mapper.Map(_request);

        Assert.That(result.ApprenticeName, Is.EqualTo($"{_apprenticeshipResponse.FirstName} {_apprenticeshipResponse.LastName}"));
    }

    [Test]
    public async Task OldProviderName_IsMapped()
    {
        var result = await _mapper.Map(_request);

        Assert.That(result.OldProviderName, Is.EqualTo(_apprenticeshipResponse.ProviderName));
    }

    [Test]
    public async Task WhenRequestingTheCancelChangeOfProviderRequestPage_ThenTheGetApprenticeshipIsCalledOnce()
    {
        await _mapper.Map(_request);

        _mockCommitmentsApiClient.Verify(m => m.GetApprenticeship(_request.ApprenticeshipId.Value, It.IsAny<CancellationToken>()), Times.Once());
    }
}