using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Apprentice;

[TestFixture]
public class ResumeRequestViewModelToResumeApprenticeshipRequestMapperTests
{
    private ResumeRequestViewModelToResumeApprenticeshipRequestMapper _mapper;
    private ResumeRequestViewModel _source;
    private ResumeApprenticeshipRequest _result;

    [SetUp]
    public async Task Arrange()
    {
        //Arrange
        var fixture = new Fixture();
        _source = fixture.Create<ResumeRequestViewModel>();
           
        _mapper = new ResumeRequestViewModelToResumeApprenticeshipRequestMapper();

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