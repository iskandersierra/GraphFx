namespace GraphFx;

public static class GraphFormatter
{
    public static IGraphFormatter<TNode, TEdge> Create<TNode, TEdge>(
        IStringFormatter<TNode> nodeFormatter,
        IStringFormatter<TEdge> edgeFormatter)
        where TNode : notnull
        where TEdge : notnull
    {
        return new CustomGraphFormatter<TNode, TEdge>(nodeFormatter, edgeFormatter);
    }

    public static IGraphFormatter<TNode, TEdge> Create<TNode, TEdge>(
        Func<TNode, string> nodeFormatter,
        Func<TEdge, string> edgeFormatter)
        where TNode : notnull
        where TEdge : notnull
    {
        return new CustomGraphFormatter<TNode, TEdge>(
            StringFormatter.Create(nodeFormatter),
            StringFormatter.Create(edgeFormatter));
    }

    private class CustomGraphFormatter<TNode, TEdge> : IGraphFormatter<TNode, TEdge>
        where TNode : notnull
        where TEdge : notnull
    {
        public CustomGraphFormatter(
            IStringFormatter<TNode> nodeFormatter,
            IStringFormatter<TEdge> edgeFormatter)
        {
            NodeFormatter = nodeFormatter ?? throw new ArgumentNullException(nameof(nodeFormatter));
            EdgeFormatter = edgeFormatter ?? throw new ArgumentNullException(nameof(edgeFormatter));
        }

        public IStringFormatter<TNode> NodeFormatter { get; }

        public IStringFormatter<TEdge> EdgeFormatter { get; }
    }

    public static IGraphFormatter<TNode> Create<TNode>(
        IStringFormatter<TNode> nodeFormatter)
        where TNode : notnull
    {
        return new CustomGraphFormatter<TNode>(nodeFormatter);
    }

    public static IGraphFormatter<TNode> Create<TNode>(
        Func<TNode, string> nodeFormatter)
        where TNode : notnull
    {
        return new CustomGraphFormatter<TNode>(StringFormatter.Create(nodeFormatter));
    }

    private class CustomGraphFormatter<TNode> : IGraphFormatter<TNode>
        where TNode : notnull
    {
        public CustomGraphFormatter(
            IStringFormatter<TNode> nodeFormatter)
        {
            NodeFormatter = nodeFormatter ?? throw new ArgumentNullException(nameof(nodeFormatter));
        }

        public IStringFormatter<TNode> NodeFormatter { get; }
    }

    public static IGraphFormatter<TNode, TEdge> WithNodeFormatter<TNode, TEdge>(
        this IGraphFormatter<TNode, TEdge> formatter,
        IStringFormatter<TNode> nodeFormatter)
        where TNode : notnull
        where TEdge : notnull
    {
        return new CustomGraphFormatter<TNode, TEdge>(nodeFormatter, formatter.EdgeFormatter);
    }

    public static IGraphFormatter<TNode, TEdge> WithNodeFormatter<TNode, TEdge>(
        this IGraphFormatter<TNode, TEdge> formatter,
        Func<TNode, string> nodeFormatter)
        where TNode : notnull
        where TEdge : notnull
    {
        return new CustomGraphFormatter<TNode, TEdge>(StringFormatter.Create(nodeFormatter), formatter.EdgeFormatter);
    }

    public static IGraphFormatter<TNode, TEdge> WithEdgeFormatter<TNode, TEdge>(
        this IGraphFormatter<TNode, TEdge> formatter,
        IStringFormatter<TEdge> edgeFormatter)
        where TNode : notnull
        where TEdge : notnull
    {
        return new CustomGraphFormatter<TNode, TEdge>(formatter.NodeFormatter, edgeFormatter);
    }

    public static IGraphFormatter<TNode, TEdge> WithEdgeFormatter<TNode, TEdge>(
        this IGraphFormatter<TNode, TEdge> formatter,
        Func<TEdge, string> edgeFormatter)
        where TNode : notnull
        where TEdge : notnull
    {
        return new CustomGraphFormatter<TNode, TEdge>(formatter.NodeFormatter, StringFormatter.Create(edgeFormatter));
    }
}

public static class GraphFormatter<TNode, TEdge>
    where TNode : notnull
    where TEdge : notnull
{
    public static readonly IGraphFormatter<TNode, TEdge> Default = new DefaultGraphFormatter<TNode, TEdge>();

    private sealed class DefaultGraphFormatter<TNodeValue, TEdgeValue> : IGraphFormatter<TNodeValue, TEdgeValue>
        where TNodeValue : notnull
        where TEdgeValue : notnull
    {
        public IStringFormatter<TNodeValue> NodeFormatter => StringFormatter<TNodeValue>.Default;

        public IStringFormatter<TEdgeValue> EdgeFormatter => StringFormatter<TEdgeValue>.Default;
    }
}

public static class GraphFormatter<TNode>
    where TNode : notnull
{
    public static readonly IGraphFormatter<TNode> Default = new DefaultGraphFormatter<TNode>();

    private sealed class DefaultGraphFormatter<TNodeValue> : IGraphFormatter<TNodeValue>
        where TNodeValue : notnull
    {
        public IStringFormatter<TNodeValue> NodeFormatter => StringFormatter<TNodeValue>.Default;
    }
}
