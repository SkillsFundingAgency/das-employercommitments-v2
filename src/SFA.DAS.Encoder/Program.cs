using System;
using System.Net;
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
                        Console.Write(error.Tag);
                    }
                });
        }

        private static void Run<TArgs, TCommand>(TArgs args) 
                where TArgs : CommandLineBase 
                where TCommand : ICommand
        {
            SetEnvironmentName(args);
            SetConnectionString(args);

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

        private static void SetEnvironmentName(CommandLineBase args)
        {
            SetEnvironmentVariableIfNotNull("APPSETTING_EnvironmentName", args.EnvironmentName);
        }

        private static void SetConnectionString(CommandLineBase args)
        {
            SetEnvironmentVariableIfNotNull("APPSETTING_ConfigurationStorageConnectionString", args.ConnectionString);
        }

        private static void SetEnvironmentVariableIfNotNull(string name, string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return;
            }

            Console.Write($"Setting {name}", value);
            Environment.SetEnvironmentVariable(name, value);
        }
    }
}
