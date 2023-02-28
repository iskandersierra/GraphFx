using System.Reflection;

namespace GraphFx;

public static class SeededDirectedGraphSourceExtensions
{
    public static IEnumerable<VertexOrLabeledEdge<TVertex, TEdgeLabel>> GetAllVerticesAndEdges<TVertex, TEdgeLabel>(
        this ISeededDirectedGraphSource<TVertex, TEdgeLabel> source)
        where TVertex : notnull
        where TEdgeLabel : notnull
    {
        var vertices = new HashSet<TVertex>(source.VertexComparer);
        var queue = new Queue<TVertex>(source.SeedVertices);

        while (queue.Count > 0)
        {
            var vertex = queue.Dequeue();
            yield return vertex;
            foreach (var (edge, targetVertex) in source.GetEdges(vertex))
            {
                if (vertices.Add(targetVertex))
                {
                    queue.Enqueue(targetVertex);
                }

                yield return Edge.Create(vertex, edge, targetVertex);
            }
        }
    }

    public static IEnumerable<TVertex> GetAllVertices<TVertex, TEdgeLabel>(
        this ISeededDirectedGraphSource<TVertex, TEdgeLabel> source)
        where TVertex : notnull
        where TEdgeLabel : notnull
    {
        var vertices = new HashSet<TVertex>(source.VertexComparer);
        var queue = new Queue<TVertex>(source.SeedVertices);

        while (queue.Count > 0)
        {
            var vertex = queue.Dequeue();
            yield return vertex;
            foreach (var (_, targetVertex) in source.GetEdges(vertex))
            {
                if (vertices.Add(targetVertex))
                {
                    queue.Enqueue(targetVertex);
                }
            }
        }
    }

    public static IEnumerable<Edge<TVertex, TEdgeLabel>> GetAllEdges<TVertex, TEdgeLabel>(
        this ISeededDirectedGraphSource<TVertex, TEdgeLabel> source)
        where TVertex : notnull
        where TEdgeLabel : notnull
    {
        var vertices = new HashSet<TVertex>(source.VertexComparer);
        var queue = new Queue<TVertex>(source.SeedVertices);

        while (queue.Count > 0)
        {
            var vertex = queue.Dequeue();
            foreach (var (edge, targetVertex) in source.GetEdges(vertex))
            {
                if (vertices.Add(targetVertex))
                {
                    queue.Enqueue(targetVertex);
                }

                yield return Edge.Create(vertex, edge, targetVertex);
            }
        }
    }

    public static IDirectedGraph<TVertex, TEdgeLabel> ToDirectedGraph<TVertex, TEdgeLabel>(
        this ISeededDirectedGraphSource<TVertex, TEdgeLabel> source)
        where TVertex : notnull
        where TEdgeLabel : notnull
    {
        if (source == null) throw new ArgumentNullException(nameof(source));

        var builder = DirectedGraph
            .Builder<TVertex, TEdgeLabel>()
            .WithVertexComparer(source.VertexComparer);

        foreach (var vertex in source.GetAllVerticesAndEdges())
        {
            vertex.Switch(
                n => builder.AddVertex(n),
                t => builder.AddEdge(t.Source, t.Label, t.Target));
        }

        return builder.Build();
    }

    public static IEnumerable<VertexOrEdge<TVertex>> GetAllVerticesAndEdges<TVertex>(
        this ISeededDirectedGraphSource<TVertex> source)
        where TVertex : notnull
    {
        var vertices = new HashSet<TVertex>(source.VertexComparer);
        var queue = new Queue<TVertex>(source.SeedVertices);

        while (queue.Count > 0)
        {
            var vertex = queue.Dequeue();
            yield return vertex;
            foreach (var targetVertex in source.GetEdges(vertex))
            {
                if (vertices.Add(targetVertex))
                {
                    queue.Enqueue(targetVertex);
                }

                yield return Edge.Create(vertex, targetVertex);
            }
        }
    }

