using CommandLine;
using SFA.DAS.Encoding;

namespace SFA.DAS.Encoder.CommandLines
{
    public class CommandLineBase
    {
        [Option('e', "encoding", HelpText = "Specifies the type of encoding that should be used")]
        public EncodingType Encoding { get; set; }

        [Option('n', "environment", HelpText = "Specifies the DAS environment name to use (default is LOCAL)")]
        public string EnvironmentName { get; set; }

        [Option('s', "connection", HelpText = "Specifies the Azure storage connection string to use for configuration")]
        public string ConnectionString { get; set; }
    }
}