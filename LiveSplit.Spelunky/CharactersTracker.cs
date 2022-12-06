

using LiveSplit.Spelunky.Utilities.Forms;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace LiveSplit.Spelunky
{
  public class CharactersTracker : SquareResizableForm
  {
    public static readonly Size InitialClientSize = new Size(640, 40);
    private CharactersVisualizer CharactersVisualizer;
    private IContainer components;

    public CharactersTracker()
    {
      Size initialClientSize = CharactersTracker.InitialClientSize;
      double height = (double) initialClientSize.Height;
      initialClientSize = CharactersTracker.InitialClientSize;
      double width = (double) initialClientSize.Width;
      // ISSUE: explicit constructor call
      base.\u002Ector(height / width);
      this.CharactersVisualizer = new CharactersVisualizer();
      this.InitializeComponent();
      MenuUtils.DisableCloseButton((Form) this);
      this.CharactersVisualizer.Location = new Point(0, 0);
      this.CharactersVisualizer.Size = this.ClientSize;
      this.Controls.Add((Control) this.CharactersVisualizer);
    }

    protected override void OnResize(EventArgs e)
    {
      base.OnResize(e);
      this.CharactersVisualizer.Invalidate();
      this.CharactersVisualizer.Size = this.ClientSize;
    }

    public void Update(SpelunkyHooks spelunky) => this.CharactersVisualizer.SetCharacters(spelunky.CharactersState);

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.SuspendLayout();
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = SystemColors.ActiveCaptionText;
      this.ClientSize = CharactersTracker.InitialClientSize;
      this.FormBorderStyle = FormBorderStyle.FixedSingle;
      this.Name = nameof (CharactersTracker);
      this.ShowIcon = false;
      this.Text = "Remaining Locked Characters";
      this.ResumeLayout(false);
    }
  }
}
