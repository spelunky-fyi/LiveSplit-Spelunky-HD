

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace LiveSplit.Spelunky
{
  public class AboutDialog : Form
  {
    private IContainer components;
    private Label TitleLabel;
    private Label DevelopedBySashavol1;
    private Label AllCharContribByKonatoK;
    private LinkLabel DevelopedBySashavol2;
    private Button OKButton;

    public AboutDialog()
    {
      this.InitializeComponent();
      this.TitleLabel.Text = "SpelunkySplitter v" + new Factory().Version.ToString();
      this.DevelopedBySashavol2.LinkClicked += new LinkLabelLinkClickedEventHandler(this.DevelopedBySashavol2_LinkClicked);
      this.OKButton.Click += new EventHandler(this.OKButton_Click);
    }

    private void OKButton_Click(object sender, EventArgs e) => this.Close();

    private void DevelopedBySashavol2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) => Process.Start("http://sashavol.com");

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.TitleLabel = new Label();
      this.DevelopedBySashavol1 = new Label();
      this.AllCharContribByKonatoK = new Label();
      this.DevelopedBySashavol2 = new LinkLabel();
      this.OKButton = new Button();
      this.SuspendLayout();
      this.TitleLabel.AutoSize = true;
      this.TitleLabel.Location = new Point(13, 13);
      this.TitleLabel.Name = "TitleLabel";
      this.TitleLabel.Size = new Size(51, 13);
      this.TitleLabel.TabIndex = 0;
      this.TitleLabel.Text = "#TITLE#";
      this.DevelopedBySashavol1.AutoSize = true;
      this.DevelopedBySashavol1.Location = new Point(13, 32);
      this.DevelopedBySashavol1.Name = "DevelopedBySashavol1";
      this.DevelopedBySashavol1.Size = new Size(73, 13);
      this.DevelopedBySashavol1.TabIndex = 1;
      this.DevelopedBySashavol1.Text = "Developed by";
      this.AllCharContribByKonatoK.AutoSize = true;
      this.AllCharContribByKonatoK.Location = new Point(13, 51);
      this.AllCharContribByKonatoK.Name = "AllCharContribByKonatoK";
      this.AllCharContribByKonatoK.Size = new Size(194, 13);
      this.AllCharContribByKonatoK.TabIndex = 2;
      this.AllCharContribByKonatoK.Text = "All Characters% contributed by KonatoK";
      this.DevelopedBySashavol2.AutoSize = true;
      this.DevelopedBySashavol2.Location = new Point(83, 32);
      this.DevelopedBySashavol2.Name = "DevelopedBySashavol2";
      this.DevelopedBySashavol2.Size = new Size(49, 13);
      this.DevelopedBySashavol2.TabIndex = 3;
      this.DevelopedBySashavol2.TabStop = true;
      this.DevelopedBySashavol2.Text = "sashavol";
      this.OKButton.Location = new Point(168, 77);
      this.OKButton.Name = "OKButton";
      this.OKButton.Size = new Size(75, 23);
      this.OKButton.TabIndex = 4;
      this.OKButton.Text = "OK";
      this.OKButton.UseVisualStyleBackColor = true;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(253, 109);
      this.Controls.Add((Control) this.OKButton);
      this.Controls.Add((Control) this.DevelopedBySashavol2);
      this.Controls.Add((Control) this.AllCharContribByKonatoK);
      this.Controls.Add((Control) this.DevelopedBySashavol1);
      this.Controls.Add((Control) this.TitleLabel);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (AboutDialog);
      this.Text = "About SpelunkySplitter";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
