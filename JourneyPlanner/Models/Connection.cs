namespace JourneyPlanner.Models;

public record Connection(Station Source, Station Destination, int Duration);