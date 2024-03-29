﻿using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Web.Extensions;
using static SFA.DAS.CommitmentsV2.Api.Types.Responses.GetDataLocksResponse;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Extensions;

public class DataLockStatusExtensionsTests
{
    private DataLock _dataLock;
    private IReadOnlyCollection<DataLock> _dataLocks;       

    [TestCase(DataLockErrorCode.Dlock03, true)]
    [TestCase(DataLockErrorCode.Dlock04, true)]
    [TestCase(DataLockErrorCode.Dlock05, true)]
    [TestCase(DataLockErrorCode.Dlock03 | DataLockErrorCode.Dlock04, true)]
    [TestCase(DataLockErrorCode.Dlock03 | DataLockErrorCode.Dlock07, true)]
    [TestCase(DataLockErrorCode.Dlock07, false)]
    public void HasCourseDataLock_Returns_Correct_Value(DataLockErrorCode errorCode, bool expectedResult)
    {
        //Arrange
        _dataLock = new DataLock() {
            ErrorCode = errorCode
        };

        //Act
        var actualResult = _dataLock.WithCourseError();

        //Assert
        Assert.That(actualResult, Is.EqualTo(expectedResult));
    }

    [TestCase(DataLockErrorCode.Dlock03, true)]
    [TestCase(DataLockErrorCode.Dlock04, true)]
    [TestCase(DataLockErrorCode.Dlock05, true)]        
    [TestCase(DataLockErrorCode.Dlock06, true)]
    [TestCase(DataLockErrorCode.Dlock07, false)]
    public void HasDataLockCourseTriaged_Returns_Correct_Value(DataLockErrorCode errorCode, bool expectedResult)
    {
        //Arrange
        _dataLocks = new List<DataLock>
        { new()
            {
                Id = 1,
                TriageStatus = TriageStatus.Restart,
                DataLockStatus = Status.Fail,
                IsResolved = false,
                ErrorCode = errorCode
            },
            new()
            {
                Id = 2,
                TriageStatus = TriageStatus.Unknown,
                DataLockStatus = Status.Pass,
                IsResolved = false
            }
        };

        //Act
        var actualResult = _dataLocks.HasDataLockCourseTriaged();

        //Assert
        Assert.That(actualResult, Is.EqualTo(expectedResult));
    }


    [TestCase(DataLockErrorCode.Dlock03, true)]
    [TestCase(DataLockErrorCode.Dlock04, true)]
    [TestCase(DataLockErrorCode.Dlock05, true)]
    [TestCase(DataLockErrorCode.Dlock06, true)]
    [TestCase(DataLockErrorCode.Dlock07, false)]
    public void HasDataLockCourseChangeTriaged_Returns_Correct_Value(DataLockErrorCode errorCode, bool expectedResult)
    {
        //Arrange
        _dataLocks = new List<DataLock>
        { new()
            {
                Id = 1,
                TriageStatus = TriageStatus.Change,
                DataLockStatus = Status.Fail,
                IsResolved = false,
                ErrorCode = errorCode
            },
            new()
            {
                Id = 2,
                TriageStatus = TriageStatus.Unknown,
                DataLockStatus = Status.Pass,
                IsResolved = false
            }
        };

        //Act
        var actualResult = _dataLocks.HasDataLockCourseChangeTriaged();

        //Assert
        Assert.That(actualResult, Is.EqualTo(expectedResult));
    }

    [TestCase(DataLockErrorCode.Dlock03, false)]
    [TestCase(DataLockErrorCode.Dlock04, false)]
    [TestCase(DataLockErrorCode.Dlock05, false)]
    [TestCase(DataLockErrorCode.Dlock06, false)]
    [TestCase(DataLockErrorCode.Dlock07, true)]
    public void HasDataLockPriceTriaged_Returns_Correct_Value(DataLockErrorCode errorCode, bool expectedResult)
    {
        //Arrange
        _dataLocks = new List<DataLock>
        { new()
            {
                Id = 1,
                TriageStatus = TriageStatus.Change,
                DataLockStatus = Status.Fail,
                IsResolved = false,
                ErrorCode = errorCode
            },
            new()
            {
                Id = 2,
                TriageStatus = TriageStatus.Unknown,
                DataLockStatus = Status.Pass,
                IsResolved = false
            }
        };

        //Act
        var actualResult = _dataLocks.HasDataLockPriceTriaged();

        //Assert
        Assert.That(actualResult, Is.EqualTo(expectedResult));
    }
}