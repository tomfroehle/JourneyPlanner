using JourneyPlanner.Models;
using QuikGraph;

namespace JourneyPlanner.QuickGraph;

public record QuickGraphEdge(Station Source, Station Target, int Cost) : IEdge<Station>;