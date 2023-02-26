﻿namespace GraphFx;

public static class DirectedGraph
{
    public static GraphBuilder<TNode, TEdge> Builder<TNode, TEdge>()
        where TNode : notnull
        where TEdge : notnull
    {
        return new GraphBuilder<TNode, TEdge>();
    }

    public class GraphBuilder<TNode, TEdge>
        where TNode : notnull
        where TEdge : notnull
    {
        private readonly List<NodeOrLabeledEdge<TNode, TEdge>> data = new();

        private IEqualityComparer<TNode>? nodeComparer;
        private IGraphFormatter<TNode, TEdge>? formatter;


        internal GraphBuilder()
        {
        }

        public GraphBuilder<TNode, TEdge> AddNode(TNode node)
        {
            if (node == null) throw new ArgumentNullException(nameof(node));
            data.Add(node);
            return this;
        }

        public GraphBuilder<TNode, TEdge> AddNodes(IEnumerable<TNode> nodes)
        {
            if (nodes == null) throw new ArgumentNullException(nameof(nodes));
            foreach (var node in nodes)
            {
                AddNode(node);
            }

            return this;
        }

        public GraphBuilder<TNode, TEdge> AddNodes(params TNode[] nodes)
        {
            if (nodes == null) throw new ArgumentNullException(nameof(nodes));
            return AddNodes((IEnumerable<TNode>) nodes);
        }

        public GraphBuilder<TNode, TEdge> AddEdge(LabeledEdgeDefinition<TNode, TEdge> labeledEdge)
        {
            data.Add(labeledEdge);
            return this;
        }

        public GraphBuilder<TNode, TEdge> AddEdge(TNode source, TEdge edge, TNode target)
        {
            return AddEdge(EdgeDefinition.Create(source, edge, target));
        }

        public GraphBuilder<TNode, TEdge> AddEdges(IEnumerable<LabeledEdgeDefinition<TNode, TEdge>> edges)
        {
            if (edges == null) throw new ArgumentNullException(nameof(edges));
            foreach (var edge in edges)
            {
                AddEdge(edge);
            }

            return this;
        }

        public GraphBuilder<TNode, TEdge> AddEdges(params LabeledEdgeDefinition<TNode, TEdge>[] edges)
        {
            if (edges == null) throw new ArgumentNullException(nameof(edges));
            return AddEdges((IEnumerable<LabeledEdgeDefinition<TNode, TEdge>>) edges);
        }

        public GraphBuilder<TNode, TEdge> WithNodeComparer(IEqualityComparer<TNode> comparer)
        {
            nodeComparer = comparer;
            return this;
        }

        public GraphBuilder<TNode, TEdge> WithFormatter(IGraphFormatter<TNode, TEdge> formatter)
        {
            this.formatter = formatter;
            return this;
        }

        public GraphBuilder<TNode, TEdge> WithFormatter(
            IStringFormatter<TNode> nodeFormatter,
            IStringFormatter<TEdge> edgeFormatter)
        {
            return this.WithFormatter(GraphFormatter.Create(nodeFormatter, edgeFormatter));
        }

        public GraphBuilder<TNode, TEdge> WithFormatter(
            Func<TNode, string> nodeFormatter,
            Func<TEdge, string> edgeFormatter)
        {
            return this.WithFormatter(GraphFormatter.Create(nodeFormatter, edgeFormatter));
        }

        public GraphBuilder<TNode, TEdge> WithNodeFormatter(
            IStringFormatter<TNode> nodeFormatter)
        {
            this.formatter ??= GraphFormatter<TNode, TEdge>.Default;
            this.formatter = this.formatter.WithNodeFormatter(nodeFormatter);
            return this;
        }

        public GraphBuilder<TNode, TEdge> WithNodeFormatter(
            Func<TNode, string> nodeFormatter)
        {
            return this.WithNodeFormatter(StringFormatter.Create(nodeFormatter));
        }

        public GraphBuilder<TNode, TEdge> WithEdgeFormatter(
            IStringFormatter<TEdge> edgeFormatter)
        {
            this.formatter ??= GraphFormatter<TNode, TEdge>.Default;
            this.formatter = this.formatter.WithEdgeFormatter(edgeFormatter);
            return this;
        }

        public GraphBuilder<TNode, TEdge> WithEdgeFormatter(
            Func<TEdge, string> edgeFormatter)
        {
            return this.WithEdgeFormatter(StringFormatter.Create(edgeFormatter));
        }

        public IDirectedGraph<TNode, TEdge> Build()
        {
            var nodeComp = nodeComparer ?? EqualityComparer<TNode>.Default;
            var fmt = formatter ?? GraphFormatter<TNode, TEdge>.Default;

            var nodeList = new List<TNode>();
            var edgeList = new List<LabeledEdgeDefinition<TNode, TEdge>>();
            var nodeSet = new HashSet<TNode>(nodeComp);

            foreach (var item in data)
            {
                item.Switch(
                    node =>
                    {
                        if (nodeSet.Add(node))
                        {
                            nodeList.Add(node);
                        }
                    },
                    edge =>
                    {
                        if (nodeSet.Add(edge.Source))
                        {
                            nodeList.Add(edge.Source);
                        }

                        if (nodeSet.Add(edge.Target))
                        {
                            nodeList.Add(edge.Target);
                        }

                        edgeList.Add(edge);
                    });
            }

            return new BuiltGraph<TNode, TEdge>(nodeList, edgeList, nodeComp, fmt);
        }
    }

