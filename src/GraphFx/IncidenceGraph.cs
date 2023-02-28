namespace GraphFx;

public static class IncidenceGraph
{
    public static IncidenceGraphBuilder<TVertex> Create<TVertex>(
        IIncidenceGraph<TVertex> fromDefinition)
        where TVertex : notnull
    {
        return new IncidenceGraphBuilder<TVertex>(fromDefinition);
    }

    public static IncidenceGraphBuilder<TVertex> Create<TVertex>()
        where TVertex : notnull
    {
        return new IncidenceGraphBuilder<TVertex>();
    }

    public static IncidenceGraphBuilder<TVertex, TEdgeLabel> Create<TVertex, TEdgeLabel>(
        IIncidenceGraph<TVertex, TEdgeLabel> fromDefinition)
        where TVertex : notnull
        where TEdgeLabel : notnull
    {
        return new IncidenceGraphBuilder<TVertex, TEdgeLabel>(fromDefinition);
    }

    public static IncidenceGraphBuilder<TVertex, TEdgeLabel> Create<TVertex, TEdgeLabel>()
        where TVertex : notnull
        where TEdgeLabel : notnull
    {
        return new IncidenceGraphBuilder<TVertex, TEdgeLabel>();
    }

    public delegate IEnumerable<TVertex> OutgoingEdges<TVertex>(TVertex vertex) where TVertex : notnull;
    public delegate int OutgoingDegree<TVertex>(TVertex vertex) where TVertex : notnull;
    public delegate IEnumerable<OutgoingEdge<TVertex, TEdgeLabel>> OutgoingLabeledEdges<TVertex, TEdgeLabel>(
        TVertex sourceVertex)
        where TVertex : notnull
        where TEdgeLabel : notnull;

    private class BuiltIncidenceGraph<TVertex> :
        IIncidenceGraph<TVertex>
        where TVertex : notnull
    {
        private readonly OutgoingDegree<TVertex> outgoingDegree;
        private readonly OutgoingEdges<TVertex> outgoingEdges;

        internal BuiltIncidenceGraph(
            IEqualityComparer<TVertex> vertexEqualityComparer,
            IComparer<TVertex> vertexComparer,
            IStringFormatter<TVertex> vertexFormatter,
            IEnumerable<TVertex> seedVertices,
            OutgoingDegree<TVertex> outgoingDegree,
            OutgoingEdges<TVertex> outgoingEdges)
        {
            VertexEqualityComparer = vertexEqualityComparer;
            VertexComparer = vertexComparer;
            VertexFormatter = vertexFormatter;
            SeedVertices = seedVertices;
            this.outgoingDegree = outgoingDegree;
            this.outgoingEdges = outgoingEdges;
        }

        public IEnumerable<TVertex> SeedVertices { get; }

        public IEqualityComparer<TVertex> VertexEqualityComparer { get; }

        public IComparer<TVertex> VertexComparer { get; }

        public IStringFormatter<TVertex> VertexFormatter { get; }

        public int OutgoingDegree(TVertex vertex) => outgoingDegree(vertex);

        public IEnumerable<TVertex> OutgoingEdges(TVertex vertex) => outgoingEdges(vertex);
    }

