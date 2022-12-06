

namespace LiveSplit.Spelunky.SegmentFactories
{
  public class JournalEntryProgressSegmentFactory : ISegmentFactory
  {
    private int TargetEntryUnlockCount;

    private string TargetPercTotalStr => (100 * this.TargetEntryUnlockCount / JournalState.NumEntries).ToString();

    public JournalEntryProgressSegmentFactory(int targetEntryUnlockCount) => this.TargetEntryUnlockCount = targetEntryUnlockCount;

    public ISegment NewInstance() => (ISegment) new JournalEntryProgressSegment(this.TargetEntryUnlockCount);
  }
}
