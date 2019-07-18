using CommandLine;

namespace SFA.DAS.Encoder.CommandLines
{
    [Verb("encode", HelpText = "Encodes the supplied values")]
    public class EncodeCommandLine : CommandLineBase
    {
        [Option('v', "value", HelpText = "Specifies a value that will be encoded using the specified encoding type")]
        public long Value { get; set; }
    }
}
