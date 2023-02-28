namespace GraphFx;

public static class DirectedGraph
{
    public static GraphBuilder<TVertex, TEdgeLabel> Builder<TVertex, TEdgeLabel>()
        where TVertex : notnull
        where TEdgeLabel : notnull
    {
        return new GraphBuilder<TVertex, TEdgeLabel>();
    }

    public class GraphBuilder<TVertex, TEdgeLabel>
        where TVertex : notnull
        where TEdgeLabel : notnull
    {
        private readonly List<VertexOrLabeledEdge<TVertex, TEdgeLabel>> data = new();

        private IEqualityComparer<TVertex>? vertexComparer;
        private IGraphFormatter<TVertex, TEdgeLabel>? formatter;


        internal GraphBuilder()
        {
        }

        public GraphBuilder<TVertex, TEdgeLabel> AddVertex(TVertex vertex)
        {
            if (vertex == null) throw new ArgumentNullException(nameof(vertex));
            data.Add(vertex);
            return this;
        }

        public GraphBuilder<TVertex, TEdgeLabel> AddVertices(IEnumerable<TVertex> vertices)
        {
            if (vertices == null) throw new ArgumentNullException(nameof(vertices));
            foreach (var vertex in vertices)
            {
                AddVertex(vertex);
            }

            return this;
        }

        public GraphBuilder<TVertex, TEdgeLabel> AddVertices(params TVertex[] vertices)
        {
            if (vertices == null) throw new ArgumentNullException(nameof(vertices));
            return AddVertices((IEnumerable<TVertex>) vertices);
        }

        public GraphBuilder<TVertex, TEdgeLabel> AddEdge(LabeledEdge<TVertex, TEdgeLabel> labeledEdge)
        {
            data.Add(labeledEdge);
            return this;
        }

        public GraphBuilder<TVertex, TEdgeLabel> AddEdge(TVertex source, TEdgeLabel edge, TVertex target)
        {
            return AddEdge(Edge.CreateLabeled(source, edge, target));
        }

        public GraphBuilder<TVertex, TEdgeLabel> AddEdges(IEnumerable<LabeledEdge<TVertex, TEdgeLabel>> edges)
        {
            if (edges == null) throw new ArgumentNullException(nameof(edges));
            foreach (var edge in edges)
            {
                AddEdge(edge);
            }

            return this;
        }

        public GraphBuilder<TVertex, TEdgeLabel> AddEdges(params LabeledEdge<TVertex, TEdgeLabel>[] edges)
        {
            if (edges == null) throw new ArgumentNullException(nameof(edges));
            return AddEdges((IEnumerable<LabeledEdge<TVertex, TEdgeLabel>>) edges);
        }

        public GraphBuilder<TVertex, TEdgeLabel> WithVertexComparer(IEqualityComparer<TVertex> comparer)
        {
            vertexComparer = comparer;
            return this;
        }

        public GraphBuilder<TVertex, TEdgeLabel> WithFormatter(IGraphFormatter<TVertex, TEdgeLabel> formatter)
        {
            this.formatter = formatter;
            return this;
        }

        public GraphBuilder<TVertex, TEdgeLabel> WithFormatter(
            IStringFormatter<TVertex> vertexFormatter,
            IStringFormatter<TEdgeLabel> edgeFormatter)
        {
            return this.WithFormatter(GraphFormatter.Create(vertexFormatter, edgeFormatter));
        }

        public GraphBuilder<TVertex, TEdgeLabel> WithFormatter(
            Func<TVertex, string> vertexFormatter,
            Func<TEdgeLabel, string> edgeFormatter)
        {
            return this.WithFormatter(GraphFormatter.Create(vertexFormatter, edgeFormatter));
        }

        public GraphBuilder<TVertex, TEdgeLabel> WithVertexFormatter(
            IStringFormatter<TVertex> vertexFormatter)
        {
            this.formatter ??= GraphFormatter<TVertex, TEdgeLabel>.Default;
            this.formatter = this.formatter.WithVertexFormatter(vertexFormatter);
            return this;
        }

        public GraphBuilder<TVertex, TEdgeLabel> WithVertexFormatter(
            Func<TVertex, string> vertexFormatter)
        {
            return this.WithVertexFormatter(StringFormatter.Create(vertexFormatter));
        }

        public GraphBuilder<TVertex, TEdgeLabel> WithEdgeFormatter(
            IStringFormatter<TEdgeLabel> edgeFormatter)
        {
            this.formatter ??= GraphFormatter<TVertex, TEdgeLabel>.Default;
            this.formatter = this.formatter.WithEdgeFormatter(edgeFormatter);
            return this;
        }

        public GraphBuilder<TVertex, TEdgeLabel> WithEdgeFormatter(
            Func<TEdgeLabel, string> edgeFormatter)
        {
            return this.WithEdgeFormatter(StringFormatter.Create(edgeFormatter));
        }

        public IDirectedGraph<TVertex, TEdgeLabel> Build()
        {
            var vertexComp = vertexComparer ?? EqualityComparer<TVertex>.Default;
            var fmt = formatter ?? GraphFormatter<TVertex, TEdgeLabel>.Default;

            var vertexList = new List<TVertex>();
            var edgeList = new List<LabeledEdge<TVertex, TEdgeLabel>>();
            var vertexSet = new HashSet<TVertex>(vertexComp);

            foreach (var item in data)
            {
                item.Switch(
                    vertex =>
                    {
                        if (vertexSet.Add(vertex))
                        {
                            vertexList.Add(vertex);
                        }
                    },
                    edge =>
                    {
                        if (vertexSet.Add(edge.Source))
                        {
                            vertexList.Add(edge.Source);
                        }

                        if (vertexSet.Add(edge.Target))
                        {
                            vertexList.Add(edge.Target);
                        }

                        edgeList.Add(edge);
                    });
            }

            return new BuiltGraph<TVertex, TEdgeLabel>(vertexList, edgeList, vertexComp, fmt);
        }
    }

