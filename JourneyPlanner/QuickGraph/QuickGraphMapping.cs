using JourneyPlanner.Models;
using QuikGraph;

namespace JourneyPlanner.QuickGraph;

public static class QuickGraphMapping
{
    public static AdjacencyGraph<Station, QuickGraphEdge> BuildGraph(Network network)
    {
        var graph = new AdjacencyGraph<Station, QuickGraphEdge>();
        var edges = network.Connections.Select(BuildEdge);
        graph.AddVertexRange(network.Stations);
        graph.AddEdgeRange(edges);
        return graph;
    }

    public static Journey BuildRoute(IEnumerable<QuickGraphEdge> path)
    {
        var edges = path.ToList();
        return new Journey(edges.Select(BuildConnection).ToArray(), edges.Sum(e => e.Cost));
    }

    private static Connection BuildConnection(QuickGraphEdge quickGraphEdge)
    {
        return new Connection(quickGraphEdge.Source, quickGraphEdge.Target, quickGraphEdge.Cost);
    }

    private static QuickGraphEdge BuildEdge(Connection connection)
    {
        return new QuickGraphEdge(connection.Source, connection.Destination, connection.Duration);
    }
}