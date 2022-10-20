using JourneyPlanner.Models;
using QuikGraph;
using QuikGraph.Algorithms.ShortestPath;

namespace JourneyPlanner.QuickGraph;

public static class QuickGraphSearch
{
    public static IEnumerable<Journey> FindJourneys(Station[] startStations, Station[] destinationStations,
        Network network)
    {
        var graph = QuickGraphMapping.BuildGraph(network);
        var startVertices = startStations.Select(QuickGraphMapping.BuildVertex).ToList();
        var destinationVertices = destinationStations.Select(QuickGraphMapping.BuildVertex).ToList();
        RemoveTransferConnections(graph, startVertices, destinationVertices);
        var shortestPaths = SearchAllShortestPaths(graph, startVertices, destinationVertices);
        return shortestPaths.Select(QuickGraphMapping.BuildRoute);
    }

    private static void RemoveTransferConnections(
        IMutableEdgeListGraph<string, QuickGraphEdge> graph,
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

    private static IEnumerable<IEnumerable<QuickGraphEdge>> SearchAllShortestPaths(
        IVertexAndEdgeListGraph<string, QuickGraphEdge> graph,
        IReadOnlyList<string> startVertices,
        IReadOnlyList<string> destinationVertices)
    {
        var graphAlgorithm = new FloydWarshallAllShortestPathAlgorithm<string, QuickGraphEdge>(graph, e => e.Cost);
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