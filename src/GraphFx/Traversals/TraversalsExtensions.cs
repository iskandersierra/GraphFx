namespace GraphFx.Traversals;

public static class TraversalsExtensions
{
    public static IEnumerable<TVertex> DepthFirstSearchRecursive<TVertex>(
        this IIncidenceGraph<TVertex> graph)
        where TVertex : notnull
    {
        var visited = new HashSet<TVertex>(graph.VertexEqualityComparer);

        IEnumerable<TVertex> Loop(TVertex vertex)
        {
            if (visited.Add(vertex))
            {
                yield return vertex;
                foreach (var adjacentVertex in graph.OutgoingEdges(vertex))
                {
                    foreach (var result in Loop(adjacentVertex))
                    {
                        yield return result;
                    }
                }
            }
        }

        foreach (var vertex in graph.SeedVertices)
        {
            foreach (var result in Loop(vertex))
            {
                yield return result;
            }
        }
    }
}
