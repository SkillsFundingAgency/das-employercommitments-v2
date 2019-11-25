using NUnit.Framework;
using SFA.DAS.CommitmentsV2.Types;
using System.Collections.Generic;
using SFA.DAS.EmployerCommitmentsV2.Web.Extensions;
using System.Linq;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Extensions
{
    [TestFixture]
    public class CohortSummaryExtensionTest
    {
        [Test]
        public void Filters_CohortSummary_For_Reviews_Correctly()
        {
            var fixture = new CohortSummaryExtensionTestFixture();
            fixture.CreateTestData();

            fixture.CohortSummary_Filtered_Correctly_For_Reviews();
        }

        [Test]
        public void Filters_CohortSummary_For_Drafts_Correctly()
        {
            var fixture = new CohortSummaryExtensionTestFixture();
            fixture.CreateTestData();

            fixture.CohortSummary_Filtered_Correctly_For_Drafts();
        }
    }


     public class CohortSummaryExtensionTestFixture
     {  
        public List<CohortSummary> Cohorts { get; set; }

        public CohortSummaryExtensionTestFixture()
        {
            Cohorts = new List<CohortSummary>();
        }

        public CohortSummaryExtensionTestFixture CreateTestData()
        {
            Cohorts = new List<CohortSummary>
            {
                new CohortSummary
                {
                    CohortId = 1,
                    IsDraft = true,
                    WithParty = Party.Employer
                },
                new CohortSummary
                {
                    CohortId = 2,
                    IsDraft = false,
                    WithParty = Party.Employer
                },
                new CohortSummary
                {
                    CohortId = 3,
                    IsDraft = true,
                    WithParty = Party.Provider
                },
                new CohortSummary
                {
                    CohortId = 4,
                    IsDraft = false,
                    WithParty = Party.Provider
                },
            };

            return this;
        }

        public void CohortSummary_Filtered_Correctly_For_Reviews()
        {
            var filteredChorots = Cohorts.Filter(CohortStatus.Review);

            Assert.AreEqual(1, filteredChorots.Count());
            Assert.AreEqual(2, filteredChorots.ToArray()[0].CohortId);
        }

        public void CohortSummary_Filtered_Correctly_For_Drafts()
        {
            var filteredChorots = Cohorts.Filter(CohortStatus.Draft);

            Assert.AreEqual(1, filteredChorots.Count());
            Assert.AreEqual(1, filteredChorots.ToArray()[0].CohortId);
        }
    }
}
