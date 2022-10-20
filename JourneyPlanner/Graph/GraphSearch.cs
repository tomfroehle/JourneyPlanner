using JourneyPlanner.Models;
using QuikGraph;
using QuikGraph.Algorithms.ShortestPath;

namespace JourneyPlanner.Graph;

public static class GraphSearch
{
    public static IEnumerable<Journey> FindJourneys(Station[] startStations, Station[] destinationStations,
        Network network)
    {
        var graph = GraphMapping.BuildGraph(network);
        var startVertices = startStations.Select(GraphMapping.BuildVertex).ToList();
        var destinationVertices = destinationStations.Select(GraphMapping.BuildVertex).ToList();
        RemoveTransferConnections(graph, startVertices, destinationVertices);
        return SearchAllShortestPaths(graph, startVertices, destinationVertices).Select(GraphMapping.BuildRoute);
    }

    private static void RemoveTransferConnections(
        IMutableEdgeListGraph<string, Edge> graph,
        IEnumerable<string> startVertices,
        IEnumerable<string> destinationVertices)
    {
        foreach (var startVertex in startVertices)
        {
            graph.RemoveEdgeIf(e => e.Source == startVertex && e.Cost == 0);
        }

        foreach (var destinationVertex in destinationVertices)
        {
            graph.RemoveEdgeIf(e => e.Target == destinationVertex && e.Cost == 0);
        }
    }

    private static IEnumerable<IEnumerable<Edge>> SearchAllShortestPaths(
        IVertexAndEdgeListGraph<string, Edge> graph,
        IReadOnlyList<string> startVertices,
        IReadOnlyList<string> destinationVertices)
    {
        var graphAlgorithm = new FloydWarshallAllShortestPathAlgorithm<string, Edge>(graph, e => e.Cost);
        graphAlgorithm.Compute();
        foreach (var startVertex in startVertices)
        {
            foreach (var destinationVertex in destinationVertices)
            {
                if (graphAlgorithm.TryGetPath(startVertex, destinationVertex, out var path))
                {
                    yield return path;
                }
            }
        }
    }
}