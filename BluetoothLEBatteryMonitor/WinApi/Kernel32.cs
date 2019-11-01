using System;
using System.IO;
using System.Runtime.InteropServices;

namespace BluetoothLEBatteryMonitor.WinApi
{
    class Kernel32
    {
        //https://docs.microsoft.com/en-us/windows/win32/api/fileapi/nf-fileapi-createfilew
        [DllImport("Kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr CreateFileW(
            [MarshalAs(UnmanagedType.LPWStr)] string filename,
            [MarshalAs(UnmanagedType.U4)] FileAccess access,
            [MarshalAs(UnmanagedType.U4)] FileShare share,
            IntPtr securityAttributes,
            [MarshalAs(UnmanagedType.U4)] FileMode creationDisposition,
            [MarshalAs(UnmanagedType.U4)] FileAttributes flagsAndAttributes,
            IntPtr templateFile
        );

        public static readonly int ERROR_INSUFFICIENT_BUFFER = 0x7A;
        public static readonly int ERROR_INVALID_DATA = 0xD;
    }
}
