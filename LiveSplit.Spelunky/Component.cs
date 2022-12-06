

using LiveSplit.Model;
using LiveSplit.UI;
using LiveSplit.UI.Components;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;

namespace LiveSplit.Spelunky
{
  internal class Component : IComponent, IDisposable
  {
    private SpelunkySettings Settings;
    private StatusWindow StatusWindow;
    private JournalTracker JournalTracker;
    private CharactersTracker CharactersTracker;
    private AutoSplitter AutoSplitter;
    private IDictionary<string, Action> _ContextMenuControls = (IDictionary<string, Action>) new Dictionary<string, Action>();

    public string ComponentName => "SpelunkySplitter";

    public bool IsLayoutComponent { get; private set; }

    public float HorizontalWidth => 100f;

    public float MinimumHeight => 100f;

    public float VerticalHeight => 100f;

    public float MinimumWidth => 100f;

    public float MaximumHeight => 100f;

    public float PaddingTop => 1f;

    public float PaddingBottom => 1f;

    public float PaddingLeft => 1f;

    public float PaddingRight => 1f;

    public IDictionary<string, Action> ContextMenuControls => this._ContextMenuControls;

    public Component(LiveSplitState state, bool isLayoutComponent)
    {
      this.IsLayoutComponent = isLayoutComponent;
      this.Settings = new SpelunkySettings();
      this.StatusWindow = new StatusWindow();
      this.JournalTracker = new JournalTracker();
      this.CharactersTracker = new CharactersTracker();
      this.HandleAutoSplitterChange((object) this.Settings, EventArgs.Empty);
      this.Settings.PropertyChanged += new SpelunkySettings.PropertyChangedHandler(this.HandleAutoSplitterChange);
    }

    private void HandleAutoSplitterChange(object sender, EventArgs args)
    {
      this.ClearAutoSplitter();
      if (this.StatusWindow.IsDisposed)
        this.StatusWindow = new StatusWindow();
      this.StatusWindow.CurrentRun = Category.GetFriendlyName(this.Settings.CurrentRunCategoryType);
      if (this.Settings.AutoSplittingEnabled)
        this.StatusWindow.Show();
      else
        this.StatusWindow.Hide();
    }

    public XmlNode GetSettings(XmlDocument document) => this.Settings.GetSettings(document);

    public void SetSettings(XmlNode node) => this.Settings.SetSettings(node);

    public void DrawHorizontal(Graphics g, LiveSplitState state, float height, Region clipRegion)
    {
    }

    public void DrawVertical(Graphics g, LiveSplitState state, float width, Region clipRegion)
    {
    }

    public Control GetSettingsControl(LayoutMode mode) => (Control) this.Settings;

    private EnabledPatchContainer MakePatchesFromSettings(SpelunkyHooks hooks)
    {
      EnabledPatchContainer enabledPatchContainer = new EnabledPatchContainer();
      if (this.Settings.ForceAlternativeSaveFile)
        enabledPatchContainer.TryAddAndEnable((EnabledPatchContainer.PatchInstantiator) (() => (IPatch) new SaveChangePatch(hooks)));
      return enabledPatchContainer;
    }

    private void GetHooksIfNeeded(LiveSplitState state)
    {
      if (this.AutoSplitter != null)
      {
        if (!this.AutoSplitter.Hooks.Invalidated)
          return;
        this.AutoSplitter.Dispose();
        this.AutoSplitter = (AutoSplitter) null;
      }
      SpelunkyHooks hooks1 = new SpelunkyHooks(new RawProcess("Spelunky"));
      this.JournalTracker.ClientSize = new Size((int) ((double) JournalTracker.InitialClientSize.Width * this.Settings.JournalTrackerScale), (int) ((double) JournalTracker.InitialClientSize.Height * this.Settings.JournalTrackerScale));
      this.CharactersTracker.ClientSize = new Size((int) ((double) CharactersTracker.InitialClientSize.Width * this.Settings.CharactersTrackerScale), (int) ((double) CharactersTracker.InitialClientSize.Height * this.Settings.CharactersTrackerScale));
      SpelunkyHooks hooks2 = hooks1;
      System.Type currentRunCategoryType = this.Settings.CurrentRunCategoryType;
      EnabledPatchContainer patches = this.MakePatchesFromSettings(hooks1);
      TimerModel timer = new TimerModel();
      timer.CurrentState = state;
      string autoSaveLoadOpt = this.Settings.AutoLoadSaveFile ? this.Settings.SaveFile : (string) null;
      JournalTracker maybeJournalTracker = this.Settings.ShowJournalTracker ? this.JournalTracker : (JournalTracker) null;
      CharactersTracker maybeCharactersTracker = this.Settings.ShowCharactersTracker ? this.CharactersTracker : (CharactersTracker) null;
      this.AutoSplitter = new AutoSplitter(hooks2, currentRunCategoryType, patches, timer, autoSaveLoadOpt, maybeJournalTracker, maybeCharactersTracker);
    }

    private void ClearAutoSplitter()
    {
      if (this.AutoSplitter == null)
        return;
      this.AutoSplitter.Dispose();
      this.AutoSplitter = (AutoSplitter) null;
    }

    public void Update(
      IInvalidator invalidator,
      LiveSplitState state,
      float width,
      float height,
      LayoutMode mode)
    {
      if (!this.Settings.AutoSplittingEnabled)
        return;
      try
      {
        this.GetHooksIfNeeded(state);
        SegmentStatus segmentStatus = this.AutoSplitter.Update(state);
        if (this.StatusWindow.IsDisposed)
          return;
        this.StatusWindow.SetStatus(segmentStatus.Type, segmentStatus.Message);
      }
      catch (Exception ex)
      {
        this.ClearAutoSplitter();
        if (this.StatusWindow.IsDisposed)
          return;
        this.StatusWindow.SetErrorStatus(ex.Message);
      }
    }

    public void Dispose()
    {
      this.ClearAutoSplitter();
      if (!this.StatusWindow.IsDisposed)
      {
        this.StatusWindow.Hide();
        this.StatusWindow.Dispose();
      }
      this.Settings.Hide();
      this.Settings.Dispose();
    }
  }
}
