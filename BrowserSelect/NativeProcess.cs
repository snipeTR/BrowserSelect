using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace BrowserSelect
{
    /// <summary>
    /// Small Win32 helpers for inspecting processes: the application that launched BrowserSelect
    /// (used by "Source App" rules) and the executables that are currently running
    /// (used by the "display running browsers only" option).
    /// </summary>
    internal static class NativeProcess
    {
        private const int ProcessBasicInformation = 0;
        private const uint PROCESS_QUERY_LIMITED_INFORMATION = 0x1000;
        private const int VK_MENU = 0x12;

        [StructLayout(LayoutKind.Sequential)]
        private struct PROCESS_BASIC_INFORMATION
        {
            public IntPtr ExitStatus;
            public IntPtr PebBaseAddress;
            public IntPtr AffinityMask;
            public IntPtr BasePriority;
            public IntPtr UniqueProcessId;
            public IntPtr InheritedFromUniqueProcessId;
        }

        [DllImport("ntdll.dll")]
        private static extern int NtQueryInformationProcess(IntPtr processHandle, int processInformationClass,
            ref PROCESS_BASIC_INFORMATION processInformation, int processInformationLength, out int returnLength);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr OpenProcess(uint desiredAccess, bool inheritHandle, int processId);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool CloseHandle(IntPtr handle);

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool QueryFullProcessImageName(IntPtr process, int flags, StringBuilder exeName,
            ref int size);

        [DllImport("user32.dll")]
        private static extern short GetAsyncKeyState(int vKey);

        /// <summary>true if the Alt key is physically held down right now.</summary>
        public static bool IsAltKeyDown()
        {
            try
            {
                return (GetAsyncKeyState(VK_MENU) & 0x8000) != 0;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>Full path of the executable of a process, or null if it cannot be queried.</summary>
        public static string GetExecutablePath(int processId)
        {
            IntPtr handle = OpenProcess(PROCESS_QUERY_LIMITED_INFORMATION, false, processId);
            if (handle == IntPtr.Zero)
                return null;
            try
            {
                var buffer = new StringBuilder(1024);
                int size = buffer.Capacity;
                return QueryFullProcessImageName(handle, 0, buffer, ref size) ? buffer.ToString(0, size) : null;
            }
            finally
            {
                CloseHandle(handle);
            }
        }

        private static int GetParentProcessId(Process process)
        {
            var pbi = new PROCESS_BASIC_INFORMATION();
            int returnLength;
            int status = NtQueryInformationProcess(process.Handle, ProcessBasicInformation, ref pbi,
                Marshal.SizeOf(pbi), out returnLength);
            if (status != 0)
                return -1;
            return pbi.InheritedFromUniqueProcessId.ToInt32();
        }

        // processes that only relay a link to the real handler; skip them to find the actual source
        private static readonly HashSet<string> Relays = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "cmd.exe", "rundll32.exe", "openwith.exe", "conhost.exe"
        };

        /// <summary>
        /// Returns the full executable path of the application that launched BrowserSelect
        /// (e.g. C:\...\OUTLOOK.EXE), skipping known relay processes. Returns null if unknown.
        /// </summary>
        public static string GetSourceApplicationPath()
        {
            try
            {
                Process current = Process.GetCurrentProcess();
                DateTime childStart = current.StartTime;
                Process child = current;
                for (int depth = 0; depth < 4; depth++)
                {
                    int parentId = GetParentProcessId(child);
                    if (parentId <= 0)
                        return null;
                    Process parent;
                    try
                    {
                        parent = Process.GetProcessById(parentId);
                    }
                    catch (ArgumentException)
                    {
                        return null; // parent already exited
                    }

                    // a parent that started after its child is a recycled PID, not the real parent
                    try
                    {
                        if (parent.StartTime > childStart)
                            return null;
                        childStart = parent.StartTime;
                    }
                    catch
                    {
                        // access denied for StartTime on protected processes; accept the parent
                    }

                    string path = GetExecutablePath(parentId) ?? (parent.ProcessName + ".exe");
                    if (!Relays.Contains(Path.GetFileName(path)))
                        return path;
                    child = parent;
                }
            }
            catch
            {
                // never let source detection break opening a link
            }
            return null;
        }

        /// <summary>Set of full executable paths of all processes we are allowed to query.</summary>
        public static HashSet<string> GetRunningExecutables()
        {
            var result = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (Process process in Process.GetProcesses())
            {
                try
                {
                    string path = GetExecutablePath(process.Id);
                    if (!string.IsNullOrEmpty(path))
                        result.Add(path);
                    // also keep the bare process name to match browsers that can't be resolved to a path
                    result.Add(process.ProcessName + ".exe");
                }
                catch
                {
                    // process exited or is inaccessible
                }
                finally
                {
                    process.Dispose();
                }
            }
            return result;
        }
    }
}
