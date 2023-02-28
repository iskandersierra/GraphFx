using System.Diagnostics.CodeAnalysis;

namespace GraphFx;

public static class ImplicitGraph
{
    public static ImplicitGraphBuilder<TVertex> Create<TVertex>(
        IImplicitGraph<TVertex> fromDefinition)
        where TVertex : notnull
    {
        return new ImplicitGraphBuilder<TVertex>(fromDefinition);
    }

    public static ImplicitGraphBuilder<TVertex> Create<TVertex>()
        where TVertex : notnull
    {
        return new ImplicitGraphBuilder<TVertex>();
    }

    public static ImplicitGraphBuilder<TVertex, TEdgeLabel> Create<TVertex, TEdgeLabel>(
        IImplicitGraph<TVertex, TEdgeLabel> fromDefinition)
        where TVertex : notnull
        where TEdgeLabel : notnull
    {
        return new ImplicitGraphBuilder<TVertex, TEdgeLabel>(fromDefinition);
    }

    public static ImplicitGraphBuilder<TVertex, TEdgeLabel> Create<TVertex, TEdgeLabel>()
        where TVertex : notnull
        where TEdgeLabel : notnull
    {
        return new ImplicitGraphBuilder<TVertex, TEdgeLabel>();
    }

    public delegate bool ContainsVertex<in TVertex>(TVertex vertex) where TVertex : notnull;

    public delegate bool ContainsEdge<in TVertex>(
        TVertex sourceVertex, TVertex targetVertex)
        where TVertex : notnull;

    public delegate bool TryGetEdgeLabel<in TVertex, TEdgeLabel>(
        TVertex sourceVertex, TVertex targetVertex,
        [NotNullWhen(true)] out TEdgeLabel edgeLabel)
        where TVertex : notnull
        where TEdgeLabel : notnull;

    private class BuiltImplicitGraph<TVertex> :
        IImplicitGraph<TVertex>
        where TVertex : notnull
    {
        private readonly ContainsVertex<TVertex> containsVertex;
        private readonly ContainsEdge<TVertex> containsEdge;

        internal BuiltImplicitGraph(
            IEqualityComparer<TVertex> vertexEqualityComparer,
            IComparer<TVertex> vertexComparer,
            IStringFormatter<TVertex> vertexFormatter,
            ContainsVertex<TVertex> containsVertex,
            ContainsEdge<TVertex> containsEdge)
        {
            this.VertexEqualityComparer = vertexEqualityComparer;
            this.VertexComparer = vertexComparer;
            this.VertexFormatter = vertexFormatter;
            this.containsVertex = containsVertex;
            this.containsEdge = containsEdge;
        }

        public IEqualityComparer<TVertex> VertexEqualityComparer { get; }
        public IComparer<TVertex> VertexComparer { get; }
        public IStringFormatter<TVertex> VertexFormatter { get; }
        public bool ContainsVertex(TVertex vertex) => containsVertex(vertex);

        public bool ContainsEdge(TVertex sourceVertex, TVertex targetVertex) =>
            containsEdge(sourceVertex, targetVertex);
    }

