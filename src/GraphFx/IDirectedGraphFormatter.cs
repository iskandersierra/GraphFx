namespace GraphFx;

public interface IDirectedGraphFormatter<TVertex, TEdgeLabel>
    where TVertex : notnull
    where TEdgeLabel : notnull
{
    string FormatGraph(IDirectedGraph<TVertex, TEdgeLabel> graph);
}

public interface IDirectedGraphFormatter<TVertex>
    where TVertex : notnull
{
    string FormatGraph(IDirectedGraph<TVertex> graph);
}