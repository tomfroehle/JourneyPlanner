using JourneyPlanner.Models;
using QuikGraph;

namespace JourneyPlanner.Graph;

public static class GraphMapping
{
    public static AdjacencyGraph<string, Edge> BuildGraph(Network network)
    {
        var graph = new AdjacencyGraph<string, Edge>();
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

    public static Journey BuildRoute(IEnumerable<Edge> path)
    {
        var edges = path.ToList();
        return new Journey(edges.Select(BuildConnection).ToArray(), edges.Sum(e => e.Cost));
    }

    private static Connection BuildConnection(Edge edge)
    {
        return new Connection(BuildStation(edge.Source), BuildStation(edge.Target), edge.Cost);
    }

    private static Edge BuildEdge(Connection connection)
    {
        return new Edge(BuildVertex(connection.Source), BuildVertex(connection.Destination), connection.Duration);
    }

    private static Station BuildStation(string edgeSource)
    {
        var split = edgeSource.Split('-');
        return new Station(split[0], split[1]);
    }
}