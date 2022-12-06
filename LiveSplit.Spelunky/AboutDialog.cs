

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
        private Label SourceRecoveredByGarebear;
        private Button OKButton;

    public AboutDialog()
    {
      this.InitializeComponent();
      this.TitleLabel.Text = "SpelunkySplitter v" + new Factory().Version.ToString();
      this.OKButton.Click += new EventHandler(this.OKButton_Click);
    }

    private void OKButton_Click(object sender, EventArgs e) => this.Close();

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
            this.TitleLabel = new System.Windows.Forms.Label();
            this.DevelopedBySashavol1 = new System.Windows.Forms.Label();
            this.AllCharContribByKonatoK = new System.Windows.Forms.Label();
            this.OKButton = new System.Windows.Forms.Button();
            this.SourceRecoveredByGarebear = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // TitleLabel
            // 
            this.TitleLabel.AutoSize = true;
            this.TitleLabel.Location = new System.Drawing.Point(13, 13);
            this.TitleLabel.Name = "TitleLabel";
            this.TitleLabel.Size = new System.Drawing.Size(51, 13);
            this.TitleLabel.TabIndex = 0;
            this.TitleLabel.Text = "#TITLE#";
            // 
            // DevelopedBySashavol1
            // 
            this.DevelopedBySashavol1.AutoSize = true;
            this.DevelopedBySashavol1.Location = new System.Drawing.Point(13, 32);
            this.DevelopedBySashavol1.Name = "DevelopedBySashavol1";
            this.DevelopedBySashavol1.Size = new System.Drawing.Size(118, 13);
            this.DevelopedBySashavol1.TabIndex = 1;
            this.DevelopedBySashavol1.Text = "Developed by sashavol";
            // 
            // AllCharContribByKonatoK
            // 
            this.AllCharContribByKonatoK.AutoSize = true;
            this.AllCharContribByKonatoK.Location = new System.Drawing.Point(13, 51);
            this.AllCharContribByKonatoK.Name = "AllCharContribByKonatoK";
            this.AllCharContribByKonatoK.Size = new System.Drawing.Size(194, 13);
            this.AllCharContribByKonatoK.TabIndex = 2;
            this.AllCharContribByKonatoK.Text = "All Characters% contributed by KonatoK";
            // 
            // OKButton
            // 
            this.OKButton.Location = new System.Drawing.Point(168, 96);
            this.OKButton.Name = "OKButton";
            this.OKButton.Size = new System.Drawing.Size(75, 23);
            this.OKButton.TabIndex = 4;
            this.OKButton.Text = "OK";
            this.OKButton.UseVisualStyleBackColor = true;
            // 
            // SourceRecoveredByGarebear
            // 
            this.SourceRecoveredByGarebear.AutoSize = true;
            this.SourceRecoveredByGarebear.Location = new System.Drawing.Point(13, 70);
            this.SourceRecoveredByGarebear.Name = "SourceRecoveredByGarebear";
            this.SourceRecoveredByGarebear.Size = new System.Drawing.Size(213, 13);
            this.SourceRecoveredByGarebear.TabIndex = 5;
            this.SourceRecoveredByGarebear.Text = "Project recovered and updated by garebear";
            // 
            // AboutDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(253, 128);
            this.Controls.Add(this.SourceRecoveredByGarebear);
            this.Controls.Add(this.OKButton);
            this.Controls.Add(this.AllCharContribByKonatoK);
            this.Controls.Add(this.DevelopedBySashavol1);
            this.Controls.Add(this.TitleLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AboutDialog";
            this.Text = "About SpelunkySplitter";
            this.ResumeLayout(false);
            this.PerformLayout();

    }
  }
}
