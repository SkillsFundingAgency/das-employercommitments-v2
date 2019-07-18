using CommandLine;
using SFA.DAS.Encoding;

namespace SFA.DAS.Encoder.CommandLines
{
    public class CommandLineBase
    {
        [Option('e', "encoding", HelpText = "Specifies the type of encoding that should be used")]

        public EncodingType Encoding { get; set; }
    }
}