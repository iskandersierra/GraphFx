namespace GraphFx;

public static class BidirectionalGraph
{
    public static BidirectionalGraphBuilder<TVertex> Create<TVertex>(
        IBidirectionalGraph<TVertex> fromDefinition)
        where TVertex : notnull
    {
        return new BidirectionalGraphBuilder<TVertex>(fromDefinition);
    }

    public static BidirectionalGraphBuilder<TVertex> Create<TVertex>()
        where TVertex : notnull
    {
        return new BidirectionalGraphBuilder<TVertex>();
    }

    public static BidirectionalGraphBuilder<TVertex, TEdgeLabel> Create<TVertex, TEdgeLabel>(
        IBidirectionalGraph<TVertex, TEdgeLabel> fromDefinition)
        where TVertex : notnull
        where TEdgeLabel : notnull
    {
        return new BidirectionalGraphBuilder<TVertex, TEdgeLabel>(fromDefinition);
    }

    public static BidirectionalGraphBuilder<TVertex, TEdgeLabel> Create<TVertex, TEdgeLabel>()
        where TVertex : notnull
        where TEdgeLabel : notnull
    {
        return new BidirectionalGraphBuilder<TVertex, TEdgeLabel>();
    }

    public delegate IEnumerable<TVertex> OutgoingEdges<TVertex>(TVertex vertex) where TVertex : notnull;
    public delegate int OutgoingDegree<TVertex>(TVertex vertex) where TVertex : notnull;
    public delegate IEnumerable<OutgoingEdge<TVertex, TEdgeLabel>> OutgoingLabeledEdges<TVertex, TEdgeLabel>(
        TVertex sourceVertex)
        where TVertex : notnull
        where TEdgeLabel : notnull;

    public delegate IEnumerable<TVertex> IncomingEdges<TVertex>(TVertex vertex) where TVertex : notnull;
    public delegate int IncomingDegree<TVertex>(TVertex vertex) where TVertex : notnull;
    public delegate IEnumerable<IncomingEdge<TVertex, TEdgeLabel>> IncomingLabeledEdges<TVertex, TEdgeLabel>(
        TVertex sourceVertex)
        where TVertex : notnull
        where TEdgeLabel : notnull;

    private class BuiltBidirectionalGraph<TVertex> :
        IBidirectionalGraph<TVertex>
        where TVertex : notnull
    {
        private readonly OutgoingDegree<TVertex> outgoingDegree;
        private readonly OutgoingEdges<TVertex> outgoingEdges;
        private readonly IncomingDegree<TVertex> incomingDegree;
        private readonly IncomingEdges<TVertex> incomingEdges;

        internal BuiltBidirectionalGraph(
            IEqualityComparer<TVertex> vertexEqualityComparer,
            IComparer<TVertex> vertexComparer,
            IStringFormatter<TVertex> vertexFormatter,
            IEnumerable<TVertex> seedVertices,
            OutgoingDegree<TVertex> outgoingDegree,
            OutgoingEdges<TVertex> outgoingEdges,
            IncomingDegree<TVertex> incomingDegree,
            IncomingEdges<TVertex> incomingEdges)
        {
            VertexEqualityComparer = vertexEqualityComparer;
            VertexComparer = vertexComparer;
            VertexFormatter = vertexFormatter;
            SeedVertices = seedVertices;
            this.outgoingDegree = outgoingDegree;
            this.outgoingEdges = outgoingEdges;
            this.incomingDegree = incomingDegree;
            this.incomingEdges = incomingEdges;
        }

        public IEnumerable<TVertex> SeedVertices { get; }

        public IEqualityComparer<TVertex> VertexEqualityComparer { get; }

        public IComparer<TVertex> VertexComparer { get; }

        public IStringFormatter<TVertex> VertexFormatter { get; }

        public int OutgoingDegree(TVertex vertex) => outgoingDegree(vertex);

        public IEnumerable<TVertex> OutgoingEdges(TVertex vertex) => outgoingEdges(vertex);

        public int IncomingDegree(TVertex vertex) => incomingDegree(vertex);

        public IEnumerable<TVertex> IncomingEdges(TVertex vertex) => incomingEdges(vertex);
    }

