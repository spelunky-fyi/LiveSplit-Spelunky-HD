

namespace LiveSplit.Spelunky.SegmentFactories
{
  public class TunnelManSegmentFactory : ISegmentFactory
  {
    private string Item;
    private string Area1;
    private string Area2;
    private TunnelManChapter BeforeChapter;
    private TunnelManChapter AfterChapter;
    private int BeforeRemaining;
    private int AfterRemaining;

    public TunnelManSegmentFactory(
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

    public ISegment NewInstance() => (ISegment) new TunnelManSegment(this.Item, this.Area1, this.Area2, this.BeforeChapter, this.AfterChapter, this.BeforeRemaining, this.AfterRemaining);
  }
}
