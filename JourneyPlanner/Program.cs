using JourneyPlanner.Graph;
using JourneyPlanner.Models;

namespace JourneyPlanner;

internal class Program
{
    public static void Main(string[] args)
    {
        var start = "Jungfernsteig";
        var destination = "Hauptbahnhof";
        var journeys = JourneySearcher.FindShortestJourneys(start, destination);
        foreach (var journey in journeys)
        {
            Print(journey);
        }
    }

    private static void Print(Journey journey)
    {
        Console.WriteLine($"{string.Join(',', journey.Connections.Select(PrintConnection))}, {journey.Duration}");

        static string PrintConnection(Connection s)
        {
            return $"{s.Source.Name}-{s.Source.Line} -> {s.Destination.Name}-{s.Destination.Line}";
        }
    }
}

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