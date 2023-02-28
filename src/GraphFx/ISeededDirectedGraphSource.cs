namespace GraphFx;

public interface ISeededDirectedGraphSource<TVertex, TEdgeLabel>
    where TVertex : notnull
    where TEdgeLabel : notnull
{
    IEnumerable<TVertex> SeedVertices { get; }

    IEnumerable<(TEdgeLabel, TVertex)> GetEdges(TVertex sourceVertex);

    IEqualityComparer<TVertex> VertexComparer { get; }

    IGraphFormatter<TVertex, TEdgeLabel> Formatter { get; }
}

public interface ISeededDirectedGraphSource<TVertex>
    where TVertex : notnull
{
    IEnumerable<TVertex> SeedVertices { get; }

    IEnumerable<TVertex> GetEdges(TVertex sourceVertex);

    IEqualityComparer<TVertex> VertexComparer { get; }

    IGraphFormatter<TVertex> Formatter { get; }
}
