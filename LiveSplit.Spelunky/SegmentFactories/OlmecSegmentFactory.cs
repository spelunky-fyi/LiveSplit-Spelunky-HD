

namespace LiveSplit.Spelunky.SegmentFactories
{
  public class OlmecSegmentFactory : ISegmentFactory
  {
    public ISegment NewInstance() => (ISegment) new OlmecSegment();
  }
}
