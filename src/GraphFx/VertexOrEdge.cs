using OneOf;

namespace GraphFx;

[GenerateOneOf]
public partial class VertexOrLabeledEdge<TVertex, TEdgeLabel> :
    OneOfBase<TVertex, Edge<TVertex, TEdgeLabel>>
    where TVertex : notnull
    where TEdgeLabel : notnull
{
}

[GenerateOneOf]
public partial class VertexOrEdge<TVertex> :
    OneOfBase<TVertex, Edge<TVertex>>
    where TVertex : notnull
{
}