    public class BidirectionalGraphBuilder<TVertex>
        where TVertex : notnull
    {
        private IEqualityComparer<TVertex> vertexEqualityComparer;
        private IComparer<TVertex> vertexComparer;
        private IStringFormatter<TVertex> vertexFormatter;
        private IEnumerable<TVertex>? seedVertices;
        private OutgoingDegree<TVertex>? outgoingDegree;
        private OutgoingEdges<TVertex>? outgoingEdges;
        private IncomingDegree<TVertex>? incomingDegree;
        private IncomingEdges<TVertex>? incomingEdges;

        internal BidirectionalGraphBuilder()
        {
            vertexEqualityComparer = EqualityComparer<TVertex>.Default;
            vertexComparer = Comparer<TVertex>.Default;
            vertexFormatter = StringFormatter<TVertex>.Default;
        }

        internal BidirectionalGraphBuilder(IBidirectionalGraph<TVertex> fromDefinition)
        {
            vertexEqualityComparer = fromDefinition.VertexEqualityComparer;
            vertexComparer = fromDefinition.VertexComparer;
            vertexFormatter = fromDefinition.VertexFormatter;
            seedVertices = fromDefinition.SeedVertices;
            outgoingDegree = fromDefinition.OutgoingDegree;
            outgoingEdges = fromDefinition.OutgoingEdges;
            incomingDegree = fromDefinition.IncomingDegree;
            incomingEdges = fromDefinition.IncomingEdges;
        }

        public BidirectionalGraphBuilder<TVertex> WithVertexEqualityComparer(
            IEqualityComparer<TVertex> vertexEqualityComparer)
        {
            this.vertexEqualityComparer = vertexEqualityComparer ?? throw new ArgumentNullException(nameof(vertexEqualityComparer));
            return this;
        }

        public BidirectionalGraphBuilder<TVertex> WithVertexComparer(IComparer<TVertex> vertexComparer)
        {
            this.vertexComparer = vertexComparer ?? throw new ArgumentNullException(nameof(vertexComparer));
            return this;
        }

        public BidirectionalGraphBuilder<TVertex> WithVertexFormatter(IStringFormatter<TVertex> vertexFormatter)
        {
            this.vertexFormatter = vertexFormatter ?? throw new ArgumentNullException(nameof(vertexFormatter));
            return this;
        }

        public BidirectionalGraphBuilder<TVertex> WithSeedVertices(IEnumerable<TVertex> seedVertices)
        {
            this.seedVertices = seedVertices ?? throw new ArgumentNullException(nameof(seedVertices));
            return this;
        }

        public BidirectionalGraphBuilder<TVertex> WithOutgoingDegree(OutgoingDegree<TVertex> outgoingDegree)
        {
            this.outgoingDegree = outgoingDegree ?? throw new ArgumentNullException(nameof(outgoingDegree));
            return this;
        }

        public BidirectionalGraphBuilder<TVertex> WithOutgoingEdges(OutgoingEdges<TVertex> outgoingEdges)
        {
            this.outgoingEdges = outgoingEdges ?? throw new ArgumentNullException(nameof(outgoingEdges));
            return this;
        }

        public BidirectionalGraphBuilder<TVertex> WithIncomingDegree(IncomingDegree<TVertex> incomingDegree)
        {
            this.incomingDegree = incomingDegree ?? throw new ArgumentNullException(nameof(incomingDegree));
            return this;
        }

        public BidirectionalGraphBuilder<TVertex> WithIncomingEdges(IncomingEdges<TVertex> incomingEdges)
        {
            this.incomingEdges = incomingEdges ?? throw new ArgumentNullException(nameof(incomingEdges));
            return this;
        }

        public IBidirectionalGraph<TVertex> Build()
        {
            if (seedVertices is null) throw new InvalidOperationException(nameof(seedVertices));
            if (outgoingDegree is null) throw new InvalidOperationException(nameof(outgoingDegree));
            if (outgoingEdges is null) throw new InvalidOperationException(nameof(outgoingEdges));
            if (incomingDegree is null) throw new InvalidOperationException(nameof(incomingDegree));
            if (incomingEdges is null) throw new InvalidOperationException(nameof(incomingEdges));

            return new BuiltBidirectionalGraph<TVertex>(
                vertexEqualityComparer,
                vertexComparer,
                vertexFormatter,
                seedVertices,
                outgoingDegree,
                outgoingEdges,
                incomingDegree,
                incomingEdges);
        }
    }

