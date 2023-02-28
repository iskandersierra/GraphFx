namespace GraphFx;

public static class EnumerableGraph
{
    public static EnumerableGraphBuilder<TVertex> Create<TVertex>(
        IEnumerableGraph<TVertex> fromDefinition)
        where TVertex : notnull
    {
        return new EnumerableGraphBuilder<TVertex>(fromDefinition);
    }

    public static EnumerableGraphBuilder<TVertex> Create<TVertex>()
        where TVertex : notnull
    {
        return new EnumerableGraphBuilder<TVertex>();
    }

    public static EnumerableGraphBuilder<TVertex, TEdgeLabel> Create<TVertex, TEdgeLabel>(
        IEnumerableGraph<TVertex, TEdgeLabel> fromDefinition)
        where TVertex : notnull
        where TEdgeLabel : notnull
    {
        return new EnumerableGraphBuilder<TVertex, TEdgeLabel>(fromDefinition);
    }

    public static EnumerableGraphBuilder<TVertex, TEdgeLabel> Create<TVertex, TEdgeLabel>()
        where TVertex : notnull
        where TEdgeLabel : notnull
    {
        return new EnumerableGraphBuilder<TVertex, TEdgeLabel>();
    }

    private class BuiltEnumerableGraph<TVertex> :
        IEnumerableGraph<TVertex>
        where TVertex : notnull
    {
        internal BuiltEnumerableGraph(
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

    public class EnumerableGraphBuilder<TVertex>
        where TVertex : notnull
    {
        private IEqualityComparer<TVertex> vertexEqualityComparer;
        private IComparer<TVertex> vertexComparer;
        private IStringFormatter<TVertex> vertexFormatter;
        private IEnumerable<TVertex>? vertices;
        private IEnumerable<Edge<TVertex>>? edges;

        internal EnumerableGraphBuilder()
        {
            vertexEqualityComparer = EqualityComparer<TVertex>.Default;
            vertexComparer = Comparer<TVertex>.Default;
            vertexFormatter = StringFormatter<TVertex>.Default;
        }

        internal EnumerableGraphBuilder(IEnumerableGraph<TVertex> fromDefinition)
        {
            if (fromDefinition == null) throw new ArgumentNullException(nameof(fromDefinition));

            vertexEqualityComparer = fromDefinition.VertexEqualityComparer;
            vertexComparer = fromDefinition.VertexComparer;
            vertexFormatter = fromDefinition.VertexFormatter;
            vertices = fromDefinition.Vertices;
            edges = fromDefinition.Edges;
        }

        public EnumerableGraphBuilder<TVertex> WithVertexEqualityComparer(
            IEqualityComparer<TVertex> comparer)
        {
            this.vertexEqualityComparer = comparer ?? throw new ArgumentNullException(nameof(comparer));
            return this;
        }

        public EnumerableGraphBuilder<TVertex> WithVertexComparer(
            IComparer<TVertex> comparer)
        {
            this.vertexComparer = comparer ?? throw new ArgumentNullException(nameof(comparer));
            return this;
        }

        public EnumerableGraphBuilder<TVertex> WithVertexFormatter(
            IStringFormatter<TVertex> formatter)
        {
            this.vertexFormatter = formatter ?? throw new ArgumentNullException(nameof(formatter));
            return this;
        }

        public EnumerableGraphBuilder<TVertex> WithVertices(IEnumerable<TVertex> vertices)
        {
            this.vertices = vertices ?? throw new ArgumentNullException(nameof(vertices));
            return this;
        }

        public EnumerableGraphBuilder<TVertex> WithEdges(IEnumerable<Edge<TVertex>> edges)
        {
            this.edges = edges ?? throw new ArgumentNullException(nameof(edges));
            return this;
        }

        public IEnumerableGraph<TVertex> Build()
        {
            if (vertices == null) throw new ArgumentNullException(nameof(vertices));
            if (edges == null) throw new ArgumentNullException(nameof(edges));

            return new BuiltEnumerableGraph<TVertex>(
                vertexEqualityComparer,
                vertexComparer,
                vertexFormatter,
                vertices,
                edges);
        }
    }

    private class BuiltEnumerableGraph<TVertex, TEdgeLabel> :
        IEnumerableGraph<TVertex, TEdgeLabel>
        where TVertex : notnull
        where TEdgeLabel : notnull
    {
        internal BuiltEnumerableGraph(
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

    public class EnumerableGraphBuilder<TVertex, TEdgeLabel>
        where TVertex : notnull
        where TEdgeLabel : notnull
    {
        private IEqualityComparer<TVertex> vertexEqualityComparer;
        private IComparer<TVertex> vertexComparer;
        private IStringFormatter<TVertex> vertexFormatter;
        private IStringFormatter<TEdgeLabel> edgeLabelFormatter;
        private IEnumerable<TVertex>? vertices;
        private IEnumerable<Edge<TVertex, TEdgeLabel>>? labeledEdges;

        internal EnumerableGraphBuilder()
        {
            vertexEqualityComparer = EqualityComparer<TVertex>.Default;
            vertexComparer = Comparer<TVertex>.Default;
            vertexFormatter = StringFormatter<TVertex>.Default;
            edgeLabelFormatter = StringFormatter<TEdgeLabel>.Default;
        }

        internal EnumerableGraphBuilder(IEnumerableGraph<TVertex, TEdgeLabel> fromDefinition)
        {
            if (fromDefinition == null) throw new ArgumentNullException(nameof(fromDefinition));

            vertexEqualityComparer = fromDefinition.VertexEqualityComparer;
            vertexComparer = fromDefinition.VertexComparer;
            vertexFormatter = fromDefinition.VertexFormatter;
            edgeLabelFormatter = fromDefinition.EdgeLabelFormatter;
            vertices = fromDefinition.Vertices;
            labeledEdges = fromDefinition.LabeledEdges;
        }

        public EnumerableGraphBuilder<TVertex, TEdgeLabel> WithVertexEqualityComparer(
            IEqualityComparer<TVertex> comparer)
        {
            this.vertexEqualityComparer = comparer ?? throw new ArgumentNullException(nameof(comparer));
            return this;
        }

        public EnumerableGraphBuilder<TVertex, TEdgeLabel> WithVertexComparer(
            IComparer<TVertex> comparer)
        {
            this.vertexComparer = comparer ?? throw new ArgumentNullException(nameof(comparer));
            return this;
        }

        public EnumerableGraphBuilder<TVertex, TEdgeLabel> WithVertexFormatter(
            IStringFormatter<TVertex> formatter)
        {
            this.vertexFormatter = formatter ?? throw new ArgumentNullException(nameof(formatter));
            return this;
        }

        public EnumerableGraphBuilder<TVertex, TEdgeLabel> WithEdgeLabelFormatter(
            IStringFormatter<TEdgeLabel> formatter)
        {
            this.edgeLabelFormatter = formatter ?? throw new ArgumentNullException(nameof(formatter));
            return this;
        }

        public EnumerableGraphBuilder<TVertex, TEdgeLabel> WithVertices(IEnumerable<TVertex> vertices)
        {
            this.vertices = vertices ?? throw new ArgumentNullException(nameof(vertices));
            return this;
        }

        public EnumerableGraphBuilder<TVertex, TEdgeLabel> WithLabeledEdges(
            IEnumerable<Edge<TVertex, TEdgeLabel>> edges)
        {
            this.labeledEdges = edges ?? throw new ArgumentNullException(nameof(edges));
            return this;
        }

        public IEnumerableGraph<TVertex, TEdgeLabel> Build()
        {
            if (vertices == null) throw new ArgumentNullException(nameof(vertices));
            if (labeledEdges == null) throw new ArgumentNullException(nameof(labeledEdges));

            return new BuiltEnumerableGraph<TVertex, TEdgeLabel>(
                vertexEqualityComparer,
                vertexComparer,
                vertexFormatter,
                edgeLabelFormatter,
                vertices,
                labeledEdges);
        }
    }
}