    public class IncidenceGraphBuilder<TVertex>
        where TVertex : notnull
    {
        private IEqualityComparer<TVertex> vertexEqualityComparer;
        private IComparer<TVertex> vertexComparer;
        private IStringFormatter<TVertex> vertexFormatter;
        private IEnumerable<TVertex>? seedVertices;
        private OutgoingDegree<TVertex>? outgoingDegree;
        private OutgoingEdges<TVertex>? outgoingEdges;

        internal IncidenceGraphBuilder()
        {
            vertexEqualityComparer = EqualityComparer<TVertex>.Default;
            vertexComparer = Comparer<TVertex>.Default;
            vertexFormatter = StringFormatter<TVertex>.Default;
        }

        internal IncidenceGraphBuilder(IIncidenceGraph<TVertex> fromDefinition)
        {
            vertexEqualityComparer = fromDefinition.VertexEqualityComparer;
            vertexComparer = fromDefinition.VertexComparer;
            vertexFormatter = fromDefinition.VertexFormatter;
            seedVertices = fromDefinition.SeedVertices;
            outgoingDegree = fromDefinition.OutgoingDegree;
            outgoingEdges = fromDefinition.OutgoingEdges;
        }

        public IncidenceGraphBuilder<TVertex> WithVertexEqualityComparer(
            IEqualityComparer<TVertex> vertexEqualityComparer)
        {
            this.vertexEqualityComparer = vertexEqualityComparer ?? throw new ArgumentNullException(nameof(vertexEqualityComparer));
            return this;
        }

        public IncidenceGraphBuilder<TVertex> WithVertexComparer(IComparer<TVertex> vertexComparer)
        {
            this.vertexComparer = vertexComparer ?? throw new ArgumentNullException(nameof(vertexComparer));
            return this;
        }

        public IncidenceGraphBuilder<TVertex> WithVertexFormatter(IStringFormatter<TVertex> vertexFormatter)
        {
            this.vertexFormatter = vertexFormatter ?? throw new ArgumentNullException(nameof(vertexFormatter));
            return this;
        }

        public IncidenceGraphBuilder<TVertex> WithSeedVertices(IEnumerable<TVertex> seedVertices)
        {
            this.seedVertices = seedVertices ?? throw new ArgumentNullException(nameof(seedVertices));
            return this;
        }

        public IncidenceGraphBuilder<TVertex> WithOutgoingDegree(OutgoingDegree<TVertex> outgoingDegree)
        {
            this.outgoingDegree = outgoingDegree ?? throw new ArgumentNullException(nameof(outgoingDegree));
            return this;
        }

        public IncidenceGraphBuilder<TVertex> WithOutgoingEdges(OutgoingEdges<TVertex> outgoingEdges)
        {
            this.outgoingEdges = outgoingEdges ?? throw new ArgumentNullException(nameof(outgoingEdges));
            return this;
        }

        public IIncidenceGraph<TVertex> Build()
        {
            if (seedVertices is null) throw new InvalidOperationException(nameof(seedVertices));
            if (outgoingDegree is null) throw new InvalidOperationException(nameof(outgoingDegree));
            if (outgoingEdges is null) throw new InvalidOperationException(nameof(outgoingEdges));

            return new BuiltIncidenceGraph<TVertex>(
                vertexEqualityComparer,
                vertexComparer,
                vertexFormatter,
                seedVertices,
                outgoingDegree,
                outgoingEdges);
        }
    }

    private class BuiltIncidenceGraph<TVertex, TEdgeLabel> :
        IIncidenceGraph<TVertex, TEdgeLabel>
        where TVertex : notnull
        where TEdgeLabel : notnull
    {
        private readonly OutgoingDegree<TVertex> outgoingDegree;
        private readonly OutgoingLabeledEdges<TVertex, TEdgeLabel> outgoingLabeledEdges;

        internal BuiltIncidenceGraph(
            IEqualityComparer<TVertex> vertexEqualityComparer,
            IComparer<TVertex> vertexComparer,
            IStringFormatter<TVertex> vertexFormatter,
            IStringFormatter<TEdgeLabel> edgeLabelFormatter,
            IEnumerable<TVertex> seedVertices,
            OutgoingDegree<TVertex> outgoingDegree,
            OutgoingLabeledEdges<TVertex, TEdgeLabel> outgoingLabeledEdges)
        {
            VertexEqualityComparer = vertexEqualityComparer;
            VertexComparer = vertexComparer;
            VertexFormatter = vertexFormatter;
            EdgeLabelFormatter = edgeLabelFormatter;
            SeedVertices = seedVertices;
            this.outgoingDegree = outgoingDegree;
            this.outgoingLabeledEdges = outgoingLabeledEdges;
        }

        public IEnumerable<TVertex> SeedVertices { get; }

        public IEqualityComparer<TVertex> VertexEqualityComparer { get; }

        public IComparer<TVertex> VertexComparer { get; }

        public IStringFormatter<TVertex> VertexFormatter { get; }

        public IStringFormatter<TEdgeLabel> EdgeLabelFormatter { get; }

        public int OutgoingDegree(TVertex vertex) => outgoingDegree(vertex);

        public IEnumerable<TVertex> OutgoingEdges(TVertex vertex) =>
            outgoingLabeledEdges(vertex).Select(e => e.Target);

        public IEnumerable<OutgoingEdge<TVertex, TEdgeLabel>> OutgoingLabeledEdges(TVertex sourceVertex) =>
            outgoingLabeledEdges(sourceVertex);
    }

