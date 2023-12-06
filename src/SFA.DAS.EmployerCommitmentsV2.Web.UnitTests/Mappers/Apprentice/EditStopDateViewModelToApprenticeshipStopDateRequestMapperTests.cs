using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Web.Authentication;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using System;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Apprentice
{
    public class EditStopDateViewModelToApprenticeshipStopDateRequestMapperTests
    {
        private EditStopDateViewModelToApprenticeshipStopDateRequestMapper _mapper;
        private Mock<IAuthenticationService> _mockAuthenticationService;
        private Fixture _autoFixture;
        private EditStopDateViewModel _viewModel;

        [SetUp]
        public void Arrange()
        {
            _mockAuthenticationService = new Mock<IAuthenticationService>();
            _mockAuthenticationService.Setup(x => x.UserInfo).Returns(It.IsAny<UserInfo>());
            _autoFixture = new Fixture();
            _autoFixture.Customize<EditStopDateViewModel>(c => c.Without(x => x.NewStopDate));
            _mapper = new EditStopDateViewModelToApprenticeshipStopDateRequestMapper(_mockAuthenticationService.Object);            
            _viewModel = _autoFixture.Create<EditStopDateViewModel>();
            _viewModel.NewStopDate = new CommitmentsV2.Shared.Models.MonthYearModel("062020");
        }

        [Test]
        public async Task ApprenticeshipHashedId_IsMapped()
        {
            //Act
            var result = await _mapper.Map(_viewModel);

            //Assert
            Assert.That(result.AccountId, Is.EqualTo(_viewModel.AccountId));
        }

        [Test]
        public async Task NewStopDate_IsMapped()
        {          
            //Act
            var result = await _mapper.Map(_viewModel);

            //Assert
            Assert.That(result.NewStopDate, Is.EqualTo(new DateTime(_viewModel.NewStopDate.Year.Value, _viewModel.NewStopDate.Month.Value, _viewModel.NewStopDate.Day.Value)));
        }
   
    }
}
