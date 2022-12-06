

namespace LiveSplit.Spelunky.SegmentFactories
{
  public class TutorialSegmentFactory : ISegmentFactory
  {
    public ISegment NewInstance() => (ISegment) new TutorialSegment();
  }
}
