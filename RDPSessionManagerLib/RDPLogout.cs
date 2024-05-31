using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace RDPSessionManager
{
    public class RDPLogout
    {
        [DllImport("wtsapi32.dll", SetLastError = true)]
        private static extern bool WTSEnumerateSessions(IntPtr hServer, int reserved, int version, out IntPtr ppSessionInfo, out int pCount);

        [DllImport("wtsapi32.dll", SetLastError = true)]
        private static extern void WTSFreeMemory(IntPtr pMemory);

        [DllImport("wtsapi32.dll", SetLastError = true)]
        private static extern bool WTSLogoffSession(IntPtr hServer, int sessionId, bool bWait);

        private const int WTS_CURRENT_SERVER_HANDLE = 0;

        public static List<WTS_SESSION_INFO> GetRDPSessions()
        {
            List<WTS_SESSION_INFO> rdpSessions = new List<WTS_SESSION_INFO>();

            IntPtr ppSessionInfo = IntPtr.Zero;
            int count = 0;

            bool result = WTSEnumerateSessions((IntPtr)WTS_CURRENT_SERVER_HANDLE, 0, 1, out ppSessionInfo, out count);
            int dataSize = Marshal.SizeOf(typeof(WTS_SESSION_INFO));
            long current = (long)ppSessionInfo;

            if (result)
            {
                for (int i = 0; i < count; i++)
                {
                    WTS_SESSION_INFO sessionInfo = (WTS_SESSION_INFO)Marshal.PtrToStructure((IntPtr)current, typeof(WTS_SESSION_INFO));
                    current += dataSize;

                    // 只添加远程的RDP会话，不包括本地会话
                    if (sessionInfo.State != WTS_CONNECTSTATE_CLASS.WTSActive)
                    {
                        rdpSessions.Add(sessionInfo);
                    }
                }
            }

            WTSFreeMemory(ppSessionInfo);
            return rdpSessions;
        }

        public static void ForceLogout(int sessionId)
        {
            bool result = WTSLogoffSession((IntPtr)WTS_CURRENT_SERVER_HANDLE, sessionId, true);
            if (result)
            {
                Console.WriteLine($"Session {sessionId} has been logged off.");
            }
            else
            {
                Console.WriteLine($"Failed to log off session {sessionId}. Error code: {Marshal.GetLastWin32Error()}");
            }
        }

        public static void LogoutAllRDPSessions()
        {
            List<WTS_SESSION_INFO> rdpSessions = GetRDPSessions();
            foreach (var session in rdpSessions)
            {
                Console.WriteLine($"Logging off session {session.SessionID} ({session.pWinStationName})");
                ForceLogout(session.SessionID);
            }
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct WTS_SESSION_INFO
    {
        public int SessionID;
        [MarshalAs(UnmanagedType.LPStr)]
        public string pWinStationName;
        public WTS_CONNECTSTATE_CLASS State;
    }

    public enum WTS_CONNECTSTATE_CLASS
    {
        WTSActive,
        WTSConnected,
        WTSConnectQuery,
        WTSShadow,
        WTSDisconnected,
        WTSIdle,
        WTSListen,
        WTSReset,
        WTSDown,
        WTSInit
    }
}
