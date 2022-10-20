using JourneyPlanner.Models;

namespace JourneyPlanner;

public static class NetworkBuilder
{
    public static Network Build()
    {
        var files = Directory.EnumerateFiles("LinePlans");
        var linePlans = files.Select(BuildLinePlan).ToList();
        return Merge(linePlans);
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

    private static Network BuildLinePlan(string fileName)
    {
        var fileContent = File.ReadAllLines(fileName);
        var line = fileContent[0];
        var stations = fileContent
            .Skip(1)
            .TakeWhile(l => l.Contains("-") is false)
            .Select(l => new Station(l, line))
            .ToArray();
        var connections = fileContent.Skip(1 + stations.Length).Select(Parse).ToArray();
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
}