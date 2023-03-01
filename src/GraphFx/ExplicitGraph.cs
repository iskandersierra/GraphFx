namespace GraphFx;

public static class ExplicitGraph
{
    public static ExplicitGraphBuilder<TVertex> Create<TVertex>(
        IGraph<TVertex> fromDefinition)
        where TVertex : notnull
    {
        return new ExplicitGraphBuilder<TVertex>(fromDefinition);
    }

    public static ExplicitGraphBuilder<TVertex> Create<TVertex>(
        IExplicitGraph<TVertex> fromDefinition)
        where TVertex : notnull
    {
        return new ExplicitGraphBuilder<TVertex>(fromDefinition);
    }

    public static ExplicitGraphBuilder<TVertex> Create<TVertex>(
        IEqualityComparer<TVertex>? vertexEqualityComparer = null,
        IComparer<TVertex>? vertexComparer = null,
        IStringFormatter<TVertex>? vertexFormatter = null)
        where TVertex : notnull
    {
        return new ExplicitGraphBuilder<TVertex>(
            vertexEqualityComparer,
            vertexComparer,
            vertexFormatter);
    }

    public static ExplicitGraphBuilder<TVertex, TEdgeLabel> Create<TVertex, TEdgeLabel>(
        IGraph<TVertex, TEdgeLabel> fromDefinition)
        where TVertex : notnull
        where TEdgeLabel : notnull
    {
        return new ExplicitGraphBuilder<TVertex, TEdgeLabel>(fromDefinition);
    }

    public static ExplicitGraphBuilder<TVertex, TEdgeLabel> Create<TVertex, TEdgeLabel>(
        IExplicitGraph<TVertex, TEdgeLabel> fromDefinition)
        where TVertex : notnull
        where TEdgeLabel : notnull
    {
        return new ExplicitGraphBuilder<TVertex, TEdgeLabel>(fromDefinition);
    }

