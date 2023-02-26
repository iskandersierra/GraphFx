namespace GraphFx;

public interface ISeededDirectedGraphSource<TNode, TEdge>
    where TNode : notnull
    where TEdge : notnull
{
    IEnumerable<TNode> SeedNodes { get; }

    IEnumerable<(TEdge, TNode)> GetEdges(TNode sourceNode);

    IEqualityComparer<TNode> NodeComparer { get; }

    IGraphFormatter<TNode, TEdge> Formatter { get; }
}

public interface ISeededDirectedGraphSource<TNode>
    where TNode : notnull
{
    IEnumerable<TNode> SeedNodes { get; }

    IEnumerable<TNode> GetEdges(TNode sourceNode);

    IEqualityComparer<TNode> NodeComparer { get; }

    IGraphFormatter<TNode> Formatter { get; }
}
