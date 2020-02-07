using NUnit.Framework;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Models.ApprenticeFilterModelTests
{
    public class WhenGettingShowSearch
    {
        [TestCase(0, false, TestName = "No search results")]
        [TestCase(Constants.ApprenticesSearch.NumberOfApprenticesRequiredForSearch - 1, false, TestName = "Less than required number by 1")]
        [TestCase(Constants.ApprenticesSearch.NumberOfApprenticesRequiredForSearch, true, TestName = "Equal to required number")]
        [TestCase(Constants.ApprenticesSearch.NumberOfApprenticesRequiredForSearch + 1, true, TestName = "Greater than required number by 1")]
        [TestCase(Constants.ApprenticesSearch.NumberOfApprenticesRequiredForSearch + 100, true, TestName = "Greater than required number by 100")]
        public void Then_The_Show_Search_Flag_Is_Set_Based_On_Number_Of_Apprentices(int numberOfApprentices, bool expectedBool)
        {
            //Act
            var filterModel = new ApprenticesFilterModel
            {
                TotalNumberOfApprenticeships = numberOfApprentices
            };
            
            //Assert
            Assert.AreEqual(expectedBool, filterModel.ShowSearch);
        }
    }
}