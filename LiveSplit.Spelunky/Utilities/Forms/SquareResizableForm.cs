

using System;
using System.Drawing;
using System.Windows.Forms;

namespace LiveSplit.Spelunky.Utilities.Forms
{
  public class SquareResizableForm : Form
  {
    private double DesiredClientHeightWidthRatio;

    public SquareResizableForm(double desiredClientHeightWidthRatio) => this.DesiredClientHeightWidthRatio = desiredClientHeightWidthRatio;

    protected override void OnResize(EventArgs e)
    {
      base.OnResize(e);
      Size clientSize = this.ClientSize;
      double height = (double) clientSize.Height;
      clientSize = this.ClientSize;
      double width = (double) clientSize.Width;
      if (Math.Abs(this.DesiredClientHeightWidthRatio - height / width) < double.Epsilon)
        return;
      this.ClientSize = new Size(this.ClientSize.Width, (int) Math.Ceiling(this.DesiredClientHeightWidthRatio * (double) this.ClientSize.Width));
    }
  }
}
