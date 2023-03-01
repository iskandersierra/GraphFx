namespace GraphFx.Traversals;

public class TraversalEvents<TVertex>
    where TVertex : notnull
{
    public Func<VertexTraversalInfo<TVertex>, bool>? OnVertex { get; set; }

    public Func<EdgeTraversalInfo<TVertex>, bool>? OnEdge { get; set; }

    public Func<EdgeTraversalInfo<TVertex>, bool>? OnBacktrackEdge { get; set; }

    public Action<TraversalStats>? OnCompleted { get; set; }
}

public class TraversalEvents<TVertex, TEdgeLabel>
    where TVertex : notnull
    where TEdgeLabel : notnull
{
    public Func<VertexTraversalInfo<TVertex>, bool>? OnVertex { get; set; }

    public Func<EdgeTraversalInfo<TVertex, TEdgeLabel>, bool>? OnEdge { get; set; }

    public Func<EdgeTraversalInfo<TVertex, TEdgeLabel>, bool>? OnBacktrackEdge { get; set; }

    public Action<TraversalStats>? OnCompleted { get; set; }
}
