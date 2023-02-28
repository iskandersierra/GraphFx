namespace GraphFx;

public static class GraphFormatter
{
    public static IGraphFormatter<TVertex, TEdgeLabel> Create<TVertex, TEdgeLabel>(
        IStringFormatter<TVertex> vertexFormatter,
        IStringFormatter<TEdgeLabel> edgeFormatter)
        where TVertex : notnull
        where TEdgeLabel : notnull
    {
        return new CustomGraphFormatter<TVertex, TEdgeLabel>(vertexFormatter, edgeFormatter);
    }

    public static IGraphFormatter<TVertex, TEdgeLabel> Create<TVertex, TEdgeLabel>(
        Func<TVertex, string> vertexFormatter,
        Func<TEdgeLabel, string> edgeFormatter)
        where TVertex : notnull
        where TEdgeLabel : notnull
    {
        return new CustomGraphFormatter<TVertex, TEdgeLabel>(
            StringFormatter.Create(vertexFormatter),
            StringFormatter.Create(edgeFormatter));
    }

    private class CustomGraphFormatter<TVertex, TEdgeLabel> : IGraphFormatter<TVertex, TEdgeLabel>
        where TVertex : notnull
        where TEdgeLabel : notnull
    {
        public CustomGraphFormatter(
            IStringFormatter<TVertex> vertexFormatter,
            IStringFormatter<TEdgeLabel> edgeFormatter)
        {
            VertexFormatter = vertexFormatter ?? throw new ArgumentNullException(nameof(vertexFormatter));
            EdgeFormatter = edgeFormatter ?? throw new ArgumentNullException(nameof(edgeFormatter));
        }

        public IStringFormatter<TVertex> VertexFormatter { get; }

        public IStringFormatter<TEdgeLabel> EdgeFormatter { get; }
    }

    public static IGraphFormatter<TVertex> Create<TVertex>(
        IStringFormatter<TVertex> vertexFormatter)
        where TVertex : notnull
    {
        return new CustomGraphFormatter<TVertex>(vertexFormatter);
    }

    public static IGraphFormatter<TVertex> Create<TVertex>(
        Func<TVertex, string> vertexFormatter)
        where TVertex : notnull
    {
        return new CustomGraphFormatter<TVertex>(StringFormatter.Create(vertexFormatter));
    }

    private class CustomGraphFormatter<TVertex> : IGraphFormatter<TVertex>
        where TVertex : notnull
    {
        public CustomGraphFormatter(
            IStringFormatter<TVertex> vertexFormatter)
        {
            VertexFormatter = vertexFormatter ?? throw new ArgumentNullException(nameof(vertexFormatter));
        }

        public IStringFormatter<TVertex> VertexFormatter { get; }
    }

    public static IGraphFormatter<TVertex, TEdgeLabel> WithVertexFormatter<TVertex, TEdgeLabel>(
        this IGraphFormatter<TVertex, TEdgeLabel> formatter,
        IStringFormatter<TVertex> vertexFormatter)
        where TVertex : notnull
        where TEdgeLabel : notnull
    {
        return new CustomGraphFormatter<TVertex, TEdgeLabel>(vertexFormatter, formatter.EdgeFormatter);
    }

    public static IGraphFormatter<TVertex, TEdgeLabel> WithVertexFormatter<TVertex, TEdgeLabel>(
        this IGraphFormatter<TVertex, TEdgeLabel> formatter,
        Func<TVertex, string> vertexFormatter)
        where TVertex : notnull
        where TEdgeLabel : notnull
    {
        return new CustomGraphFormatter<TVertex, TEdgeLabel>(StringFormatter.Create(vertexFormatter), formatter.EdgeFormatter);
    }

    public static IGraphFormatter<TVertex, TEdgeLabel> WithEdgeFormatter<TVertex, TEdgeLabel>(
        this IGraphFormatter<TVertex, TEdgeLabel> formatter,
        IStringFormatter<TEdgeLabel> edgeFormatter)
        where TVertex : notnull
        where TEdgeLabel : notnull
    {
        return new CustomGraphFormatter<TVertex, TEdgeLabel>(formatter.VertexFormatter, edgeFormatter);
    }

    public static IGraphFormatter<TVertex, TEdgeLabel> WithEdgeFormatter<TVertex, TEdgeLabel>(
        this IGraphFormatter<TVertex, TEdgeLabel> formatter,
        Func<TEdgeLabel, string> edgeFormatter)
        where TVertex : notnull
        where TEdgeLabel : notnull
    {
        return new CustomGraphFormatter<TVertex, TEdgeLabel>(formatter.VertexFormatter, StringFormatter.Create(edgeFormatter));
    }
}

public static class GraphFormatter<TVertex, TEdgeLabel>
    where TVertex : notnull
    where TEdgeLabel : notnull
{
    public static readonly IGraphFormatter<TVertex, TEdgeLabel> Default = new DefaultGraphFormatter<TVertex, TEdgeLabel>();

    private sealed class DefaultGraphFormatter<TVertexValue, TEdgeValue> : IGraphFormatter<TVertexValue, TEdgeValue>
        where TVertexValue : notnull
        where TEdgeValue : notnull
    {
        public IStringFormatter<TVertexValue> VertexFormatter => StringFormatter<TVertexValue>.Default;

        public IStringFormatter<TEdgeValue> EdgeFormatter => StringFormatter<TEdgeValue>.Default;
    }
}

public static class GraphFormatter<TVertex>
    where TVertex : notnull
{
    public static readonly IGraphFormatter<TVertex> Default = new DefaultGraphFormatter<TVertex>();

    private sealed class DefaultGraphFormatter<TVertexValue> : IGraphFormatter<TVertexValue>
        where TVertexValue : notnull
    {
        public IStringFormatter<TVertexValue> VertexFormatter => StringFormatter<TVertexValue>.Default;
    }
}
