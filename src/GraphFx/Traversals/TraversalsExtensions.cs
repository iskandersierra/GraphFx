using OneOf;
using System.Diagnostics;
using System.Linq;

namespace GraphFx.Traversals;

public static class TraversalsExtensions
{
    #region [ TraverseSearch ]

    public static void TraverseSearchRecursive<TVertex, TEdgeLabel>(
        this IIncidenceGraph<TVertex, TEdgeLabel> graph,
        TraversalEvents<TVertex, TEdgeLabel> events,
        TraversalOptions? options = null)
        where TVertex : notnull
        where TEdgeLabel : notnull
    {
        if (events == null) throw new ArgumentNullException(nameof(events));
        options ??= TraversalOptions.DefaultOptions;
        var vertexCount = 0;
        var edgeCount = 0;
        var maxDepth = 1;
        var visited = new HashSet<TVertex>(graph.VertexEqualityComparer);

        bool LoopEdge(TVertex source, OutgoingEdge<TVertex, TEdgeLabel> edge, int depth)
        {
            var isNewTargetVertex = !visited.Contains(edge.Target);

            if (!isNewTargetVertex && !options.IncludeEdgesWithVisitedTargets) return true;

            edgeCount++;

            if (events.OnEdge is { } onEdge &&
                !onEdge(new(
                    new(source, edge.Label, edge.Target),
                    new(depth, maxDepth, vertexCount, edgeCount),
                    isNewTargetVertex)))
                return false;

            if (edgeCount >= options.MaxEdges) return false;

            if (isNewTargetVertex && !LoopNewVertex(edge.Target, depth + 1)) return false;

            if (options.YieldBacktrackingEdges &&
                events.OnBacktrackEdge is { } onBacktrackEdge &&
                !onBacktrackEdge(new(
                    new(source, edge.Label, edge.Target),
                    new(depth, maxDepth, vertexCount, edgeCount),
                    isNewTargetVertex,
                    true)))
                return false;

            return true;
        }

        bool LoopNewVertex(TVertex source, int depth)
        {
            if (depth > options.MaxDepth) return true;
            maxDepth = Math.Max(maxDepth, depth);

            if (options.YieldNodeLast == false)
            {
                vertexCount++;

                if (events.OnVertex is { } onVertex &&
                    !onVertex(new(source, new(depth, maxDepth, vertexCount, edgeCount))))
                    return false;

                if (vertexCount >= options.MaxVertices) return false;
            }

            foreach (var edge in graph.OutgoingLabeledEdges(source))
            {
                if (!LoopEdge(source, edge, depth)) return false;
            }

            if (options.YieldNodeLast == true)
            {
                vertexCount++;

                if (events.OnVertex is { } onVertex &&
                    !onVertex(new(source, new(depth, maxDepth, vertexCount, edgeCount))))
                    return false;

                if (vertexCount >= options.MaxVertices) return false;
            }

            return true;
        }

        bool LoopVertex(TVertex source, int depth)
        {
            var isNewVertex = visited.Add(source);
            if (!isNewVertex) return true;

            return LoopNewVertex(source, depth);
        }

        if (options.MaxDepth >= 1)
            foreach (var seed in graph.SeedVertices)
                if (!LoopVertex(seed, 1)) break;

        if (events.OnCompleted is { } onCompleted)
            onCompleted(new(1, maxDepth, vertexCount, edgeCount));
    }

