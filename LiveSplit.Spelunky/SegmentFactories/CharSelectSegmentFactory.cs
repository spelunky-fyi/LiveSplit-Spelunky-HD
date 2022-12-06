

namespace LiveSplit.Spelunky.SegmentFactories
{
  public class CharSelectSegmentFactory : ISegmentFactory
  {
    public ISegment NewInstance() => (ISegment) new CharSelectSegment();
  }
}
