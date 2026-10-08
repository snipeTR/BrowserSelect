using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace BrowserSelect
{
    /// <summary>Focuses the matching browser window on the leftmost/topmost monitor.</summary>
    internal static class BrowserWindowFocus
    {
        private const int SW_RESTORE = 9;

        private delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

        [DllImport("user32.dll")]
        private static extern bool EnumWindows(EnumWindowsProc callback, IntPtr lParam);

        [DllImport("user32.dll")]
        private static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool IsIconic(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint processId);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool ShowWindow(IntPtr hWnd, int command);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        public static void FocusWindowOnFirstMonitor(string executablePath)
        {
            if (string.IsNullOrEmpty(executablePath))
                return;

            var handles = new List<IntPtr>();
            EnumWindows(delegate(IntPtr hWnd, IntPtr lParam)
            {
                if (!IsWindowVisible(hWnd))
                    return true;

                uint processId;
                GetWindowThreadProcessId(hWnd, out processId);
                try
                {
                    using (Process process = Process.GetProcessById((int)processId))
                    {
                        // MainModule access can fail for elevated/protected processes.
                        if (string.Equals(process.MainModule.FileName, executablePath,
                            StringComparison.OrdinalIgnoreCase))
                            handles.Add(hWnd);
                    }
                }
                catch
                {
                    // Ignore windows whose process exited or cannot be inspected.
                }
                return true;
            }, IntPtr.Zero);

            var target = handles
                .Select(h => new { Handle = h, Screen = Screen.FromHandle(h) })
                .OrderBy(x => x.Screen.Bounds.Left)
                .ThenBy(x => x.Screen.Bounds.Top)
                .FirstOrDefault();

            if (target != null)
            {
                FocusWindow(target.Handle);
            }
        }

        private static void FocusWindow(IntPtr hWnd)
        {
            if (hWnd == IntPtr.Zero)
                return;

            // SW_RESTORE returns minimized windows to their previous normal/maximized
            // placement. Leave non-minimized windows untouched to preserve their state.
            if (IsIconic(hWnd))
                ShowWindow(hWnd, SW_RESTORE);

            SetForegroundWindow(hWnd);
        }
    }
}
