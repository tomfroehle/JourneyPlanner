using QuikGraph;

namespace JourneyPlanner.Graph
{
    public record Edge(string Source, string Target, int Cost) : IEdge<string>;
}