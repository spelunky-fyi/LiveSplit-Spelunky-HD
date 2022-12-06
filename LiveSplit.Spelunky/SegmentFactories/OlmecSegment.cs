

namespace LiveSplit.Spelunky.SegmentFactories
{
  public class OlmecSegment : ISegment
  {
    private SpelunkyLevel? LastLevel;

    public SegmentStatus CheckStatus(SpelunkyHooks spelunky) => new SegmentStatus()
    {
      Type = SegmentStatusType.INFO,
      Message = "Waiting for Olmec completion."
    };

    public bool Cycle(SpelunkyHooks spelunky)
    {
      if (this.CheckStatus(spelunky).Type == SegmentStatusType.ERROR)
        return false;
      SpelunkyLevel currentLevel = spelunky.CurrentLevel;
      int currentState = (int) spelunky.CurrentState;
      int num;
      if (spelunky.TunnelManChapter == TunnelManChapter.EmptyCompleted && spelunky.CurrentState == SpelunkyState.VictoryCutscene)
      {
        SpelunkyLevel? lastLevel = this.LastLevel;
        SpelunkyLevel spelunkyLevel = SpelunkyLevel.L4_4;
        if ((lastLevel.GetValueOrDefault() == spelunkyLevel ? (lastLevel.HasValue ? 1 : 0) : 0) != 0)
        {
          num = currentLevel == SpelunkyLevel.L4_4 ? 1 : 0;
          goto label_6;
        }
      }
      num = 0;
label_6:
      this.LastLevel = new SpelunkyLevel?(currentLevel);
      return num != 0;
    }
  }
}
