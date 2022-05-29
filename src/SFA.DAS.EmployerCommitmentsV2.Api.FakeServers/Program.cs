using System;

namespace SFA.DAS.EmployerCommitmentsV2.Api.FakeServers
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            CommitmentsOuterApiBuilder.Create(44328)
               .WithCourseDeliveryModels()
               .Build();

            Console.WriteLine("Approvals Outer API running on port 44328");
            Console.WriteLine("Course Games Developer (650) has a Single DeliveryModel, all other course have multiple");
            Console.WriteLine("Press any key to stop the APIs server");
            Console.ReadKey();
        }
    }
}