    private class BuiltGraph<TVertex, TEdgeLabel> :
        IDirectedGraph<TVertex, TEdgeLabel>
        where TVertex : notnull
        where TEdgeLabel : notnull
    {
        internal BuiltGraph(
            IReadOnlyList<TVertex> verticesCollection,
            IReadOnlyList<LabeledEdge<TVertex, TEdgeLabel>> edgesCollection,
            IEqualityComparer<TVertex> vertexComparer,
            IGraphFormatter<TVertex, TEdgeLabel> formatter)
        {
            this.Vertices = verticesCollection ?? throw new ArgumentNullException(nameof(verticesCollection));
            this.Edges = edgesCollection ?? throw new ArgumentNullException(nameof(edgesCollection));
            this.VertexComparer = vertexComparer ?? throw new ArgumentNullException(nameof(vertexComparer));
            Formatter = formatter ?? throw new ArgumentNullException(nameof(formatter));
        }

        public IReadOnlyList<TVertex> Vertices { get; }

        public IReadOnlyList<LabeledEdge<TVertex, TEdgeLabel>> Edges { get; }

        public IEqualityComparer<TVertex> VertexComparer { get; }

        public IGraphFormatter<TVertex, TEdgeLabel> Formatter { get; }
    }

    public static GraphBuilder<TVertex> Builder<TVertex>()
        where TVertex : notnull
    {
        return new GraphBuilder<TVertex>();
    }

