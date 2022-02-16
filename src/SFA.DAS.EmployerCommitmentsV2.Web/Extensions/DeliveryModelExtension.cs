using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.CommitmentsV2.Types.Dtos;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Extensions
{
    public static class DeliveryModelExtension
    {
        public static string ToDisplayString(this DeliveryModelDto deliveryModel) =>
            deliveryModel.Code.ToDisplayString();

        public static string ToDisplayString(this DeliveryModel deliveryModel) =>
            deliveryModel switch
            {
                DeliveryModel.Flexible => "Flexi-job",
                _ => null,
            };
    }
}