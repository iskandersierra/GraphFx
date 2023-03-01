using GraphFx.Traversals;

namespace GraphFx;

public static class IncidenceGraph
{
    public static IncidenceGraphBuilder<TVertex> Create<TVertex>(
        IIncidenceGraph<TVertex> fromDefinition)
        where TVertex : notnull
    {
        return new IncidenceGraphBuilder<TVertex>(fromDefinition);
    }

    public static IncidenceGraphBuilder<TVertex> Create<TVertex>(
        IGraph<TVertex> fromDefinition)
        where TVertex : notnull
    {
        return new IncidenceGraphBuilder<TVertex>(fromDefinition);
    }

    public static IncidenceGraphBuilder<TVertex> Create<TVertex>(
        IEqualityComparer<TVertex>? vertexEqualityComparer = null,
        IComparer<TVertex>? vertexComparer = null,
        IStringFormatter<TVertex>? vertexFormatter = null)
        where TVertex : notnull
    {
        return new IncidenceGraphBuilder<TVertex>(
            vertexEqualityComparer,
            vertexComparer,
            vertexFormatter);
    }

    public static IncidenceGraphBuilder<TVertex, TEdgeLabel> Create<TVertex, TEdgeLabel>(
        IIncidenceGraph<TVertex, TEdgeLabel> fromDefinition)
        where TVertex : notnull
        where TEdgeLabel : notnull
    {
        return new IncidenceGraphBuilder<TVertex, TEdgeLabel>(fromDefinition);
    }

    public static IncidenceGraphBuilder<TVertex, TEdgeLabel> Create<TVertex, TEdgeLabel>(
        IGraph<TVertex, TEdgeLabel> fromDefinition)
        where TVertex : notnull
        where TEdgeLabel : notnull
    {
        return new IncidenceGraphBuilder<TVertex, TEdgeLabel>(fromDefinition);
    }

