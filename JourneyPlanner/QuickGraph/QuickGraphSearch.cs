using JourneyPlanner.Models;
using QuikGraph;
using QuikGraph.Algorithms.ShortestPath;

namespace JourneyPlanner.QuickGraph;

public static class QuickGraphSearch
{
    public static IEnumerable<Journey> FindJourneys(Station[] startStations, Station[] destinationStations,
        Network network)
    {
        var graph = QuickGraphMapping.Map(network);
        RemoveTransferConnections(graph, startStations, destinationStations);
        var shortestPaths = SearchAllShortestPaths(graph, startStations, destinationStations);
        return shortestPaths.Select(QuickGraphMapping.Map);
    }

    private static void RemoveTransferConnections(
        IMutableEdgeListGraph<Station, QuickGraphEdge> graph,
        IEnumerable<Station> startVertices,
        IEnumerable<Station> destinationVertices)
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
        IVertexAndEdgeListGraph<Station, QuickGraphEdge> graph,
        IReadOnlyList<Station> startVertices,
        IReadOnlyList<Station> destinationVertices)
    {
        var graphAlgorithm = new FloydWarshallAllShortestPathAlgorithm<Station, QuickGraphEdge>(graph, e => e.Cost);
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