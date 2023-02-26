using System.CommandLine;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Spectre.Console;

namespace GraphFx.Benchmarks;

public static class Commands
{
    public static async Task<int> ExecuteAsync(string[] args)
    {
        var rootCommand = new RootCommand();

        rootCommand.AddCommand(BenchmarkCommand());
        rootCommand.AddCommand(ProfileCommand());

        return await rootCommand.InvokeAsync(args);
    }

    private static Command BenchmarkCommand()
    {
        var command = new Command("bench", "Run benchmarks");

        var nameOption = new Option<string>("--name", "Benchmark name. Use list to see available benchmarks.")
        {
            IsRequired = true,
        };

        nameOption.AddAlias("-n");

        command.Add(nameOption);

        var benchmarks = new[]
            {
                typeof(DirectedGraphBuild),
            }
            .ToDictionary(t => t.Name, StringComparer.InvariantCultureIgnoreCase);

        command.SetHandler((name) =>
        {
            if (benchmarks.TryGetValue(name, out var type))
            {
                BenchmarkRunner.Run(type);
            }
            else
            {
                switch (name)
                {
                    case null:
                    case "":
                        AnsiConsole.MarkupLine("[red]No filter specified[/]");
                        break;
                    case "list":
                        break;
                    default:
                        AnsiConsole.MarkupLine("[red]Invalid filter specified[/]");
                        break;
                }
                AnsiConsole.MarkupLine($"Choose one of: {String.Join(", ", benchmarks.Keys)}");
            }
        }, nameOption);

        return command;
    }

    private static Command ProfileCommand()
    {
        var command = new Command("profile", "Run profile");

        var nameOption = new Option<string>("--name", "Benchmark name. Use list to see available benchmarks.")
        {
            IsRequired = true,
        };

        nameOption.AddAlias("-n");

        command.Add(nameOption);

        var countOption = new Option<int>("--count", "Iteration count.")
        {
            IsRequired = true,
        };

        countOption.AddAlias("-c");
        countOption.SetDefaultValue(100);
        countOption.AddValidator(result =>
        {
            if (result.GetValueOrDefault<int>() <= 0)
            {
                result.ErrorMessage = "Count must be greater than 0.";
            }
        });

        command.Add(countOption);

        var sizeOption = new Option<int>("--size", "Iteration size.")
        {
            IsRequired = true,
        };

        sizeOption.AddAlias("-s");
        sizeOption.SetDefaultValue(100);
        sizeOption.AddValidator(result =>
        {
            if (result.GetValueOrDefault<int>() <= 0)
            {
                result.ErrorMessage = "Size must be greater than 0.";
            }
        });

        command.Add(sizeOption);

        var benchmarks = new Dictionary<string, Action<int, int>>(StringComparer.InvariantCultureIgnoreCase)
            {
                ["DirectedGraphBuild_Unlabeled"] = (count, size) =>
                {
                    var bench = new DirectedGraphBuild();
                    bench.Size = size;
                    bench.Setup();
                    for (int i = 0; i < count; i++)
                    {
                        bench.Unlabeled();
                    }
                },
            };

        command.SetHandler((name, count, size) =>
        {
            if (benchmarks.TryGetValue(name, out var action))
            {
                action(count, size);
            }
            else
            {
                switch (name)
                {
                    case null:
                    case "":
                        AnsiConsole.MarkupLine("[red]No filter specified[/]");
                        break;
                    case "list":
                        break;
                    default:
                        AnsiConsole.MarkupLine("[red]Invalid filter specified[/]");
                        break;
                }
                AnsiConsole.MarkupLine($"Choose one of: {String.Join(", ", benchmarks.Keys)}");
            }
        }, nameOption, countOption, sizeOption);

        return command;
    }
}