    public static IEnumerable<TVertex> GetAllVertices<TVertex, TEdgeLabel>(
        this ISeededDirectedGraphSource<TVertex> source)
        where TVertex : notnull
    {
        var vertices = new HashSet<TVertex>(source.VertexComparer);
        var queue = new Queue<TVertex>(source.SeedVertices);

        while (queue.Count > 0)
        {
            var vertex = queue.Dequeue();
            yield return vertex;
            foreach (var targetVertex in source.GetEdges(vertex))
            {
                if (vertices.Add(targetVertex))
                {
                    queue.Enqueue(targetVertex);
                }
            }
        }
    }

    public static IEnumerable<Edge<TVertex>> GetAllEdges<TVertex, TEdgeLabel>(
        this ISeededDirectedGraphSource<TVertex> source)
        where TVertex : notnull
    {
        var vertices = new HashSet<TVertex>(source.VertexComparer);
        var queue = new Queue<TVertex>(source.SeedVertices);

        while (queue.Count > 0)
        {
            var vertex = queue.Dequeue();
            foreach (var targetVertex in source.GetEdges(vertex))
            {
                if (vertices.Add(targetVertex))
                {
                    queue.Enqueue(targetVertex);
                }

                yield return Edge.Create(vertex, targetVertex);
            }
        }
    }

    public static IDirectedGraph<TVertex> ToDirectedGraph<TVertex>(
        this ISeededDirectedGraphSource<TVertex> source)
        where TVertex : notnull
    {
        if (source == null) throw new ArgumentNullException(nameof(source));

        var builder = DirectedGraph
            .Builder<TVertex>()
            .WithVertexComparer(source.VertexComparer)
            .WithFormatter(source.Formatter);

        foreach (var vertex in source.GetAllVerticesAndEdges())
        {
            vertex.Switch(
                n => builder.AddVertex(n),
                t => builder.AddEdge(t.Source, t.Target));
        }

        return builder.Build();
    }

    public static ISeededDirectedGraphSource<TVertex, TEdgeLabel> WithVertexComparer<TVertex, TEdgeLabel>(
        this ISeededDirectedGraphSource<TVertex, TEdgeLabel> source,
        IEqualityComparer<TVertex> vertexComparer)
        where TVertex : notnull
        where TEdgeLabel : notnull
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (vertexComparer == null) throw new ArgumentNullException(nameof(vertexComparer));

