using System.Diagnostics;

namespace GraphFx;

[DebuggerDisplay("{ToDebugString()}")]
public readonly struct LabeledEdge<TVertex, TEdgeLabel>
    where TVertex : notnull
    where TEdgeLabel : notnull
{
    public LabeledEdge(TVertex source, TEdgeLabel label, TVertex target)
    {
        Source = source ?? throw new ArgumentNullException(nameof(source));
        Label = label ?? throw new ArgumentNullException(nameof(label));
        Target = target ?? throw new ArgumentNullException(nameof(target));
    }

    public readonly TVertex Source;
    public readonly TEdgeLabel Label;
    public readonly TVertex Target;

    public void Deconstruct(out TVertex source, out TEdgeLabel edge, out TVertex target)
    {
        source = Source;
        edge = Label;
        target = Target;
    }

    public string ToDebugString()
    {
        return $"{Source} == {Label} => {Target}";
    }
}

[DebuggerDisplay("{ToDebugString()}")]
public readonly struct Edge<TVertex>
    where TVertex : notnull
{
    public Edge(TVertex source, TVertex target)
    {
        Source = source ?? throw new ArgumentNullException(nameof(source));
        Target = target ?? throw new ArgumentNullException(nameof(target));
    }

    public readonly TVertex Source;
    public readonly TVertex Target;

    public void Deconstruct(out TVertex source, out TVertex target)
    {
        source = Source;
        target = Target;
    }

    public string ToDebugString()
    {
        return $"{Source} ==> {Target}";
    }
}

public static class Edge
{
    public static LabeledEdge<TVertex, TEdgeLabel> CreateLabeled<TVertex, TEdgeLabel>(TVertex source, TEdgeLabel edge, TVertex target)
        where TVertex : notnull
        where TEdgeLabel : notnull
    {
        return new LabeledEdge<TVertex, TEdgeLabel>(source, edge, target);
    }

    public static Edge<TVertex> Create<TVertex>(TVertex source, TVertex target)
        where TVertex : notnull
    {
        return new Edge<TVertex>(source, target);
    }
}