    public class ImplicitGraphBuilder<TVertex>
        where TVertex : notnull
    {
        private IEqualityComparer<TVertex> vertexEqualityComparer;
        private IComparer<TVertex> vertexComparer;
        private IStringFormatter<TVertex> vertexFormatter;
        private ContainsVertex<TVertex>? containsVertex;
        private ContainsEdge<TVertex>? containsEdge;

        internal ImplicitGraphBuilder()
        {
            vertexEqualityComparer = EqualityComparer<TVertex>.Default;
            vertexComparer = Comparer<TVertex>.Default;
            vertexFormatter = StringFormatter<TVertex>.Default;
        }

        internal ImplicitGraphBuilder(
            IImplicitGraph<TVertex> fromDefinition)
        {
            if (fromDefinition == null) throw new ArgumentNullException(nameof(fromDefinition));

            vertexEqualityComparer = fromDefinition.VertexEqualityComparer;
            vertexComparer = fromDefinition.VertexComparer;
            vertexFormatter = fromDefinition.VertexFormatter;
            containsVertex = fromDefinition.ContainsVertex;
            containsEdge = fromDefinition.ContainsEdge;
        }

        public ImplicitGraphBuilder<TVertex> WithVertexEqualityComparer(
            IEqualityComparer<TVertex> comparer)
        {
            this.vertexEqualityComparer = comparer ?? throw new ArgumentNullException(nameof(comparer));
            return this;
        }

        public ImplicitGraphBuilder<TVertex> WithVertexComparer(
            IComparer<TVertex> comparer)
        {
            this.vertexComparer = comparer ?? throw new ArgumentNullException(nameof(comparer));
            return this;
        }

        public ImplicitGraphBuilder<TVertex> WithVertexFormatter(
            IStringFormatter<TVertex> formatter)
        {
            this.vertexFormatter = formatter ?? throw new ArgumentNullException(nameof(formatter));
            return this;
        }

        public ImplicitGraphBuilder<TVertex> WithContainsVertex(
            ContainsVertex<TVertex> contains)
        {
            this.containsVertex = contains ?? throw new ArgumentNullException(nameof(contains));
            return this;
        }

        public ImplicitGraphBuilder<TVertex> WithContainsEdge(
            ContainsEdge<TVertex> contains)
        {
            this.containsEdge = contains ?? throw new ArgumentNullException(nameof(contains));
            return this;
        }

        public IImplicitGraph<TVertex> Build()
        {
            if (containsVertex == null) throw new ArgumentNullException(nameof(containsVertex));
            if (containsEdge == null) throw new ArgumentNullException(nameof(containsEdge));

            return new BuiltImplicitGraph<TVertex>(
                vertexEqualityComparer,
                vertexComparer,
                vertexFormatter,
                containsVertex,
                containsEdge);
        }
    }

    private class BuiltImplicitGraph<TVertex, TEdgeLabel> :
        IImplicitGraph<TVertex, TEdgeLabel>
        where TVertex : notnull
        where TEdgeLabel : notnull
    {
        private readonly ContainsVertex<TVertex> containsVertex;
        private readonly TryGetEdgeLabel<TVertex, TEdgeLabel> tryGetEdgeLabel;

        internal BuiltImplicitGraph(
            IEqualityComparer<TVertex> vertexEqualityComparer,
            IComparer<TVertex> vertexComparer,
            IStringFormatter<TVertex> vertexFormatter,
            IStringFormatter<TEdgeLabel> edgeLabelFormatter,
            ContainsVertex<TVertex> containsVertex,
            TryGetEdgeLabel<TVertex, TEdgeLabel> tryGetEdgeLabel)
        {
            VertexEqualityComparer = vertexEqualityComparer;
            VertexComparer = vertexComparer;
            VertexFormatter = vertexFormatter;
            EdgeLabelFormatter = edgeLabelFormatter;
            this.containsVertex = containsVertex;
            this.tryGetEdgeLabel = tryGetEdgeLabel;
        }

        public IEqualityComparer<TVertex> VertexEqualityComparer { get; }
        public IComparer<TVertex> VertexComparer { get; }
        public IStringFormatter<TVertex> VertexFormatter { get; }
        public IStringFormatter<TEdgeLabel> EdgeLabelFormatter { get; }
        public bool ContainsVertex(TVertex vertex) => containsVertex(vertex);

        public bool ContainsEdge(TVertex sourceVertex, TVertex targetVertex)
        {
            return tryGetEdgeLabel(sourceVertex, targetVertex, out _);
        }

        public bool TryGetEdgeLabel(TVertex sourceVertex, TVertex targetVertex,
            [NotNullWhen(true)] out TEdgeLabel edgeLabel) =>
            tryGetEdgeLabel(sourceVertex, targetVertex, out edgeLabel);
    }

