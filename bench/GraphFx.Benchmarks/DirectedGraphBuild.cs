using BenchmarkDotNet.Attributes;

namespace GraphFx.Benchmarks;

[MemoryDiagnoser]
public class DirectedGraphBuild
{
    [Params(100, 1_000, 10_000)]
    public int Size { get; set; }

    [Params(10)]
    public int RandomSeed { get; set; }

    private Random random;

    //private DirectedGraph.GraphBuilder<int>? builder;

    //[GlobalSetup]
    //public void Setup()
    //{
    //    this.random = new Random(this.RandomSeed);
    //    this.builder = DirectedGraph.Builder<int>();

    //    for (int i = 0; i < Size; i++)
    //    {
    //        var num = random.Next(2, Size * 10);
    //        var next = num % 2 == 0 ? num / 2 : num * 3 + 1;
    //        this.builder.AddEdge(num, next);
    //    }
    //}


    //[Benchmark]
    //public IDirectedGraph<int> Unlabeled()
    //{
    //    return builder.Build();
    //}
}