    public static void TraverseSearchRecursive<TVertex>(
        this IIncidenceGraph<TVertex> graph,
        TraversalEvents<TVertex> events,
        TraversalOptions? options = null)
        where TVertex : notnull
    {
        if (events == null) throw new ArgumentNullException(nameof(events));
        options ??= TraversalOptions.DefaultOptions;
        var vertexCount = 0;
        var edgeCount = 0;
        var maxDepth = 1;
        var visited = new HashSet<TVertex>(graph.VertexEqualityComparer);

        bool LoopEdge(TVertex source, TVertex target, int depth)
        {
            var isNewTargetVertex = visited.Add(target);

            if (!isNewTargetVertex && !options.IncludeEdgesWithVisitedTargets) return true;

            edgeCount++;

            if (events.OnEdge is { } onEdge &&
                !onEdge(new(
                    new(source, target),
                    new(depth, maxDepth, vertexCount, edgeCount),
                    isNewTargetVertex)))
                return false;

            if (edgeCount >= options.MaxEdges) return false;

            if (isNewTargetVertex && !LoopNewVertex(target, depth + 1)) return false;

            if (options.YieldBacktrackingEdges &&
                events.OnBacktrackEdge is { } onBacktrackEdge &&
                !onBacktrackEdge(new(
                    new(source, target),
                    new(depth, maxDepth, vertexCount, edgeCount),
                    isNewTargetVertex)))
                return false;

            return true;
        }

        bool LoopNewVertex(TVertex source, int depth)
        {
            if (depth > options.MaxDepth) return true;
            maxDepth = Math.Max(maxDepth, depth);

            if (options.YieldNodeLast == false)
            {
                vertexCount++;

                if (events.OnVertex is { } onVertex &&
                    !onVertex(new(source, new(depth, maxDepth, vertexCount, edgeCount))))
                    return false;

                if (vertexCount >= options.MaxVertices) return false;
            }

            foreach (var target in graph.OutgoingEdges(source))
            {
                if (!LoopEdge(source, target, depth)) return false;
            }

            if (options.YieldNodeLast == true)
            {
                vertexCount++;

                if (events.OnVertex is { } onVertex &&
                    !onVertex(new(source, new(depth, maxDepth, vertexCount, edgeCount))))
                    return false;

                if (vertexCount >= options.MaxVertices) return false;
            }

            return true;
        }

        bool LoopVertex(TVertex source, int depth)
        {
            var isNewVertex = visited.Add(source);
            if (!isNewVertex) return true;

            return LoopNewVertex(source, depth);
        }

        if (options.MaxDepth >= 1)
            foreach (var seed in graph.SeedVertices)
                if (!LoopVertex(seed, 1)) break;

        if (events.OnCompleted is { } onCompleted)
            onCompleted(new(1, maxDepth, vertexCount, edgeCount));
    }

    public static IEnumerable<OneOf<VertexTraversalInfo<TVertex>, EdgeTraversalInfo<TVertex, TEdgeLabel>>> TraverseSearchRecursive<TVertex, TEdgeLabel>(
        this IIncidenceGraph<TVertex, TEdgeLabel> graph,
        TraversalOptions? options = null)
        where TVertex : notnull
        where TEdgeLabel : notnull
    {
        options ??= TraversalOptions.DefaultOptions;
        var vertexCount = 0;
        var edgeCount = 0;
        var maxDepth = 1;
        var visited = new HashSet<TVertex>(graph.VertexEqualityComparer);

        bool IsBeyondLimits()
        {
            if (vertexCount >= options.MaxVertices) return true;
            if (edgeCount >= options.MaxEdges) return true;
            return false;
        }

        IEnumerable<OneOf<VertexTraversalInfo<TVertex>, EdgeTraversalInfo<TVertex, TEdgeLabel>>> LoopEdge(
            TVertex source, OutgoingEdge<TVertex, TEdgeLabel> edge, int depth)
        {
            var isNewTargetVertex = !visited.Contains(edge.Target);

            if (!isNewTargetVertex && !options.IncludeEdgesWithVisitedTargets) yield break;

            edgeCount++;

            yield return new EdgeTraversalInfo<TVertex, TEdgeLabel>(
                new(source, edge.Label, edge.Target),
                new(depth, maxDepth, vertexCount, edgeCount),
                isNewTargetVertex);

            if (edgeCount < options.MaxEdges && isNewTargetVertex)
            {
                foreach (var e in LoopNewVertex(edge.Target, depth + 1))
                {
                    yield return e;
                    if (IsBeyondLimits()) break;
                }
            }

            if (options.YieldBacktrackingEdges)
                yield return new EdgeTraversalInfo<TVertex, TEdgeLabel>(
                    new(source, edge.Label, edge.Target),
                    new(depth, maxDepth, vertexCount, edgeCount),
                    isNewTargetVertex,
                    true);
        }

        IEnumerable<OneOf<VertexTraversalInfo<TVertex>, EdgeTraversalInfo<TVertex, TEdgeLabel>>> LoopNewVertex(
            TVertex source, int depth)
        {
            if (depth > options.MaxDepth) yield break;
            maxDepth = Math.Max(maxDepth, depth);

            if (options.YieldNodeLast == false)
            {
                vertexCount++;

                yield return new VertexTraversalInfo<TVertex>(
                    source,
                    new(depth, maxDepth, vertexCount, edgeCount));

                if (vertexCount >= options.MaxVertices) yield break;
            }

            foreach (var edge in graph.OutgoingLabeledEdges(source))
                foreach (var result in LoopEdge(source, edge, depth))
                    yield return result;

            if (options.YieldNodeLast == true)
            {
                vertexCount++;

                yield return new VertexTraversalInfo<TVertex>(
                    source,
                    new(depth, maxDepth, vertexCount, edgeCount));

                if (vertexCount >= options.MaxVertices) yield break;
            }
        }

        IEnumerable<OneOf<VertexTraversalInfo<TVertex>, EdgeTraversalInfo<TVertex, TEdgeLabel>>> LoopVertex(
            TVertex source, int depth)
        {
            var isNewVertex = visited.Add(source);
            if (!isNewVertex) yield break;

            foreach (var result in LoopNewVertex(source, depth))
                yield return result;
        }

        if (options.MaxDepth >= 1)
            foreach (var seed in graph.SeedVertices)
                foreach (var result in LoopVertex(seed, 1))
                    yield return result;
    }

