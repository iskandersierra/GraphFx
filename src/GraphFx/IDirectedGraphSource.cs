namespace GraphFx;

public interface IDirectedGraphSource<TNode, TEdge>
    where TNode : notnull
    where TEdge : notnull
{
    IEnumerable<TNode> Nodes { get; }

    IEnumerable<LabeledEdgeDefinition<TNode, TEdge>> Edges { get; }

    IEqualityComparer<TNode> NodeComparer { get; }

    IGraphFormatter<TNode, TEdge> Formatter { get; }
}

public interface IDirectedGraphSource<TNode>
    where TNode : notnull
{
    IEnumerable<TNode> Nodes { get; }

    IEnumerable<EdgeDefinition<TNode>> Edges { get; }

    IEqualityComparer<TNode> NodeComparer { get; }

    IGraphFormatter<TNode> Formatter { get; }
}