    private class BuiltBidirectionalGraph<TVertex, TEdgeLabel> :
        IBidirectionalGraph<TVertex, TEdgeLabel>
        where TVertex : notnull
        where TEdgeLabel : notnull
    {
        private readonly OutgoingDegree<TVertex> outgoingDegree;
        private readonly OutgoingLabeledEdges<TVertex, TEdgeLabel> outgoingLabeledEdges;
        private readonly IncomingDegree<TVertex> incomingDegree;
        private readonly IncomingLabeledEdges<TVertex, TEdgeLabel> incomingLabeledEdges;

        internal BuiltBidirectionalGraph(
            IEqualityComparer<TVertex> vertexEqualityComparer,
            IComparer<TVertex> vertexComparer,
            IStringFormatter<TVertex> vertexFormatter,
            IStringFormatter<TEdgeLabel> edgeLabelFormatter,
            IEnumerable<TVertex> seedVertices,
            OutgoingDegree<TVertex> outgoingDegree,
            OutgoingLabeledEdges<TVertex, TEdgeLabel> outgoingLabeledEdges,
            IncomingDegree<TVertex> incomingDegree,
            IncomingLabeledEdges<TVertex, TEdgeLabel> incomingLabeledEdges)
        {
            VertexEqualityComparer = vertexEqualityComparer;
            VertexComparer = vertexComparer;
            VertexFormatter = vertexFormatter;
            EdgeLabelFormatter = edgeLabelFormatter;
            SeedVertices = seedVertices;
            this.outgoingDegree = outgoingDegree;
            this.outgoingLabeledEdges = outgoingLabeledEdges;
            this.incomingDegree = incomingDegree;
            this.incomingLabeledEdges = incomingLabeledEdges;
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

        public int IncomingDegree(TVertex vertex) => incomingDegree(vertex);

        public IEnumerable<TVertex> IncomingEdges(TVertex vertex) =>
            incomingLabeledEdges(vertex).Select(e => e.Source);

        public IEnumerable<IncomingEdge<TVertex, TEdgeLabel>> IncomingLabeledEdges(TVertex targetVertex) =>
            incomingLabeledEdges(targetVertex);
    }

    public class BidirectionalGraphBuilder<TVertex, TEdgeLabel>
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
        private IncomingDegree<TVertex>? incomingDegree;
        private IncomingLabeledEdges<TVertex, TEdgeLabel>? incomingLabeledEdges;

        internal BidirectionalGraphBuilder()
        {
            vertexEqualityComparer = EqualityComparer<TVertex>.Default;
            vertexComparer = Comparer<TVertex>.Default;
            vertexFormatter = StringFormatter<TVertex>.Default;
            edgeLabelFormatter = StringFormatter<TEdgeLabel>.Default;
        }

        internal BidirectionalGraphBuilder(IBidirectionalGraph<TVertex, TEdgeLabel> fromDefinition)
        {
            vertexEqualityComparer = fromDefinition.VertexEqualityComparer;
            vertexComparer = fromDefinition.VertexComparer;
            vertexFormatter = fromDefinition.VertexFormatter;
            edgeLabelFormatter = fromDefinition.EdgeLabelFormatter;
            seedVertices = fromDefinition.SeedVertices;
            outgoingDegree = fromDefinition.OutgoingDegree;
            outgoingLabeledEdges = fromDefinition.OutgoingLabeledEdges;
            incomingDegree = fromDefinition.IncomingDegree;
            incomingLabeledEdges = fromDefinition.IncomingLabeledEdges;
        }

        public BidirectionalGraphBuilder<TVertex, TEdgeLabel> WithVertexEqualityComparer(
            IEqualityComparer<TVertex> vertexEqualityComparer)
        {
            this.vertexEqualityComparer = vertexEqualityComparer ?? throw new ArgumentNullException(nameof(vertexEqualityComparer));
            return this;
        }

        public BidirectionalGraphBuilder<TVertex, TEdgeLabel> WithVertexComparer(IComparer<TVertex> vertexComparer)
        {
            this.vertexComparer = vertexComparer ?? throw new ArgumentNullException(nameof(vertexComparer));
            return this;
        }

        public BidirectionalGraphBuilder<TVertex, TEdgeLabel> WithVertexFormatter(IStringFormatter<TVertex> vertexFormatter)
        {
            this.vertexFormatter = vertexFormatter ?? throw new ArgumentNullException(nameof(vertexFormatter));
            return this;
        }

        public BidirectionalGraphBuilder<TVertex, TEdgeLabel> WithEdgeLabelFormatter(IStringFormatter<TEdgeLabel> edgeLabelFormatter)
        {
            this.edgeLabelFormatter = edgeLabelFormatter ?? throw new ArgumentNullException(nameof(edgeLabelFormatter));
            return this;
        }

        public BidirectionalGraphBuilder<TVertex, TEdgeLabel> WithSeedVertices(IEnumerable<TVertex> seedVertices)
        {
            this.seedVertices = seedVertices ?? throw new ArgumentNullException(nameof(seedVertices));
            return this;
        }

        public BidirectionalGraphBuilder<TVertex, TEdgeLabel> WithOutgoingDegree(OutgoingDegree<TVertex> outgoingDegree)
        {
            this.outgoingDegree = outgoingDegree ?? throw new ArgumentNullException(nameof(outgoingDegree));
            return this;
        }

        public BidirectionalGraphBuilder<TVertex, TEdgeLabel> WithOutgoingLabeledEdges(OutgoingLabeledEdges<TVertex, TEdgeLabel> outgoingLabeledEdges)
        {
            this.outgoingLabeledEdges = outgoingLabeledEdges ?? throw new ArgumentNullException(nameof(outgoingLabeledEdges));
            return this;
        }

        public BidirectionalGraphBuilder<TVertex, TEdgeLabel> WithIncomingDegree(IncomingDegree<TVertex> incomingDegree)
        {
            this.incomingDegree = incomingDegree ?? throw new ArgumentNullException(nameof(incomingDegree));
            return this;
        }

        public BidirectionalGraphBuilder<TVertex, TEdgeLabel> WithIncomingLabeledEdges(IncomingLabeledEdges<TVertex, TEdgeLabel> incomingLabeledEdges)
        {
            this.incomingLabeledEdges = incomingLabeledEdges ?? throw new ArgumentNullException(nameof(incomingLabeledEdges));
            return this;
        }

        public IBidirectionalGraph<TVertex, TEdgeLabel> Build()
        {
            if (seedVertices is null) throw new InvalidOperationException(nameof(seedVertices));
            if (outgoingDegree is null) throw new InvalidOperationException(nameof(outgoingDegree));
            if (outgoingLabeledEdges is null) throw new InvalidOperationException(nameof(outgoingLabeledEdges));
            if (incomingDegree is null) throw new InvalidOperationException(nameof(incomingDegree));
            if (incomingLabeledEdges is null) throw new InvalidOperationException(nameof(incomingLabeledEdges));

            return new BuiltBidirectionalGraph<TVertex, TEdgeLabel>(
                vertexEqualityComparer,
                vertexComparer,
                vertexFormatter,
                edgeLabelFormatter,
                seedVertices,
                outgoingDegree,
                outgoingLabeledEdges,
                incomingDegree,
                incomingLabeledEdges);
        }
    }
}