    private class BuiltGraph<TNode, TEdge> :
        IDirectedGraph<TNode, TEdge>
        where TNode : notnull
        where TEdge : notnull
    {
        internal BuiltGraph(
            IReadOnlyList<TNode> nodesCollection,
            IReadOnlyList<LabeledEdgeDefinition<TNode, TEdge>> edgesCollection,
            IEqualityComparer<TNode> nodeComparer,
            IGraphFormatter<TNode, TEdge> formatter)
        {
            this.Nodes = nodesCollection ?? throw new ArgumentNullException(nameof(nodesCollection));
            this.Edges = edgesCollection ?? throw new ArgumentNullException(nameof(edgesCollection));
            this.NodeComparer = nodeComparer ?? throw new ArgumentNullException(nameof(nodeComparer));
            Formatter = formatter ?? throw new ArgumentNullException(nameof(formatter));
        }

        public IReadOnlyList<TNode> Nodes { get; }

        public IReadOnlyList<LabeledEdgeDefinition<TNode, TEdge>> Edges { get; }

        public IEqualityComparer<TNode> NodeComparer { get; }

        public IGraphFormatter<TNode, TEdge> Formatter { get; }
    }

    public static GraphBuilder<TNode> Builder<TNode>()
        where TNode : notnull
    {
        return new GraphBuilder<TNode>();
    }

    public class GraphBuilder<TNode>
        where TNode : notnull
    {
        private readonly List<NodeOrEdge<TNode>> data = new();
        private IEqualityComparer<TNode>? nodeComparer;
        private IGraphFormatter<TNode>? formatter;

        internal GraphBuilder()
        {
        }

        public GraphBuilder<TNode> AddNode(TNode node)
        {
            if (node == null) throw new ArgumentNullException(nameof(node));
            data.Add(node);
            return this;
        }

        public GraphBuilder<TNode> AddNodes(IEnumerable<TNode> nodes)
        {
            if (nodes == null) throw new ArgumentNullException(nameof(nodes));
            foreach (var node in nodes)
            {
                this.AddNode(node);
            }
            return this;
        }

        public GraphBuilder<TNode> AddNodes(params TNode[] nodes)
        {
            if (nodes == null) throw new ArgumentNullException(nameof(nodes));
            return this.AddNodes((IEnumerable<TNode>)nodes);
        }

        public GraphBuilder<TNode> AddEdge(EdgeDefinition<TNode> edge)
        {
            data.Add(edge);
            return this;
        }

        public GraphBuilder<TNode> AddEdge(TNode source, TNode target)
        {
            return this.AddEdge(EdgeDefinition.Create(source, target));
        }

        public GraphBuilder<TNode> AddEdges(IEnumerable<EdgeDefinition<TNode>> edges)
        {
            if (edges == null) throw new ArgumentNullException(nameof(edges));
            foreach (var edge in edges)
            {
                this.AddEdge(edge);
            }
            return this;
        }

        public GraphBuilder<TNode> AddEdges(params EdgeDefinition<TNode>[] edges)
        {
            if (edges == null) throw new ArgumentNullException(nameof(edges));
            return this.AddEdges((IEnumerable<EdgeDefinition<TNode>>)edges);
        }

        public GraphBuilder<TNode> WithNodeComparer(IEqualityComparer<TNode>? comparer)
        {
            nodeComparer = comparer;
            return this;
        }

        public GraphBuilder<TNode> WithFormatter(IGraphFormatter<TNode>? formatter)
        {
            this.formatter = formatter;
            return this;
        }

        public GraphBuilder<TNode> WithFormatter(
            IStringFormatter<TNode> nodeFormatter)
        {
            return this.WithFormatter(GraphFormatter.Create(nodeFormatter));
        }

        public GraphBuilder<TNode> WithFormatter(
            Func<TNode, string> nodeFormatter)
        {
            return this.WithFormatter(GraphFormatter.Create(nodeFormatter));
        }

        public IDirectedGraph<TNode> Build()
        {
            var nodeComp = nodeComparer ?? EqualityComparer<TNode>.Default;
            var fmt = formatter ?? GraphFormatter<TNode>.Default;

            var nodeList = new List<TNode>();
            var edgeList = new List<EdgeDefinition<TNode>>();
            var nodeSet = new HashSet<TNode>(nodeComp);

            foreach (var item in data)
            {
                item.Switch(
                    node =>
                    {
                        if (nodeSet.Add(node))
                        {
                            nodeList.Add(node);
                        }
                    },
                    edge =>
                    {
                        if (nodeSet.Add(edge.Source))
                        {
                            nodeList.Add(edge.Source);
                        }

                        if (nodeSet.Add(edge.Target))
                        {
                            nodeList.Add(edge.Target);
                        }

                        edgeList.Add(edge);
                    });
            }

            return new BuiltGraph<TNode>(nodeList, edgeList, nodeComp, fmt);
        }
    }

    private class BuiltGraph<TNode> :
        IDirectedGraph<TNode>
        where TNode : notnull
    {
        internal BuiltGraph(
            IReadOnlyList<TNode> nodesCollection,
            IReadOnlyList<EdgeDefinition<TNode>> edgesCollection,
            IEqualityComparer<TNode> nodeComparer,
            IGraphFormatter<TNode> formatter)
        {
            this.Nodes = nodesCollection ?? throw new ArgumentNullException(nameof(nodesCollection));
            this.Edges = edgesCollection ?? throw new ArgumentNullException(nameof(edgesCollection));
            this.NodeComparer = nodeComparer ?? throw new ArgumentNullException(nameof(nodeComparer));
            Formatter = formatter ?? throw new ArgumentNullException(nameof(formatter));
        }

        public IReadOnlyList<TNode> Nodes { get; }

        public IReadOnlyList<EdgeDefinition<TNode>> Edges { get; }

        public IEqualityComparer<TNode> NodeComparer { get; }
        
        public IGraphFormatter<TNode> Formatter { get; }
    }
}
