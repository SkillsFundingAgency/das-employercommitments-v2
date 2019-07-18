using SFA.DAS.Encoder.CommandLines;
using SFA.DAS.Encoding;

namespace SFA.DAS.Encoder.Commands
{
    internal class EncodeCommand : ICommand
    {
        private readonly EncodeCommandLine _args;
        private readonly IConsole _console;
        private readonly IEncodingService _encodingService;

        public EncodeCommand(EncodeCommandLine args, IConsole console, IEncodingService encodingService)
        {
            _args = args;
            _console = console;
            _encodingService = encodingService;
        }

        public void Run()
        {
            var encoded = _encodingService.Encode(_args.Value, _args.Encoding);

            _console.Write($"Encoded: {encoded} Value: {_args.Value} Type:{_args.Encoding}");
        }
    }
}