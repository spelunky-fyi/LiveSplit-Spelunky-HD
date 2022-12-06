

using LiveSplit.Spelunky.Properties;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace LiveSplit.Spelunky
{
  public class JournalVisualizer : Control
  {
    private JournalState MaybeJournal;
    private const int EntriesPerColumn = 12;
    private const int EntryMarginPx = 3;

    public JournalVisualizer() => this.DoubleBuffered = true;

    public void SetJournal(JournalState journal)
    {
      if (this.MaybeJournal != null && this.MaybeJournal.Equals(journal))
        return;
      this.MaybeJournal = journal;
      this.Invalidate();
    }

    protected override void OnPaint(PaintEventArgs e)
    {
      base.OnPaint(e);
      if (this.MaybeJournal == null)
        return;
      JournalState maybeJournal = this.MaybeJournal;
      JournalVisualizer.JournalPainter journalPainter = new JournalVisualizer.JournalPainter(e.Graphics, 12, 3, this.Height);
      journalPainter.ColumnRenderWidth = journalPainter.EntryRenderHeightPx * Resources.Place_0.Width / Resources.Place_0.Height;
      foreach (int entryIndex in Enumerable.Range(0, JournalState.NumPlaceEntries))
        journalPainter.DrawEntry("Place", entryIndex, maybeJournal.PlaceEntries[entryIndex]);
      journalPainter.NewColumn();
      journalPainter.ColumnRenderWidth = journalPainter.EntryRenderHeightPx * Resources.Monster_0.Width / Resources.Monster_0.Height;
      foreach (int entryIndex in Enumerable.Range(0, JournalState.NumMonsterEntries))
        journalPainter.DrawEntry("Monster", entryIndex, maybeJournal.MonsterEntries[entryIndex]);
      journalPainter.NewColumn();
      foreach (int entryIndex in Enumerable.Range(0, JournalState.NumItemEntries))
        journalPainter.DrawEntry("Item", entryIndex, maybeJournal.ItemEntries[entryIndex]);
      journalPainter.NewColumn();
      foreach (int entryIndex in Enumerable.Range(0, JournalState.NumTrapEntries))
        journalPainter.DrawEntry("Trap", entryIndex, maybeJournal.TrapEntries[entryIndex]);
    }

    private class JournalPainter
    {
      private Graphics Graphics;
      private Point CurrentEntryDrawPosition;
      public int ColumnRenderWidth;
      private int Height;

      public int EntryRenderHeightPx { get; }

      public int EntryMarginPx { get; }

      public JournalPainter(
        Graphics graphics,
        int entriesPerColumn,
        int entryMarginPx,
        int height)
      {
        this.Graphics = graphics;
        this.Height = height;
        this.EntryMarginPx = entryMarginPx;
        this.EntryRenderHeightPx = (this.Height - (entriesPerColumn + 1) * this.EntryMarginPx) / entriesPerColumn;
        this.CurrentEntryDrawPosition = new Point(this.EntryMarginPx, this.EntryMarginPx);
        this.ColumnRenderWidth = 0;
      }

      public void DrawEntry(string entryType, int entryIndex, bool entryIsUnlocked)
      {
        Image image = (Image) Resources.ResourceManager.GetObject(string.Format("{0}_{1}", (object) entryType, (object) entryIndex) + (!entryIsUnlocked ? "" : "_Inactive"));
        double num = (double) this.EntryRenderHeightPx / (double) image.Height;
        this.Graphics.DrawImage(image, this.CurrentEntryDrawPosition.X, this.CurrentEntryDrawPosition.Y, this.ColumnRenderWidth, this.EntryRenderHeightPx);
        this.AdvanceToNextCell();
      }

      private void AdvanceToNextCell()
      {
        this.CurrentEntryDrawPosition.Y += this.EntryRenderHeightPx + this.EntryMarginPx;
        if (this.CurrentEntryDrawPosition.Y + this.EntryRenderHeightPx + this.EntryMarginPx < this.Height)
          return;
        this.NewColumn();
      }

      public void NewColumn()
      {
        this.CurrentEntryDrawPosition.Y = this.EntryMarginPx;
        this.CurrentEntryDrawPosition.X += this.ColumnRenderWidth + this.EntryMarginPx;
      }
    }
  }
}
