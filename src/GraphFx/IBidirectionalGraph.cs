namespace GraphFx;

public interface IBidirectionalGraph<TVertex> :
    IIncidenceGraph<TVertex>
    where TVertex : notnull
{
    IEnumerable<TVertex> IncomingEdges(TVertex vertex);

    int IncomingDegree(TVertex vertex);
}

public interface IBidirectionalGraph<TVertex, TEdgeLabel> :
    IBidirectionalGraph<TVertex>,
    IIncidenceGraph<TVertex, TEdgeLabel>
    where TVertex : notnull
    where TEdgeLabel : notnull
{
    IEnumerable<IncomingEdge<TVertex, TEdgeLabel>> IncomingLabeledEdges(TVertex vertex);
}
