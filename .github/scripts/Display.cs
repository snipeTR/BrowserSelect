// Display helpers for the UI screenshot workflow: screen resolution and display scale (DPI) of the
// primary monitor, changed at run time the same way Settings > Display > Scale does (no sign-out).
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

public static class Disp
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct DEVMODE
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)] public string dmDeviceName;
        public short dmSpecVersion, dmDriverVersion, dmSize, dmDriverExtra;
        public int dmFields;
        public int dmPositionX, dmPositionY, dmDisplayOrientation, dmDisplayFixedOutput;
        public short dmColor, dmDuplex, dmYResolution, dmTTOption, dmCollate;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)] public string dmFormName;
        public short dmLogPixels;
        public int dmBitsPerPel, dmPelsWidth, dmPelsHeight, dmDisplayFlags, dmDisplayFrequency;
        public int dmICMMethod, dmICMIntent, dmMediaType, dmDitherType, dmReserved1, dmReserved2, dmPanningWidth, dmPanningHeight;
    }

    [DllImport("user32.dll", CharSet = CharSet.Unicode)] static extern bool EnumDisplaySettings(string dev, int mode, ref DEVMODE dm);
    [DllImport("user32.dll", CharSet = CharSet.Unicode)] static extern int ChangeDisplaySettings(ref DEVMODE dm, int flags);
    [DllImport("user32.dll")] static extern IntPtr SetProcessDpiAwarenessContext(IntPtr ctx);
    [DllImport("user32.dll")] static extern IntPtr GetThreadDpiAwarenessContext();
    [DllImport("user32.dll")] static extern int GetAwarenessFromDpiAwarenessContext(IntPtr ctx);
    [DllImport("user32.dll")] static extern IntPtr MonitorFromPoint(POINT pt, uint flags);
    [DllImport("shcore.dll")] static extern int GetDpiForMonitor(IntPtr mon, int type, out uint x, out uint y);
    [DllImport("user32.dll")] static extern uint GetDpiForWindow(IntPtr hwnd);
    [StructLayout(LayoutKind.Sequential)] public struct POINT { public int X, Y; }

    [StructLayout(LayoutKind.Sequential)] public struct HDR { public int type; public uint size; public uint low; public int high; public uint id; }
    [StructLayout(LayoutKind.Sequential)] public struct GETSCALE { public HDR h; public int min, cur, max; }
    [StructLayout(LayoutKind.Sequential)] public struct SETSCALE { public HDR h; public int rel; }
    [DllImport("user32.dll")] static extern int GetDisplayConfigBufferSizes(uint flags, out uint paths, out uint modes);
    [DllImport("user32.dll")] static extern int QueryDisplayConfig(uint flags, ref uint numPaths, byte[] paths, ref uint numModes, byte[] modes, IntPtr topology);
    [DllImport("user32.dll")] static extern int DisplayConfigGetDeviceInfo(ref GETSCALE p);
    [DllImport("user32.dll")] static extern int DisplayConfigSetDeviceInfo(ref SETSCALE p);

    public static readonly int[] Scales = { 100, 125, 150, 175, 200, 225, 250, 300, 350, 400, 450, 500 };

    /// <summary>makes this process per-monitor DPI aware so window rectangles and screenshots are in physical pixels</summary>
    public static string MakeDpiAware()
    {
        SetProcessDpiAwarenessContext(new IntPtr(-4)); // PER_MONITOR_AWARE_V2 (fails harmlessly if already set)
        return "awareness=" + GetAwarenessFromDpiAwarenessContext(GetThreadDpiAwarenessContext());
    }

    public static string Resolution()
    {
        var dm = new DEVMODE(); dm.dmSize = (short)Marshal.SizeOf(typeof(DEVMODE));
        EnumDisplaySettings(null, -1, ref dm);
        return dm.dmPelsWidth + "x" + dm.dmPelsHeight;
    }

    public static string Modes()
    {
        var set = new SortedSet<string>();
        var dm = new DEVMODE(); dm.dmSize = (short)Marshal.SizeOf(typeof(DEVMODE));
        for (int i = 0; EnumDisplaySettings(null, i, ref dm); i++)
            set.Add(dm.dmPelsWidth.ToString("D4") + "x" + dm.dmPelsHeight);
        return string.Join(" ", set);
    }

    public static int SetResolution(int w, int h)
    {
        var dm = new DEVMODE(); dm.dmSize = (short)Marshal.SizeOf(typeof(DEVMODE));
        EnumDisplaySettings(null, -1, ref dm);
        dm.dmPelsWidth = w; dm.dmPelsHeight = h; dm.dmFields = 0x80000 | 0x100000;
        return ChangeDisplaySettings(ref dm, 0); // 0 = DISP_CHANGE_SUCCESSFUL
    }

    public static uint MonitorDpi()
    {
        uint x, y;
        GetDpiForMonitor(MonitorFromPoint(new POINT(), 1), 0, out x, out y);
        return x;
    }

    public static uint WindowDpi(IntPtr h) { return GetDpiForWindow(h); }

    static HDR Source()
    {
        uint np, nm;
        GetDisplayConfigBufferSizes(2, out np, out nm);
        var paths = new byte[np * 72]; var modes = new byte[nm * 64];
        int r = QueryDisplayConfig(2, ref np, paths, ref nm, modes, IntPtr.Zero);
        if (r != 0) throw new Exception("QueryDisplayConfig " + r);
        return new HDR { low = BitConverter.ToUInt32(paths, 0), high = BitConverter.ToInt32(paths, 4), id = BitConverter.ToUInt32(paths, 8) };
    }

    /// <summary>"recommended/current/max" scale in percent</summary>
    public static string ScaleInfo()
    {
        var g = new GETSCALE(); g.h = Source(); g.h.type = -3; g.h.size = (uint)Marshal.SizeOf(typeof(GETSCALE));
        int r = DisplayConfigGetDeviceInfo(ref g);
        if (r != 0) return "get failed " + r;
        int rec = -g.min;
        return "recommended=" + Scales[rec] + " current=" + Scales[rec + g.cur] + " max=" + Scales[Math.Min(Scales.Length - 1, rec + g.max)];
    }

    public static int SetScale(int percent)
    {
        var g = new GETSCALE(); g.h = Source(); g.h.type = -3; g.h.size = (uint)Marshal.SizeOf(typeof(GETSCALE));
        int r = DisplayConfigGetDeviceInfo(ref g);
        if (r != 0) return r;
        int rec = -g.min, target = Array.IndexOf(Scales, percent);
        if (target < 0) return -1;
        var s = new SETSCALE(); s.h = Source(); s.h.type = -4; s.h.size = (uint)Marshal.SizeOf(typeof(SETSCALE));
        s.rel = target - rec;
        return DisplayConfigSetDeviceInfo(ref s);
    }
}