        return SeededDirectedGraphSource.Create(
            source.SeedVertices,
            source.GetEdges,
            vertexComparer,
            source.Formatter);
    }

    public static ISeededDirectedGraphSource<TVertex, TEdgeLabel> WithFormatter<TVertex, TEdgeLabel>(
        this ISeededDirectedGraphSource<TVertex, TEdgeLabel> source,
        IGraphFormatter<TVertex, TEdgeLabel> formatter)
        where TVertex : notnull
        where TEdgeLabel : notnull
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (formatter == null) throw new ArgumentNullException(nameof(formatter));

        return SeededDirectedGraphSource.Create(
            source.SeedVertices,
            source.GetEdges,
            source.VertexComparer,
            formatter);
    }

    public static ISeededDirectedGraphSource<TVertex, TEdgeLabel> WithFormatter<TVertex, TEdgeLabel>(
        this ISeededDirectedGraphSource<TVertex, TEdgeLabel> source,
        Func<IGraphFormatter<TVertex, TEdgeLabel>, IGraphFormatter<TVertex, TEdgeLabel>> updateFormatter)
        where TVertex : notnull
        where TEdgeLabel : notnull
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (updateFormatter == null) throw new ArgumentNullException(nameof(updateFormatter));

        return SeededDirectedGraphSource.Create(
            source.SeedVertices,
            source.GetEdges,
            source.VertexComparer,
            updateFormatter(source.Formatter));
    }

    public static ISeededDirectedGraphSource<TVertex> WithVertexComparer<TVertex>(
        this ISeededDirectedGraphSource<TVertex> source,
        IEqualityComparer<TVertex> vertexComparer)
        where TVertex : notnull
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (vertexComparer == null) throw new ArgumentNullException(nameof(vertexComparer));

        return SeededDirectedGraphSource.Create(
            source.SeedVertices,
            source.GetEdges,
            vertexComparer,
            source.Formatter);
    }

    public static ISeededDirectedGraphSource<TVertex> WithFormatter<TVertex>(
        this ISeededDirectedGraphSource<TVertex> source,
        IGraphFormatter<TVertex> formatter)
        where TVertex : notnull
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (formatter == null) throw new ArgumentNullException(nameof(formatter));

        return SeededDirectedGraphSource.Create(
            source.SeedVertices,
            source.GetEdges,
            source.VertexComparer,
            formatter);
    }

    public static ISeededDirectedGraphSource<TVertex, TEdgeLabel> ToSeededDirectedGraph<TVertex, TEdgeLabel>(
        this IDictionary<TVertex, ICollection<(TEdgeLabel Edge, TVertex Vertex)>> adjacencyGraph,
        IEqualityComparer<TVertex>? vertexComparer = null,
        IGraphFormatter<TVertex, TEdgeLabel>? formatter = null)
        where TVertex : notnull
        where TEdgeLabel : notnull
    {
        if (adjacencyGraph == null) throw new ArgumentNullException(nameof(adjacencyGraph));

        return SeededDirectedGraphSource.Create(
            adjacencyGraph.Keys,
            vertex => adjacencyGraph.TryGetValue(vertex, out var edges) ? edges : Enumerable.Empty<(TEdgeLabel, TVertex)>(),
            vertexComparer,
            formatter);
    }

    public static ISeededDirectedGraphSource<TVertex, TEdgeLabel> ToSeededDirectedGraph<TVertex, TEdgeLabel>(
        this IEnumerable<(TVertex Source, IEnumerable<(TEdgeLabel Edge, TVertex Target)> Edges)> adjacencyGraph,
        IEqualityComparer<TVertex>? vertexComparer = null,
        IGraphFormatter<TVertex, TEdgeLabel>? formatter = null)
        where TVertex : notnull
        where TEdgeLabel : notnull
    {
        if (adjacencyGraph == null) throw new ArgumentNullException(nameof(adjacencyGraph));

        return adjacencyGraph
            .ToDictionary(
                t => t.Source,
                t => t.Edges.Select(e => (e.Edge, e.Target)).ToArray() as ICollection<(TEdgeLabel, TVertex)>,
                vertexComparer)
            .ToSeededDirectedGraph(vertexComparer, formatter);
    }

    public static ISeededDirectedGraphSource<TVertex> ToSeededDirectedGraph<TVertex>(
        this IDictionary<TVertex, ICollection<TVertex>> adjacencyGraph,
        IEqualityComparer<TVertex>? vertexComparer = null,
        IGraphFormatter<TVertex>? formatter = null)
        where TVertex : notnull
    {
        if (adjacencyGraph == null) throw new ArgumentNullException(nameof(adjacencyGraph));

        return SeededDirectedGraphSource.Create(
            adjacencyGraph.Keys,
            vertex => adjacencyGraph.TryGetValue(vertex, out var edges) ? edges : Enumerable.Empty<TVertex>(),
            vertexComparer,
            formatter);
    }

    public static ISeededDirectedGraphSource<TVertex> ToSeededDirectedGraph<TVertex>(
        this IEnumerable<(TVertex Vertex, IEnumerable<TVertex> Edges)> adjacencyGraph,
        IEqualityComparer<TVertex>? vertexComparer = null,
        IGraphFormatter<TVertex>? formatter = null)
        where TVertex : notnull
    {
        if (adjacencyGraph == null) throw new ArgumentNullException(nameof(adjacencyGraph));

        return adjacencyGraph
            .ToDictionary(x => x.Vertex, x => x.Edges.ToArray() as ICollection<TVertex>, vertexComparer)
            .ToSeededDirectedGraph(vertexComparer, formatter);
    }
}
