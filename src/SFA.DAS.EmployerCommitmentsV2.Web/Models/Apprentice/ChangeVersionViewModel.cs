using System.Collections.Generic;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice
{
    public class ChangeVersionViewModel
    {
        public string StandardTitle { get; set; }
        public string StandardUrl { get; set; }
        public string CurrentVersion { get; set; }
        public string SelectedVersion { get; set; }
        public IEnumerable<string> NewerVersions { get; set; }
    }
}