    public static ExplicitGraphBuilder<TVertex, TEdgeLabel> Create<TVertex, TEdgeLabel>(
        IEqualityComparer<TVertex>? vertexEqualityComparer = null,
        IComparer<TVertex>? vertexComparer = null,
        IStringFormatter<TVertex>? vertexFormatter = null,
        IStringFormatter<TEdgeLabel>? edgeLabelFormatter = null)
        where TVertex : notnull
        where TEdgeLabel : notnull
    {
        return new ExplicitGraphBuilder<TVertex, TEdgeLabel>(
            vertexEqualityComparer,
            vertexComparer,
            vertexFormatter,
            edgeLabelFormatter);
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

        internal ExplicitGraphBuilder(
            IEqualityComparer<TVertex>? vertexEqualityComparer,
            IComparer<TVertex>? vertexComparer,
            IStringFormatter<TVertex>? vertexFormatter)
        {
            this.vertexEqualityComparer = vertexEqualityComparer ?? EqualityComparer<TVertex>.Default;
            this.vertexComparer = vertexComparer ?? Comparer<TVertex>.Default;
            this.vertexFormatter = vertexFormatter ?? StringFormatter<TVertex>.Default;
        }

        internal ExplicitGraphBuilder(IGraph<TVertex> fromDefinition)
        {
            if (fromDefinition == null) throw new ArgumentNullException(nameof(fromDefinition));

            vertexEqualityComparer = fromDefinition.VertexEqualityComparer;
            vertexComparer = fromDefinition.VertexComparer;
            vertexFormatter = fromDefinition.VertexFormatter;
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
            IEqualityComparer<TVertex>? comparer)
        {
            this.vertexEqualityComparer = comparer ?? EqualityComparer<TVertex>.Default;
            return this;
        }

        public ExplicitGraphBuilder<TVertex> WithVertexComparer(
            IComparer<TVertex>? comparer)
        {
            this.vertexComparer = comparer ?? Comparer<TVertex>.Default;
            return this;
        }

        public ExplicitGraphBuilder<TVertex> WithVertexFormatter(
            IStringFormatter<TVertex>? formatter)
        {
            this.vertexFormatter = formatter ?? StringFormatter<TVertex>.Default;
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

        internal ExplicitGraphBuilder(
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

        internal ExplicitGraphBuilder(IGraph<TVertex, TEdgeLabel> fromDefinition)
        {
            if (fromDefinition == null) throw new ArgumentNullException(nameof(fromDefinition));

            vertexEqualityComparer = fromDefinition.VertexEqualityComparer;
            vertexComparer = fromDefinition.VertexComparer;
            vertexFormatter = fromDefinition.VertexFormatter;
            edgeLabelFormatter = fromDefinition.EdgeLabelFormatter;
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
            IEqualityComparer<TVertex>? comparer)
        {
            this.vertexEqualityComparer = comparer ?? EqualityComparer<TVertex>.Default;
            return this;
        }

        public ExplicitGraphBuilder<TVertex, TEdgeLabel> WithVertexComparer(
            IComparer<TVertex>? comparer)
        {
            this.vertexComparer = comparer ?? Comparer<TVertex>.Default;
            return this;
        }

        public ExplicitGraphBuilder<TVertex, TEdgeLabel> WithVertexFormatter(
            IStringFormatter<TVertex>? formatter)
        {
            this.vertexFormatter = formatter ?? StringFormatter<TVertex>.Default;
            return this;
        }

        public ExplicitGraphBuilder<TVertex, TEdgeLabel> WithEdgeLabelFormatter(
            IStringFormatter<TEdgeLabel>? formatter)
        {
            this.edgeLabelFormatter = formatter ?? StringFormatter<TEdgeLabel>.Default;
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

    public static IIncidenceGraph<TVertex, TEdgeLabel> ToIncidenceGraph<TVertex, TEdgeLabel>(
        this IExplicitGraph<TVertex, TEdgeLabel> graph)
        where TVertex : notnull
        where TEdgeLabel : notnull
    {
        return graph.Vertices
            .ToDictionary(
                v => v,
                v => graph.LabeledEdges
                    .Where(e => graph.VertexEqualityComparer.Equals(e.Source, v))
                    .Select(e => new OutgoingEdge<TVertex, TEdgeLabel>(e.Label, e.Target))
                    .ToArray() as IReadOnlyCollection<OutgoingEdge<TVertex, TEdgeLabel>>)
            .ToIncidenceGraph(graph);
    }
}

public class ExplicitGraph<TVertex> :
    IExplicitGraph<TVertex>
    where TVertex : notnull
{
    private readonly HashSet<TVertex> vertices;
    private readonly List<Edge<TVertex>> edges;

    public ExplicitGraph(
        IEqualityComparer<TVertex>? vertexEqualityComparer = null,
        IComparer<TVertex>? vertexComparer = null,
        IStringFormatter<TVertex>? vertexFormatter = null)
    {
        VertexEqualityComparer = vertexEqualityComparer ?? EqualityComparer<TVertex>.Default;
        VertexComparer = vertexComparer ?? Comparer<TVertex>.Default;
        VertexFormatter = vertexFormatter ?? StringFormatter<TVertex>.Default;
        vertices = new HashSet<TVertex>(VertexEqualityComparer);
        edges = new List<Edge<TVertex>>();
    }

    public IEnumerable<TVertex> Vertices => vertices;

    public IEnumerable<Edge<TVertex>> Edges => edges;

    public IEqualityComparer<TVertex> VertexEqualityComparer { get; }

    public IComparer<TVertex> VertexComparer { get; }

    public IStringFormatter<TVertex> VertexFormatter { get; }

    public void Add(TVertex vertex)
    {
        if (vertex == null) throw new ArgumentNullException(nameof(vertex));

        vertices.Add(vertex);
    }

    public void Add(Edge<TVertex> edge)
    {
        vertices.Add(edge.Source);
        vertices.Add(edge.Target);
        edges.Add(edge);
    }

    public void Add(TVertex source, TVertex target)
    {
        Add(new Edge<TVertex>(source, target));
    }

    public void AddRange(IEnumerable<TVertex> vertices)
    {
        if (vertices == null) throw new ArgumentNullException(nameof(vertices));

        foreach (var vertex in vertices)
        {
            Add(vertex);
        }
    }

    public void AddRange(IEnumerable<Edge<TVertex>> edges)
    {
        if (edges == null) throw new ArgumentNullException(nameof(edges));

        foreach (var edge in edges)
        {
            Add(edge);
        }
    }

    public void AddRange(params TVertex[] vertices)
    {
        if (vertices == null) throw new ArgumentNullException(nameof(vertices));

        AddRange((IEnumerable<TVertex>)vertices);
    }

    public void AddRange(params Edge<TVertex>[] edges)
    {
        if (edges == null) throw new ArgumentNullException(nameof(edges));

        AddRange((IEnumerable<Edge<TVertex>>)edges);
    }
}

public class ExplicitGraph<TVertex, TEdgeLabel> :
    IExplicitGraph<TVertex, TEdgeLabel>
    where TVertex : notnull
    where TEdgeLabel : notnull
{
    private readonly HashSet<TVertex> vertices;
    private readonly List<Edge<TVertex, TEdgeLabel>> edges;

    public ExplicitGraph(
        IEqualityComparer<TVertex>? vertexEqualityComparer = null,
        IComparer<TVertex>? vertexComparer = null,
        IStringFormatter<TVertex>? vertexFormatter = null,
        IStringFormatter<TEdgeLabel>? edgeLabelFormatter = null)
    {
        VertexEqualityComparer = vertexEqualityComparer ?? EqualityComparer<TVertex>.Default;
        VertexComparer = vertexComparer ?? Comparer<TVertex>.Default;
        VertexFormatter = vertexFormatter ?? StringFormatter<TVertex>.Default;
        EdgeLabelFormatter = edgeLabelFormatter ?? StringFormatter<TEdgeLabel>.Default;
        vertices = new HashSet<TVertex>(VertexEqualityComparer);
        edges = new List<Edge<TVertex, TEdgeLabel>>();
    }

    public IEnumerable<TVertex> Vertices => vertices;

    public IEnumerable<Edge<TVertex, TEdgeLabel>> LabeledEdges => edges;

    public IEnumerable<Edge<TVertex>> Edges => edges.Select(edge => edge.ToUnlabeledEdge());

    public IEqualityComparer<TVertex> VertexEqualityComparer { get; }

    public IComparer<TVertex> VertexComparer { get; }

    public IStringFormatter<TVertex> VertexFormatter { get; }

    public IStringFormatter<TEdgeLabel> EdgeLabelFormatter { get; }

    public void Add(TVertex vertex)
    {
        if (vertex == null) throw new ArgumentNullException(nameof(vertex));

        vertices.Add(vertex);
    }

    public void Add(Edge<TVertex, TEdgeLabel> edge)
    {
        vertices.Add(edge.Source);
        vertices.Add(edge.Target);
        edges.Add(edge);
    }

    public void Add(TVertex source, TEdgeLabel label, TVertex target)
    {
        Add(new Edge<TVertex, TEdgeLabel>(source, label, target));
    }

    public void AddRange(IEnumerable<TVertex> vertices)
    {
        if (vertices == null) throw new ArgumentNullException(nameof(vertices));

        foreach (var vertex in vertices)
        {
            Add(vertex);
        }
    }

    public void AddRange(IEnumerable<Edge<TVertex, TEdgeLabel>> edges)
    {
        if (edges == null) throw new ArgumentNullException(nameof(edges));

        foreach (var edge in edges)
        {
            Add(edge);
        }
    }

    public void AddRange(params TVertex[] vertices)
    {
        if (vertices == null) throw new ArgumentNullException(nameof(vertices));

        AddRange((IEnumerable<TVertex>)vertices);
    }

    public void AddRange(params Edge<TVertex, TEdgeLabel>[] edges)
    {
        if (edges == null) throw new ArgumentNullException(nameof(edges));

        AddRange((IEnumerable<Edge<TVertex, TEdgeLabel>>)edges);
    }
}
