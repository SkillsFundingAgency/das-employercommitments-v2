using FluentAssertions;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Contracts;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Apprentice;

[TestFixture]
public class ConfirmEditApprenticeshipViewModelToConfirmEditApprenticeshipRequestMapperTests
{
    private ConfirmEditApprenticeshipViewModelToConfirmEditApprenticeshipRequestMapper _mapper;
    private Mock<IAuthenticationService> _mockAuthenticationService;

    [SetUp]
    public void Arrange()
    {
        _mockAuthenticationService = new Mock<IAuthenticationService>();
        _mockAuthenticationService.Setup(x => x.UserId).Returns("TestUserId");
        _mockAuthenticationService.Setup(x => x.UserName).Returns("TestUserName");
        _mockAuthenticationService.Setup(x => x.UserEmail).Returns("test@example.com");
        
        _mapper = new ConfirmEditApprenticeshipViewModelToConfirmEditApprenticeshipRequestMapper(_mockAuthenticationService.Object);
    }

    [Test, MoqAutoData]
    public async Task Then_Maps_All_Properties_Correctly(ConfirmEditApprenticeshipViewModel source)
    {
        // Arrange
        source.BirthDay = 15;
        source.BirthMonth = 5;
        source.BirthYear = 1990;
        source.StartMonth = 1;
        source.StartYear = 2024;
        source.EndMonth = 1;
        source.EndYear = 2026;
        source.EmploymentEndMonth = 12;
        source.EmploymentEndYear = 2025;
        source.DeliveryModel = DeliveryModel.PortableFlexiJob;
        source.Option = "TBC";

        // Act
        var result = await _mapper.Map(source);

        // Assert
        result.Should().NotBeNull();
        result.ApprenticeshipId.Should().Be(source.ApprenticeshipId);
        result.AccountId.Should().Be(source.AccountId);
        result.FirstName.Should().Be(source.FirstName);
        result.LastName.Should().Be(source.LastName);
        result.Email.Should().Be(source.Email);
        result.DateOfBirth.Should().Be(new DateTime(1990, 5, 15));
        result.Cost.Should().Be(source.Cost);
        result.EmployerReference.Should().Be(source.EmployerReference);
        result.StartDate.Should().Be(new DateTime(2024, 1, 1));
        result.EndDate.Should().Be(new DateTime(2026, 1, 1));
        result.DeliveryModel.Should().Be(source.DeliveryModel.ToString());
        result.EmploymentEndDate.Should().Be(new DateTime(2025, 12, 1));
        result.EmploymentPrice.Should().Be(source.EmploymentPrice);
        result.CourseCode.Should().Be(source.CourseCode);
        result.Version.Should().Be(source.Version);
        result.Option.Should().Be(string.Empty); // TBC should be converted to empty string
        result.UserInfo.Should().NotBeNull();
        result.UserInfo.UserId.Should().Be("TestUserId");
        result.UserInfo.UserDisplayName.Should().Be("TestUserName");
        result.UserInfo.UserEmail.Should().Be("test@example.com");
    }

    [Test, MoqAutoData]
    public async Task Then_Maps_UserInfo_Correctly(ConfirmEditApprenticeshipViewModel source)
    {
        // Arrange
        // Ensure valid date properties to avoid AutoFixture issues
        source.BirthDay = 1;
        source.BirthMonth = 1;
        source.BirthYear = 1990;
        source.StartMonth = 1;
        source.StartYear = 2024;
        source.EndMonth = 1;
        source.EndYear = 2026;
        source.EmploymentEndMonth = 12;
        source.EmploymentEndYear = 2025;

        // Act
        var result = await _mapper.Map(source);

        // Assert
        result.UserInfo.Should().NotBeNull();
        result.UserInfo.UserId.Should().Be("TestUserId");
        result.UserInfo.UserDisplayName.Should().Be("TestUserName");
        result.UserInfo.UserEmail.Should().Be("test@example.com");
    }

    [Test, MoqAutoData]
    public async Task Then_Maps_Option_Not_TBC_Correctly(ConfirmEditApprenticeshipViewModel source)
    {
        // Arrange
        source.Option = "Some Option";
        // Ensure valid date properties to avoid AutoFixture issues
        source.BirthDay = 1;
        source.BirthMonth = 1;
        source.BirthYear = 1990;
        source.StartMonth = 1;
        source.StartYear = 2024;
        source.EndMonth = 1;
        source.EndYear = 2026;
        source.EmploymentEndMonth = 12;
        source.EmploymentEndYear = 2025;

        // Act
        var result = await _mapper.Map(source);

        // Assert
        result.Should().NotBeNull();
        result.Option.Should().Be("Some Option");
    }

    [Test, MoqAutoData]
    public async Task Then_Maps_Null_DeliveryModel_Correctly(ConfirmEditApprenticeshipViewModel source)
    {
        // Arrange
        source.DeliveryModel = null;
        // Ensure valid date properties to avoid AutoFixture issues
        source.BirthDay = 1;
        source.BirthMonth = 1;
        source.BirthYear = 1990;
        source.StartMonth = 1;
        source.StartYear = 2024;
        source.EndMonth = 1;
        source.EndYear = 2026;
        source.EmploymentEndMonth = 12;
        source.EmploymentEndYear = 2025;

        // Act
        var result = await _mapper.Map(source);

        // Assert
        result.Should().NotBeNull();
        result.DeliveryModel.Should().BeNull();
    }

