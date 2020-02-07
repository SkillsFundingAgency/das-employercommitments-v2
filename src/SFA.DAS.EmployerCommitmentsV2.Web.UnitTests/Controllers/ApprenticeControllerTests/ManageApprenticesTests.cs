using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.ApprenticeControllerTests
{
    [TestFixture]
    public class ManageApprenticesTests
    {
        [Test, MoqAutoData]
        public void ThenTheAccountHashedIdIsPassedToTheViewModel(
            IndexRequest request,
            ApprenticeController controller)
        {
            //Act
            var result = controller.Index(request);
            var view = ((ViewResult) result).Model as IndexViewModel;

            //Assert
            Assert.AreEqual(request.HashedAccountId, view.AccountHashedId);
        }
    }
}