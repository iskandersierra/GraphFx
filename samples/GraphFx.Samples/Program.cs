﻿using GraphFx;
using GraphFx.Samples;

var graph = DirectedGraph
    .Builder<string, int>()
    .AddEdge("A", 10, "B")
    .AddEdge("B", 20, "C")
    .AddEdge("C", 30, "A")
    .Build();

Console.WriteLine(graph.ToEnglishCompact());

var graph2 = DirectedGraph
    .Builder<string>()
    .AddEdge("A", "B")
    .AddEdge("B", "C")
    .AddEdge("C", "A")
    .Build();

Console.WriteLine(graph2.ToEnglishCompact());

var assemblies = ReflectionGraphs
    .AssemblyDependencyGraph(@"(^System\.)|(^netstandard$)")
    .ToDirectedGraph();
Console.WriteLine(assemblies.ToEnglishReadable());
