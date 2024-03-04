using Microsoft.AspNetCore.Mvc.Routing;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Stubs;

public class StubUrlHelper : IUrlHelper
{
    public string Action(UrlActionContext actionContext)
    {
        throw new NotImplementedException();
    }

    public string Content(string contentPath)
    {
        throw new NotImplementedException();
    }

    public bool IsLocalUrl(string url)
    {
        throw new NotImplementedException();
    }

    public string RouteUrl(UrlRouteContext routeContext)
    {
        throw new NotImplementedException();
    }

    public string Link(string routeName, object values)
    {
        throw new NotImplementedException();
    }

    public ActionContext ActionContext { get; }
}