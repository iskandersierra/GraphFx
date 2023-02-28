using System.Reflection;
using System.Text.RegularExpressions;
using GraphFx;

namespace GraphFx.Samples;

public static class ReflectionGraphs
{
    //public static ISeededDirectedGraphSource<Assembly> AssemblyDependencyGraph(string? excludePattern = null)
    //{
    //    var regex = excludePattern is null ? null : new Regex(excludePattern);

    //    static string GetName(AssemblyName assemblyName) => assemblyName.Name ?? "UNKNOWN";
    //    static string GetAssemblyName(Assembly assembly) => GetName(assembly.GetName());

    //    return SeededDirectedGraphSource.Create(
    //        typeof(ReflectionGraphs).Assembly,
    //        asm => asm.GetReferencedAssemblies()
    //            .Where(name => regex?.IsMatch(GetName(name)) != true)
    //            .Select(Assembly.Load),
    //        GetAssemblyName);
    //}
}
