namespace GraphFx;

public interface IIncidenceGraph<TVertex> :
    IGraph<TVertex>
    where TVertex : notnull
{
    IEnumerable<TVertex> SeedVertices { get; }
    IEnumerable<TVertex> OutgoingEdges(TVertex vertex);

    int OutgoingDegree(TVertex vertex);
}

public interface IIncidenceGraph<TVertex, TEdgeLabel> :
    IIncidenceGraph<TVertex>,
    IGraph<TVertex, TEdgeLabel>
    where TVertex : notnull
    where TEdgeLabel : notnull
{
    IEnumerable<OutgoingEdge<TVertex, TEdgeLabel>> OutgoingLabeledEdges(TVertex sourceVertex);
}
