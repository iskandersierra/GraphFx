namespace GraphFx;

public interface IEnumerableGraph<TVertex> :
    IGraph<TVertex>
    where TVertex : notnull
{
    IEnumerable<TVertex> Vertices { get; }
    IEnumerable<Edge<TVertex>> Edges { get; }
}

public interface IEnumerableGraph<TVertex, TEdgeLabel> :
    IGraph<TVertex, TEdgeLabel>,
    IEnumerableGraph<TVertex>
    where TVertex : notnull
    where TEdgeLabel : notnull
{
    IEnumerable<Edge<TVertex, TEdgeLabel>> LabeledEdges { get; }
}
