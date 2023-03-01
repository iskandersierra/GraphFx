namespace GraphFx.Traversals;

public class TraversalOptions : ICloneable
{
    internal static readonly TraversalOptions DefaultOptions = new();
    internal static readonly TraversalOptions DepthFirstSearchOptions = new TraversalOptions().ForDepthFirstSearch();
    internal static readonly TraversalOptions DepthLastSearchOptions = new TraversalOptions().ForDepthLastSearch();

    public TraversalOptions()
    {
    }

    public bool IncludeEdgesWithVisitedTargets { get; set; } = false;

    public bool YieldNodeLast { get; set; } = false;

    public bool YieldBacktrackingEdges { get; set; } = false;

    public int MaxDepth { get; set; } = int.MaxValue;

    public int MaxVertices { get; set; } = int.MaxValue;

    public int MaxEdges { get; set; } = int.MaxValue;

    object ICloneable.Clone()
    {
        return this.Clone();
    }

    public TraversalOptions Clone()
    {
        return (TraversalOptions)MemberwiseClone();
    }

    public TraversalOptions ForDepthFirstSearch()
    {
        if (!YieldNodeLast) return this;
        var clone = Clone();
        clone.YieldNodeLast = false;
        return clone;
    }

    public TraversalOptions ForDepthLastSearch()
    {
        if (YieldNodeLast) return this;
        var clone = Clone();
        clone.YieldNodeLast = true;
        return clone;
    }
}
