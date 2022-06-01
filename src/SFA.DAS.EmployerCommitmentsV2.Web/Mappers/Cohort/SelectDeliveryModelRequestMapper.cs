using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.EmployerCommitmentsV2.Web.Services;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort
{
    public class SelectDeliveryModelRequestMapper : IMapper<SelectDeliveryModelRequest, SelectDeliveryModelViewModel>
    {
        private readonly IFjaaAgencyService _iFjaaAgencyService;
        protected List<DeliveryModelMapped> _models;

        public SelectDeliveryModelRequestMapper(IFjaaAgencyService iFjaaAgencyService)
        {
            _iFjaaAgencyService = iFjaaAgencyService;
            _models = new List<DeliveryModelMapped>();
        }

        public async Task<SelectDeliveryModelViewModel> Map(SelectDeliveryModelRequest source)
        {
            bool agencyExists = await _iFjaaAgencyService.AgencyExists((int)source.AccountLegalEntityId);

            // TODO determine logic for this
            bool portableAllowed = true;

            // Add regular model
            this.AddDeliveryModel(0);

            //Employer is listed on the RoFJAA & is not involved in Portable flexi-job pilot = Regular & Flexi - Job Agency delivery models available for selection
            if (agencyExists && portableAllowed == false) { this.AddDeliveryModel(2); }

            //Employer is listed on the RoFJAA & is also involved in Portable flexi - job pilot = Regular & Flexi - Job Agency delivery models available for selection
            if (agencyExists && portableAllowed == true) { this.AddDeliveryModel(1); this.AddDeliveryModel(2);  }

            //Employer is not listed on the RoFJAA & is involved in Portable flexi - job pilot = Regular & Portable Flexi - Job delivery models available for selection
            if (!agencyExists && portableAllowed == true) { this.AddDeliveryModel(1);  }

            return new SelectDeliveryModelViewModel
            {
                AccountHashedId = source.AccountHashedId,
                AccountLegalEntityHashedId = source.AccountLegalEntityHashedId,
                CourseCode = source.CourseCode,
                ReservationId = source.ReservationId,
                StartMonthYear = source.StartMonthYear,
                DeliveryModels = _models
            };
        }

        protected void AddDeliveryModel(int deliveryModel)
        {
            if (deliveryModel == 0) { _models.Add(new DeliveryModelMapped() { Id = 0, Name = "Regular", Description = "The apprentice will have a single employment contract" }); }
            if (deliveryModel == 1) { _models.Add(new DeliveryModelMapped() { Id = 1, Name = "Portable flexi-job", Description = "The apprentice will move between multiple employment contracts" }); }
            if (deliveryModel == 2) { _models.Add(new DeliveryModelMapped() { Id = 2, Name = "Flexi-job agency", Description = "The apprentice will have a single employment contract with their flexi-job apprenticeship agency as they move between host businesses" }); }
        }

    }
}
