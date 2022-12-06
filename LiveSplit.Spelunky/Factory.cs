

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

    public string XMLURL => "http://sashavol.com/frozlunky/autosplitter/update.LiveSplit.Spelunky.xml";

    public string UpdateURL => "http://sashavol.com/frozlunky/autosplitter/";

    public Version Version => Version.Parse("1.3.5");

    public string Description => "An interactive Spelunky auto splitter.";

    public ComponentCategory Category => (ComponentCategory) 1;
  }
}
