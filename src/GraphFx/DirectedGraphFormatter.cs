using System.Text;

namespace GraphFx;

public static class DirectedGraphFormatter<TVertex, TEdgeLabel>
    where TVertex : notnull
    where TEdgeLabel : notnull
{
    private const string StartArrow = " == ";
    private const string EndArrow = " => ";

    public static readonly IDirectedGraphFormatter<TVertex, TEdgeLabel> EnglishReadable = new EnglishReadableFormatter();
    public static readonly IDirectedGraphFormatter<TVertex, TEdgeLabel> EnglishCompact = new EnglishCompactFormatter();

    private class EnglishReadableFormatter : IDirectedGraphFormatter<TVertex, TEdgeLabel>
    {
        private const string Indent = "    ";

        public string FormatGraph(IDirectedGraph<TVertex, TEdgeLabel> graph)
        {
            var sb = new StringBuilder();
            sb.Append("Vertices (").Append(graph.Vertices.Count).AppendLine("):");
            foreach (var vertex in graph.Vertices)
            {
                var vertexFormat = graph.Formatter.VertexFormatter.Format(vertex);
                sb.Append(Indent).AppendLine(vertexFormat);
            }

            sb.Append("Edges (").Append(graph.Edges.Count).AppendLine("):");
            foreach (var edge in graph.Edges)
            {
                var sourceFormat = graph.Formatter.VertexFormatter.Format(edge.Source);
                var edgeFormat = graph.Formatter.EdgeFormatter.Format(edge.Label);
                var targetFormat = graph.Formatter.VertexFormatter.Format(edge.Target);
                sb.Append(Indent)
                    .Append(sourceFormat).Append(StartArrow)
                    .Append(edgeFormat).Append(EndArrow)
                    .AppendLine(targetFormat);
            }

            return sb.ToString();
        }
    }

    private class EnglishCompactFormatter : IDirectedGraphFormatter<TVertex, TEdgeLabel>
    {
        private const string Separator = ", ";

        public string FormatGraph(IDirectedGraph<TVertex, TEdgeLabel> graph)
        {
            var sb = new StringBuilder();
            sb.Append("Vertices (").Append(graph.Vertices.Count).Append("): ");
            var first = true;
            foreach (var vertex in graph.Vertices)
            {
                if (!first)
                {
                    sb.Append(Separator);
                }

                var vertexFormat = graph.Formatter.VertexFormatter.Format(vertex);
                sb.Append(vertexFormat);
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

                var sourceFormat = graph.Formatter.VertexFormatter.Format(edge.Source);
                var edgeFormat = graph.Formatter.EdgeFormatter.Format(edge.Label);
                var targetFormat = graph.Formatter.VertexFormatter.Format(edge.Target);
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

public static class DirectedGraphFormatter<TVertex>
    where TVertex : notnull
{
    private const string Arrow = " ==> ";

    public static readonly IDirectedGraphFormatter<TVertex> EnglishReadable = new EnglishReadableFormatter();
    public static readonly IDirectedGraphFormatter<TVertex> EnglishCompact = new EnglishCompactFormatter();

    private class EnglishReadableFormatter : IDirectedGraphFormatter<TVertex>
    {
        private const string Indent = "    ";

        public string FormatGraph(IDirectedGraph<TVertex> graph)
        {
            var sb = new StringBuilder();
            sb.Append("Vertices (").Append(graph.Vertices.Count).AppendLine("):");
            foreach (var vertex in graph.Vertices)
            {
                var vertexFormat = graph.Formatter.VertexFormatter.Format(vertex);
                sb.Append(Indent).AppendLine(vertexFormat);
            }

            sb.Append("Edges (").Append(graph.Edges.Count).AppendLine("):");
            foreach (var edge in graph.Edges)
            {
                var sourceFormat = graph.Formatter.VertexFormatter.Format(edge.Source);
                var targetFormat = graph.Formatter.VertexFormatter.Format(edge.Target);
                sb.Append(Indent)
                    .Append(sourceFormat).Append(Arrow).AppendLine(targetFormat);
            }

            return sb.ToString();
        }
    }

    private class EnglishCompactFormatter : IDirectedGraphFormatter<TVertex>
    {
        private const string Separator = ", ";
        
        public string FormatGraph(IDirectedGraph<TVertex> graph)
        {
            var sb = new StringBuilder();
            sb.Append("Vertices (").Append(graph.Vertices.Count).Append("): ");
            var first = true;
            foreach (var vertex in graph.Vertices)
            {
                if (!first)
                {
                    sb.Append(Separator);
                }

                var vertexFormat = graph.Formatter.VertexFormatter.Format(vertex);
                sb.Append(vertexFormat);
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

                var sourceFormat = graph.Formatter.VertexFormatter.Format(edge.Source);
                var targetFormat = graph.Formatter.VertexFormatter.Format(edge.Target);
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
    public static string ToEnglishReadable<TVertex, TEdgeLabel>(
        this IDirectedGraph<TVertex, TEdgeLabel> graph)
        where TVertex : notnull
        where TEdgeLabel : notnull
    {
        return DirectedGraphFormatter<TVertex, TEdgeLabel>.EnglishReadable.FormatGraph(graph);
    }

    public static string ToEnglishCompact<TVertex, TEdgeLabel>(
        this IDirectedGraph<TVertex, TEdgeLabel> graph)
        where TVertex : notnull
        where TEdgeLabel : notnull
    {
        return DirectedGraphFormatter<TVertex, TEdgeLabel>.EnglishCompact.FormatGraph(graph);
    }

    public static string ToEnglishReadable<TVertex>(
        this IDirectedGraph<TVertex> graph)
        where TVertex : notnull
    {
        return DirectedGraphFormatter<TVertex>.EnglishReadable.FormatGraph(graph);
    }

    public static string ToEnglishCompact<TVertex>(
        this IDirectedGraph<TVertex> graph)
        where TVertex : notnull
    {
        return DirectedGraphFormatter<TVertex>.EnglishCompact.FormatGraph(graph);
    }
}
