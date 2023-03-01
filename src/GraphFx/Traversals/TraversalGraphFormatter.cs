using System.Text;

namespace GraphFx.Traversals;

public class TraversalGraphFormatter<TVertex> :
    IIncidenceGraphFormatter<TVertex>
    where TVertex : notnull
{
    public static readonly TraversalGraphFormatter<TVertex> Default = new();

    private static TraversalOptions? dfsOptions;
    private static TraversalOptions DFSOptions
    {
        get
        {
            if (dfsOptions == null)
            {
                dfsOptions = new()
                {
                    IncludeEdgesWithVisitedTargets = true,
                    YieldNodeLast = false,
                    MaxDepth = 10,
                };
            }
            return dfsOptions;
        }
    }

    private readonly TraversalOptions options;
    private readonly int indentSize = 4;

    public TraversalGraphFormatter()
    {
        this.options = DFSOptions;
    }

    public TraversalGraphFormatter(
        int indentSize = 4,
        TraversalOptions? options = null)
    {
        this.options = PrepareOptions(options);
        if (indentSize < 1) indentSize = 1;
        this.indentSize = indentSize;
    }

    public string Format(IIncidenceGraph<TVertex> graph)
    {
        var sb = new StringBuilder();

        sb.Append("Seeds: ");
        sb.AppendLine(string.Join(", ", 
            graph.SeedVertices.Select(graph.VertexFormatter.Format)));

        void PrintVertex(TVertex vertex, int depth, string ellipsis = "")
        {
            if (depth > 2)
                for (int i = 0; i < depth - 2; i++)
                {
                    sb.Append('│');
                    sb.Append(' ', indentSize - 1);
                }
            if (depth > 1)
                sb.Append('└').Append('─', indentSize - 1);
            sb.Append(graph.VertexFormatter.Format(vertex));
            sb.Append(ellipsis);
            sb.AppendLine();
        }

        graph.TraverseSearchRecursive(
            options: options,
            events: new()
            {
                OnVertex = (info) =>
                {
                    PrintVertex(info.Vertex, info.Stats.Depth);
                    return true;
                },
                OnEdge = (info) =>
                {
                    if (!info.IsNewTargetVertex)
                        PrintVertex(info.Edge.Target, info.Stats.Depth + 1, " ...");
                    return true;
                },
            }
        );


        return sb.ToString();
    }

    private static TraversalOptions PrepareOptions(TraversalOptions? options)
    {
        if (options is null) return DFSOptions;
        var clone = options.Clone();
        clone.IncludeEdgesWithVisitedTargets = true;
        clone.YieldBacktrackingEdges = true;
        clone.YieldNodeLast = false;
        return clone;
    }
}
