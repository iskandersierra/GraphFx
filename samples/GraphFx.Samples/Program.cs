// See https://aka.ms/new-console-template for more information

using GraphFx;

Console.WriteLine("Hello, World!");

var graph = DirectedGraph
    .Builder<string, int>()
    .AddEdge("A", 10, "B")
    .AddEdge("B", 20, "C")
    .AddEdge("C", 30, "A")
    .Build();
