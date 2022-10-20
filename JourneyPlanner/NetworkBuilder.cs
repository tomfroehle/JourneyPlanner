using JourneyPlanner.Models;

namespace JourneyPlanner;

public static class NetworkBuilder
{
    public static Network Build()
    {
        var linePlanContents = ReadLinePlans();
        var linePlans = linePlanContents.Select(BuildLinePlan).ToList();
        return Merge(linePlans);
    }

    private static IEnumerable<string[]> ReadLinePlans()
    {
        return Directory.EnumerateFiles("LinePlans").Select(File.ReadAllLines);
    }

    private static Network BuildLinePlan(string[] linePlanContent)
    {
        var line = linePlanContent[0];
        var stations = linePlanContent
            .Skip(1)
            .TakeWhile(l => l.Contains("-") is false)
            .Select(l => new Station(l, line))
            .ToArray();
        var connections = linePlanContent.Skip(1 + stations.Length).Select(Parse).ToArray();
        return new Network(stations, connections);

        Connection Parse(string connectionLine)
        {
            var parts = connectionLine.Split('-', ' ');
            var source = new Station(parts[0], line);
            var destination = new Station(parts[1], line);
            var duration = int.Parse(parts[2]);
            return new Connection(source, destination, duration);
        }
    }

    private static Network Merge(IReadOnlyList<Network> linePlans)
    {
        var stations = linePlans.SelectMany(lp => lp.Stations).ToArray();
        var transferConnections = TransferConnections(stations).ToArray();
        var connections = linePlans.SelectMany(lp => lp.Connections).Concat(transferConnections).ToArray();
        return new Network(stations, connections);
    }

    private static IEnumerable<Connection> TransferConnections(IReadOnlyList<Station> stations)
    {
        return from source in stations
            from target in stations
            where source != target && source.Name.Equals(target.Name, StringComparison.InvariantCultureIgnoreCase)
            select new Connection(source, target, 0);
    }
}