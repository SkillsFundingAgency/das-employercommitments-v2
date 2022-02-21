﻿using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.CommitmentsV2.Types.Dtos;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Extensions
{
    public static class DeliveryModelExtension
    {
        public static string ToDescription(this DeliveryModel? deliveryModel) =>
            deliveryModel?.ToDescription();

        public static string ToDescription(this DeliveryModel deliveryModel) =>
            deliveryModel switch
            {
                DeliveryModel.Flexible => "Flexi-job",
                _ => "Normal"
            };

        public static string ToNonNormalDescription(this DeliveryModelDto deliveryModel) =>
            deliveryModel.Code.ToNonNormalDescription();

        public static string ToNonNormalDescription(this DeliveryModel deliveryModel) =>
            deliveryModel switch
            {
                DeliveryModel.Flexible => "Flexi-job",
                _ => null,
            };
    }
}