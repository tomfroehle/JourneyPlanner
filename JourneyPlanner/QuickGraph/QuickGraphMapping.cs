using JourneyPlanner.Models;
using QuikGraph;

namespace JourneyPlanner.QuickGraph;

public static class QuickGraphMapping
{
    public static AdjacencyGraph<Station, QuickGraphEdge> Map(Network network)
    {
        var graph = new AdjacencyGraph<Station, QuickGraphEdge>();
        var edges = network.Edges.Select(Map);
        graph.AddVertexRange(network.Stations);
        graph.AddEdgeRange(edges);
        return graph;
    }

    public static Journey Map(IEnumerable<QuickGraphEdge> path)
    {
        var edges = path.Select(Map).ToArray();
        var duration = edges.Sum(e => e.Duration);
        return new Journey(edges, duration);
    }

    private static Edge Map(QuickGraphEdge quickGraphEdge)
    {
        return new Edge(quickGraphEdge.Source, quickGraphEdge.Target, quickGraphEdge.Cost);
    }

    private static QuickGraphEdge Map(Edge edge)
    {
        return new QuickGraphEdge(edge.Source, edge.Destination, edge.Duration);
    }
}