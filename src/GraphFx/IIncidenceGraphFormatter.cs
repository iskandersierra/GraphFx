namespace GraphFx;

public interface IIncidenceGraphFormatter<TVertex>
    where TVertex : notnull
{
    string Format(IIncidenceGraph<TVertex> graph);
}

public interface IIncidenceGraphFormatter<TVertex, TEdgeLabel>
    where TVertex : notnull
    where TEdgeLabel : notnull
{
    string Format(IIncidenceGraph<TVertex, TEdgeLabel> graph);
}
