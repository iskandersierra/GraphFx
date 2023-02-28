using System.Diagnostics.CodeAnalysis;

namespace GraphFx;

public interface IImplicitGraph<in TVertex> :
    IGraph<TVertex>
    where TVertex : notnull
{
    bool ContainsVertex(TVertex vertex);
    bool ContainsEdge(TVertex sourceVertex, TVertex targetVertex);
}

public interface IImplicitGraph<in TVertex, TEdgeLabel> :
    IImplicitGraph<TVertex>,
    IGraph<TVertex, TEdgeLabel>
    where TVertex : notnull
    where TEdgeLabel : notnull
{
    bool TryGetEdgeLabel(TVertex sourceVertex, TVertex targetVertex, [NotNullWhen(true)] out TEdgeLabel edgeLabel);
}
