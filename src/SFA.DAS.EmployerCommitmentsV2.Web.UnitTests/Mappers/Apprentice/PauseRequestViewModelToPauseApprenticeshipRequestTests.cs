﻿using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Apprentice;

[TestFixture]
public class PauseRequestViewModelToPauseApprenticeshipRequestMapperTests
{
    private PauseRequestViewModelToPauseApprenticeshipRequestMapper _mapper;
    private PauseRequestViewModel _source;
    private PauseApprenticeshipRequest _result;

    [SetUp]
    public async Task Arrange()
    {
        //Arrange
        var fixture = new Fixture();
        _source = fixture.Create<PauseRequestViewModel>();
           
        _mapper = new PauseRequestViewModelToPauseApprenticeshipRequestMapper();

        //Act            
        _result = await _mapper.Map(_source);
    }


    [Test]
    public void ApprenticeshipIdIsMappedCorrectly()
    {
        //Assert
        Assert.That(_result.ApprenticeshipId, Is.EqualTo(_source.ApprenticeshipId));
    }
}