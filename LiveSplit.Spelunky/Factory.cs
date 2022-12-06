

using LiveSplit.Model;
using LiveSplit.UI.Components;
using System;
using UpdateManager;

namespace LiveSplit.Spelunky
{
  public class Factory : IComponentFactory, IUpdateable
  {
    public string ComponentName => "Spelunky Auto Splitter";

    public IComponent Create(LiveSplitState state) => (IComponent) new Component(state, false);

    public string UpdateName => this.ComponentName;

    public string XMLURL => "";

    public string UpdateURL => "";

    public Version Version => Version.Parse("2.0.0");

    public string Description => "An interactive Spelunky auto splitter.";

    public ComponentCategory Category => (ComponentCategory) 1;
  }
}
