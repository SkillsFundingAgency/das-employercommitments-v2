using SFA.DAS.CommitmentsV2.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using static SFA.DAS.CommitmentsV2.Api.Types.Responses.GetDataLocksResponse;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Extensions
{
    public static class DataLockStatusExtensions
    {     
        public static bool HasDataLockCourseTriaged(this IReadOnlyCollection<DataLock> dataLocks)
        {
            //TO DO : Check null stuff dataLocks?
            return dataLocks.Any(x => x.TriageStatus == TriageStatus.Unknown && x.DataLockStatus == Status.Fail && !x.IsResolved && x.WithCourseError());
        }

        public static bool HasDataLockCourseChangeTriaged(this IReadOnlyCollection<DataLock> dataLocks)
        {
            //TO DO : Check null stuff dataLocks?
            return dataLocks.Any(x => x.TriageStatus == TriageStatus.Change && x.DataLockStatus == Status.Fail && !x.IsResolved && x.WithCourseError());
            
        }

        public static bool HasDataLockPriceTriaged(this IReadOnlyCollection<DataLock> dataLocks)
        {
            //TO DO : Check null stuff dataLocks?
            return dataLocks.Any(x => x.TriageStatus == TriageStatus.Change && x.DataLockStatus == Status.Fail
            && !x.IsResolved && x.ErrorCode.HasFlag(DataLockErrorCode.Dlock07));
        }

        public static bool WithCourseError(this DataLock dataLockStatus)
        {
            return dataLockStatus.ErrorCode.HasFlag(DataLockErrorCode.Dlock03)
                   || dataLockStatus.ErrorCode.HasFlag(DataLockErrorCode.Dlock04)
                   || dataLockStatus.ErrorCode.HasFlag(DataLockErrorCode.Dlock05)
                   || dataLockStatus.ErrorCode.HasFlag(DataLockErrorCode.Dlock06);
        }

    }
}
