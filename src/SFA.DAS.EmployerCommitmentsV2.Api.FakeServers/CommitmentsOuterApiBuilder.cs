using System.Net;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace SFA.DAS.EmployerCommitmentsV2.Api.FakeServers
{
    public class CommitmentsOuterApiBuilder
    {
        private readonly WireMockServer _server;

        public CommitmentsOuterApiBuilder(int port)
        {
            _server = WireMockServer.StartWithAdminInterface(port, true);
        }

        public static CommitmentsOuterApiBuilder Create(int port)
        {
            return new CommitmentsOuterApiBuilder(port);
        }

        public MockApi Build()
        {
            return new MockApi(_server);
        }

        internal CommitmentsOuterApiBuilder WithCourseDeliveryModels()
        {
            _server
                .Given(Request.Create()
                    .WithPath("/Providers/*/courses*")
                    .UsingGet())
                .RespondWith(Response.Create()
                    .WithStatusCode(HttpStatusCode.OK)
                    .WithHeader("Content-Type", "application/json")
                    .WithBodyAsJson(new
                    {
                        DeliveryModels = new[] { "Regular", "PortableFlexiJob"},
                    }));

            return this;
        }

        internal CommitmentsOuterApiBuilder WithEmployerApprenticeshipsEdit()
        {
            _server
                .Given(Request.Create()
                    .WithPath("/employer/*/apprentices/*/edit*")
                    .UsingGet())
                .RespondWith(Response.Create()
                    .WithStatusCode(HttpStatusCode.OK)
                    .WithHeader("Content-Type", "application/json")
                    .WithBodyAsJson(new
                    {
                        HasMultipleDeliveryModelOptions = true,
                        IsFundedByTransfer = false, 
                        CourseName = "My Course Name",
                    }));

            return this;
        }

    }
}