    public class ImplicitGraphBuilder<TVertex, TEdgeLabel>
        where TVertex : notnull
        where TEdgeLabel : notnull
    {
        private IEqualityComparer<TVertex> vertexEqualityComparer;
        private IComparer<TVertex> vertexComparer;
        private IStringFormatter<TVertex> vertexFormatter;
        private IStringFormatter<TEdgeLabel> edgeLabelFormatter;
        private ContainsVertex<TVertex>? containsVertex;
        private TryGetEdgeLabel<TVertex, TEdgeLabel>? tryGetEdgeLabel;

        internal ImplicitGraphBuilder()
        {
            vertexEqualityComparer = EqualityComparer<TVertex>.Default;
            vertexComparer = Comparer<TVertex>.Default;
            vertexFormatter = StringFormatter<TVertex>.Default;
            edgeLabelFormatter = StringFormatter<TEdgeLabel>.Default;
        }

        internal ImplicitGraphBuilder(IImplicitGraph<TVertex, TEdgeLabel> fromDefinition)
        {
            if (fromDefinition == null) throw new ArgumentNullException(nameof(fromDefinition));

            vertexEqualityComparer = fromDefinition.VertexEqualityComparer;
            vertexComparer = fromDefinition.VertexComparer;
            vertexFormatter = fromDefinition.VertexFormatter;
            edgeLabelFormatter = fromDefinition.EdgeLabelFormatter;
            containsVertex = fromDefinition.ContainsVertex;
            tryGetEdgeLabel = fromDefinition.TryGetEdgeLabel;
        }

        public ImplicitGraphBuilder<TVertex, TEdgeLabel> WithVertexEqualityComparer(
            IEqualityComparer<TVertex> comparer)
        {
            this.vertexEqualityComparer = comparer ?? throw new ArgumentNullException(nameof(comparer));
            return this;
        }

        public ImplicitGraphBuilder<TVertex, TEdgeLabel> WithVertexComparer(
            IComparer<TVertex> comparer)
        {
            this.vertexComparer = comparer ?? throw new ArgumentNullException(nameof(comparer));
            return this;
        }

        public ImplicitGraphBuilder<TVertex, TEdgeLabel> WithVertexFormatter(
            IStringFormatter<TVertex> formatter)
        {
            this.vertexFormatter = formatter ?? throw new ArgumentNullException(nameof(formatter));
            return this;
        }

        public ImplicitGraphBuilder<TVertex, TEdgeLabel> WithEdgeLabelFormatter(
            IStringFormatter<TEdgeLabel> formatter)
        {
            this.edgeLabelFormatter = formatter ?? throw new ArgumentNullException(nameof(formatter));
            return this;
        }

        public ImplicitGraphBuilder<TVertex, TEdgeLabel> WithContainsVertex(
            ContainsVertex<TVertex> contains)
        {
            this.containsVertex = contains ?? throw new ArgumentNullException(nameof(contains));
            return this;
        }

        public ImplicitGraphBuilder<TVertex, TEdgeLabel> WithTryGetEdgeLabel(
            TryGetEdgeLabel<TVertex, TEdgeLabel> tryGet)
        {
            this.tryGetEdgeLabel = tryGet ?? throw new ArgumentNullException(nameof(tryGet));
            return this;
        }

        public IImplicitGraph<TVertex, TEdgeLabel> Build()
        {
            if (containsVertex == null) throw new ArgumentNullException(nameof(containsVertex));
            if (tryGetEdgeLabel == null) throw new ArgumentNullException(nameof(tryGetEdgeLabel));

            return new BuiltImplicitGraph<TVertex, TEdgeLabel>(
                vertexEqualityComparer,
                vertexComparer,
                vertexFormatter,
                edgeLabelFormatter,
                containsVertex,
                tryGetEdgeLabel);
        }
    }
}
