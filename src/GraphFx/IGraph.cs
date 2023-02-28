namespace GraphFx;

public interface IGraph<in TVertex>
    where TVertex : notnull
{
    IEqualityComparer<TVertex> VertexEqualityComparer { get; }

    IComparer<TVertex> VertexComparer { get; }

    IStringFormatter<TVertex> VertexFormatter { get; }
}

public interface IGraph<in TVertex, in TEdgeLabel> :
    IGraph<TVertex>
    where TVertex : notnull
    where TEdgeLabel : notnull
{
    IStringFormatter<TEdgeLabel> EdgeLabelFormatter { get; }
}