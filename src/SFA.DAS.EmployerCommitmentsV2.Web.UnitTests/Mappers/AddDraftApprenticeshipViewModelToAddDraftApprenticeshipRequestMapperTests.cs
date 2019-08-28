using System;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Shared;
using SFA.DAS.Testing;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers
{
    [TestFixture]
    [Parallelizable]
    public class AddDraftApprenticeshipViewModelToAddDraftApprenticeshipRequestMapperTests : FluentTest<AddDraftApprenticeshipViewModelToAddDraftApprenticeshipRequestMapperTestsFixture>
    {
        [Test]
        public void Map_WhenMapping_ThenShouldSetProperties()
        {
            Test(
                f => f.Map(), 
                (f, r) =>
                {
                    r.UserId.Should().Be("X");
                    r.ProviderId.Should().Be(1);
                    r.FirstName.Should().Be(f.ViewModel.FirstName);
                    r.LastName.Should().Be(f.ViewModel.LastName);
                    r.DateOfBirth.Should().Be(f.ViewModel.DateOfBirth.Date);
                    r.Uln.Should().Be(f.ViewModel.Uln);
                    r.CourseCode.Should().Be(f.ViewModel.CourseCode);
                    r.Cost.Should().Be(f.ViewModel.Cost);
                    r.StartDate.Should().Be(f.ViewModel.StartDate.Date);
                    r.EndDate.Should().Be(f.ViewModel.EndDate.Date);
                    r.OriginatorReference.Should().Be(f.ViewModel.Reference);
                    r.ReservationId.Should().Be(f.ViewModel.ReservationId);
                });
        }
    }

    public class AddDraftApprenticeshipViewModelToAddDraftApprenticeshipRequestMapperTestsFixture
    {
        public AddDraftApprenticeshipViewModel ViewModel { get; set; }
        public AddDraftApprenticeshipViewModelToAddDraftApprenticeshipRequestMapper Mapper { get; set; }

        public AddDraftApprenticeshipViewModelToAddDraftApprenticeshipRequestMapperTestsFixture()
        {
            ViewModel = new AddDraftApprenticeshipViewModel
            {
                FirstName = "John",
                LastName = "Doe",
                BirthDay = 1,
                BirthMonth = 2,
                BirthYear = 2000,
                Uln = "AAA000",
                CourseCode = "BBB111",
                Cost = 3,
                StartMonth = 8,
                StartYear = 2019,
                EndMonth = 9,
                EndYear = 2020,
                Reference = "CCC222",
                ReservationId = Guid.NewGuid()
            };
            
            Mapper = new AddDraftApprenticeshipViewModelToAddDraftApprenticeshipRequestMapper();
        }

        public AddDraftApprenticeshipRequest Map()
        {
            return Mapper.Map(ViewModel).Result;
        }
    }
}