    public static IEnumerable<OneOf<VertexTraversalInfo<TVertex>, EdgeTraversalInfo<TVertex>>> TraverseSearchRecursive<TVertex>(
        this IIncidenceGraph<TVertex> graph,
        TraversalOptions? options = null)
        where TVertex : notnull
    {
        options ??= TraversalOptions.DefaultOptions;
        var vertexCount = 0;
        var edgeCount = 0;
        var maxDepth = 1;
        var visited = new HashSet<TVertex>(graph.VertexEqualityComparer);

        bool IsBeyondLimits()
        {
            if (vertexCount >= options.MaxVertices) return true;
            if (edgeCount >= options.MaxEdges) return true;
            return false;
        }

        IEnumerable<OneOf<VertexTraversalInfo<TVertex>, EdgeTraversalInfo<TVertex>>> LoopEdge(
            TVertex source, TVertex target, int depth)
        {
            var isNewTargetVertex = !visited.Contains(target);

            if (!isNewTargetVertex && !options.IncludeEdgesWithVisitedTargets) yield break;

            edgeCount++;

            yield return new EdgeTraversalInfo<TVertex>(
                new(source, target),
                new(depth, maxDepth, vertexCount, edgeCount),
                isNewTargetVertex);

            if (edgeCount < options.MaxEdges && isNewTargetVertex)
            {
                foreach (var e in LoopNewVertex(target, depth + 1))
                {
                    yield return e;
                    if (IsBeyondLimits()) break;
                }
            }

            if (options.YieldBacktrackingEdges)
                yield return new EdgeTraversalInfo<TVertex>(
                    new(source, target),
                    new(depth, maxDepth, vertexCount, edgeCount),
                    isNewTargetVertex,
                    true);
        }

        IEnumerable<OneOf<VertexTraversalInfo<TVertex>, EdgeTraversalInfo<TVertex>>> LoopNewVertex(
            TVertex source, int depth)
        {
            if (depth > options.MaxDepth) yield break;
            maxDepth = Math.Max(maxDepth, depth);

            if (options.YieldNodeLast == false)
            {
                vertexCount++;

                yield return new VertexTraversalInfo<TVertex>(
                    source,
                    new(depth, maxDepth, vertexCount, edgeCount));

                if (vertexCount >= options.MaxVertices) yield break;
            }

            foreach (var target in graph.OutgoingEdges(source))
                foreach (var result in LoopEdge(source, target, depth))
                    yield return result;

            if (options.YieldNodeLast == true)
            {
                vertexCount++;

                yield return new VertexTraversalInfo<TVertex>(
                    source,
                    new(depth, maxDepth, vertexCount, edgeCount));

                if (vertexCount >= options.MaxVertices) yield break;
            }
        }

        IEnumerable<OneOf<VertexTraversalInfo<TVertex>, EdgeTraversalInfo<TVertex>>> LoopVertex(
            TVertex source, int depth)
        {
            var isNewVertex = visited.Add(source);
            if (!isNewVertex) yield break;

            foreach (var result in LoopNewVertex(source, depth))
                yield return result;
        }

        if (options.MaxDepth >= 1)
            foreach (var seed in graph.SeedVertices)
                foreach (var result in LoopVertex(seed, 1))
                    yield return result;
    }

