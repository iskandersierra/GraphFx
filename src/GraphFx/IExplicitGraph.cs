namespace GraphFx;

public interface IExplicitGraph<TVertex> :
    IGraph<TVertex>
    where TVertex : notnull
{
    IEnumerable<TVertex> Vertices { get; }
    IEnumerable<Edge<TVertex>> Edges { get; }
}

public interface IExplicitGraph<TVertex, TEdgeLabel> :
    IGraph<TVertex, TEdgeLabel>,
    IExplicitGraph<TVertex>
    where TVertex : notnull
    where TEdgeLabel : notnull
{
    IEnumerable<Edge<TVertex, TEdgeLabel>> LabeledEdges { get; }
}
