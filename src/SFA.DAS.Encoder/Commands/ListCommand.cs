using System;
using System.Linq;
using SFA.DAS.Encoder.CommandLines;
using SFA.DAS.Encoding;

namespace SFA.DAS.Encoder.Commands
{
    internal class ListCommand : ICommand
    {
        private readonly ListCommandLine _args;
        private readonly IConsole _console;
        private readonly EncodingConfig _encodingConfig;

        public ListCommand(ListCommandLine args, IConsole console, EncodingConfig encodingConfig)
        {
            _args = args;
            _console = console;
            _encodingConfig = encodingConfig;
        }

        public void Run()
        {
            foreach (var encodingType in Enum.GetValues(typeof(EncodingType)).OfType<EncodingType>())
            {
                _console.Write($"{(int)encodingType}:{encodingType}");
                if (_args.Config)
                {
                    var config = _encodingConfig.Encodings.FirstOrDefault(ec => ec.EncodingType == encodingType);
                    if (config == null)
                    {
                        _console.Write("Config", "** not available");
                        _console.Write("");
                    }
                    else
                    {
                        _console.Write("Salt", config.Salt);
                        _console.Write("Alphabet", config.Alphabet);
                        _console.Write("Encoding type", config.EncodingType);
                        _console.Write("Min", config.MinHashLength);
                        _console.Write("");
                    }
                }
            }
        }
    }
}