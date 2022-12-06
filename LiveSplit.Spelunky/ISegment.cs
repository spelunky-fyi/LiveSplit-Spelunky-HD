

namespace LiveSplit.Spelunky
{
  public interface ISegment
  {
    SegmentStatus CheckStatus(SpelunkyHooks spelunky);

    bool Cycle(SpelunkyHooks spelunky);
  }
}
