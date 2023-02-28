namespace GraphFx;

public static class ExplicitGraph
{
    public static ExplicitGraphBuilder<TVertex> Create<TVertex>(
        IExplicitGraph<TVertex> fromDefinition)
        where TVertex : notnull
    {
        return new ExplicitGraphBuilder<TVertex>(fromDefinition);
    }

    public static ExplicitGraphBuilder<TVertex> Create<TVertex>()
        where TVertex : notnull
    {
        return new ExplicitGraphBuilder<TVertex>();
    }

    public static ExplicitGraphBuilder<TVertex, TEdgeLabel> Create<TVertex, TEdgeLabel>(
        IExplicitGraph<TVertex, TEdgeLabel> fromDefinition)
        where TVertex : notnull
        where TEdgeLabel : notnull
    {
        return new ExplicitGraphBuilder<TVertex, TEdgeLabel>(fromDefinition);
    }

    public static ExplicitGraphBuilder<TVertex, TEdgeLabel> Create<TVertex, TEdgeLabel>()
        where TVertex : notnull
        where TEdgeLabel : notnull
    {
        return new ExplicitGraphBuilder<TVertex, TEdgeLabel>();
    }

    private class BuiltExplicitGraph<TVertex> :
        IExplicitGraph<TVertex>
        where TVertex : notnull
    {
        internal BuiltExplicitGraph(
            IEqualityComparer<TVertex> vertexEqualityComparer,
            IComparer<TVertex> vertexComparer,
            IStringFormatter<TVertex> vertexFormatter,
            IEnumerable<TVertex> vertices,
            IEnumerable<Edge<TVertex>> edges)
        {
            VertexEqualityComparer = vertexEqualityComparer;
            VertexComparer = vertexComparer;
            VertexFormatter = vertexFormatter;
            Vertices = vertices;
            Edges = edges;
        }

        public IEnumerable<TVertex> Vertices { get; }

        public IEnumerable<Edge<TVertex>> Edges { get; }

        public IEqualityComparer<TVertex> VertexEqualityComparer { get; }

        public IComparer<TVertex> VertexComparer { get; }

        public IStringFormatter<TVertex> VertexFormatter { get; }
    }

    public class ExplicitGraphBuilder<TVertex>
        where TVertex : notnull
    {
        private IEqualityComparer<TVertex> vertexEqualityComparer;
        private IComparer<TVertex> vertexComparer;
        private IStringFormatter<TVertex> vertexFormatter;
        private IEnumerable<TVertex>? vertices;
        private IEnumerable<Edge<TVertex>>? edges;

        internal ExplicitGraphBuilder()
        {
            vertexEqualityComparer = EqualityComparer<TVertex>.Default;
            vertexComparer = Comparer<TVertex>.Default;
            vertexFormatter = StringFormatter<TVertex>.Default;
        }

        internal ExplicitGraphBuilder(IExplicitGraph<TVertex> fromDefinition)
        {
            if (fromDefinition == null) throw new ArgumentNullException(nameof(fromDefinition));

            vertexEqualityComparer = fromDefinition.VertexEqualityComparer;
            vertexComparer = fromDefinition.VertexComparer;
            vertexFormatter = fromDefinition.VertexFormatter;
            vertices = fromDefinition.Vertices;
            edges = fromDefinition.Edges;
        }

        public ExplicitGraphBuilder<TVertex> WithVertexEqualityComparer(
            IEqualityComparer<TVertex> comparer)
        {
            this.vertexEqualityComparer = comparer ?? throw new ArgumentNullException(nameof(comparer));
            return this;
        }

        public ExplicitGraphBuilder<TVertex> WithVertexComparer(
            IComparer<TVertex> comparer)
        {
            this.vertexComparer = comparer ?? throw new ArgumentNullException(nameof(comparer));
            return this;
        }

        public ExplicitGraphBuilder<TVertex> WithVertexFormatter(
            IStringFormatter<TVertex> formatter)
        {
            this.vertexFormatter = formatter ?? throw new ArgumentNullException(nameof(formatter));
            return this;
        }

        public ExplicitGraphBuilder<TVertex> WithVertices(IEnumerable<TVertex> vertices)
        {
            this.vertices = vertices ?? throw new ArgumentNullException(nameof(vertices));
            return this;
        }

        public ExplicitGraphBuilder<TVertex> WithEdges(IEnumerable<Edge<TVertex>> edges)
        {
            this.edges = edges ?? throw new ArgumentNullException(nameof(edges));
            return this;
        }

        public IExplicitGraph<TVertex> Build()
        {
            if (vertices == null) throw new ArgumentNullException(nameof(vertices));
            if (edges == null) throw new ArgumentNullException(nameof(edges));

            return new BuiltExplicitGraph<TVertex>(
                vertexEqualityComparer,
                vertexComparer,
                vertexFormatter,
                vertices,
                edges);
        }
    }

