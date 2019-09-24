using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort
{
    public class FundingBandExcessModel
    {
        private readonly int?[] _fundingBandCapsExceeded;

        public FundingBandExcessModel(int apprenticesExceedingBand, IEnumerable<int?> fundingBandCapsExceeded)
        {
            _fundingBandCapsExceeded = fundingBandCapsExceeded.ToArray();
            ApprenticesExceedingBand = apprenticesExceedingBand;
        }

        public int ApprenticesExceedingBand { get; }

        public int? SingleFundingBandCapExceeded => _fundingBandCapsExceeded.Length == 1 ? _fundingBandCapsExceeded[0] : (int?) null;
    }
}