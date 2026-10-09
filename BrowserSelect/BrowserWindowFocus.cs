using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using BrowserSelect.Properties;

namespace BrowserSelect
{
    /// <summary>
    /// "Avoid full-screen windows when opening links" (Settings > Options).
    /// Browsers send a new link to their last active window. If that window is playing a full-screen
    /// video on another monitor, the link would interrupt it. Just before the browser is started, the
    /// browser's most recently used window that is NOT full screen is brought to the front, so the
    /// browser opens the link there. If every window of the browser is full screen, the fallback setting
    /// decides: the window on the primary monitor, or nothing (the browser's last used window).
    /// </summary>
    internal static class BrowserWindowFocus
    {
        /// <summary>fallback values (Settings.Default.FullscreenFallback)</summary>
        public const string FallbackPrimary = "Primary";
        public const string FallbackLastUsed = "LastUsed";
        public static readonly string[] Fallbacks = { FallbackPrimary, FallbackLastUsed };

        private const int SW_RESTORE = 9;
        private const int GWL_STYLE = -16;
        private const int GWL_EXSTYLE = -20;
        private const uint GW_OWNER = 4;
        private const long WS_CAPTION = 0x00C00000L;
        private const long WS_EX_TOOLWINDOW = 0x00000080L;
        private const int DWMWA_EXTENDED_FRAME_BOUNDS = 9;
        private const int DWMWA_CLOAKED = 14;

        [StructLayout(LayoutKind.Sequential)]
        private struct RECT { public int Left, Top, Right, Bottom; }

        private delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

        [DllImport("user32.dll")]
        private static extern bool EnumWindows(EnumWindowsProc callback, IntPtr lParam);

        [DllImport("user32.dll")]
        private static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool IsIconic(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern IntPtr GetWindow(IntPtr hWnd, uint cmd);

        [DllImport("user32.dll")]
        private static extern int GetWindowTextLength(IntPtr hWnd);

        [DllImport("user32.dll", EntryPoint = "GetWindowLongPtr")]
        private static extern IntPtr GetWindowLongPtr64(IntPtr hWnd, int index);

        [DllImport("user32.dll", EntryPoint = "GetWindowLong")]
        private static extern IntPtr GetWindowLong32(IntPtr hWnd, int index);

        [DllImport("user32.dll")]
        private static extern bool GetWindowRect(IntPtr hWnd, out RECT rect);

        [DllImport("dwmapi.dll")]
        private static extern int DwmGetWindowAttribute(IntPtr hWnd, int attribute, out RECT value, int size);

        [DllImport("dwmapi.dll")]
        private static extern int DwmGetWindowAttribute(IntPtr hWnd, int attribute, out int value, int size);

        [DllImport("user32.dll")]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint processId);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool ShowWindow(IntPtr hWnd, int command);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        /// <summary>
        /// Command line flags that make the browser open a NEW window (the link never goes to an existing
        /// window, so there is nothing to focus).
        /// </summary>
        private static readonly string[] NewWindowFlags =
        {
            "--new-window", "-new-window", "--incognito", "-incognito", "--inprivate", "-inprivate",
            "-private-window", "--private-window", "-private", "--private", "--guest", "--app",
            "--kiosk", "-kiosk", "--start-fullscreen", "--tor"
        };

        /// <summary>true if one of the arguments opens a new browser window (see <see cref="NewWindowFlags"/>)</summary>
        public static bool OpensNewWindow(IEnumerable<string> arguments)
        {
            foreach (var raw in arguments ?? Enumerable.Empty<string>())
            {
                if (string.IsNullOrWhiteSpace(raw))
                    continue;
                foreach (var part in Form1.SplitArguments(raw))
                {
                    var flag = part.Split('=')[0].Trim();
                    if (NewWindowFlags.Any(f => f.Equals(flag, StringComparison.OrdinalIgnoreCase)))
                        return true;
                }
            }
            return false;
        }

        /// <summary>a visible top-level window of the browser</summary>
        internal class BrowserWindow
        {
            public IntPtr Handle;
            /// <summary>0 = most recently used (top of the Z-order)</summary>
            public int ZOrder;
            public bool Minimized;
            public bool FullScreen;
            public Screen Screen;

            public override string ToString()
            {
                return string.Format("0x{0:X} z={1} min={2} full={3} screen={4}", Handle.ToInt64(), ZOrder,
                    Minimized, FullScreen, Screen == null ? "?" : Screen.DeviceName);
            }
        }

        /// <summary>
        /// Called right before the browser is started with the link (never for private mode or arguments
        /// that open a new window). Does nothing when the setting is off or the browser is not running.
        /// </summary>
        public static void PrepareForLink(string executablePath)
        {
            try
            {
                if (!Settings.Default.AvoidFullscreen || string.IsNullOrEmpty(executablePath) ||
                    executablePath.StartsWith("shell:", StringComparison.OrdinalIgnoreCase))
                    return;
                var target = ChooseWindow(FindWindows(executablePath), Settings.Default.FullscreenFallback);
                if (target != null)
                    FocusWindow(target);
            }
            catch (Exception)
            {
                // focusing is only a convenience; the link is opened anyway
            }
        }

