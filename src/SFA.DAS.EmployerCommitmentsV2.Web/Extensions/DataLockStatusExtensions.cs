using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Responses;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Extensions;

public static class DataLockStatusExtensions
{
    public static bool HasDataLockCourseTriaged(this IReadOnlyCollection<DataLock> dataLocks)
    {
        return dataLocks.Any(x => x.TriageStatus == TriageStatus.Restart && x.DataLockStatus == Status.Fail && !x.IsResolved && x.WithCourseError());
    }

    public static bool HasDataLockCourseChangeTriaged(this IReadOnlyCollection<DataLock> dataLocks)
    {
        return dataLocks.Any(x => x.TriageStatus == TriageStatus.Change && x.DataLockStatus == Status.Fail && !x.IsResolved && x.WithCourseError());
    }

    public static bool HasDataLockPriceTriaged(this IReadOnlyCollection<DataLock> dataLocks)
    {
        return dataLocks.Any(x => x.TriageStatus == TriageStatus.Change && x.DataLockStatus == Status.Fail && !x.IsResolved && x.ErrorCode.HasFlag(DataLockErrorCode.Dlock07));
    }

    public static bool WithCourseError(this DataLock dataLockStatus)
    {
        return dataLockStatus.ErrorCode.HasFlag(DataLockErrorCode.Dlock03)
               || dataLockStatus.ErrorCode.HasFlag(DataLockErrorCode.Dlock04)
               || dataLockStatus.ErrorCode.HasFlag(DataLockErrorCode.Dlock05)
               || dataLockStatus.ErrorCode.HasFlag(DataLockErrorCode.Dlock06);
    }

    public static bool HasDataLockCourseTriaged(this IReadOnlyCollection<GetManageApprenticeshipDetailsResponse.GetDataLockResponse.DataLock> dataLocks)
    {
        return dataLocks.Any(x => x.TriageStatus == TriageStatus.Restart && x.DataLockStatus == Status.Fail && !x.IsResolved && x.WithCourseError());
    }

    public static bool HasDataLockCourseChangeTriaged(this IReadOnlyCollection<GetManageApprenticeshipDetailsResponse.GetDataLockResponse.DataLock> dataLocks)
    {
        return dataLocks.Any(x => x.TriageStatus == TriageStatus.Change && x.DataLockStatus == Status.Fail && !x.IsResolved && x.WithCourseError());
    }

    public static bool HasDataLockPriceTriaged(this IReadOnlyCollection<GetManageApprenticeshipDetailsResponse.GetDataLockResponse.DataLock> dataLocks)
    {
        return dataLocks.Any(x => x.TriageStatus == TriageStatus.Change && x.DataLockStatus == Status.Fail && !x.IsResolved && x.ErrorCode.HasFlag(DataLockErrorCode.Dlock07));
    }

    public static bool WithCourseError(this GetManageApprenticeshipDetailsResponse.GetDataLockResponse.DataLock dataLockStatus)
    {
        return dataLockStatus.ErrorCode.HasFlag(DataLockErrorCode.Dlock03)
               || dataLockStatus.ErrorCode.HasFlag(DataLockErrorCode.Dlock04)
               || dataLockStatus.ErrorCode.HasFlag(DataLockErrorCode.Dlock05)
               || dataLockStatus.ErrorCode.HasFlag(DataLockErrorCode.Dlock06);
    }
}