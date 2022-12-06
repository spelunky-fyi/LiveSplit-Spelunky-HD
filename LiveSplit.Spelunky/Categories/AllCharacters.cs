

using LiveSplit.Spelunky.SegmentFactories;

namespace LiveSplit.Spelunky.Categories
{
  public static class AllCharacters
  {
    public static readonly string Name = "All Characters%";
    public static readonly ISegmentFactory[] SegmentFactories = new ISegmentFactory[3]
    {
      (ISegmentFactory) new CharSelectSegmentFactory(),
      (ISegmentFactory) new TutorialSegmentFactory(),
      (ISegmentFactory) new AllCharactersSegmentFactory()
    };
  }
}
