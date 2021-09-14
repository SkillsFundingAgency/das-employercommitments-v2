
using System.Collections.Generic;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice
{
    public class ChangeOptionViewModel
    {
        public string SelectedVersion { get; set; }
        public string SelectedVersionName { get; set; }
        public string SelectedVersionUrl { get; set; }
        public IEnumerable<string> Options { get; set; }
        public string SelectedOption { get; set; }
    }
}
