namespace GraphFx;

public interface IDirectedGraphSource<TVertex, TEdgeLabel>
    where TVertex : notnull
    where TEdgeLabel : notnull
{
    IEnumerable<TVertex> Vertices { get; }

    IEnumerable<LabeledEdge<TVertex, TEdgeLabel>> Edges { get; }

    IEqualityComparer<TVertex> VertexComparer { get; }

    IGraphFormatter<TVertex, TEdgeLabel> Formatter { get; }
}

public interface IDirectedGraphSource<TVertex>
    where TVertex : notnull
{
    IEnumerable<TVertex> Vertices { get; }

    IEnumerable<Edge<TVertex>> Edges { get; }

    IEqualityComparer<TVertex> VertexComparer { get; }

    IGraphFormatter<TVertex> Formatter { get; }
}
