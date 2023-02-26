using System.Reflection;

namespace GraphFx;

public static class SeededDirectedGraphSourceExtensions
{
    public static IEnumerable<NodeOrLabeledEdge<TNode, TEdge>> GetAllNodesAndEdges<TNode, TEdge>(
        this ISeededDirectedGraphSource<TNode, TEdge> source)
        where TNode : notnull
        where TEdge : notnull
    {
        var nodes = new HashSet<TNode>(source.NodeComparer);
        var queue = new Queue<TNode>(source.SeedNodes);

        while (queue.Count > 0)
        {
            var node = queue.Dequeue();
            yield return node;
            foreach (var (edge, targetNode) in source.GetEdges(node))
            {
                if (nodes.Add(targetNode))
                {
                    queue.Enqueue(targetNode);
                }

                yield return EdgeDefinition.Create(node, edge, targetNode);
            }
        }
    }

    public static IEnumerable<TNode> GetAllNodes<TNode, TEdge>(
        this ISeededDirectedGraphSource<TNode, TEdge> source)
        where TNode : notnull
        where TEdge : notnull
    {
        var nodes = new HashSet<TNode>(source.NodeComparer);
        var queue = new Queue<TNode>(source.SeedNodes);

        while (queue.Count > 0)
        {
            var node = queue.Dequeue();
            yield return node;
            foreach (var (_, targetNode) in source.GetEdges(node))
            {
                if (nodes.Add(targetNode))
                {
                    queue.Enqueue(targetNode);
                }
            }
        }
    }

    public static IEnumerable<LabeledEdgeDefinition<TNode, TEdge>> GetAllEdges<TNode, TEdge>(
        this ISeededDirectedGraphSource<TNode, TEdge> source)
        where TNode : notnull
        where TEdge : notnull
    {
        var nodes = new HashSet<TNode>(source.NodeComparer);
        var queue = new Queue<TNode>(source.SeedNodes);

        while (queue.Count > 0)
        {
            var node = queue.Dequeue();
            foreach (var (edge, targetNode) in source.GetEdges(node))
            {
                if (nodes.Add(targetNode))
                {
                    queue.Enqueue(targetNode);
                }

                yield return EdgeDefinition.Create(node, edge, targetNode);
            }
        }
    }

    public static IDirectedGraph<TNode, TEdge> ToDirectedGraph<TNode, TEdge>(
        this ISeededDirectedGraphSource<TNode, TEdge> source)
        where TNode : notnull
        where TEdge : notnull
    {
        if (source == null) throw new ArgumentNullException(nameof(source));

        var builder = DirectedGraph
            .Builder<TNode, TEdge>()
            .WithNodeComparer(source.NodeComparer);

        foreach (var node in source.GetAllNodesAndEdges())
        {
            node.Switch(
                n => builder.AddNode(n),
                t => builder.AddEdge(t.Source, t.Edge, t.Target));
        }

        return builder.Build();
    }

    public static IEnumerable<NodeOrEdge<TNode>> GetAllNodesAndEdges<TNode>(
        this ISeededDirectedGraphSource<TNode> source)
        where TNode : notnull
    {
        var nodes = new HashSet<TNode>(source.NodeComparer);
        var queue = new Queue<TNode>(source.SeedNodes);

        while (queue.Count > 0)
        {
            var node = queue.Dequeue();
            yield return node;
            foreach (var targetNode in source.GetEdges(node))
            {
                if (nodes.Add(targetNode))
                {
                    queue.Enqueue(targetNode);
                }

                yield return EdgeDefinition.Create(node, targetNode);
            }
        }
    }

    public static IEnumerable<TNode> GetAllNodes<TNode, TEdge>(
        this ISeededDirectedGraphSource<TNode> source)
        where TNode : notnull
    {
        var nodes = new HashSet<TNode>(source.NodeComparer);
        var queue = new Queue<TNode>(source.SeedNodes);

        while (queue.Count > 0)
        {
            var node = queue.Dequeue();
            yield return node;
            foreach (var targetNode in source.GetEdges(node))
            {
                if (nodes.Add(targetNode))
                {
                    queue.Enqueue(targetNode);
                }
            }
        }
    }

    public static IEnumerable<EdgeDefinition<TNode>> GetAllEdges<TNode, TEdge>(
        this ISeededDirectedGraphSource<TNode> source)
        where TNode : notnull
    {
        var nodes = new HashSet<TNode>(source.NodeComparer);
        var queue = new Queue<TNode>(source.SeedNodes);

        while (queue.Count > 0)
        {
            var node = queue.Dequeue();
            foreach (var targetNode in source.GetEdges(node))
            {
                if (nodes.Add(targetNode))
                {
                    queue.Enqueue(targetNode);
                }

                yield return EdgeDefinition.Create(node, targetNode);
            }
        }
    }

    public static IDirectedGraph<TNode> ToDirectedGraph<TNode>(
        this ISeededDirectedGraphSource<TNode> source)
        where TNode : notnull
    {
        if (source == null) throw new ArgumentNullException(nameof(source));

        var builder = DirectedGraph
            .Builder<TNode>()
            .WithNodeComparer(source.NodeComparer)
            .WithFormatter(source.Formatter);

        foreach (var node in source.GetAllNodesAndEdges())
        {
            node.Switch(
                n => builder.AddNode(n),
                t => builder.AddEdge(t.Source, t.Target));
        }

        return builder.Build();
    }

