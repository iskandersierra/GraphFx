namespace GraphFx;

public static class SeededDirectedGraphSource
{
    public static ISeededDirectedGraphSource<TNode, TEdge> Create<TNode, TEdge>(
        IEnumerable<TNode> seedNodes,
        Func<TNode, IEnumerable<(TEdge, TNode)>> getEdges,
        IEqualityComparer<TNode>? nodeComparer = null,
        IGraphFormatter<TNode, TEdge>? formatter = null)
        where TNode : notnull
        where TEdge : notnull
    {
        if (seedNodes == null) throw new ArgumentNullException(nameof(seedNodes));
        if (getEdges == null) throw new ArgumentNullException(nameof(getEdges));
        nodeComparer ??= EqualityComparer<TNode>.Default;
        formatter ??= GraphFormatter<TNode, TEdge>.Default;

        return new CustomSeededDirectedGraphSource<TNode, TEdge>(seedNodes, getEdges, nodeComparer, formatter);
    }

    public static ISeededDirectedGraphSource<TNode, TEdge> Create<TNode, TEdge>(
        TNode seedNode,
        Func<TNode, IEnumerable<(TEdge, TNode)>> getEdges,
        IEqualityComparer<TNode>? nodeComparer = null,
        IGraphFormatter<TNode, TEdge>? formatter = null)
        where TNode : notnull
        where TEdge : notnull
    {
        return Create(new[] { seedNode }, getEdges, nodeComparer, formatter);
    }

    public static ISeededDirectedGraphSource<TNode> Create<TNode>(
        IEnumerable<TNode> seedNodes,
        Func<TNode, IEnumerable<TNode>> getEdges,
        IEqualityComparer<TNode>? nodeComparer = null,
        IGraphFormatter<TNode>? formatter = null)
        where TNode : notnull
    {
        if (seedNodes == null) throw new ArgumentNullException(nameof(seedNodes));
        if (getEdges == null) throw new ArgumentNullException(nameof(getEdges));
        nodeComparer ??= EqualityComparer<TNode>.Default;
        formatter ??= GraphFormatter<TNode>.Default;

        return new CustomSeededDirectedGraphSource<TNode>(seedNodes, getEdges, nodeComparer, formatter);
    }

    public static ISeededDirectedGraphSource<TNode> Create<TNode>(
        TNode seedNode,
        Func<TNode, IEnumerable<TNode>> getEdges,
        IEqualityComparer<TNode>? nodeComparer = null,
        IGraphFormatter<TNode>? formatter = null)
        where TNode : notnull
    {
        return Create(new[] { seedNode }, getEdges, nodeComparer, formatter);
    }

    public static ISeededDirectedGraphSource<TNode> Create<TNode>(
        IEnumerable<TNode> seedNodes,
        Func<TNode, IEnumerable<TNode>> getEdges,
        Func<TNode, string> formatter,
        IEqualityComparer<TNode>? nodeComparer = null)
        where TNode : notnull
    {
        return Create(seedNodes, getEdges, nodeComparer, GraphFormatter.Create(formatter));
    }

    public static ISeededDirectedGraphSource<TNode> Create<TNode>(
        TNode seedNode,
        Func<TNode, IEnumerable<TNode>> getEdges,
        Func<TNode, string> formatter,
        IEqualityComparer<TNode>? nodeComparer = null)
        where TNode : notnull
    {
        return Create(seedNode, getEdges, nodeComparer, GraphFormatter.Create(formatter));
    }

    private class CustomSeededDirectedGraphSource<TNode, TEdge> :
        ISeededDirectedGraphSource<TNode, TEdge>
        where TNode : notnull
        where TEdge : notnull
    {
        private readonly Func<TNode, IEnumerable<(TEdge, TNode)>> getEdges;

        public CustomSeededDirectedGraphSource(
            IEnumerable<TNode> nodes,
            Func<TNode, IEnumerable<(TEdge, TNode)>> getEdges,
            IEqualityComparer<TNode> nodeComparer,
            IGraphFormatter<TNode, TEdge> formatter)
        {
            SeedNodes = nodes;
            this.getEdges = getEdges;
            NodeComparer = nodeComparer;
            Formatter = formatter;
        }

        public IEnumerable<TNode> SeedNodes { get; }

        public IEqualityComparer<TNode> NodeComparer { get; }

        public IGraphFormatter<TNode, TEdge> Formatter { get; }

        public IEnumerable<(TEdge, TNode)> GetEdges(TNode node)
        {
            return getEdges(node);
        }
    }

    private class CustomSeededDirectedGraphSource<TNode> :
        ISeededDirectedGraphSource<TNode>
        where TNode : notnull
    {
        private readonly Func<TNode, IEnumerable<TNode>> getEdges;

        public CustomSeededDirectedGraphSource(
            IEnumerable<TNode> nodes,
            Func<TNode, IEnumerable<TNode>> getEdges,
            IEqualityComparer<TNode> nodeComparer,
            IGraphFormatter<TNode> formatter)
        {
            SeedNodes = nodes;
            this.getEdges = getEdges;
            NodeComparer = nodeComparer;
            Formatter = formatter;
        }

        public IEnumerable<TNode> SeedNodes { get; }

        public IEqualityComparer<TNode> NodeComparer { get; }

        public IGraphFormatter<TNode> Formatter { get; }

        public IEnumerable<TNode> GetEdges(TNode node)
        {
            return getEdges(node);
        }
    }
}