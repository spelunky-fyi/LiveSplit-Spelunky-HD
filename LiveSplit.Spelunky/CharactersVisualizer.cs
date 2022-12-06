

using LiveSplit.Spelunky.Properties;
using System.Drawing;
using System.Windows.Forms;

namespace LiveSplit.Spelunky
{
  public class CharactersVisualizer : Control
  {
    private CharactersState MaybeCharacters;
    private const int EntriesPerColumn = 1;
    private const int EntryMarginPx = 0;
    private int[] CharacterOrder = new int[16]
    {
      2,
      4,
      6,
      7,
      9,
      12,
      5,
      11,
      13,
      1,
      8,
      14,
      10,
      3,
      15,
      0
    };

    public CharactersVisualizer() => this.DoubleBuffered = true;

    public void SetCharacters(CharactersState journal)
    {
      if (this.MaybeCharacters != null && this.MaybeCharacters.Equals(journal))
        return;
      this.MaybeCharacters = journal;
      this.Invalidate();
    }

    protected override void OnPaint(PaintEventArgs e)
    {
      base.OnPaint(e);
      if (this.MaybeCharacters == null)
        return;
      CharactersState maybeCharacters = this.MaybeCharacters;
      CharactersVisualizer.CharactersPainter charactersPainter = new CharactersVisualizer.CharactersPainter(e.Graphics, 1, 0, this.Height);
      charactersPainter.ColumnRenderWidth = charactersPainter.EntryRenderHeightPx * Resources.Char_0.Width / Resources.Char_0.Height;
      foreach (int entryIndex in this.CharacterOrder)
        charactersPainter.DrawEntry("Char", entryIndex, maybeCharacters.UnlockedChars[entryIndex]);
    }

    private class CharactersPainter
    {
      private Graphics Graphics;
      private Point CurrentEntryDrawPosition;
      public int ColumnRenderWidth;
      private int Height;

      public int EntryRenderHeightPx { get; }

      public int EntryMarginPx { get; }

      public CharactersPainter(
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
