namespace GraphFx;

public interface IGraphFormatter<TVertex, TEdgeLabel>
    where TVertex : notnull
    where TEdgeLabel : notnull
{
    IStringFormatter<TVertex> VertexFormatter { get; }

    IStringFormatter<TEdgeLabel> EdgeFormatter { get; }
}

public interface IGraphFormatter<TVertex>
    where TVertex : notnull
{
    IStringFormatter<TVertex> VertexFormatter { get; }
}
