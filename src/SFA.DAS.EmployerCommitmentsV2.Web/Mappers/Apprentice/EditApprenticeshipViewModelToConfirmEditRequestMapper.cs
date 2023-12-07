using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice;

public class EditApprenticeshipViewModelToConfirmEditRequestMapper : IMapper<EditApprenticeshipRequestViewModel, ConfirmEditApprenticeshipRequest>
{
    public Task<ConfirmEditApprenticeshipRequest> Map(EditApprenticeshipRequestViewModel source)
    {
        return Task.FromResult(new ConfirmEditApprenticeshipRequest
        {
            AccountHashedId = source.AccountHashedId,
            ApprenticeshipHashedId = source.HashedApprenticeshipId,
            FirstName = source.FirstName,
            LastName = source.LastName,
            BirthDay = source.DateOfBirth.Day,
            BirthMonth = source.DateOfBirth.Month,
            BirthYear = source.DateOfBirth.Year,
            ULN = source.ULN,
            Cost = source.Cost,
            EmployerReference = source.EmployerReference,
            StartMonth = source.StartDate.Month,
            StartYear = source.StartDate.Year,
            EndMonth = source.EndDate.Month,
            EndYear = source.EndDate.Year,
            CourseCode = source.CourseCode
        });
    }
}