    [Test, MoqAutoData]
    public async Task Then_Maps_All_DeliveryModel_Values_Correctly(ConfirmEditApprenticeshipViewModel source)
    {
        // Arrange - ensure valid date properties to avoid AutoFixture issues
        source.BirthDay = 1;
        source.BirthMonth = 1;
        source.BirthYear = 1990;
        source.StartMonth = 1;
        source.StartYear = 2024;
        source.EndMonth = 1;
        source.EndYear = 2026;
        source.EmploymentEndMonth = 12;
        source.EmploymentEndYear = 2025;

        // Act & Assert
        foreach (DeliveryModel deliveryModel in Enum.GetValues(typeof(DeliveryModel)))
        {
            source.DeliveryModel = deliveryModel;
            var result = await _mapper.Map(source);
            result.Should().NotBeNull();
            result.DeliveryModel.Should().Be(deliveryModel.ToString());
        }
    }

    [Test, MoqAutoData]
    public async Task Then_Maps_Null_Date_Values_Correctly(ConfirmEditApprenticeshipViewModel source)
    {
        // Arrange
        source.BirthDay = null;
        source.BirthMonth = null;
        source.BirthYear = null;
        source.StartMonth = null;
        source.StartYear = null;
        source.EndMonth = null;
        source.EndYear = null;
        source.EmploymentEndMonth = null;
        source.EmploymentEndYear = null;

        // Act
        var result = await _mapper.Map(source);

        // Assert
        result.Should().NotBeNull();
        result.DateOfBirth.Should().BeNull();
        result.StartDate.Should().BeNull();
        result.EndDate.Should().BeNull();
        result.EmploymentEndDate.Should().BeNull();
    }

    [Test, MoqAutoData]
    public async Task Then_Maps_Null_String_Values_Correctly(ConfirmEditApprenticeshipViewModel source)
    {
        // Arrange
        source.FirstName = null;
        source.LastName = null;
        source.Email = null;
        source.EmployerReference = null;
        source.CourseCode = null;
        source.Version = null;
        source.Option = null;
        // Ensure valid date properties to avoid AutoFixture issues
        source.BirthDay = 1;
        source.BirthMonth = 1;
        source.BirthYear = 1990;
        source.StartMonth = 1;
        source.StartYear = 2024;
        source.EndMonth = 1;
        source.EndYear = 2026;
        source.EmploymentEndMonth = 12;
        source.EmploymentEndYear = 2025;

        // Act
        var result = await _mapper.Map(source);

        // Assert
        result.Should().NotBeNull();
        result.FirstName.Should().BeNull();
        result.LastName.Should().BeNull();
        result.Email.Should().BeNull();
        result.EmployerReference.Should().BeNull();
        result.CourseCode.Should().BeNull();
        result.Version.Should().BeNull();
        result.Option.Should().BeNull();
    }

    [Test, MoqAutoData]
    public async Task Then_Maps_Null_Decimal_Values_Correctly(ConfirmEditApprenticeshipViewModel source)
    {
        // Arrange
        source.Cost = null;
        // Ensure valid date properties to avoid AutoFixture issues
        source.BirthDay = 1;
        source.BirthMonth = 1;
        source.BirthYear = 1990;
        source.StartMonth = 1;
        source.StartYear = 2024;
        source.EndMonth = 1;
        source.EndYear = 2026;
        source.EmploymentEndMonth = 12;
        source.EmploymentEndYear = 2025;

        // Act
        var result = await _mapper.Map(source);

        // Assert
        result.Should().NotBeNull();
        result.Cost.Should().BeNull();
    }

    [Test, MoqAutoData]
    public async Task Then_Maps_Null_Int_Values_Correctly(ConfirmEditApprenticeshipViewModel source)
    {
        // Arrange
        source.EmploymentPrice = null;
        // Ensure valid date properties to avoid AutoFixture issues
        source.BirthDay = 1;
        source.BirthMonth = 1;
        source.BirthYear = 1990;
        source.StartMonth = 1;
        source.StartYear = 2024;
        source.EndMonth = 1;
        source.EndYear = 2026;
        source.EmploymentEndMonth = 12;
        source.EmploymentEndYear = 2025;

        // Act
        var result = await _mapper.Map(source);

        // Assert
        result.Should().NotBeNull();
        result.EmploymentPrice.Should().BeNull();
    }

    [Test, MoqAutoData]
    public async Task Then_Maps_Empty_String_Values_Correctly(ConfirmEditApprenticeshipViewModel source)
    {
        // Arrange
        source.FirstName = string.Empty;
        source.LastName = string.Empty;
        source.Email = string.Empty;
        source.EmployerReference = string.Empty;
        source.CourseCode = string.Empty;
        source.Version = string.Empty;
        source.Option = string.Empty;
        // Ensure valid date properties to avoid AutoFixture issues
        source.BirthDay = 1;
        source.BirthMonth = 1;
        source.BirthYear = 1990;
        source.StartMonth = 1;
        source.StartYear = 2024;
        source.EndMonth = 1;
        source.EndYear = 2026;
        source.EmploymentEndMonth = 12;
        source.EmploymentEndYear = 2025;

        // Act
        var result = await _mapper.Map(source);

        // Assert
        result.Should().NotBeNull();
        result.FirstName.Should().Be(string.Empty);
        result.LastName.Should().Be(string.Empty);
        result.Email.Should().Be(string.Empty);
        result.EmployerReference.Should().Be(string.Empty);
        result.CourseCode.Should().Be(string.Empty);
        result.Version.Should().Be(string.Empty);
        result.Option.Should().Be(string.Empty);
    }
} 