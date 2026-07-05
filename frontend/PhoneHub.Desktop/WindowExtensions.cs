using System.Diagnostics;
using System.Runtime.InteropServices;

namespace PhoneHub.Desktop
{
    public static class WindowExtensions
    {
        [DllImport("user32.dll")]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        private const int GWL_STYLE = -16;
        private const int WS_SYSMENU = 0x80000;

        public static void RemoveSystemMenu(this Window window)
        {
            var hwnd = WindowNative.GetWindowHandle(window);
            var style = GetWindowLong(hwnd, GWL_STYLE);
            SetWindowLong(hwnd, GWL_STYLE, style & ~WS_SYSMENU);
        }
    }
}
