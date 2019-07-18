using CommandLine;

namespace SFA.DAS.Encoder.CommandLines
{
    [Verb("list", HelpText = "Lists the defined encoding types and optionally their config")]
    public class ListCommandLine : CommandLineBase
    {
        [Option('c', "config", HelpText = "Show config for each encoding type")]
        public bool Config { get; set; }
    }
}
