namespace GraphFx;

public interface IDirectedGraph<TVertex, TEdgeLabel>
    where TVertex : notnull
    where TEdgeLabel : notnull
{
    IReadOnlyList<TVertex> Vertices { get; }

    IReadOnlyList<Edge<TVertex, TEdgeLabel>> Edges { get; }

    IEqualityComparer<TVertex> VertexComparer { get; }

    IGraphFormatter<TVertex, TEdgeLabel> Formatter { get; }
}

public interface IDirectedGraph<TVertex>
    where TVertex : notnull
{
    IReadOnlyList<TVertex> Vertices { get; }

    IReadOnlyList<Edge<TVertex>> Edges { get; }

    IEqualityComparer<TVertex> VertexComparer { get; }

    IGraphFormatter<TVertex> Formatter { get; }
}