    public class GraphBuilder<TVertex>
        where TVertex : notnull
    {
        private readonly List<VertexOrEdge<TVertex>> data = new();
        private IEqualityComparer<TVertex>? vertexComparer;
        private IGraphFormatter<TVertex>? formatter;

        internal GraphBuilder()
        {
        }

        public GraphBuilder<TVertex> AddVertex(TVertex vertex)
        {
            if (vertex == null) throw new ArgumentNullException(nameof(vertex));
            data.Add(vertex);
            return this;
        }

        public GraphBuilder<TVertex> AddVertices(IEnumerable<TVertex> vertices)
        {
            if (vertices == null) throw new ArgumentNullException(nameof(vertices));
            foreach (var vertex in vertices)
            {
                this.AddVertex(vertex);
            }
            return this;
        }

        public GraphBuilder<TVertex> AddVertices(params TVertex[] vertices)
        {
            if (vertices == null) throw new ArgumentNullException(nameof(vertices));
            return this.AddVertices((IEnumerable<TVertex>)vertices);
        }

        public GraphBuilder<TVertex> AddEdge(Edge<TVertex> edge)
        {
            data.Add(edge);
            return this;
        }

        public GraphBuilder<TVertex> AddEdge(TVertex source, TVertex target)
        {
            return this.AddEdge(Edge.Create(source, target));
        }

        public GraphBuilder<TVertex> AddEdges(IEnumerable<Edge<TVertex>> edges)
        {
            if (edges == null) throw new ArgumentNullException(nameof(edges));
            foreach (var edge in edges)
            {
                this.AddEdge(edge);
            }
            return this;
        }

        public GraphBuilder<TVertex> AddEdges(params Edge<TVertex>[] edges)
        {
            if (edges == null) throw new ArgumentNullException(nameof(edges));
            return this.AddEdges((IEnumerable<Edge<TVertex>>)edges);
        }

        public GraphBuilder<TVertex> WithVertexComparer(IEqualityComparer<TVertex>? comparer)
        {
            vertexComparer = comparer;
            return this;
        }

        public GraphBuilder<TVertex> WithFormatter(IGraphFormatter<TVertex>? formatter)
        {
            this.formatter = formatter;
            return this;
        }

        public GraphBuilder<TVertex> WithFormatter(
            IStringFormatter<TVertex> vertexFormatter)
        {
            return this.WithFormatter(GraphFormatter.Create(vertexFormatter));
        }

        public GraphBuilder<TVertex> WithFormatter(
            Func<TVertex, string> vertexFormatter)
        {
            return this.WithFormatter(GraphFormatter.Create(vertexFormatter));
        }

        public IDirectedGraph<TVertex> Build()
        {
            var vertexComp = vertexComparer ?? EqualityComparer<TVertex>.Default;
            var fmt = formatter ?? GraphFormatter<TVertex>.Default;

            var vertexList = new List<TVertex>();
            var edgeList = new List<Edge<TVertex>>();
            var vertexSet = new HashSet<TVertex>(vertexComp);

            foreach (var item in data)
            {
                item.Switch(
                    vertex =>
                    {
                        if (vertexSet.Add(vertex))
                        {
                            vertexList.Add(vertex);
                        }
                    },
                    edge =>
                    {
                        if (vertexSet.Add(edge.Source))
                        {
                            vertexList.Add(edge.Source);
                        }

                        if (vertexSet.Add(edge.Target))
                        {
                            vertexList.Add(edge.Target);
                        }

                        edgeList.Add(edge);
                    });
            }

            return new BuiltGraph<TVertex>(vertexList, edgeList, vertexComp, fmt);
        }
    }

    private class BuiltGraph<TVertex> :
        IDirectedGraph<TVertex>
        where TVertex : notnull
    {
        internal BuiltGraph(
            IReadOnlyList<TVertex> verticesCollection,
            IReadOnlyList<Edge<TVertex>> edgesCollection,
            IEqualityComparer<TVertex> vertexComparer,
            IGraphFormatter<TVertex> formatter)
        {
            this.Vertices = verticesCollection ?? throw new ArgumentNullException(nameof(verticesCollection));
            this.Edges = edgesCollection ?? throw new ArgumentNullException(nameof(edgesCollection));
            this.VertexComparer = vertexComparer ?? throw new ArgumentNullException(nameof(vertexComparer));
            Formatter = formatter ?? throw new ArgumentNullException(nameof(formatter));
        }

        public IReadOnlyList<TVertex> Vertices { get; }

        public IReadOnlyList<Edge<TVertex>> Edges { get; }

        public IEqualityComparer<TVertex> VertexComparer { get; }
        
        public IGraphFormatter<TVertex> Formatter { get; }
    }
}
