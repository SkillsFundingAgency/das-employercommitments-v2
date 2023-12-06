﻿using AutoFixture.NUnit3;
using NUnit.Framework;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Models.ApprenticeFilterModelTests
{
    public class WhenGettingSortRouteData
    {
        [Test, AutoData]
        public void Then_Should_Set_SortField(
            string sortField,
            ApprenticesFilterModel model)
        {
            //Act
            var actual = model.BuildSortRouteData(sortField);

            //Assert
            Assert.That(actual[nameof(model.SortField)], Is.EqualTo(sortField));
        }

        [Test, AutoData]
        public void And_No_SortField_Then_Should_Set_SortField(
            string sortField,
            ApprenticesFilterModel model)
        {
            //Arrange
            model.SortField = null;

            //Act
            var actual = model.BuildSortRouteData(sortField);

            //Assert
            Assert.That(actual[nameof(model.SortField)], Is.EqualTo(sortField));
        }

        [Test, AutoData]
        public void Then_Should_Set_ReverseSort(
            string sortField,
            ApprenticesFilterModel model)
        {
            //Arrange
            model.SortField = sortField;
            var expected = !model.ReverseSort;

            //Act
            var actual = model.BuildSortRouteData(sortField);

            //Assert
            Assert.That(actual[nameof(model.ReverseSort)], Is.EqualTo(expected.ToString()));
        }

        [Test, AutoData]
        public void And_No_SortField_Then_Should_Set_ReverseSort(
            string sortField,
            ApprenticesFilterModel model)
        {
            //Arrange
            model.SortField = null;
            var expected = !model.ReverseSort;

            //Act
            var actual = model.BuildSortRouteData(sortField);

            //Assert
            Assert.That(actual[nameof(model.ReverseSort)], Is.EqualTo(expected.ToString()));
        }
    }
}