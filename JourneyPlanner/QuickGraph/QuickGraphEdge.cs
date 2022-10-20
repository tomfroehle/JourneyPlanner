using QuikGraph;

namespace JourneyPlanner.QuickGraph;

public record QuickGraphEdge(string Source, string Target, int Cost) : IEdge<string>;