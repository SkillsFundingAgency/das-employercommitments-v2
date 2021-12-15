using AutoFixture;
using AutoFixture.AutoMoq;
using System;
using System.Collections.Generic;
using System.Text;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Customisations
{
    public class DomainCustomisations : CompositeCustomization
    {
        public DomainCustomisations() : base(
            new AutoMoqCustomization { ConfigureMembers = true })
        {
        }
    }
}
