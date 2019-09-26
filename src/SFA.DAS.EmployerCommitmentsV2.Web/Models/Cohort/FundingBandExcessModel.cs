using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort
{
    public class FundingBandExcessModel
    {
        private readonly int[] _fundingBandCapsExceeded;

        public FundingBandExcessModel(int numberOfApprenticesExceedingFundingBandCap, IEnumerable<int?> fundingBandCapsExceeded)
        {
            _fundingBandCapsExceeded = fundingBandCapsExceeded.Where(x=>x.HasValue).Select(x=>x.Value).ToArray();
            NumberOfApprenticesExceedingFundingBandCap = numberOfApprenticesExceedingFundingBandCap;
        }

        public int NumberOfApprenticesExceedingFundingBandCap { get; }

        public string DisplaySingleFundingBandCap => _fundingBandCapsExceeded.Length == 1 ? $"£{string.Format("{0:#,0}", _fundingBandCapsExceeded[0])}." : ".";
    }
}