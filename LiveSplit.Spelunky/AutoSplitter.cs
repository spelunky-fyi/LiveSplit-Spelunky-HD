

using LiveSplit.Model;
using System;
using System.Collections.Generic;
using System.IO;

namespace LiveSplit.Spelunky
{
  public class AutoSplitter : IDisposable
  {
    private EnabledPatchContainer Patches;
    private ISegment[] Segments;
    private TimerModel Timer;
    private string AutoSaveLoadOpt;
    private bool SaveLoaded;
    private JournalTracker MaybeJournalTracker;
    private CharactersTracker MaybeCharactersTracker;

    public SpelunkyHooks Hooks { get; private set; }

    public Type CategoryType { get; private set; }

    public string SaveBackupPath => this.Hooks.GameDirectoryPath + "\\Data\\spelunky_save.ss.bak";

    public AutoSplitter(
      SpelunkyHooks hooks,
      Type categoryType,
      EnabledPatchContainer patches,
      TimerModel timer,
      string autoSaveLoadOpt,
      JournalTracker maybeJournalTracker,
      CharactersTracker maybeCharactersTracker)
    {
      this.Hooks = hooks;
      this.CategoryType = categoryType;
      this.Patches = patches;
      this.Segments = Category.NewSegmentInstances(categoryType);
      this.Timer = timer;
      this.AutoSaveLoadOpt = autoSaveLoadOpt;
      this.SaveLoaded = false;
      this.MaybeJournalTracker = maybeJournalTracker;
      this.MaybeCharactersTracker = maybeCharactersTracker;
      this.AssertHooksActive();
    }

    private SegmentStatus UpdateAutoLoad(LiveSplitState state)
    {
      if (state.CurrentSplitIndex == -1)
      {
        if (this.AutoSaveLoadOpt == null || this.SaveLoaded)
          return (SegmentStatus) null;
        if (this.Hooks.CurrentState != SpelunkyState.SplashScreen)
          return new SegmentStatus()
          {
            Type = SegmentStatusType.ERROR,
            Message = "Return to the splash screen (before the main menu) to autoload the save."
          };
        try
        {
          string gameSavePath = this.Hooks.GameSavePath;
          if (File.Exists(gameSavePath) && !File.Exists(this.SaveBackupPath))
            File.Copy(gameSavePath, this.SaveBackupPath);
          File.Copy(this.AutoSaveLoadOpt, gameSavePath, true);
          this.SaveLoaded = true;
          return (SegmentStatus) null;
        }
        catch (Exception ex)
        {
          return new SegmentStatus()
          {
            Type = SegmentStatusType.ERROR,
            Message = "Failed to autoload: " + ex.Message
          };
        }
      }
      else
      {
        this.SaveLoaded = false;
        return (SegmentStatus) null;
      }
    }

    public SegmentStatus Update(LiveSplitState state)
    {
      this.AssertHooksActive();
      if (this.MaybeJournalTracker != null)
      {
        if (!this.MaybeJournalTracker.Visible)
          this.MaybeJournalTracker.Show();
        this.MaybeJournalTracker.Update(this.Hooks);
      }
      if (this.MaybeCharactersTracker != null)
      {
        if (!this.MaybeCharactersTracker.Visible)
          this.MaybeCharactersTracker.Show();
        this.MaybeCharactersTracker.Update(this.Hooks);
      }
      if (((ICollection<ISegment>) state.Run).Count != this.Segments.Length - 1)
        return new SegmentStatus()
        {
          Type = SegmentStatusType.ERROR,
          Message = "Expected " + (object) (this.Segments.Length - 1) + " segments, got " + (object) ((ICollection<ISegment>) state.Run).Count + " (correct this before continuing)."
        };
      if (state.CurrentSplitIndex + 1 >= this.Segments.Length)
        return new SegmentStatus()
        {
          Type = SegmentStatusType.INFO,
          Message = "Run completed!"
        };
      SegmentStatus segmentStatus1 = this.UpdateAutoLoad(state);
      if (segmentStatus1 != null)
        return segmentStatus1;
      AutoSplitter.SplitAction splitAction = state.CurrentSplitIndex != -1 ? (AutoSplitter.SplitAction) (() => this.Timer.Split()) : (AutoSplitter.SplitAction) (() => this.Timer.Start());
      ISegment segment = this.Segments[state.CurrentSplitIndex + 1];
      SegmentStatus segmentStatus2 = segment.CheckStatus(this.Hooks);
      if (segment.Cycle(this.Hooks))
        splitAction();
      return segmentStatus2;
    }

    private void AssertHooksActive()
    {
      if (this.Hooks.Invalidated)
        throw new Exception("Process exited unexpectedly");
    }

    public void Dispose()
    {
      if (!this.Hooks.Process.HasExited)
        this.Patches.RevertAll();
      this.Hooks.Dispose();
      if (this.MaybeJournalTracker != null && this.MaybeJournalTracker.IsHandleCreated)
        this.MaybeJournalTracker.BeginInvoke((Delegate) (() => this.MaybeJournalTracker.Hide()));
      if (this.MaybeCharactersTracker == null || !this.MaybeCharactersTracker.IsHandleCreated)
        return;
      this.MaybeCharactersTracker.BeginInvoke((Delegate) (() => this.MaybeCharactersTracker.Hide()));
    }

    private delegate void SplitAction();
  }
}
