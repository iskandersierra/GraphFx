using GraphFx.Benchmarks;
using Spectre.Console;

//BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);
int exitCode;
try
{
    exitCode = await Commands.ExecuteAsync(args);
}
catch (Exception exception)
{
    AnsiConsole.WriteException(exception);
    exitCode = 1;
}
//Console.WriteLine("Press any key to exit...");
//Console.ReadKey();
return exitCode;
