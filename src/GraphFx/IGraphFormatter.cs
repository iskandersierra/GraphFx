namespace GraphFx;

public interface IGraphFormatter<TNode, TEdge>
    where TNode : notnull
    where TEdge : notnull
{
    IStringFormatter<TNode> NodeFormatter { get; }

    IStringFormatter<TEdge> EdgeFormatter { get; }
}

public interface IGraphFormatter<TNode>
    where TNode : notnull
{
    IStringFormatter<TNode> NodeFormatter { get; }
}