    public class IncidenceGraphBuilder<TVertex, TEdgeLabel>
        where TVertex : notnull
        where TEdgeLabel : notnull
    {
        private IEqualityComparer<TVertex> vertexEqualityComparer;
        private IComparer<TVertex> vertexComparer;
        private IStringFormatter<TVertex> vertexFormatter;
        private IStringFormatter<TEdgeLabel> edgeLabelFormatter;
        private IEnumerable<TVertex>? seedVertices;
        private OutgoingDegree<TVertex>? outgoingDegree;
        private OutgoingLabeledEdges<TVertex, TEdgeLabel>? outgoingLabeledEdges;

        internal IncidenceGraphBuilder()
        {
            vertexEqualityComparer = EqualityComparer<TVertex>.Default;
            vertexComparer = Comparer<TVertex>.Default;
            vertexFormatter = StringFormatter<TVertex>.Default;
            edgeLabelFormatter = StringFormatter<TEdgeLabel>.Default;
        }

        internal IncidenceGraphBuilder(IIncidenceGraph<TVertex, TEdgeLabel> fromDefinition)
        {
            vertexEqualityComparer = fromDefinition.VertexEqualityComparer;
            vertexComparer = fromDefinition.VertexComparer;
            vertexFormatter = fromDefinition.VertexFormatter;
            edgeLabelFormatter = fromDefinition.EdgeLabelFormatter;
            seedVertices = fromDefinition.SeedVertices;
            outgoingDegree = fromDefinition.OutgoingDegree;
            outgoingLabeledEdges = fromDefinition.OutgoingLabeledEdges;
        }

        public IncidenceGraphBuilder<TVertex, TEdgeLabel> WithVertexEqualityComparer(
            IEqualityComparer<TVertex> vertexEqualityComparer)
        {
            this.vertexEqualityComparer = vertexEqualityComparer ?? throw new ArgumentNullException(nameof(vertexEqualityComparer));
            return this;
        }

        public IncidenceGraphBuilder<TVertex, TEdgeLabel> WithVertexComparer(IComparer<TVertex> vertexComparer)
        {
            this.vertexComparer = vertexComparer ?? throw new ArgumentNullException(nameof(vertexComparer));
            return this;
        }

        public IncidenceGraphBuilder<TVertex, TEdgeLabel> WithVertexFormatter(IStringFormatter<TVertex> vertexFormatter)
        {
            this.vertexFormatter = vertexFormatter ?? throw new ArgumentNullException(nameof(vertexFormatter));
            return this;
        }

        public IncidenceGraphBuilder<TVertex, TEdgeLabel> WithEdgeLabelFormatter(IStringFormatter<TEdgeLabel> edgeLabelFormatter)
        {
            this.edgeLabelFormatter = edgeLabelFormatter ?? throw new ArgumentNullException(nameof(edgeLabelFormatter));
            return this;
        }

        public IncidenceGraphBuilder<TVertex, TEdgeLabel> WithSeedVertices(IEnumerable<TVertex> seedVertices)
        {
            this.seedVertices = seedVertices ?? throw new ArgumentNullException(nameof(seedVertices));
            return this;
        }

        public IncidenceGraphBuilder<TVertex, TEdgeLabel> WithOutgoingDegree(OutgoingDegree<TVertex> outgoingDegree)
        {
            this.outgoingDegree = outgoingDegree ?? throw new ArgumentNullException(nameof(outgoingDegree));
            return this;
        }

        public IncidenceGraphBuilder<TVertex, TEdgeLabel> WithOutgoingLabeledEdges(OutgoingLabeledEdges<TVertex, TEdgeLabel> outgoingLabeledEdges)
        {
            this.outgoingLabeledEdges = outgoingLabeledEdges ?? throw new ArgumentNullException(nameof(outgoingLabeledEdges));
            return this;
        }

        public IIncidenceGraph<TVertex, TEdgeLabel> Build()
        {
            if (seedVertices is null) throw new InvalidOperationException(nameof(seedVertices));
            if (outgoingDegree is null) throw new InvalidOperationException(nameof(outgoingDegree));
            if (outgoingLabeledEdges is null) throw new InvalidOperationException(nameof(outgoingLabeledEdges));

            return new BuiltIncidenceGraph<TVertex, TEdgeLabel>(
                vertexEqualityComparer,
                vertexComparer,
                vertexFormatter,
                edgeLabelFormatter,
                seedVertices,
                outgoingDegree,
                outgoingLabeledEdges);
        }
    }
}
