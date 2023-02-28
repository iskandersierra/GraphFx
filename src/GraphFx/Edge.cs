using System.Diagnostics;

namespace GraphFx;

[DebuggerDisplay("{ToDebugString()}")]
public readonly struct Edge<TVertex, TEdgeLabel>
    where TVertex : notnull
    where TEdgeLabel : notnull
{
    public Edge(TVertex source, TEdgeLabel label, TVertex target)
    {
        Source = source ?? throw new ArgumentNullException(nameof(source));
        Label = label ?? throw new ArgumentNullException(nameof(label));
        Target = target ?? throw new ArgumentNullException(nameof(target));
    }

    public readonly TVertex Source;
    public readonly TEdgeLabel Label;
    public readonly TVertex Target;

    public void Deconstruct(out TVertex source, out TEdgeLabel label, out TVertex target)
    {
        source = Source;
        label = Label;
        target = Target;
    }

    public string ToDebugString()
    {
        return $"{Source} == {Label} => {Target}";
    }

    public Edge<TVertex> ToUnlabeledEdge()
    {
        return new Edge<TVertex>(Source, Target);
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

[DebuggerDisplay("{ToDebugString()}")]
public readonly struct OutgoingEdge<TVertex, TEdgeLabel>
    where TVertex : notnull
    where TEdgeLabel : notnull
{
    public OutgoingEdge(TEdgeLabel label, TVertex target)
    {
        Label = label ?? throw new ArgumentNullException(nameof(label));
        Target = target ?? throw new ArgumentNullException(nameof(target));
    }

    public readonly TEdgeLabel Label;
    public readonly TVertex Target;

    public void Deconstruct(out TEdgeLabel label, out TVertex target)
    {
        label = Label;
        target = Target;
    }

    public string ToDebugString()
    {
        return $"== {Label} => {Target}";
    }
}

[DebuggerDisplay("{ToDebugString()}")]
public readonly struct IncomingEdge<TVertex, TEdgeLabel>
    where TVertex : notnull
    where TEdgeLabel : notnull
{
    public IncomingEdge(TEdgeLabel label, TVertex source)
    {
        Label = label ?? throw new ArgumentNullException(nameof(label));
        Source = source ?? throw new ArgumentNullException(nameof(source));
    }

    public readonly TEdgeLabel Label;
    public readonly TVertex Source;

    public void Deconstruct(out TEdgeLabel label, out TVertex source)
    {
        label = Label;
        source = Source;
    }

    public string ToDebugString()
    {
        return $"== {Label} => {Source}";
    }
}

public static class Edge
{
    public static Edge<TVertex, TEdgeLabel> Create<TVertex, TEdgeLabel>(
        TVertex source, TEdgeLabel label, TVertex target)
        where TVertex : notnull
        where TEdgeLabel : notnull
    {
        return new Edge<TVertex, TEdgeLabel>(source, label, target);
    }

    public static Edge<TVertex> Create<TVertex>(TVertex source, TVertex target)
        where TVertex : notnull
    {
        return new Edge<TVertex>(source, target);
    }

    public static OutgoingEdge<TVertex, TEdgeLabel> CreateOutgoing<TVertex, TEdgeLabel>(
        TEdgeLabel label, TVertex target)
        where TVertex : notnull
        where TEdgeLabel : notnull
    {
        return new OutgoingEdge<TVertex, TEdgeLabel>(label, target);
    }

    public static IncomingEdge<TVertex, TEdgeLabel> CreateIncoming<TVertex, TEdgeLabel>(
        TEdgeLabel label, TVertex target)
        where TVertex : notnull
        where TEdgeLabel : notnull
    {
        return new IncomingEdge<TVertex, TEdgeLabel>(label, target);
    }
}
