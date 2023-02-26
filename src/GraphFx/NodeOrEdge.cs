using OneOf;

namespace GraphFx;

[GenerateOneOf]
public partial class NodeOrEdge<TNode, TEdge> :
    OneOfBase<TNode, EdgeDefinition<TNode, TEdge>>
    where TNode : notnull
    where TEdge : notnull
{
}

[GenerateOneOf]
public partial class NodeOrEdgeSimple<TNode> :
    OneOfBase<TNode, EdgeDefinition<TNode>>
    where TNode : notnull
{
}
