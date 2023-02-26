namespace GraphFx;

public interface IDirectedGraph<TNode, TEdge>
    where TNode : notnull
    where TEdge : notnull
{
    IReadOnlyList<TNode> Nodes { get; }

    IReadOnlyList<EdgeDefinition<TNode, TEdge>> Edges { get; }

    IEqualityComparer<TNode> NodeComparer { get; }

    IGraphFormatter<TNode, TEdge> Formatter { get; }
}

public interface IDirectedGraph<TNode>
    where TNode : notnull
{
    IReadOnlyList<TNode> Nodes { get; }

    IReadOnlyList<EdgeDefinition<TNode>> Edges { get; }

    IEqualityComparer<TNode> NodeComparer { get; }

    IGraphFormatter<TNode> Formatter { get; }
}
