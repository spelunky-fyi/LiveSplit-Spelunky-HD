

namespace LiveSplit.Spelunky.SegmentFactories
{
  public class TunnelManSegment : ISegment
  {
    private string Item;
    private string Area1;
    private string Area2;
    private TunnelManChapter BeforeChapter;
    private TunnelManChapter AfterChapter;
    private int BeforeRemaining;
    private int AfterRemaining;
    private TunnelManChapter? LastChapter;
    private int? LastRemaining;

    public TunnelManSegment(
      string item,
      string area1,
      string area2,
      TunnelManChapter beforeChapter,
      TunnelManChapter afterChapter,
      int beforeRemaining,
      int afterRemaining)
    {
      this.Item = item;
      this.Area1 = area1;
      this.Area2 = area2;
      this.BeforeChapter = beforeChapter;
      this.AfterChapter = afterChapter;
      this.BeforeRemaining = beforeRemaining;
      this.AfterRemaining = afterRemaining;
    }

    public SegmentStatus CheckStatus(SpelunkyHooks spelunky)
    {
      TunnelManChapter tunnelManChapter = spelunky.TunnelManChapter;
      TunnelManChapter? lastChapter;
      if (tunnelManChapter > this.BeforeChapter)
      {
        lastChapter = this.LastChapter;
        TunnelManChapter beforeChapter = this.BeforeChapter;
        if ((lastChapter.GetValueOrDefault() > beforeChapter ? (lastChapter.HasValue ? 1 : 0) : 0) != 0)
          goto label_5;
      }
      if (tunnelManChapter == this.BeforeChapter)
      {
        lastChapter = this.LastChapter;
        TunnelManChapter beforeChapter = this.BeforeChapter;
        if ((lastChapter.GetValueOrDefault() == beforeChapter ? (lastChapter.HasValue ? 1 : 0) : 0) != 0 && spelunky.TunnelManRemaining < this.BeforeRemaining)
        {
          int? lastRemaining = this.LastRemaining;
          int beforeRemaining = this.BeforeRemaining;
          if ((lastRemaining.GetValueOrDefault() < beforeRemaining ? (lastRemaining.HasValue ? 1 : 0) : 0) != 0)
            goto label_5;
        }
      }
      return new SegmentStatus()
      {
        Type = SegmentStatusType.INFO,
        Message = string.Format("Waiting for Tunnel Man to receive {0} between the {1} and {2}.", (object) this.Item, (object) this.Area1, (object) this.Area2)
      };
label_5:
      return new SegmentStatus()
      {
        Type = SegmentStatusType.ERROR,
        Message = string.Format("Tunnel Man has already received {0} between the {1} and {2}.", (object) this.Item, (object) this.Area1, (object) this.Area2)
      };
    }

    public bool Cycle(SpelunkyHooks spelunky)
    {
      if (this.CheckStatus(spelunky).Type == SegmentStatusType.ERROR)
        return false;
      TunnelManChapter tunnelManChapter = spelunky.TunnelManChapter;
      int tunnelManRemaining = spelunky.TunnelManRemaining;
      TunnelManChapter? lastChapter = this.LastChapter;
      TunnelManChapter beforeChapter = this.BeforeChapter;
      int num;
      if ((lastChapter.GetValueOrDefault() == beforeChapter ? (lastChapter.HasValue ? 1 : 0) : 0) != 0)
      {
        int? lastRemaining = this.LastRemaining;
        int beforeRemaining = this.BeforeRemaining;
        if ((lastRemaining.GetValueOrDefault() == beforeRemaining ? (lastRemaining.HasValue ? 1 : 0) : 0) != 0 && tunnelManChapter == this.AfterChapter)
        {
          num = tunnelManRemaining == this.AfterRemaining ? 1 : 0;
          goto label_6;
        }
      }
      num = 0;
label_6:
      this.LastChapter = new TunnelManChapter?(tunnelManChapter);
      this.LastRemaining = new int?(tunnelManRemaining);
      return num != 0;
    }
  }
}
