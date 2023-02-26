using System.Reflection;
using System.Text.RegularExpressions;
using GraphFx;

namespace GraphFx.Samples;

public static class ReflectionGraphs
{
    public static ISeededDirectedGraphSource<Assembly> AssemblyDependencyGraph(string? excludePattern = null)
    {
        var excludedRegex = excludePattern is null ? null : 
                new Regex(excludePattern);

        string GetName(AssemblyName assemblyName) => assemblyName.Name ?? "UNKNOWN";

        bool IsIncluded(AssemblyName assemblyName)
        {
            return excludedRegex?.IsMatch(GetName(assemblyName)) != true;
        }

        IEnumerable<Assembly> Seeds()
        {
            yield return typeof(ReflectionGraphs).Assembly;
        }

        IEnumerable<Assembly> GetDependencies(Assembly current)
        {
            return current.GetReferencedAssemblies()
                .Where(IsIncluded)
                .Select(Assembly.Load);
        }

        return SeededDirectedGraphSource
            .Create(Seeds(), GetDependencies)
            .WithFormatter(GraphFormatter.Create<Assembly>(assembly => GetName(assembly.GetName())));
    }
}