    public static ISeededDirectedGraphSource<TNode, TEdge> WithNodeComparer<TNode, TEdge>(
        this ISeededDirectedGraphSource<TNode, TEdge> source,
        IEqualityComparer<TNode> nodeComparer)
        where TNode : notnull
        where TEdge : notnull
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (nodeComparer == null) throw new ArgumentNullException(nameof(nodeComparer));

        return SeededDirectedGraphSource.Create(
            source.SeedNodes,
            source.GetEdges,
            nodeComparer,
            source.Formatter);
    }

    public static ISeededDirectedGraphSource<TNode, TEdge> WithFormatter<TNode, TEdge>(
        this ISeededDirectedGraphSource<TNode, TEdge> source,
        IGraphFormatter<TNode, TEdge> formatter)
        where TNode : notnull
        where TEdge : notnull
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (formatter == null) throw new ArgumentNullException(nameof(formatter));

        return SeededDirectedGraphSource.Create(
            source.SeedNodes,
            source.GetEdges,
            source.NodeComparer,
            formatter);
    }

    public static ISeededDirectedGraphSource<TNode, TEdge> WithFormatter<TNode, TEdge>(
        this ISeededDirectedGraphSource<TNode, TEdge> source,
        Func<IGraphFormatter<TNode, TEdge>, IGraphFormatter<TNode, TEdge>> updateFormatter)
        where TNode : notnull
        where TEdge : notnull
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (updateFormatter == null) throw new ArgumentNullException(nameof(updateFormatter));

        return SeededDirectedGraphSource.Create(
            source.SeedNodes,
            source.GetEdges,
            source.NodeComparer,
            updateFormatter(source.Formatter));
    }

    public static ISeededDirectedGraphSource<TNode> WithNodeComparer<TNode>(
        this ISeededDirectedGraphSource<TNode> source,
        IEqualityComparer<TNode> nodeComparer)
        where TNode : notnull
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (nodeComparer == null) throw new ArgumentNullException(nameof(nodeComparer));

        return SeededDirectedGraphSource.Create(
            source.SeedNodes,
            source.GetEdges,
            nodeComparer,
            source.Formatter);
    }

    public static ISeededDirectedGraphSource<TNode> WithFormatter<TNode>(
        this ISeededDirectedGraphSource<TNode> source,
        IGraphFormatter<TNode> formatter)
        where TNode : notnull
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (formatter == null) throw new ArgumentNullException(nameof(formatter));

        return SeededDirectedGraphSource.Create(
            source.SeedNodes,
            source.GetEdges,
            source.NodeComparer,
            formatter);
    }

    public static ISeededDirectedGraphSource<TNode, TEdge> ToSeededDirectedGraph<TNode, TEdge>(
        this IDictionary<TNode, ICollection<(TEdge Edge, TNode Node)>> adjacencyGraph,
        IEqualityComparer<TNode>? nodeComparer = null,
        IGraphFormatter<TNode, TEdge>? formatter = null)
        where TNode : notnull
        where TEdge : notnull
    {
        if (adjacencyGraph == null) throw new ArgumentNullException(nameof(adjacencyGraph));

        return SeededDirectedGraphSource.Create(
            adjacencyGraph.Keys,
            node => adjacencyGraph.TryGetValue(node, out var edges) ? edges : Enumerable.Empty<(TEdge, TNode)>(),
            nodeComparer,
            formatter);
    }

    public static ISeededDirectedGraphSource<TNode, TEdge> ToSeededDirectedGraph<TNode, TEdge>(
        this IEnumerable<(TNode Source, IEnumerable<(TEdge Edge, TNode Target)> Edges)> adjacencyGraph,
        IEqualityComparer<TNode>? nodeComparer = null,
        IGraphFormatter<TNode, TEdge>? formatter = null)
        where TNode : notnull
        where TEdge : notnull
    {
        if (adjacencyGraph == null) throw new ArgumentNullException(nameof(adjacencyGraph));

        return adjacencyGraph
            .ToDictionary(
                t => t.Source,
                t => t.Edges.Select(e => (e.Edge, e.Target)).ToArray() as ICollection<(TEdge, TNode)>,
                nodeComparer)
            .ToSeededDirectedGraph(nodeComparer, formatter);
    }

    public static ISeededDirectedGraphSource<TNode> ToSeededDirectedGraph<TNode>(
        this IDictionary<TNode, ICollection<TNode>> adjacencyGraph,
        IEqualityComparer<TNode>? nodeComparer = null,
        IGraphFormatter<TNode>? formatter = null)
        where TNode : notnull
    {
        if (adjacencyGraph == null) throw new ArgumentNullException(nameof(adjacencyGraph));

        return SeededDirectedGraphSource.Create(
            adjacencyGraph.Keys,
            node => adjacencyGraph.TryGetValue(node, out var edges) ? edges : Enumerable.Empty<TNode>(),
            nodeComparer,
            formatter);
    }

    public static ISeededDirectedGraphSource<TNode> ToSeededDirectedGraph<TNode>(
        this IEnumerable<(TNode Node, IEnumerable<TNode> Edges)> adjacencyGraph,
        IEqualityComparer<TNode>? nodeComparer = null,
        IGraphFormatter<TNode>? formatter = null)
        where TNode : notnull
    {
        if (adjacencyGraph == null) throw new ArgumentNullException(nameof(adjacencyGraph));

        return adjacencyGraph
            .ToDictionary(x => x.Node, x => x.Edges.ToArray() as ICollection<TNode>, nodeComparer)
            .ToSeededDirectedGraph(nodeComparer, formatter);
    }
}
