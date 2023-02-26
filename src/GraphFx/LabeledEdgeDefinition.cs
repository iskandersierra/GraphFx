using System.Diagnostics;

namespace GraphFx;

[DebuggerDisplay("{ToDebugString()}")]
public readonly struct LabeledEdgeDefinition<TNode, TEdge>
    where TNode : notnull
    where TEdge : notnull
{
    public LabeledEdgeDefinition(TNode source, TEdge edge, TNode target)
    {
        Source = source;
        Edge = edge;
        Target = target;
    }

    public readonly TNode Source;
    public readonly TEdge Edge;
    public readonly TNode Target;

    public void Deconstruct(out TNode source, out TEdge edge, out TNode target)
    {
        source = Source;
        edge = Edge;
        target = Target;
    }

    public string ToDebugString()
    {
        return $"{Source} == {Edge} => {Target}";
    }
}

[DebuggerDisplay("{ToDebugString()}")]
public readonly struct EdgeDefinition<TNode>
    where TNode : notnull
{
    public EdgeDefinition(TNode source, TNode target)
    {
        Source = source;
        Target = target;
    }

    public readonly TNode Source;
    public readonly TNode Target;

    public void Deconstruct(out TNode source, out TNode target)
    {
        source = Source;
        target = Target;
    }

    public string ToDebugString()
    {
        return $"{Source} ==> {Target}";
    }
}

public static class EdgeDefinition
{
    public static LabeledEdgeDefinition<TNode, TEdge> Create<TNode, TEdge>(TNode source, TEdge edge, TNode target)
        where TNode : notnull
        where TEdge : notnull
    {
        return new LabeledEdgeDefinition<TNode, TEdge>(source, edge, target);
    }

    public static EdgeDefinition<TNode> Create<TNode>(TNode source, TNode target)
        where TNode : notnull
    {
        return new EdgeDefinition<TNode>(source, target);
    }
}
