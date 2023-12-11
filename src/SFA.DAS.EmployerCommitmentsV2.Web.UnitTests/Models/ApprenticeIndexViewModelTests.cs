using NUnit.Framework;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Models;

public class ApprenticeIndexViewModelTests
{
    [Test]
    [MoqInlineAutoData(true, "das-table__sort das-table__sort--desc")]
    [MoqInlineAutoData(false, "das-table__sort das-table__sort--asc")]
    public void ThenTheSortByHeaderClassNameIsSetCorrectly(
        bool isReverse,
        string expected,
        IndexViewModel model)
    {
        //Arrange
        model.SortedByHeaderClassName = "";
        model.FilterModel.ReverseSort = isReverse;

        //Act
        model.SortedByHeader();

        //Assert
        Assert.That(model.SortedByHeaderClassName, Is.EqualTo(expected));
    }
}