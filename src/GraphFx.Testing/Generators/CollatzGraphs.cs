namespace GraphFx.Testing.Generators;

public static class CollatzGraphs
{
    public enum DerivationRule
    {
        HalfN,
        ThreeNPlusOne,
    }

    public static IImplicitGraph<int, DerivationRule> Implicit()
    {
        return ImplicitGraph
            .Create<int, DerivationRule>()
            .WithContainsVertex(vertex => vertex >= 1)
            .WithTryGetEdgeLabel((int source, int target, out DerivationRule label) =>
            {
                label = DerivationRule.HalfN;
                if (source <= 1) return false;
                if (source % 2 == 0) return target == source / 2;
                label = DerivationRule.ThreeNPlusOne;
                return target == source * 3 + 1;
            })
            .Build();
    }

    public static IIncidenceGraph<int, DerivationRule> Incidence(IEnumerable<int> seed)
    {
        if (seed == null) throw new ArgumentNullException(nameof(seed));
        return IncidenceGraph.Create<int, DerivationRule>()
            .WithSeedVertices(seed.Where(n => n >= 1))
            .WithOutgoingLabeledEdges(OutgoingEdges)
            .WithOutgoingDegree(OutgoingDegree)
            .Build();
    }

    public static IIncidenceGraph<int, DerivationRule> Incidence(params int[] seed)
    {
        if (seed == null) throw new ArgumentNullException(nameof(seed));
        return Incidence((IEnumerable<int>)seed);
    }

    public static IIncidenceGraph<int, DerivationRule> IncidenceInverted(IEnumerable<int> seed)
    {
        if (seed == null) throw new ArgumentNullException(nameof(seed));
        return IncidenceGraph.Create<int, DerivationRule>()
            .WithSeedVertices(seed.Where(n => n >= 1))
            .WithOutgoingLabeledEdges(vertex => IncomingEdges(vertex).Select(edge => Edge.CreateOutgoing(edge.Label, edge.Source)))
            .WithOutgoingDegree(IncomingDegree)
            .Build();
    }

    public static IEnumerable<OutgoingEdge<int, DerivationRule>> OutgoingEdges(int source)
    {
        if (source <= 1) yield break;
        if (source % 2 == 0)
        {
            yield return Edge.CreateOutgoing(DerivationRule.HalfN, source / 2);
        }
        else
        {
            yield return Edge.CreateOutgoing(DerivationRule.ThreeNPlusOne, source * 3 + 1);
        }
    }

    public static int OutgoingDegree(int source) => source <= 1 ? 0 : 1;

    public static IEnumerable<IncomingEdge<int, DerivationRule>> IncomingEdges(int target)
    {
        if (target <= 1) yield break;
        yield return Edge.CreateIncoming(DerivationRule.HalfN, target * 2);
        if ((target - 1) % 3 == 0)
        {
            yield return Edge.CreateIncoming(DerivationRule.ThreeNPlusOne, (target - 1) / 3);
        }
    }

    public static int IncomingDegree(int target)
    {
        if (target <= 1) return 0;
        if ((target - 1) % 3 == 0) return 2;
        return 1;
    }
}
