

using LiveSplit.Spelunky.SegmentFactories;

namespace LiveSplit.Spelunky.Categories
{
  public static class AllShortcuts
  {
    public static readonly string Name = "All Shortcuts + Olmec%";
    public static readonly ISegmentFactory[] SegmentFactories = new ISegmentFactory[12]
    {
      (ISegmentFactory) new CharSelectSegmentFactory(),
      (ISegmentFactory) new TutorialSegmentFactory(),
      (ISegmentFactory) new TunnelManSegmentFactory("a bomb", "Mines", "Jungle", TunnelManChapter.MinesToJungle, TunnelManChapter.MinesToJungle, 3, 2),
      (ISegmentFactory) new TunnelManSegmentFactory("a rope", "Mines", "Jungle", TunnelManChapter.MinesToJungle, TunnelManChapter.MinesToJungle, 2, 1),
      (ISegmentFactory) new TunnelManSegmentFactory("$10000", "Mines", "Jungle", TunnelManChapter.MinesToJungle, TunnelManChapter.EmptyPostMtj, 1, 0),
      (ISegmentFactory) new TunnelManSegmentFactory("two bombs", "Jungle", "Ice Caves", TunnelManChapter.JungleToIceCaves, TunnelManChapter.JungleToIceCaves, 3, 2),
      (ISegmentFactory) new TunnelManSegmentFactory("two ropes", "Jungle", "Ice Caves", TunnelManChapter.JungleToIceCaves, TunnelManChapter.JungleToIceCaves, 2, 1),
      (ISegmentFactory) new TunnelManSegmentFactory("a shotgun", "Jungle", "Ice Caves", TunnelManChapter.JungleToIceCaves, TunnelManChapter.EmptyPostJtic, 1, 0),
      (ISegmentFactory) new TunnelManSegmentFactory("3 bombs", "Ice Caves", "Temple", TunnelManChapter.IceCavesToTemple, TunnelManChapter.IceCavesToTemple, 3, 2),
      (ISegmentFactory) new TunnelManSegmentFactory("3 ropes", "Ice Caves", "Temple", TunnelManChapter.IceCavesToTemple, TunnelManChapter.IceCavesToTemple, 2, 1),
      (ISegmentFactory) new TunnelManSegmentFactory("a key", "Ice Caves", "Temple", TunnelManChapter.IceCavesToTemple, TunnelManChapter.EmptyCompleted, 1, 0),
      (ISegmentFactory) new OlmecSegmentFactory()
    };
  }
}
