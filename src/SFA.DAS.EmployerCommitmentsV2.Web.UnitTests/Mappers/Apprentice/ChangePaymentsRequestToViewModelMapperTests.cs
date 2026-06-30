using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Contracts;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Responses;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Apprentice;

public class ChangePaymentsRequestToViewModelMapperTests
{
    private Mock<IApprovalsApiClient> _approvalsApiClient;
    private Mock<ICurrentDateTime> _currentDateTime;
    private ChangePaymentsRequestToViewModelMapper _mapper;

    [SetUp]
    public void SetUp()
    {
        _approvalsApiClient = new Mock<IApprovalsApiClient>();
        _currentDateTime = new Mock<ICurrentDateTime>();
        _currentDateTime.Setup(x => x.UtcNow).Returns(new DateTime(2026, 1, 12, 10, 0, 0, DateTimeKind.Utc));
        _mapper = new ChangePaymentsRequestToViewModelMapper(
            _approvalsApiClient.Object,
            _currentDateTime.Object);
    }

    [Test]
    public async Task Map_WhenPaymentsNotFrozen_SetsPauseFlowViewModel()
    {
        const long accountId = 10;
        const long apprenticeshipId = 20;
        var request = new ChangePaymentsRequest
        {
            AccountHashedId = "ACC",
            ApprenticeshipHashedId = "APP",
            AccountId = accountId,
            ApprenticeshipId = apprenticeshipId
        };

        _approvalsApiClient.Setup(x => x.GetChangePayments(accountId, apprenticeshipId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GetChangePaymentsResponse
            {
                FirstName = "Brian",
                LastName = "May",
                Uln = "1447020943",
                CourseName = "Aviation operations manager, Level 4",
                FreezeStatus = false
            });

        var result = await _mapper.Map(request);

        result.FreezeStatus.Should().BeFalse();
        result.PauseDate.Should().Be(new DateTime(2026, 1, 12));
        result.ResumeDate.Should().BeNull();
        result.ApprenticeName.Should().Be("Brian May");
    }

    [Test]
    public async Task Map_WhenPaymentsFrozen_SetsResumeFlowViewModel()
    {
        const long accountId = 10;
        const long apprenticeshipId = 20;
        var frozenDate = new DateTime(2026, 1, 5);
        var request = new ChangePaymentsRequest
        {
            AccountHashedId = "ACC",
            ApprenticeshipHashedId = "APP",
            AccountId = accountId,
            ApprenticeshipId = apprenticeshipId
        };

        _approvalsApiClient.Setup(x => x.GetChangePayments(accountId, apprenticeshipId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GetChangePaymentsResponse
            {
                FirstName = "Brian",
                LastName = "May",
                FreezeStatus = true,
                PaymentFreezeDate = frozenDate
            });

        var result = await _mapper.Map(request);

        result.FreezeStatus.Should().BeTrue();
        result.PauseDate.Should().Be(frozenDate.Date);
        result.ResumeDate.Should().Be(new DateTime(2026, 1, 12));
    }
}