        /// <summary>
        /// The window that should receive the link, or null to leave it to the browser.
        /// <paramref name="windows"/> must be in Z-order (most recently used first).
        /// </summary>
        internal static BrowserWindow ChooseWindow(IList<BrowserWindow> windows, string fallback)
        {
            if (windows == null || windows.Count == 0)
                return null; // browser not running: it opens a new window anyway
            var normal = windows.Where(w => !w.FullScreen).OrderBy(w => w.ZOrder).ToList();
            // most recently used window that is not full screen; a minimized one only if there is no other
            var target = normal.FirstOrDefault(w => !w.Minimized) ?? normal.FirstOrDefault();
            if (target != null)
                return target;
            // every window is full screen
            if (string.Equals(fallback, FallbackLastUsed, StringComparison.OrdinalIgnoreCase))
                return null;
            var primary = Screen.PrimaryScreen;
            return windows.OrderBy(w => w.ZOrder)
                .FirstOrDefault(w => w.Screen != null && primary != null && w.Screen.DeviceName == primary.DeviceName);
        }

        /// <summary>visible top-level windows of the browser executable, most recently used first</summary>
        internal static List<BrowserWindow> FindWindows(string executablePath)
        {
            var result = new List<BrowserWindow>();
            var pathByProcess = new Dictionary<uint, string>();
            int z = 0;
            EnumWindows(delegate (IntPtr hWnd, IntPtr lParam)
            {
                int order = z++;
                if (!IsCandidate(hWnd))
                    return true;
                uint processId;
                GetWindowThreadProcessId(hWnd, out processId);
                string path;
                if (!pathByProcess.TryGetValue(processId, out path))
                {
                    path = NativeProcess.GetExecutablePath((int)processId);
                    pathByProcess[processId] = path;
                }
                if (path == null || !string.Equals(path, executablePath, StringComparison.OrdinalIgnoreCase))
                    return true;
                bool minimized = IsIconic(hWnd);
                result.Add(new BrowserWindow
                {
                    Handle = hWnd,
                    ZOrder = order,
                    Minimized = minimized,
                    FullScreen = !minimized && IsFullScreen(hWnd),
                    Screen = Screen.FromHandle(hWnd)
                });
                return true;
            }, IntPtr.Zero);
            return result;
        }

        /// <summary>visible, unowned, not a tool window, not cloaked (other virtual desktop), has a title and a size</summary>
        private static bool IsCandidate(IntPtr hWnd)
        {
            if (!IsWindowVisible(hWnd))
                return false;
            if (GetWindow(hWnd, GW_OWNER) != IntPtr.Zero)
                return false;
            if ((GetStyle(hWnd, GWL_EXSTYLE) & WS_EX_TOOLWINDOW) != 0)
                return false;
            int cloaked;
            if (DwmGetWindowAttribute(hWnd, DWMWA_CLOAKED, out cloaked, sizeof(int)) == 0 && cloaked != 0)
                return false;
            if (GetWindowTextLength(hWnd) == 0)
                return false;
            if (IsIconic(hWnd))
                return true;
            var r = GetBounds(hWnd);
            return r.Width > 0 && r.Height > 0;
        }

        private static long GetStyle(IntPtr hWnd, int index)
        {
            return (IntPtr.Size == 8 ? GetWindowLongPtr64(hWnd, index) : GetWindowLong32(hWnd, index)).ToInt64();
        }

        /// <summary>visible bounds of the window (DWM extended frame bounds, without the invisible resize borders)</summary>
        private static Rectangle GetBounds(IntPtr hWnd)
        {
            RECT r;
            if (DwmGetWindowAttribute(hWnd, DWMWA_EXTENDED_FRAME_BOUNDS, out r, Marshal.SizeOf(typeof(RECT))) != 0)
                GetWindowRect(hWnd, out r);
            return Rectangle.FromLTRB(r.Left, r.Top, r.Right, r.Bottom);
        }

        /// <summary>
        /// Full screen = the window covers the whole monitor (Bounds, including the taskbar area) and has no
        /// title bar (WS_CAPTION). A maximized window with an auto-hidden taskbar also covers the whole
        /// monitor, but it keeps its title bar style, so it is not treated as full screen.
        /// </summary>
        public static bool IsFullScreen(IntPtr hWnd)
        {
            var bounds = GetBounds(hWnd);
            var monitor = Screen.FromHandle(hWnd).Bounds;
            long style = GetStyle(hWnd, GWL_STYLE);
            return IsFullScreen(bounds, monitor, style);
        }

        /// <summary>the decision of <see cref="IsFullScreen(IntPtr)"/> from the raw values (testable)</summary>
        internal static bool IsFullScreen(Rectangle window, Rectangle monitor, long style)
        {
            if (!window.Contains(monitor))
                return false;
            // full-screen browsers (F11, video/presentation mode) remove the title bar and sizing frame;
            // a maximized window keeps WS_CAPTION even when it covers the monitor (auto-hidden taskbar)
            bool hasCaption = (style & WS_CAPTION) == WS_CAPTION;
            return !hasCaption;
        }

        private static void FocusWindow(BrowserWindow window)
        {
            if (window.Handle == IntPtr.Zero)
                return;
            // SW_RESTORE returns minimized windows to their previous normal/maximized placement.
            // Leave non-minimized windows untouched to preserve their state.
            if (IsIconic(window.Handle))
                ShowWindow(window.Handle, SW_RESTORE);
            SetForegroundWindow(window.Handle);
        }
    }
}
