

namespace LiveSplit.Spelunky.SegmentFactories
{
  public class JournalEntryProgressSegment : ISegment
  {
    private int TargetEntryUnlockCount;

    public JournalEntryProgressSegment(int targetEntryUnlockCount) => this.TargetEntryUnlockCount = targetEntryUnlockCount;

    private SegmentStatus CheckStatus(JournalState journal)
    {
      if (journal.NumUnlockedEntries > this.TargetEntryUnlockCount)
        return new SegmentStatus()
        {
          Type = SegmentStatusType.ERROR,
          Message = string.Format("More than {0} entries have been unlocked.", (object) this.TargetEntryUnlockCount)
        };
      return new SegmentStatus()
      {
        Type = SegmentStatusType.INFO,
        Message = "Waiting for journal (" + string.Format("{0}/{1} places, ", (object) journal.NumUnlockedPlaceEntries, (object) JournalState.NumPlaceEntries) + string.Format("{0}/{1} monsters, ", (object) journal.NumUnlockedMonsterEntries, (object) JournalState.NumMonsterEntries) + string.Format("{0}/{1} items, ", (object) journal.NumUnlockedItemEntries, (object) JournalState.NumItemEntries) + string.Format("{0}/{1} traps", (object) journal.NumUnlockedTrapEntries, (object) JournalState.NumTrapEntries) + ")."
      };
    }

    public SegmentStatus CheckStatus(SpelunkyHooks spelunky) => this.CheckStatus(spelunky.JournalState);

    public bool Cycle(SpelunkyHooks spelunky)
    {
      JournalState journalState = spelunky.JournalState;
      return this.CheckStatus(journalState).Type != SegmentStatusType.ERROR && journalState.NumUnlockedEntries >= this.TargetEntryUnlockCount;
    }
  }
}
