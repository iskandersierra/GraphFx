namespace GraphFx;

public static class SeededDirectedGraphSource
{
    public static ISeededDirectedGraphSource<TVertex, TEdgeLabel> Create<TVertex, TEdgeLabel>(
        IEnumerable<TVertex> seedVertices,
        Func<TVertex, IEnumerable<(TEdgeLabel, TVertex)>> getEdges,
        IEqualityComparer<TVertex>? vertexComparer = null,
        IGraphFormatter<TVertex, TEdgeLabel>? formatter = null)
        where TVertex : notnull
        where TEdgeLabel : notnull
    {
        if (seedVertices == null) throw new ArgumentNullException(nameof(seedVertices));
        if (getEdges == null) throw new ArgumentNullException(nameof(getEdges));
        vertexComparer ??= EqualityComparer<TVertex>.Default;
        formatter ??= GraphFormatter<TVertex, TEdgeLabel>.Default;

        return new CustomSeededDirectedGraphSource<TVertex, TEdgeLabel>(seedVertices, getEdges, vertexComparer, formatter);
    }

    public static ISeededDirectedGraphSource<TVertex, TEdgeLabel> Create<TVertex, TEdgeLabel>(
        TVertex seedVertex,
        Func<TVertex, IEnumerable<(TEdgeLabel, TVertex)>> getEdges,
        IEqualityComparer<TVertex>? vertexComparer = null,
        IGraphFormatter<TVertex, TEdgeLabel>? formatter = null)
        where TVertex : notnull
        where TEdgeLabel : notnull
    {
        return Create(new[] { seedVertex }, getEdges, vertexComparer, formatter);
    }

    public static ISeededDirectedGraphSource<TVertex> Create<TVertex>(
        IEnumerable<TVertex> seedVertices,
        Func<TVertex, IEnumerable<TVertex>> getEdges,
        IEqualityComparer<TVertex>? vertexComparer = null,
        IGraphFormatter<TVertex>? formatter = null)
        where TVertex : notnull
    {
        if (seedVertices == null) throw new ArgumentNullException(nameof(seedVertices));
        if (getEdges == null) throw new ArgumentNullException(nameof(getEdges));
        vertexComparer ??= EqualityComparer<TVertex>.Default;
        formatter ??= GraphFormatter<TVertex>.Default;

        return new CustomSeededDirectedGraphSource<TVertex>(seedVertices, getEdges, vertexComparer, formatter);
    }

    public static ISeededDirectedGraphSource<TVertex> Create<TVertex>(
        TVertex seedVertex,
        Func<TVertex, IEnumerable<TVertex>> getEdges,
        IEqualityComparer<TVertex>? vertexComparer = null,
        IGraphFormatter<TVertex>? formatter = null)
        where TVertex : notnull
    {
        return Create(new[] { seedVertex }, getEdges, vertexComparer, formatter);
    }

    public static ISeededDirectedGraphSource<TVertex> Create<TVertex>(
        IEnumerable<TVertex> seedVertices,
        Func<TVertex, IEnumerable<TVertex>> getEdges,
        Func<TVertex, string> formatter,
        IEqualityComparer<TVertex>? vertexComparer = null)
        where TVertex : notnull
    {
        return Create(seedVertices, getEdges, vertexComparer, GraphFormatter.Create(formatter));
    }

    public static ISeededDirectedGraphSource<TVertex> Create<TVertex>(
        TVertex seedVertex,
        Func<TVertex, IEnumerable<TVertex>> getEdges,
        Func<TVertex, string> formatter,
        IEqualityComparer<TVertex>? vertexComparer = null)
        where TVertex : notnull
    {
        return Create(seedVertex, getEdges, vertexComparer, GraphFormatter.Create(formatter));
    }

    private class CustomSeededDirectedGraphSource<TVertex, TEdgeLabel> :
        ISeededDirectedGraphSource<TVertex, TEdgeLabel>
        where TVertex : notnull
        where TEdgeLabel : notnull
    {
        private readonly Func<TVertex, IEnumerable<(TEdgeLabel, TVertex)>> getEdges;

        public CustomSeededDirectedGraphSource(
            IEnumerable<TVertex> vertices,
            Func<TVertex, IEnumerable<(TEdgeLabel, TVertex)>> getEdges,
            IEqualityComparer<TVertex> vertexComparer,
            IGraphFormatter<TVertex, TEdgeLabel> formatter)
        {
            SeedVertices = vertices;
            this.getEdges = getEdges;
            VertexComparer = vertexComparer;
            Formatter = formatter;
        }

        public IEnumerable<TVertex> SeedVertices { get; }

        public IEqualityComparer<TVertex> VertexComparer { get; }

        public IGraphFormatter<TVertex, TEdgeLabel> Formatter { get; }

        public IEnumerable<(TEdgeLabel, TVertex)> GetEdges(TVertex vertex)
        {
            return getEdges(vertex);
        }
    }

    private class CustomSeededDirectedGraphSource<TVertex> :
        ISeededDirectedGraphSource<TVertex>
        where TVertex : notnull
    {
        private readonly Func<TVertex, IEnumerable<TVertex>> getEdges;

        public CustomSeededDirectedGraphSource(
            IEnumerable<TVertex> vertices,
            Func<TVertex, IEnumerable<TVertex>> getEdges,
            IEqualityComparer<TVertex> vertexComparer,
            IGraphFormatter<TVertex> formatter)
        {
            SeedVertices = vertices;
            this.getEdges = getEdges;
            VertexComparer = vertexComparer;
            Formatter = formatter;
        }

        public IEnumerable<TVertex> SeedVertices { get; }

        public IEqualityComparer<TVertex> VertexComparer { get; }

        public IGraphFormatter<TVertex> Formatter { get; }

        public IEnumerable<TVertex> GetEdges(TVertex vertex)
        {
            return getEdges(vertex);
        }
    }
}