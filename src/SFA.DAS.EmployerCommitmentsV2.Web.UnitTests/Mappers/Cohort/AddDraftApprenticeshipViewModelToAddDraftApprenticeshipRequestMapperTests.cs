using FluentAssertions;
using SFA.DAS.EmployerCommitmentsV2.Contracts;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Requests;
using SFA.DAS.EmployerCommitmentsV2.Web.Authentication;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.DraftApprenticeship;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Cohort;

[TestFixture]
[Parallelizable]
public class AddDraftApprenticeshipViewModelToAddDraftApprenticeshipRequestMapperTests 
{
    [Test]
    public void Map_WhenMapping_ThenShouldSetProperties()
    {
        var fixture = new AddDraftApprenticeshipViewModelToAddDraftApprenticeshipRequestMapperTestsFixture();
        var request = fixture.Map();

        request.ProviderId.Should().Be(1);
        request.FirstName.Should().Be(fixture.ViewModel.FirstName);
        request.LastName.Should().Be(fixture.ViewModel.LastName);
        request.Email.Should().Be(fixture.ViewModel.Email);
        request.DateOfBirth.Should().Be(fixture.ViewModel.DateOfBirth.Date);
        request.Uln.Should().Be(fixture.ViewModel.Uln);
        request.CourseCode.Should().Be(fixture.ViewModel.CourseCode);
        request.DeliveryModel.Should().Be(fixture.ViewModel.DeliveryModel);
        request.Cost.Should().Be(fixture.ViewModel.Cost);
        request.StartDate.Should().Be(fixture.ViewModel.StartDate.Date);
        request.EndDate.Should().Be(fixture.ViewModel.EndDate.Date);
        request.OriginatorReference.Should().Be(fixture.ViewModel.Reference);
        request.ReservationId.Should().Be(fixture.ViewModel.ReservationId);
    }
}

public class AddDraftApprenticeshipViewModelToAddDraftApprenticeshipRequestMapperTestsFixture
{
    public AddDraftApprenticeshipViewModel ViewModel { get; set; }
    public AddDraftApprenticeshipRequestMapper Mapper { get; set; }

    public AddDraftApprenticeshipViewModelToAddDraftApprenticeshipRequestMapperTestsFixture()
    {
        ViewModel = new AddDraftApprenticeshipViewModel
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "test@test.com",
            BirthDay = 1,
            BirthMonth = 2,
            BirthYear = 2000,
            Uln = "AAA000",
            DeliveryModel = CommitmentsV2.Types.DeliveryModel.Regular,
            CourseCode = "111",
            Cost = 3,
            StartMonth = 8,
            StartYear = 2019,
            EndMonth = 9,
            EndYear = 2020,
            Reference = "CCC222",
            ReservationId = Guid.NewGuid()
        };

        Mapper = new AddDraftApprenticeshipRequestMapper(Mock.Of<IAuthenticationService>());
    }

    public AddDraftApprenticeshipApimRequest Map()
    {
        return Mapper.Map(ViewModel).Result;
    }
}