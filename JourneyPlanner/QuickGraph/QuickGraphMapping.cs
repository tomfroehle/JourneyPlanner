using JourneyPlanner.Models;
using QuikGraph;

namespace JourneyPlanner.QuickGraph;

public static class QuickGraphMapping
{
    public static AdjacencyGraph<string, QuickGraphEdge> BuildGraph(Network network)
    {
        var graph = new AdjacencyGraph<string, QuickGraphEdge>();
        var vertices = network.Stations.Select(BuildVertex);
        var edges = network.Connections.Select(BuildEdge);
        graph.AddVertexRange(vertices);
        graph.AddEdgeRange(edges);
        return graph;
    }

    public static string BuildVertex(Station station)
    {
        return $"{station.Name}-{station.Line}";
    }

    public static Journey BuildRoute(IEnumerable<QuickGraphEdge> path)
    {
        var edges = path.ToList();
        return new Journey(edges.Select(BuildConnection).ToArray(), edges.Sum(e => e.Cost));
    }

    private static Connection BuildConnection(QuickGraphEdge quickGraphEdge)
    {
        return new Connection(BuildStation(quickGraphEdge.Source), BuildStation(quickGraphEdge.Target),
            quickGraphEdge.Cost);
    }

    private static QuickGraphEdge BuildEdge(Connection connection)
    {
        return new QuickGraphEdge(BuildVertex(connection.Source), BuildVertex(connection.Destination),
            connection.Duration);
    }

    private static Station BuildStation(string edgeSource)
    {
        var split = edgeSource.Split('-');
        return new Station(split[0], split[1]);
    }
}