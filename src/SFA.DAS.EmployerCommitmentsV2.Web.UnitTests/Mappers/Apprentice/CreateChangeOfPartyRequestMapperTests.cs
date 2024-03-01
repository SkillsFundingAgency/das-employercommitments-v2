using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Apprentice;

[TestFixture]
public class CreateChangeOfPartyRequestMapperTests
{
    private CreateChangeOfPartyRequestMapper _mapper;
    private SendNewTrainingProviderViewModel _source;
    private CreateChangeOfPartyRequestRequest _result;

    [SetUp]
    public async Task Arrange()
    {
        //Arrange
        var fixture = new Fixture();
        _source = new SendNewTrainingProviderViewModel
        {                
            ProviderId = fixture.Create<long>(),
            AccountHashedId = fixture.Create<string>(),
            AccountId = fixture.Create<long>(),
            Confirm = fixture.Create<bool>(),
            OldProviderName = fixture.Create<string>(),
            NewProviderName = fixture.Create<string>(),
            EmployerName = fixture.Create<string>(),
            ApprenticeName = fixture.Create<string>(),
            ApprenticeshipStatus = ApprenticeshipStatus.Stopped,
            ApprenticeshipId = fixture.Create<int>(),
            ApprenticeshipHashedId = fixture.Create<string>()                        
        };
        _mapper = new CreateChangeOfPartyRequestMapper();

        //Act            
        _result = await _mapper.Map(TestHelper.Clone(_source));
    }

    [Test]
    public void ChangeOfPartyRequestTypeIsMappedCorrectly()
    {
        //Assert
        Assert.That(_result.ChangeOfPartyRequestType, Is.EqualTo(ChangeOfPartyRequestType.ChangeProvider));
    }

    [Test]
    public void NewPartyIdIsMappedCorrectly()
    {
        //Assert
        Assert.That(_result.NewPartyId, Is.EqualTo(_source.ProviderId));
    }
}