    public static IEnumerable<EdgeTraversalInfo<TVertex, TEdgeLabel>> TraverseSearchEdgesRecursive<TVertex, TEdgeLabel>(
        this IIncidenceGraph<TVertex, TEdgeLabel> graph,
        TraversalOptions? options = null)
        where TVertex : notnull
        where TEdgeLabel : notnull
    {
        options ??= TraversalOptions.DefaultOptions;
        var vertexCount = 0;
        var edgeCount = 0;
        var maxDepth = 1;
        var visited = new HashSet<TVertex>(graph.VertexEqualityComparer);

        bool IsBeyondLimits()
        {
            if (vertexCount >= options.MaxVertices) return true;
            if (edgeCount >= options.MaxEdges) return true;
            return false;
        }

        IEnumerable<EdgeTraversalInfo<TVertex, TEdgeLabel>> LoopEdge(
            TVertex source, OutgoingEdge<TVertex, TEdgeLabel> edge, int depth)
        {
            var isNewTargetVertex = !visited.Contains(edge.Target);

            if (!isNewTargetVertex && !options.IncludeEdgesWithVisitedTargets) yield break;

            edgeCount++;

            yield return new EdgeTraversalInfo<TVertex, TEdgeLabel>(
                new(source, edge.Label, edge.Target),
                new(depth, maxDepth, vertexCount, edgeCount),
                isNewTargetVertex);

            if (edgeCount < options.MaxEdges && isNewTargetVertex)
            {
                foreach (var e in LoopNewVertex(edge.Target, depth + 1))
                {
                    yield return e;
                    if (IsBeyondLimits()) break;
                }
            }

            if (options.YieldBacktrackingEdges)
                yield return new EdgeTraversalInfo<TVertex, TEdgeLabel>(
                    new(source, edge.Label, edge.Target),
                    new(depth, maxDepth, vertexCount, edgeCount),
                    isNewTargetVertex,
                    true);
        }

        IEnumerable<EdgeTraversalInfo<TVertex, TEdgeLabel>> LoopNewVertex(
            TVertex source, int depth)
        {
            if (depth > options.MaxDepth) yield break;
            maxDepth = Math.Max(maxDepth, depth);

            if (options.YieldNodeLast == false)
            {
                vertexCount++;
                if (vertexCount >= options.MaxVertices) yield break;
            }

            foreach (var edge in graph.OutgoingLabeledEdges(source))
                foreach (var result in LoopEdge(source, edge, depth))
                    yield return result;

            if (options.YieldNodeLast == true)
            {
                vertexCount++;
                if (vertexCount >= options.MaxVertices) yield break;
            }
        }

        IEnumerable<EdgeTraversalInfo<TVertex, TEdgeLabel>> LoopVertex(
            TVertex source, int depth)
        {
            var isNewVertex = visited.Add(source);
            if (!isNewVertex) yield break;

            foreach (var result in LoopNewVertex(source, depth))
                yield return result;
        }

        if (options.MaxDepth >= 1)
            foreach (var seed in graph.SeedVertices)
                foreach (var result in LoopVertex(seed, 1))
                    yield return result;
    }

