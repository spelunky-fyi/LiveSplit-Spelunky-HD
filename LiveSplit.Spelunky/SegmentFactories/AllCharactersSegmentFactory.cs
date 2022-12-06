

namespace LiveSplit.Spelunky.SegmentFactories
{
  public class AllCharactersSegmentFactory : ISegmentFactory
  {
    public ISegment NewInstance() => (ISegment) new AllCharactersSegment();
  }
}
