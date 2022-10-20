using Snapshooter.NUnit;

namespace JourneyPlanner.Tests;

public class IntegrationTests
{
    [Test]
    public void Should_Match_Snapshot()
    {
        var journeys = JourneySearcher.FindShortestJourneys("Jungfernsteig", "Hauptbahnhof");

        journeys.MatchSnapshot();
    }

    [Test]
    public void Should_Be_Empty_If_There_Are_No_Routes()
    {
        var journeys = JourneySearcher.FindShortestJourneys("non-existing", "Hauptbahnhof");

        CollectionAssert.IsEmpty(journeys);
    }
}