using GraphFx;
using GraphFx.Samples;
using GraphFx.Testing.Generators;

//var explicitGraph = new ExplicitGraph<string, int>();
//explicitGraph = new ExplicitGraph<string, int>();
//explicitGraph.Add("A", 10, "B");
//explicitGraph.Add("B", 20, "C");
//explicitGraph.Add("C", 30, "A");
//explicitGraph.Add("B", 40, "D");

//Console.WriteLine(explicitGraph.ToIncidenceGraph().Format());

var collatz = CollatzGraphs.Incidence(32);
Console.WriteLine(collatz.Format());

var collatzInverted = CollatzGraphs.IncidenceInverted(Enumerable.Range(1, 2));
Console.WriteLine(collatzInverted.Format());

//var graph = DirectedGraph
//    .Builder<string, int>()
//    .AddEdge("A", 10, "B")
//    .AddEdge("B", 20, "C")
//    .AddEdge("C", 30, "A")
//    .Build();

//Console.WriteLine(graph.ToEnglishCompact());

//var graph2 = DirectedGraph
//    .Builder<string>()
//    .AddEdge("A", "B")
//    .AddEdge("B", "C")
//    .AddEdge("C", "A")
//    .AddVertices("C", "D", "E", "F")
//    .Build();

//Console.WriteLine(graph2.ToEnglishCompact());

//var assemblies = ReflectionGraphs
//    .AssemblyDependencyGraph(@"(^System\.(Console|Text|Runtime|Private))|(^netstandard$)")
//    .ToDirectedGraph();
//Console.WriteLine(assemblies.ToEnglishReadable());

Console.WriteLine();