    public static IEnumerable<EdgeTraversalInfo<TVertex>> TraverseSearchEdgesRecursive<TVertex>(
        this IIncidenceGraph<TVertex> graph,
        TraversalOptions? options = null)
        where TVertex : notnull
    {
        options ??= TraversalOptions.DefaultOptions;
        var vertexCount = 0;
        var edgeCount = 0;
        var maxDepth = 1;
        var visited = new HashSet<TVertex>(graph.VertexEqualityComparer);

        bool IsBeyondLimits()
        {
            if (vertexCount >= options.MaxVertices) return true;
            if (edgeCount >= options.MaxEdges) return true;
            return false;
        }

        IEnumerable<EdgeTraversalInfo<TVertex>> LoopEdge(
            TVertex source, TVertex target, int depth)
        {
            var isNewTargetVertex = !visited.Contains(target);

            if (!isNewTargetVertex && !options.IncludeEdgesWithVisitedTargets) yield break;

            edgeCount++;

            yield return new EdgeTraversalInfo<TVertex>(
                new(source, target),
                new(depth, maxDepth, vertexCount, edgeCount),
                isNewTargetVertex);

            if (edgeCount < options.MaxEdges && isNewTargetVertex)
            {
                foreach (var e in LoopNewVertex(target, depth + 1))
                {
                    yield return e;
                    if (IsBeyondLimits()) break;
                }
            }

            if (options.YieldBacktrackingEdges)
                yield return new EdgeTraversalInfo<TVertex>(
                    new(source, target),
                    new(depth, maxDepth, vertexCount, edgeCount),
                    isNewTargetVertex,
                    true);
        }

        IEnumerable<EdgeTraversalInfo<TVertex>> LoopNewVertex(
            TVertex source, int depth)
        {
            if (depth > options.MaxDepth) yield break;
            maxDepth = Math.Max(maxDepth, depth);

            if (options.YieldNodeLast == false)
            {
                vertexCount++;
                if (vertexCount >= options.MaxVertices) yield break;
            }

            foreach (var target in graph.OutgoingEdges(source))
                foreach (var result in LoopEdge(source, target, depth))
                    yield return result;

            if (options.YieldNodeLast == true)
            {
                vertexCount++;
                if (vertexCount >= options.MaxVertices) yield break;
            }
        }

        IEnumerable<EdgeTraversalInfo<TVertex>> LoopVertex(
            TVertex source, int depth)
        {
            var isNewVertex = visited.Add(source);
            if (!isNewVertex) yield break;

            foreach (var result in LoopNewVertex(source, depth))
                yield return result;
        }

        if (options.MaxDepth >= 1)
            foreach (var seed in graph.SeedVertices)
                foreach (var result in LoopVertex(seed, 1))
                    yield return result;
    }

    public static IEnumerable<VertexTraversalInfo<TVertex>> TraverseSearchVerticesRecursive<TVertex>(
        this IIncidenceGraph<TVertex> graph,
        TraversalOptions? options = null)
        where TVertex : notnull
    {
        options ??= TraversalOptions.DefaultOptions;
        var vertexCount = 0;
        var edgeCount = 0;
        var maxDepth = 1;
        var visited = new HashSet<TVertex>(graph.VertexEqualityComparer);

        bool IsBeyondLimits()
        {
            if (vertexCount >= options.MaxVertices) return true;
            if (edgeCount >= options.MaxEdges) return true;
            return false;
        }

        IEnumerable<VertexTraversalInfo<TVertex>> LoopEdge(
            TVertex source, TVertex target, int depth)
        {
            var isNewTargetVertex = !visited.Contains(target);

            if (!isNewTargetVertex && !options.IncludeEdgesWithVisitedTargets) yield break;

            edgeCount++;

            if (edgeCount < options.MaxEdges && isNewTargetVertex)
            {
                foreach (var e in LoopNewVertex(target, depth + 1))
                {
                    yield return e;
                    if (IsBeyondLimits()) break;
                }
            }
        }

        IEnumerable<VertexTraversalInfo<TVertex>> LoopNewVertex(
            TVertex source, int depth)
        {
            if (depth > options.MaxDepth) yield break;
            maxDepth = Math.Max(maxDepth, depth);

            if (options.YieldNodeLast == false)
            {
                vertexCount++;

                yield return new VertexTraversalInfo<TVertex>(
                    source,
                    new(depth, maxDepth, vertexCount, edgeCount));

                if (vertexCount >= options.MaxVertices) yield break;
            }

            foreach (var target in graph.OutgoingEdges(source))
                foreach (var result in LoopEdge(source, target, depth))
                    yield return result;

            if (options.YieldNodeLast == true)
            {
                vertexCount++;

                yield return new VertexTraversalInfo<TVertex>(
                    source,
                    new(depth, maxDepth, vertexCount, edgeCount));

                if (vertexCount >= options.MaxVertices) yield break;
            }
        }

        IEnumerable<VertexTraversalInfo<TVertex>> LoopVertex(
            TVertex source, int depth)
        {
            var isNewVertex = visited.Add(source);
            if (!isNewVertex) yield break;

            foreach (var result in LoopNewVertex(source, depth))
                yield return result;
        }

        if (options.MaxDepth >= 1)
            foreach (var seed in graph.SeedVertices)
                foreach (var result in LoopVertex(seed, 1))
                    yield return result;
    }

    #endregion [ TraverseSearch ]

    #region [ DepthFirstSearch ]

    private static TraversalOptions PrepareDepthFirstSearchOptions(TraversalOptions? options) =>
        options is null ? TraversalOptions.DepthFirstSearchOptions : options.ForDepthFirstSearch();

