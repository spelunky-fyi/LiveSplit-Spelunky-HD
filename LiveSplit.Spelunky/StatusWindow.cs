

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace LiveSplit.Spelunky
{
  public class StatusWindow : Form
  {
    private SegmentStatusType? LastStatusType;
    private IContainer components;
    private Label CurrentRunPrefixLabel;
    private Label StatusPrefixLabel;
    private Label CurrentRunLabel;
    private Label StatusLabel;

    public StatusWindow()
    {
      this.InitializeComponent();
      this.CurrentRunLabel.Text = "";
      this.StatusLabel.Text = "";
    }

    public string CurrentRun
    {
      get => this.CurrentRunLabel.Text;
      set => this.CurrentRunLabel.Text = value;
    }

    public void SetStatus(SegmentStatusType type, string text)
    {
      if (type != SegmentStatusType.INFO)
      {
        if (type != SegmentStatusType.ERROR)
          return;
        this.SetErrorStatus(text);
      }
      else
        this.SetInfoStatus(text);
    }

    public void SetInfoStatus(string text)
    {
      if (this.InvokeRequired)
      {
        this.BeginInvoke(new MethodInvoker(delegate ()
        {
            this.SetInfoStatus(text);
        }));
      }
      else
      {
        SegmentStatusType? lastStatusType = this.LastStatusType;
        SegmentStatusType segmentStatusType = SegmentStatusType.INFO;
        if ((lastStatusType.GetValueOrDefault() == segmentStatusType ? (lastStatusType.HasValue ? 1 : 0) : 0) != 0 && this.StatusLabel.Text.Equals(text))
          return;
        this.StatusLabel.ForeColor = Color.Black;
        this.StatusLabel.Text = text;
        this.LastStatusType = new SegmentStatusType?(SegmentStatusType.INFO);
      }
    }

    public void SetErrorStatus(string text)
    {
      if (this.InvokeRequired)
      {
        this.BeginInvoke(new MethodInvoker(delegate ()
        {
            this.SetErrorStatus(text);
        }));
      }
      else
      {
        SegmentStatusType? lastStatusType = this.LastStatusType;
        SegmentStatusType segmentStatusType = SegmentStatusType.ERROR;
        if ((lastStatusType.GetValueOrDefault() == segmentStatusType ? (lastStatusType.HasValue ? 1 : 0) : 0) != 0 && this.StatusLabel.Text.Equals(text))
          return;
        this.StatusLabel.ForeColor = Color.Red;
        this.StatusLabel.Text = text;
        this.LastStatusType = new SegmentStatusType?(SegmentStatusType.ERROR);
      }
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager resources = new ComponentResourceManager(typeof (StatusWindow));
      this.CurrentRunPrefixLabel = new Label();
      this.StatusPrefixLabel = new Label();
      this.CurrentRunLabel = new Label();
      this.StatusLabel = new Label();
      this.SuspendLayout();
      this.CurrentRunPrefixLabel.AutoSize = true;
      this.CurrentRunPrefixLabel.Location = new Point(4, 9);
      this.CurrentRunPrefixLabel.Name = "CurrentRunPrefixLabel";
      this.CurrentRunPrefixLabel.Size = new Size(67, 13);
      this.CurrentRunPrefixLabel.TabIndex = 0;
      this.CurrentRunPrefixLabel.Text = "Current Run:";
      this.StatusPrefixLabel.AutoSize = true;
      this.StatusPrefixLabel.Location = new Point(4, 32);
      this.StatusPrefixLabel.Name = "StatusPrefixLabel";
      this.StatusPrefixLabel.Size = new Size(40, 13);
      this.StatusPrefixLabel.TabIndex = 1;
      this.StatusPrefixLabel.Text = "Status:";
      this.CurrentRunLabel.AutoSize = true;
      this.CurrentRunLabel.Location = new Point(70, 9);
      this.CurrentRunLabel.Name = "CurrentRunLabel";
      this.CurrentRunLabel.Size = new Size(98, 13);
      this.CurrentRunLabel.TabIndex = 2;
      this.CurrentRunLabel.Text = "#CURRENTRUN#";
      this.StatusLabel.AutoSize = true;
      this.StatusLabel.Location = new Point(44, 32);
      this.StatusLabel.Name = "StatusLabel";
      this.StatusLabel.Size = new Size(64, 13);
      this.StatusLabel.TabIndex = 3;
      this.StatusLabel.Text = "#STATUS#";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(431, 57);
      this.Controls.Add((Control) this.StatusLabel);
      this.Controls.Add((Control) this.CurrentRunLabel);
      this.Controls.Add((Control) this.StatusPrefixLabel);
      this.Controls.Add((Control) this.CurrentRunPrefixLabel);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.Icon = (Icon) resources.GetObject("$this.Icon");
      this.MaximizeBox = false;
      this.Name = "StatusWindow";
      this.Text = "SpelunkySplitter Activity";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
