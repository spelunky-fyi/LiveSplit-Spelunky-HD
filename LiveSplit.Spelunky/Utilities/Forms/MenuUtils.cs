

using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace LiveSplit.Spelunky.Utilities.Forms
{
  public static class MenuUtils
  {
    private const int MF_BYPOSITION = 1024;

    [DllImport("User32.dll")]
    private static extern int RemoveMenu(IntPtr hMenu, int nPosition, int wFlags);

    [DllImport("User32.dll")]
    private static extern int GetSystemMenu(IntPtr hWnd, bool bRevert);

    [DllImport("User32.dll")]
    private static extern int GetMenuItemCount(IntPtr hWnd);

    public static void DisableCloseButton(Form form)
    {
      IntPtr systemMenu = (IntPtr) MenuUtils.GetSystemMenu(form.Handle, false);
      MenuUtils.RemoveMenu(systemMenu, MenuUtils.GetMenuItemCount(systemMenu) - 1, 1024);
    }
  }
}
