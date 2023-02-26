namespace GraphFx;

public static class SeededDirectedGraphSourceExtensions
{
    public static IEnumerable<NodeOrEdge<TNode, TEdge>> GetAllNodesAndEdges<TNode, TEdge>(
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

    public static IEnumerable<EdgeDefinition<TNode, TEdge>> GetAllEdges<TNode, TEdge>(
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

    public static IEnumerable<NodeOrEdgeSimple<TNode>> GetAllNodesAndEdges<TNode>(
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

    public static IDirectedGraph<TNode> ToDirectedGraph<TNode, TEdge>(
        this ISeededDirectedGraphSource<TNode> source)
        where TNode : notnull
    {
        if (source == null) throw new ArgumentNullException(nameof(source));

        var builder = DirectedGraph
            .Builder<TNode>()
            .WithNodeComparer(source.NodeComparer);

        foreach (var node in source.GetAllNodesAndEdges())
        {
            node.Switch(
                               n => builder.AddNode(n),
                                              t => builder.AddEdge(t.Source, t.Target));
        }

        return builder.Build();
    }
}