    private class BuiltExplicitGraph<TVertex, TEdgeLabel> :
        IExplicitGraph<TVertex, TEdgeLabel>
        where TVertex : notnull
        where TEdgeLabel : notnull
    {
        internal BuiltExplicitGraph(
            IEqualityComparer<TVertex> vertexEqualityComparer,
            IComparer<TVertex> vertexComparer,
            IStringFormatter<TVertex> vertexFormatter,
            IStringFormatter<TEdgeLabel> edgeLabelFormatter,
            IEnumerable<TVertex> vertices,
            IEnumerable<Edge<TVertex, TEdgeLabel>> labeledEdges)
        {
            VertexEqualityComparer = vertexEqualityComparer;
            VertexComparer = vertexComparer;
            VertexFormatter = vertexFormatter;
            EdgeLabelFormatter = edgeLabelFormatter;
            Vertices = vertices;
            LabeledEdges = labeledEdges;
        }

        public IEnumerable<TVertex> Vertices { get; }

        public IEnumerable<Edge<TVertex>> Edges => LabeledEdges.Select(e => e.ToUnlabeledEdge());

        public IEqualityComparer<TVertex> VertexEqualityComparer { get; }

        public IComparer<TVertex> VertexComparer { get; }

        public IStringFormatter<TVertex> VertexFormatter { get; }

        public IEnumerable<Edge<TVertex, TEdgeLabel>> LabeledEdges { get; }

        public IStringFormatter<TEdgeLabel> EdgeLabelFormatter { get; }
    }

    public class ExplicitGraphBuilder<TVertex, TEdgeLabel>
        where TVertex : notnull
        where TEdgeLabel : notnull
    {
        private IEqualityComparer<TVertex> vertexEqualityComparer;
        private IComparer<TVertex> vertexComparer;
        private IStringFormatter<TVertex> vertexFormatter;
        private IStringFormatter<TEdgeLabel> edgeLabelFormatter;
        private IEnumerable<TVertex>? vertices;
        private IEnumerable<Edge<TVertex, TEdgeLabel>>? labeledEdges;

        internal ExplicitGraphBuilder()
        {
            vertexEqualityComparer = EqualityComparer<TVertex>.Default;
            vertexComparer = Comparer<TVertex>.Default;
            vertexFormatter = StringFormatter<TVertex>.Default;
            edgeLabelFormatter = StringFormatter<TEdgeLabel>.Default;
        }

        internal ExplicitGraphBuilder(IExplicitGraph<TVertex, TEdgeLabel> fromDefinition)
        {
            if (fromDefinition == null) throw new ArgumentNullException(nameof(fromDefinition));

            vertexEqualityComparer = fromDefinition.VertexEqualityComparer;
            vertexComparer = fromDefinition.VertexComparer;
            vertexFormatter = fromDefinition.VertexFormatter;
            edgeLabelFormatter = fromDefinition.EdgeLabelFormatter;
            vertices = fromDefinition.Vertices;
            labeledEdges = fromDefinition.LabeledEdges;
        }

        public ExplicitGraphBuilder<TVertex, TEdgeLabel> WithVertexEqualityComparer(
            IEqualityComparer<TVertex> comparer)
        {
            this.vertexEqualityComparer = comparer ?? throw new ArgumentNullException(nameof(comparer));
            return this;
        }

        public ExplicitGraphBuilder<TVertex, TEdgeLabel> WithVertexComparer(
            IComparer<TVertex> comparer)
        {
            this.vertexComparer = comparer ?? throw new ArgumentNullException(nameof(comparer));
            return this;
        }

        public ExplicitGraphBuilder<TVertex, TEdgeLabel> WithVertexFormatter(
            IStringFormatter<TVertex> formatter)
        {
            this.vertexFormatter = formatter ?? throw new ArgumentNullException(nameof(formatter));
            return this;
        }

        public ExplicitGraphBuilder<TVertex, TEdgeLabel> WithEdgeLabelFormatter(
            IStringFormatter<TEdgeLabel> formatter)
        {
            this.edgeLabelFormatter = formatter ?? throw new ArgumentNullException(nameof(formatter));
            return this;
        }

        public ExplicitGraphBuilder<TVertex, TEdgeLabel> WithVertices(IEnumerable<TVertex> vertices)
        {
            this.vertices = vertices ?? throw new ArgumentNullException(nameof(vertices));
            return this;
        }

        public ExplicitGraphBuilder<TVertex, TEdgeLabel> WithLabeledEdges(
            IEnumerable<Edge<TVertex, TEdgeLabel>> edges)
        {
            this.labeledEdges = edges ?? throw new ArgumentNullException(nameof(edges));
            return this;
        }

        public IExplicitGraph<TVertex, TEdgeLabel> Build()
        {
            if (vertices == null) throw new ArgumentNullException(nameof(vertices));
            if (labeledEdges == null) throw new ArgumentNullException(nameof(labeledEdges));

            return new BuiltExplicitGraph<TVertex, TEdgeLabel>(
                vertexEqualityComparer,
                vertexComparer,
                vertexFormatter,
                edgeLabelFormatter,
                vertices,
                labeledEdges);
        }
    }
}