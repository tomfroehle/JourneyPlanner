using JourneyPlanner.Graph;
using JourneyPlanner.Models;

namespace JourneyPlanner;

public static class JourneySearcher
{
    public static IEnumerable<Journey> FindShortestJourneys(string start, string destination)
    {
        var network = NetworkBuilder.Build();
        var startStations = FindStations(network.Stations, start);
        var destinationStations = FindStations(network.Stations, destination);
        var journeys = GraphSearch.FindJourneys(startStations, destinationStations, network);
        return journeys.OrderBy(j => j.Duration);
    }

    private static Station[] FindStations(Station[] networkStations, string name)
    {
        return networkStations.Where(station => station.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase))
            .ToArray();
    }
}