    public static void DepthFirstSearchRecursive<TVertex, TEdgeLabel>(
        this IIncidenceGraph<TVertex, TEdgeLabel> graph,
        TraversalEvents<TVertex, TEdgeLabel> events,
        TraversalOptions? options = null)
        where TVertex : notnull
        where TEdgeLabel : notnull =>
        graph.TraverseSearchRecursive<TVertex, TEdgeLabel>(events, PrepareDepthFirstSearchOptions(options));

    public static void DepthFirstSearchRecursive<TVertex>(
        this IIncidenceGraph<TVertex> graph,
        TraversalEvents<TVertex> events,
        TraversalOptions? options = null)
        where TVertex : notnull =>
        graph.TraverseSearchRecursive<TVertex>(events, PrepareDepthFirstSearchOptions(options));

    public static IEnumerable<EdgeTraversalInfo<TVertex, TEdgeLabel>> DepthFirstSearchEdgesRecursive<TVertex, TEdgeLabel>(
        this IIncidenceGraph<TVertex, TEdgeLabel> graph,
        TraversalOptions? options = null)
        where TVertex : notnull
        where TEdgeLabel : notnull =>
        graph.TraverseSearchEdgesRecursive<TVertex, TEdgeLabel>(PrepareDepthFirstSearchOptions(options));

    public static IEnumerable<EdgeTraversalInfo<TVertex>> DepthFirstSearchEdgesRecursive<TVertex>(
        this IIncidenceGraph<TVertex> graph,
        TraversalOptions? options = null)
        where TVertex : notnull =>
        graph.TraverseSearchEdgesRecursive<TVertex>(PrepareDepthFirstSearchOptions(options));

    public static IEnumerable<VertexTraversalInfo<TVertex>> DepthFirstSearchVerticesRecursive<TVertex>(
        this IIncidenceGraph<TVertex> graph,
        TraversalOptions? options = null)
        where TVertex : notnull =>
        graph.TraverseSearchVerticesRecursive<TVertex>(PrepareDepthFirstSearchOptions(options));

    #endregion [ DepthFirstSearch ]

    #region [ DepthLastSearch ]

    private static TraversalOptions PrepareDepthLastSearchOptions(TraversalOptions? options) =>
        options is null ? TraversalOptions.DepthLastSearchOptions : options.ForDepthLastSearch();

    public static void DepthLastSearchRecursive<TVertex, TEdgeLabel>(
        this IIncidenceGraph<TVertex, TEdgeLabel> graph,
        TraversalEvents<TVertex, TEdgeLabel> events,
        TraversalOptions? options = null)
        where TVertex : notnull
        where TEdgeLabel : notnull =>
        graph.TraverseSearchRecursive<TVertex, TEdgeLabel>(events, PrepareDepthLastSearchOptions(options));

    public static void DepthLastSearchRecursive<TVertex>(
        this IIncidenceGraph<TVertex> graph,
        TraversalEvents<TVertex> events,
        TraversalOptions? options = null)
        where TVertex : notnull =>
        graph.TraverseSearchRecursive<TVertex>(events, PrepareDepthLastSearchOptions(options));

    public static IEnumerable<EdgeTraversalInfo<TVertex, TEdgeLabel>> DepthLastSearchEdgesRecursive<TVertex, TEdgeLabel>(
        this IIncidenceGraph<TVertex, TEdgeLabel> graph,
        TraversalOptions? options = null)
        where TVertex : notnull
        where TEdgeLabel : notnull =>
        graph.TraverseSearchEdgesRecursive<TVertex, TEdgeLabel>(PrepareDepthLastSearchOptions(options));

    public static IEnumerable<EdgeTraversalInfo<TVertex>> DepthLastSearchEdgesRecursive<TVertex>(
        this IIncidenceGraph<TVertex> graph,
        TraversalOptions? options = null)
        where TVertex : notnull =>
        graph.TraverseSearchEdgesRecursive<TVertex>(PrepareDepthLastSearchOptions(options));

    public static IEnumerable<VertexTraversalInfo<TVertex>> DepthLastSearchVerticesRecursive<TVertex>(
        this IIncidenceGraph<TVertex> graph,
        TraversalOptions? options = null)
        where TVertex : notnull =>
        graph.TraverseSearchVerticesRecursive<TVertex>(PrepareDepthLastSearchOptions(options));

    #endregion [ DepthLastSearch ]
}