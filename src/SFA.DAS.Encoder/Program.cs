using CommandLine;
using Microsoft.Extensions.Configuration;
using SFA.DAS.Configuration.AzureTableStorage;
using SFA.DAS.Encoder.CommandLines;
using SFA.DAS.Encoder.Commands;
using SFA.DAS.Encoder.DependencyResolution;

namespace SFA.DAS.Encoder
{
    class Program
    {
        private static readonly IConsole Console = new Console();

        private static void Main(string[] args)
        {
            System.Console.Title = Constants.AppName;

            Parser.Default.ParseArguments<DecodeCommandLine, EncodeCommandLine, ListCommandLine>(args)
                .WithParsed<EncodeCommandLine>(Run<EncodeCommandLine, EncodeCommand>)
                .WithParsed<DecodeCommandLine>(Run<DecodeCommandLine, DecodeCommand>)
                .WithParsed<ListCommandLine>(Run<ListCommandLine, ListCommand>)
                .WithNotParsed(parserResult =>
                {
                    Console.Write("The command line is incorrect:");
                    foreach (Error error in parserResult)
                    {
                        Console.Write((object)error.Tag);
                    }
                });
        }

        private static void Run<TArgs, TCommand>(TArgs args) 
                where TArgs : class 
                where TCommand : ICommand
        {
            var container = IoC.InitializeIoC(c =>
            {
                c.For<IConfiguration>().Use(ctx => SetupConfiguration());
                c.For<IConsole>().Use(Console);
                c.For<TArgs>().Use(args);
            });

            var command = container.GetInstance<TCommand>();

            command.Run();
        }

        private static IConfiguration SetupConfiguration()
        {
            IConfiguration config = new ConfigurationBuilder()
                .AddAzureTableStorage(Constants.ConfigurationKeys.EncodingConfig)
                .Build();

            return config;
        }
    }
}
