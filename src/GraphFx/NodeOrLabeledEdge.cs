using OneOf;

namespace GraphFx;

[GenerateOneOf]
public partial class NodeOrLabeledEdge<TNode, TEdge> :
    OneOfBase<TNode, LabeledEdgeDefinition<TNode, TEdge>>
    where TNode : notnull
    where TEdge : notnull
{
}

[GenerateOneOf]
public partial class NodeOrEdge<TNode> :
    OneOfBase<TNode, EdgeDefinition<TNode>>
    where TNode : notnull
{
}
