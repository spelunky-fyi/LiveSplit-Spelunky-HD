

using LiveSplit.Spelunky.SegmentFactories;

namespace LiveSplit.Spelunky.Categories
{
  public static class AllJournalEntries
  {
    public static readonly string Name = "All Journal Entries%";
    public static readonly ISegmentFactory[] SegmentFactories = new ISegmentFactory[4]
    {
      (ISegmentFactory) new CharSelectSegmentFactory(),
      (ISegmentFactory) new TutorialSegmentFactory(),
      (ISegmentFactory) new JournalEntryProgressSegmentFactory(JournalState.NumEntries / 2),
      (ISegmentFactory) new JournalEntryProgressSegmentFactory(JournalState.NumEntries)
    };
  }
}
