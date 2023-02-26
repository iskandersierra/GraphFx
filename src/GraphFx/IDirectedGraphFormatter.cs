namespace GraphFx;

public interface IDirectedGraphFormatter<TNode, TEdge>
    where TNode : notnull
    where TEdge : notnull
{
    string FormatGraph(IDirectedGraph<TNode, TEdge> graph);
}

public interface IDirectedGraphFormatter<TNode>
    where TNode : notnull
{
    string FormatGraph(IDirectedGraph<TNode> graph);
}