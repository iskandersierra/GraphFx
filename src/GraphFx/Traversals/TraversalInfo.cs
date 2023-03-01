using System.Diagnostics;

namespace GraphFx.Traversals;

[DebuggerDisplay("{ToDebugString()}")]
public readonly struct TraversalStats
{
    public readonly int Depth;
    public readonly int MaxDepth;
    public readonly int VertexCount;
    public readonly int EdgeCount;

    public TraversalStats(int depth, int maxDepth, int vertexCount, int edgeCount)
    {
        Depth = depth;
        MaxDepth = maxDepth;
        VertexCount = vertexCount;
        EdgeCount = edgeCount;
    }

    public string ToDebugString()
    {
        return $"Depth = {Depth}/{MaxDepth}, VertexCount = {VertexCount}, EdgeCount = {EdgeCount}, ";
    }
}

[DebuggerDisplay("{ToDebugString()}")]
public readonly struct EdgeTraversalInfo<TVertex>
    where TVertex : notnull
{
    public readonly Edge<TVertex> Edge;
    public readonly TraversalStats Stats;
    public readonly bool IsNewTargetVertex;
    public readonly bool IsBacktracking;

    public EdgeTraversalInfo(Edge<TVertex> edge, TraversalStats stats, bool isNewTargetVertex, bool isBacktracking = false)
    {
        Edge = edge;
        IsNewTargetVertex = isNewTargetVertex;
        IsBacktracking = isBacktracking;
        Stats = stats;
    }

    public void Deconstruct(out Edge<TVertex> edge, out TraversalStats stats)
    {
        edge = Edge;
        stats = Stats;
    }

    public void Deconstruct(out Edge<TVertex> edge, out TraversalStats stats, out bool isNewTargetVertex)
    {
        edge = Edge;
        stats = Stats;
        isNewTargetVertex = IsNewTargetVertex;
    }

    public void Deconstruct(out Edge<TVertex> edge, out TraversalStats stats, out bool isNewTargetVertex, out bool isBacktracking)
    {
        edge = Edge;
        stats = Stats;
        isNewTargetVertex = IsNewTargetVertex;
        isBacktracking = IsBacktracking;
    }

    public string ToDebugString()
    {
        return $"Edge: {Edge.ToDebugString()}, to {(IsNewTargetVertex ? "NEW" : "VISITED")} vertex{(IsBacktracking ? " (backtracking)" : "")}";
    }
}

[DebuggerDisplay("{ToDebugString()}")]
public readonly struct EdgeTraversalInfo<TVertex, TEdgeLabel>
    where TVertex : notnull
    where TEdgeLabel : notnull
{
    public readonly Edge<TVertex, TEdgeLabel> Edge;
    public readonly TraversalStats Stats;
    public readonly bool IsNewTargetVertex;
    public readonly bool IsBacktracking;

    public EdgeTraversalInfo(Edge<TVertex, TEdgeLabel> edge, TraversalStats stats, bool isNewTargetVertex, bool isBacktracking = false)
    {
        Edge = edge;
        IsNewTargetVertex = isNewTargetVertex;
        IsBacktracking = isBacktracking;
        Stats = stats;
    }

    public void Deconstruct(out Edge<TVertex, TEdgeLabel> edge, out TraversalStats stats)
    {
        edge = Edge;
        stats = Stats;
    }

    public void Deconstruct(out Edge<TVertex, TEdgeLabel> edge, out TraversalStats stats, out bool isNewTargetVertex)
    {
        edge = Edge;
        stats = Stats;
        isNewTargetVertex = IsNewTargetVertex;
    }

    public void Deconstruct(out Edge<TVertex, TEdgeLabel> edge, out TraversalStats stats, out bool isNewTargetVertex, out bool isBacktracking)
    {
        edge = Edge;
        stats = Stats;
        isNewTargetVertex = IsNewTargetVertex;
        isBacktracking = IsBacktracking;
    }

    public string ToDebugString()
    {
        return $"Edge: {Edge.ToDebugString()}, to {(IsNewTargetVertex ? "NEW" : "VISITED")} vertex{(IsBacktracking ? " (backtracking)" : "")}";
    }
}

[DebuggerDisplay("{ToDebugString()}")]
public readonly struct VertexTraversalInfo<TVertex>
    where TVertex : notnull
{
    public readonly TVertex Vertex;
    public readonly TraversalStats Stats;

    public VertexTraversalInfo(TVertex vertex, TraversalStats stats)
    {
        Vertex = vertex;
        Stats = stats;
    }

    public void Deconstruct(out TVertex vertex, out TraversalStats stats)
    {
        vertex = Vertex;
        stats = Stats;
    }

    public string ToDebugString()
    {
        return $"Vertex: {Vertex}";
    }
}
