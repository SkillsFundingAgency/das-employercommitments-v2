using System;
using System.Linq;
using SFA.DAS.Encoder.CommandLines;
using SFA.DAS.Encoding;

namespace SFA.DAS.Encoder.Commands
{
    internal class DecodeCommand : ICommand
    {
        private readonly DecodeCommandLine _args;
        private readonly IConsole _console;
        private readonly IEncodingService _encodingService;

        public DecodeCommand(DecodeCommandLine args, IConsole console, IEncodingService encodingService)
        {
            _args = args;
            _console = console;
            _encodingService = encodingService;
        }

        public void Run()
        {
            if (_args.Which)
            {
                FindDecoder();
            }
            else
            {
                TryDecoder();
            }
        }

        private void TryDecoder()
        {
            if (_encodingService.TryDecode(_args.Value, _args.Encoding, out var decodedValue))
            {
                Write(_args.Encoding, _args.Value, decodedValue, "Decoded");
            }
            else
            {
                Write(_args.Encoding, _args.Value, 0, "Decoding (failed)");
            }
        }

        private void FindDecoder()
        {
            _console.Write($"Finding decoder for value {_args.Value}");

            foreach (var encodingType in Enum.GetValues(typeof(EncodingType)).OfType<EncodingType>())
            {
                string message;
                try
                {
                    if (_encodingService.TryDecode(_args.Value, encodingType, out var decodedValue))
                    {
                        message = $"Success - {decodedValue}";
                    }
                    else
                    {
                        message = "Decoding failed";
                    }
                }
                catch (Exception e)
                {
                    message = $"Decoding failed - {e.Message}";
                }

                _console.Write($"{encodingType,-30} {message}");
            }
        }

        private void Write(EncodingType encodingType, string encodedValue, long decodedValue, string direction)
        {
            _console.Write($"Encoded: {encodedValue} Value: {decodedValue} Type:{encodingType} Action:{direction}");
        }
    }
}