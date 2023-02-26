using System.Text;

namespace GraphFx;

public static class DirectedGraphFormatter<TNode, TEdge>
    where TNode : notnull
    where TEdge : notnull
{
    private const string StartArrow = " == ";
    private const string EndArrow = " => ";

    public static readonly IDirectedGraphFormatter<TNode, TEdge> EnglishReadable = new EnglishReadableFormatter();
    public static readonly IDirectedGraphFormatter<TNode, TEdge> EnglishCompact = new EnglishCompactFormatter();

    private class EnglishReadableFormatter : IDirectedGraphFormatter<TNode, TEdge>
    {
        private const string Indent = "    ";

        public string FormatGraph(IDirectedGraph<TNode, TEdge> graph)
        {
            var sb = new StringBuilder();
            sb.Append("Nodes (").Append(graph.Nodes.Count).AppendLine("):");
            foreach (var node in graph.Nodes)
            {
                var nodeFormat = graph.Formatter.NodeFormatter.Format(node);
                sb.Append(Indent).AppendLine(nodeFormat);
            }

            sb.Append("Edges (").Append(graph.Edges.Count).AppendLine("):");
            foreach (var edge in graph.Edges)
            {
                var sourceFormat = graph.Formatter.NodeFormatter.Format(edge.Source);
                var edgeFormat = graph.Formatter.EdgeFormatter.Format(edge.Edge);
                var targetFormat = graph.Formatter.NodeFormatter.Format(edge.Target);
                sb.Append(Indent)
                    .Append(sourceFormat).Append(StartArrow)
                    .Append(edgeFormat).Append(EndArrow)
                    .AppendLine(targetFormat);
            }

            return sb.ToString();
        }
    }

    private class EnglishCompactFormatter : IDirectedGraphFormatter<TNode, TEdge>
    {
        private const string Separator = ", ";

        public string FormatGraph(IDirectedGraph<TNode, TEdge> graph)
        {
            var sb = new StringBuilder();
            sb.Append("Nodes (").Append(graph.Nodes.Count).Append("): ");
            var first = true;
            foreach (var node in graph.Nodes)
            {
                if (!first)
                {
                    sb.Append(Separator);
                }

                var nodeFormat = graph.Formatter.NodeFormatter.Format(node);
                sb.Append(nodeFormat);
                first = false;
            }

            sb.AppendLine();

            sb.Append("Edges (").Append(graph.Edges.Count).Append("): ");
            first = true;
            foreach (var edge in graph.Edges)
            {
                if (!first)
                {
                    sb.Append(Separator);
                }

                var sourceFormat = graph.Formatter.NodeFormatter.Format(edge.Source);
                var edgeFormat = graph.Formatter.EdgeFormatter.Format(edge.Edge);
                var targetFormat = graph.Formatter.NodeFormatter.Format(edge.Target);
                sb.Append(sourceFormat).Append(StartArrow)
                    .Append(edgeFormat).Append(EndArrow)
                    .Append(targetFormat);
                first = false;
            }

            sb.AppendLine();

            return sb.ToString();
        }
    }
}

public static class DirectedGraphFormatter<TNode>
    where TNode : notnull
{
    private const string Arrow = " ==> ";

    public static readonly IDirectedGraphFormatter<TNode> EnglishReadable = new EnglishReadableFormatter();
    public static readonly IDirectedGraphFormatter<TNode> EnglishCompact = new EnglishCompactFormatter();

    private class EnglishReadableFormatter : IDirectedGraphFormatter<TNode>
    {
        private const string Indent = "    ";

        public string FormatGraph(IDirectedGraph<TNode> graph)
        {
            var sb = new StringBuilder();
            sb.Append("Nodes (").Append(graph.Nodes.Count).AppendLine("):");
            foreach (var node in graph.Nodes)
            {
                var nodeFormat = graph.Formatter.NodeFormatter.Format(node);
                sb.Append(Indent).AppendLine(nodeFormat);
            }

            sb.Append("Edges (").Append(graph.Edges.Count).AppendLine("):");
            foreach (var edge in graph.Edges)
            {
                var sourceFormat = graph.Formatter.NodeFormatter.Format(edge.Source);
                var targetFormat = graph.Formatter.NodeFormatter.Format(edge.Target);
                sb.Append(Indent)
                    .Append(sourceFormat).Append(Arrow).AppendLine(targetFormat);
            }

            return sb.ToString();
        }
    }

    private class EnglishCompactFormatter : IDirectedGraphFormatter<TNode>
    {
        private const string Separator = ", ";
        
        public string FormatGraph(IDirectedGraph<TNode> graph)
        {
            var sb = new StringBuilder();
            sb.Append("Nodes (").Append(graph.Nodes.Count).Append("): ");
            var first = true;
            foreach (var node in graph.Nodes)
            {
                if (!first)
                {
                    sb.Append(Separator);
                }

                var nodeFormat = graph.Formatter.NodeFormatter.Format(node);
                sb.Append(nodeFormat);
                first = false;
            }

            sb.AppendLine();

            sb.Append("Edges (").Append(graph.Edges.Count).Append("): ");
            first = true;
            foreach (var edge in graph.Edges)
            {
                if (!first)
                {
                    sb.Append(Separator);
                }

                var sourceFormat = graph.Formatter.NodeFormatter.Format(edge.Source);
                var targetFormat = graph.Formatter.NodeFormatter.Format(edge.Target);
                sb.Append(sourceFormat).Append(Arrow).Append(targetFormat);
                first = false;
            }

            sb.AppendLine();

            return sb.ToString();
        }
    }
}

public static class DirectedGraphFormatter
{
    public static string ToEnglishReadable<TNode, TEdge>(
        this IDirectedGraph<TNode, TEdge> graph)
        where TNode : notnull
        where TEdge : notnull
    {
        return DirectedGraphFormatter<TNode, TEdge>.EnglishReadable.FormatGraph(graph);
    }

    public static string ToEnglishCompact<TNode, TEdge>(
        this IDirectedGraph<TNode, TEdge> graph)
        where TNode : notnull
        where TEdge : notnull
    {
        return DirectedGraphFormatter<TNode, TEdge>.EnglishCompact.FormatGraph(graph);
    }

    public static string ToEnglishReadable<TNode>(
        this IDirectedGraph<TNode> graph)
        where TNode : notnull
    {
        return DirectedGraphFormatter<TNode>.EnglishReadable.FormatGraph(graph);
    }

    public static string ToEnglishCompact<TNode>(
        this IDirectedGraph<TNode> graph)
        where TNode : notnull
    {
        return DirectedGraphFormatter<TNode>.EnglishCompact.FormatGraph(graph);
    }
}