    public static IncidenceGraphBuilder<TVertex, TEdgeLabel> Create<TVertex, TEdgeLabel>(
        IEqualityComparer<TVertex>? vertexEqualityComparer = null,
        IComparer<TVertex>? vertexComparer = null,
        IStringFormatter<TVertex>? vertexFormatter = null,
        IStringFormatter<TEdgeLabel>? edgeLabelFormatter = null)
        where TVertex : notnull
        where TEdgeLabel : notnull
    {
        return new IncidenceGraphBuilder<TVertex, TEdgeLabel>(
            vertexEqualityComparer,
            vertexComparer,
            vertexFormatter,
            edgeLabelFormatter);
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

        internal IncidenceGraphBuilder(
            IEqualityComparer<TVertex>? vertexEqualityComparer,
            IComparer<TVertex>? vertexComparer,
            IStringFormatter<TVertex>? vertexFormatter)
        {
            this.vertexEqualityComparer = vertexEqualityComparer ?? EqualityComparer<TVertex>.Default;
            this.vertexComparer = vertexComparer ?? Comparer<TVertex>.Default;
            this.vertexFormatter = vertexFormatter ?? StringFormatter<TVertex>.Default;
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

        internal IncidenceGraphBuilder(IGraph<TVertex> fromDefinition)
        {
            vertexEqualityComparer = fromDefinition.VertexEqualityComparer;
            vertexComparer = fromDefinition.VertexComparer;
            vertexFormatter = fromDefinition.VertexFormatter;
        }

        public IncidenceGraphBuilder<TVertex> WithVertexEqualityComparer(
            IEqualityComparer<TVertex>? comparer)
        {
            this.vertexEqualityComparer = comparer ?? EqualityComparer<TVertex>.Default;
            return this;
        }

        public IncidenceGraphBuilder<TVertex> WithVertexComparer(IComparer<TVertex>? comparer)
        {
            this.vertexComparer = comparer ?? Comparer<TVertex>.Default;
            return this;
        }

        public IncidenceGraphBuilder<TVertex> WithVertexFormatter(IStringFormatter<TVertex>? formatter)
        {
            this.vertexFormatter = formatter ?? StringFormatter<TVertex>.Default;
            return this;
        }

        public IncidenceGraphBuilder<TVertex> WithSeedVertices(IEnumerable<TVertex> seed)
        {
            this.seedVertices = seed ?? throw new ArgumentNullException(nameof(seed));
            return this;
        }

        public IncidenceGraphBuilder<TVertex> WithOutgoingDegree(OutgoingDegree<TVertex> degree)
        {
            this.outgoingDegree = degree ?? throw new ArgumentNullException(nameof(degree));
            return this;
        }

        public IncidenceGraphBuilder<TVertex> WithOutgoingEdges(OutgoingEdges<TVertex> edges)
        {
            this.outgoingEdges = edges ?? throw new ArgumentNullException(nameof(edges));
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

        internal IncidenceGraphBuilder(
            IEqualityComparer<TVertex>? vertexEqualityComparer,
            IComparer<TVertex>? vertexComparer,
            IStringFormatter<TVertex>? vertexFormatter,
            IStringFormatter<TEdgeLabel>? edgeLabelFormatter)
        {
            this.vertexEqualityComparer = vertexEqualityComparer ?? EqualityComparer<TVertex>.Default;
            this.vertexComparer = vertexComparer ?? Comparer<TVertex>.Default;
            this.vertexFormatter = vertexFormatter ?? StringFormatter<TVertex>.Default;
            this.edgeLabelFormatter = edgeLabelFormatter ?? StringFormatter<TEdgeLabel>.Default;
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

        internal IncidenceGraphBuilder(IGraph<TVertex, TEdgeLabel> fromDefinition)
        {
            vertexEqualityComparer = fromDefinition.VertexEqualityComparer;
            vertexComparer = fromDefinition.VertexComparer;
            vertexFormatter = fromDefinition.VertexFormatter;
            edgeLabelFormatter = fromDefinition.EdgeLabelFormatter;
        }

        public IncidenceGraphBuilder<TVertex, TEdgeLabel> WithVertexEqualityComparer(
            IEqualityComparer<TVertex>? comparer)
        {
            this.vertexEqualityComparer = comparer ?? EqualityComparer<TVertex>.Default;
            return this;
        }

        public IncidenceGraphBuilder<TVertex, TEdgeLabel> WithVertexComparer(IComparer<TVertex>? comparer)
        {
            this.vertexComparer = comparer ?? Comparer<TVertex>.Default;
            return this;
        }

        public IncidenceGraphBuilder<TVertex, TEdgeLabel> WithVertexFormatter(IStringFormatter<TVertex>? formatter)
        {
            this.vertexFormatter = formatter ?? StringFormatter<TVertex>.Default;
            return this;
        }

        public IncidenceGraphBuilder<TVertex, TEdgeLabel> WithEdgeLabelFormatter(IStringFormatter<TEdgeLabel>? formatter)
        {
            this.edgeLabelFormatter = formatter ?? StringFormatter<TEdgeLabel>.Default;
            return this;
        }

        public IncidenceGraphBuilder<TVertex, TEdgeLabel> WithSeedVertices(IEnumerable<TVertex> seed)
        {
            this.seedVertices = seed ?? throw new ArgumentNullException(nameof(seed));
            return this;
        }

        public IncidenceGraphBuilder<TVertex, TEdgeLabel> WithOutgoingDegree(OutgoingDegree<TVertex> degree)
        {
            this.outgoingDegree = degree ?? throw new ArgumentNullException(nameof(degree));
            return this;
        }

        public IncidenceGraphBuilder<TVertex, TEdgeLabel> WithOutgoingLabeledEdges(OutgoingLabeledEdges<TVertex, TEdgeLabel> edges)
        {
            this.outgoingLabeledEdges = edges ?? throw new ArgumentNullException(nameof(edges));
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

    private static readonly TraversalOptions ToExplicitTraversalOptions = new TraversalOptions() {
        IncludeEdgesWithVisitedTargets = true
    }.ForDepthFirstSearch();

    public static ExplicitGraph<TVertex> ToExplicitGraph<TVertex>(
        this IIncidenceGraph<TVertex> incidenceGraph)
        where TVertex : notnull
    {
        var explicitGraph = new ExplicitGraph<TVertex>(
            incidenceGraph.VertexEqualityComparer,
            incidenceGraph.VertexComparer,
            incidenceGraph.VertexFormatter);

        incidenceGraph.DepthFirstSearchRecursive(
            events: new()
            {
                OnVertex = info =>
                {
                    explicitGraph.Add(info.Vertex);
                    return true;
                },
                OnEdge = info =>
                {
                    explicitGraph.Add(info.Edge);
                    return true;
                },
            },
            options: ToExplicitTraversalOptions);

        return explicitGraph;
    }

    public static ExplicitGraph<TVertex, TEdgeLabel> ToExplicitGraph<TVertex, TEdgeLabel>(
        this IIncidenceGraph<TVertex, TEdgeLabel> incidenceGraph)
        where TVertex : notnull
        where TEdgeLabel : notnull
    {
        var explicitGraph = new ExplicitGraph<TVertex, TEdgeLabel>(
            incidenceGraph.VertexEqualityComparer,
            incidenceGraph.VertexComparer,
            incidenceGraph.VertexFormatter,
            incidenceGraph.EdgeLabelFormatter);

        incidenceGraph.DepthFirstSearchRecursive(
            events: new()
            {
                OnVertex = info =>
                {
                    explicitGraph.Add(info.Vertex);
                    return true;
                },
                OnEdge = info =>
                {
                    explicitGraph.Add(info.Edge);
                    return true;
                },
            },
            options: ToExplicitTraversalOptions);

        return explicitGraph;
    }

    public static string Format<TVertex>(this IIncidenceGraph<TVertex> graph)
        where TVertex : notnull
    {
        return TraversalGraphFormatter<TVertex>.Default.Format(graph);
    }

    public static IIncidenceGraph<TVertex, TEdgeLabel> ToIncidenceGraph<TVertex, TEdgeLabel>(
        this IDictionary<TVertex, IReadOnlyCollection<OutgoingEdge<TVertex, TEdgeLabel>>> adjacencyGraph,
        IGraph<TVertex, TEdgeLabel> referenceGraph)
        where TVertex : notnull
        where TEdgeLabel : notnull
    {
        return Create(referenceGraph)
            .WithSeedVertices(adjacencyGraph.Keys)
            .WithOutgoingDegree(vertex => adjacencyGraph.TryGetValue(vertex, out var edges) ? edges.Count : 0)
            .WithOutgoingLabeledEdges(vertex =>
                adjacencyGraph.TryGetValue(vertex, out var edges)
                    ? edges
                    : Enumerable.Empty<OutgoingEdge<TVertex, TEdgeLabel>>())
            .Build();
    }

    public static IIncidenceGraph<TVertex, TEdgeLabel> ToIncidenceGraph<TVertex, TEdgeLabel>(
        this IDictionary<TVertex, IReadOnlyCollection<OutgoingEdge<TVertex, TEdgeLabel>>> adjacencyGraph,
        IEqualityComparer<TVertex>? vertexEqualityComparer = null,
        IComparer<TVertex>? vertexComparer = null,
        IStringFormatter<TVertex>? vertexFormatter = null,
        IStringFormatter<TEdgeLabel>? edgeLabelFormatter = null)
        where TVertex : notnull
        where TEdgeLabel : notnull
    {
        return Create(vertexEqualityComparer, vertexComparer, vertexFormatter, edgeLabelFormatter)
            .WithSeedVertices(adjacencyGraph.Keys)
            .WithOutgoingDegree(vertex => adjacencyGraph.TryGetValue(vertex, out var edges) ? edges.Count : 0)
            .WithOutgoingLabeledEdges(vertex =>
                adjacencyGraph.TryGetValue(vertex, out var edges)
                    ? edges
                    : Enumerable.Empty<OutgoingEdge<TVertex, TEdgeLabel>>())
            .Build();
    }

    public static IIncidenceGraph<TVertex> ToIncidenceGraph<TVertex>(
        this IDictionary<TVertex, IReadOnlyCollection<TVertex>> adjacencyGraph,
        IGraph<TVertex> referenceGraph)
        where TVertex : notnull
    {
        return Create(referenceGraph)
            .WithSeedVertices(adjacencyGraph.Keys)
            .WithOutgoingDegree(vertex => adjacencyGraph.TryGetValue(vertex, out var edges) ? edges.Count : 0)
            .WithOutgoingEdges(vertex =>
                adjacencyGraph.TryGetValue(vertex, out var edges)
                    ? edges
                    : Enumerable.Empty<TVertex>())
            .Build();
    }

    public static IIncidenceGraph<TVertex> ToIncidenceGraph<TVertex>(
        this IDictionary<TVertex, IReadOnlyCollection<TVertex>> adjacencyGraph,
        IEqualityComparer<TVertex>? vertexEqualityComparer = null,
        IComparer<TVertex>? vertexComparer = null,
        IStringFormatter<TVertex>? vertexFormatter = null)
        where TVertex : notnull
    {
        return Create(vertexEqualityComparer, vertexComparer, vertexFormatter)
            .WithSeedVertices(adjacencyGraph.Keys)
            .WithOutgoingDegree(vertex => adjacencyGraph.TryGetValue(vertex, out var edges) ? edges.Count : 0)
            .WithOutgoingEdges(vertex =>
                adjacencyGraph.TryGetValue(vertex, out var edges)
                    ? edges
                    : Enumerable.Empty<TVertex>())
            .Build();
    }
}
