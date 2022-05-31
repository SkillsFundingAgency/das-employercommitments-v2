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

        public SelectDeliveryModelRequestMapper(IFjaaAgencyService iFjaaAgencyService)
        {
            _iFjaaAgencyService = iFjaaAgencyService;
        }

        public Task<SelectDeliveryModelViewModel> Map(SelectDeliveryModelRequest source)
        {
            return Task.FromResult(new SelectDeliveryModelViewModel
            {
                AccountHashedId = source.AccountHashedId,
                AccountLegalEntityHashedId = source.AccountLegalEntityHashedId,
                CourseCode = source.CourseCode,
                ReservationId = source.ReservationId,
                StartMonthYear = source.StartMonthYear,
                DeliveryModels = this.Models()
            });
        }

        protected List<DeliveryModelMapped> Models()
        {
            List<DeliveryModelMapped> models = new List<DeliveryModelMapped>();
            models.Add(new DeliveryModelMapped() { Id = 0, Name = "Regular", Text = "The apprentice will have a single employment contract" });
            models.Add(new DeliveryModelMapped() { Id = 1, Name = "Portable flexi-job", Text = "The apprentice will move between multiple employment contracts" });
            models.Add(new DeliveryModelMapped() { Id = 2, Name = "Flexi-job agency", Text = "The apprentice will have a single employment contract with their flexi-job apprenticeship agency as they move between host businesses" });
            return models;
        }
    }
}
