using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Customisations;

public class DomainAutoDataAttribute : AutoDataAttribute
{
    public DomainAutoDataAttribute() : base(() =>
    {
        var fixture = new Fixture();

        fixture
            .Customize(new DomainCustomisations())
            .Customize<BindingInfo>(c => c.OmitAutoProperties());

        return fixture;
    })
    {
    }
}