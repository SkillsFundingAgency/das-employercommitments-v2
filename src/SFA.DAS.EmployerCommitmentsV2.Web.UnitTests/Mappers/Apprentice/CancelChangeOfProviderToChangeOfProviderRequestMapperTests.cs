﻿using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.Testing.AutoFixture;
using System.Text;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Apprentice;

public class CancelChangeOfProviderToChangeOfProviderRequestMapperTests
{
    private CancelChangeOfProviderToChangeOfProviderRequestMapper _mapper;

    [SetUp]
    public void Arrange()
    {
        _mapper = new CancelChangeOfProviderToChangeOfProviderRequestMapper();
    }

    [Test, MoqAutoData]
    public async Task ApprenticeshipHashedId_IsMapped(CancelChangeOfProviderRequestViewModel viewModel)
    {
        var result = await _mapper.Map(viewModel);

        Assert.That(result.ApprenticeshipHashedId, Is.EqualTo(viewModel.ApprenticeshipHashedId));
    }

    [Test, MoqAutoData]
    public async Task AccountHashedId_IsMapped(CancelChangeOfProviderRequestViewModel viewModel)
    {
        var result = await _mapper.Map(viewModel);

        Assert.That(result.AccountHashedId, Is.EqualTo(viewModel.AccountHashedId));
    }

    [Test, MoqAutoData]
    public async Task Provider_IsMapped(CancelChangeOfProviderRequestViewModel viewModel)
    {
        var result = await _mapper.Map(viewModel);

        Assert.That(result.ProviderId, Is.EqualTo(viewModel.ProviderId));
    }

    [Test, MoqAutoData]
    public async Task ProviderName_IsMapped(CancelChangeOfProviderRequestViewModel viewModel)
    {
        var result = await _mapper.Map(viewModel);

        Assert.That(result.ProviderName, Is.EqualTo(viewModel.ProviderName));
    }

    [Test, MoqAutoData]
    public async Task EmployerWillAdd_IsMapped(CancelChangeOfProviderRequestViewModel viewModel)
    {
        var result = await _mapper.Map(viewModel);

        Assert.That(result.EmployerWillAdd, Is.EqualTo(viewModel.EmployerWillAdd));
    }

    [Test, MoqAutoData]
    public async Task NewStartMonth_IsMapped(CancelChangeOfProviderRequestViewModel viewModel)
    {
        var result = await _mapper.Map(viewModel);

        Assert.That(result.NewStartMonth, Is.EqualTo(viewModel.NewStartMonth));
    }

    [Test, MoqAutoData]
    public async Task NewStartYear_IsMapped(CancelChangeOfProviderRequestViewModel viewModel)
    {
        var result = await _mapper.Map(viewModel);

        Assert.That(result.NewStartYear, Is.EqualTo(viewModel.NewStartYear));
    }

    [Test, MoqAutoData]
    public async Task NewEndMonth_IsMapped(CancelChangeOfProviderRequestViewModel viewModel)
    {
        var result = await _mapper.Map(viewModel);

        Assert.That(result.NewEndMonth, Is.EqualTo(viewModel.NewEndMonth));
    }

    [Test, MoqAutoData]
    public async Task NewEndYear_IsMapped(CancelChangeOfProviderRequestViewModel viewModel)
    {
        var result = await _mapper.Map(viewModel);

        Assert.That(result.NewEndYear, Is.EqualTo(viewModel.NewEndYear));
    }

    [Test, MoqAutoData]
    public async Task NewPrice_IsMapped(CancelChangeOfProviderRequestViewModel viewModel)
    {
        var result = await _mapper.Map(viewModel);

        Assert.That(result.NewPrice, Is.EqualTo(viewModel.NewPrice));
    }
}