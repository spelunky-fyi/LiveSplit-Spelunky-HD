

using System;
using System.Collections.Generic;
using System.Linq;

namespace LiveSplit.Spelunky
{
  public static class Category
  {
    public static string GetFriendlyName(Type categoryType) => (string) categoryType.GetField("Name").GetValue((object) null);

    public static ISegment[] NewSegmentInstances(Type categoryType) => ((IEnumerable<ISegmentFactory>) (ISegmentFactory[]) categoryType.GetField("SegmentFactories").GetValue((object) null)).Select<ISegmentFactory, ISegment>((Func<ISegmentFactory, ISegment>) (segmentFactory => segmentFactory.NewInstance())).ToArray<ISegment>();
  }
}
