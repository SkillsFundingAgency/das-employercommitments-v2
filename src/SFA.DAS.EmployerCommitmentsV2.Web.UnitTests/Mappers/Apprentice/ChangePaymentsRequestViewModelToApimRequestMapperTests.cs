using FluentAssertions;
using Moq;
using SFA.DAS.EmployerCommitmentsV2.Contracts;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Apprentice;

public class ChangePaymentsRequestViewModelToApimRequestMapperTests
{
    private Mock<IAuthenticationService> _authenticationService;
    private ChangePaymentsRequestViewModelToApimRequestMapper _mapper;

    [SetUp]
    public void SetUp()
    {
        _authenticationService = new Mock<IAuthenticationService>();
        _authenticationService.Setup(x => x.UserName).Returns("Test User");
        _authenticationService.Setup(x => x.UserEmail).Returns("test@example.com");
        _authenticationService.Setup(x => x.UserId).Returns("user-id");
        _mapper = new ChangePaymentsRequestViewModelToApimRequestMapper(_authenticationService.Object);
    }

    [Test]
    public async Task Map_WhenPausing_SendsPaymentFreezeDateAndReason()
    {
        var pauseDate = new DateTime(2026, 1, 12);
        var viewModel = new ChangePaymentsRequestViewModel
        {
            FreezeStatus = false,
            PauseDate = pauseDate,
            FreezePaymentsReason = 2
        };

        var result = await _mapper.Map(viewModel);

        result.PaymentFreezeDate.Should().Be(pauseDate);
        result.FreezePaymentsReason.Should().Be(2);
        result.UserInfo.UserDisplayName.Should().Be("Test User");
    }

    [Test]
    public async Task Map_WhenResuming_SendsClearedPaymentFields()
    {
        var viewModel = new ChangePaymentsRequestViewModel
        {
            FreezeStatus = true,
            PauseDate = new DateTime(2026, 1, 5),
            FreezePaymentsReason = 2
        };

        var result = await _mapper.Map(viewModel);

        result.PaymentFreezeDate.Should().BeNull();
        result.FreezePaymentsReason.Should().BeNull();
    }
}
