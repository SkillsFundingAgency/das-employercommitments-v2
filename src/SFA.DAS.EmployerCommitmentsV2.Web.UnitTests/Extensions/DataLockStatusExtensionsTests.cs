using NUnit.Framework;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Web.Extensions;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Extensions
{
    public class DataLockStatusExtensionsTests
    {
        private GetDataLocksResponse.DataLock _dataLock;

        [SetUp]
        public void Arrange()
        {
            _dataLock = new GetDataLocksResponse.DataLock();
        }

        [TestCase(DataLockErrorCode.Dlock03, true)]
        [TestCase(DataLockErrorCode.Dlock04, true)]
        [TestCase(DataLockErrorCode.Dlock05, true)]
        [TestCase(DataLockErrorCode.Dlock03 | DataLockErrorCode.Dlock04, true)]
        [TestCase(DataLockErrorCode.Dlock03 | DataLockErrorCode.Dlock07, true)]
        [TestCase(DataLockErrorCode.Dlock07, false)]
        public void HasCourseDataLock_Returns_Correct_Value(DataLockErrorCode errorCode, bool expectedResult)
        {
            _dataLock.ErrorCode = errorCode;
            Assert.AreEqual(expectedResult, _dataLock.WithCourseError());
        }
    }
}
