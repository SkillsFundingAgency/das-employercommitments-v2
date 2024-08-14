namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Shared
{
    public class SuccessBannerModel
    {
        public SuccessBannerModel(string id, string heading, string body)
        {
            Id = id;
            Heading = heading;
            Body = body;
        }

        public string Id { get; set; }
        public string Heading { get; set; }
        public string Body { get; set; }
    }
}
