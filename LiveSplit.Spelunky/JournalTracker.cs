

using LiveSplit.Spelunky.Utilities.Forms;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace LiveSplit.Spelunky
{
  public class JournalTracker : SquareResizableForm
  {
    public static readonly Size InitialClientSize = new Size(348, 340);
    private JournalVisualizer JournalVisualizer;
    private IContainer components;

    public JournalTracker() : base((double)JournalTracker.InitialClientSize.Height / (double)JournalTracker.InitialClientSize.Width)
        {
      this.JournalVisualizer = new JournalVisualizer();
      this.InitializeComponent();
      MenuUtils.DisableCloseButton((Form) this);
      this.JournalVisualizer.Location = new Point(0, 0);
      this.JournalVisualizer.Size = this.ClientSize;
      this.Controls.Add((Control) this.JournalVisualizer);
    }

    protected override void OnResize(EventArgs e)
    {
      base.OnResize(e);
      this.JournalVisualizer.Invalidate();
      this.JournalVisualizer.Size = this.ClientSize;
    }

    public void Update(SpelunkyHooks spelunky) => this.JournalVisualizer.SetJournal(spelunky.JournalState);

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
      this.ClientSize = JournalTracker.InitialClientSize;
      this.FormBorderStyle = FormBorderStyle.FixedSingle;
      this.Name = "JournalTracker";
      this.ShowIcon = false;
      this.Text = "Remaining Journal Entries";
      this.ResumeLayout(false);
    }
  }
}
