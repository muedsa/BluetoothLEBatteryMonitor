using System;
using System.Runtime.InteropServices;
using System.Text;

namespace BluetoothLEBatteryMonitor.WinApi
{
    class SetupAPI
    {
        //https://docs.microsoft.com/en-us/windows/win32/api/setupapi/nf-setupapi-setupdigetclassdevsw
        [DllImport(@"Setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr SetupDiGetClassDevsW(
            ref Guid ClassGuid,
            String Enumerator,
            IntPtr hwndParent,
            UInt32 Flags
        );

        //https://docs.microsoft.com/en-us/windows/win32/api/setupapi/nf-setupapi-setupdienumdeviceinfo
        [DllImport(@"Setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern Boolean SetupDiEnumDeviceInfo(
            IntPtr DeviceInfoSet,
            Int32 MemberIndex,
            ref SP_DEVINFO_DATA DeviceInfoData
        );

        //https://docs.microsoft.com/en-us/windows/win32/api/setupapi/nf-setupapi-setupdigetdeviceregistrypropertyw
        [DllImport(@"Setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern Boolean SetupDiGetDeviceRegistryPropertyW(
            IntPtr DeviceInfoSet,
            ref SP_DEVINFO_DATA DeviceInfoData,
            Int32 Property,
            IntPtr PropertyRegDataType,
            StringBuilder PropertyBuffer,
            Int32 PropertyBufferSize,
            ref Int32 RequiredSize
        );

        //https://docs.microsoft.com/zh-cn/windows-hardware/drivers/install/devpropkey
        public struct DEVPROPKEY
        {
            public Guid fmtid;
            public UInt64 pid;
        }

        //https://docs.microsoft.com/en-us/windows/win32/api/setupapi/nf-setupapi-setupdigetdevicepropertyw
        [DllImport(@"Setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern Boolean SetupDiGetDevicePropertyW(
            IntPtr DeviceInfoSet,
            ref SP_DEVINFO_DATA DeviceInfoData,
            ref DEVPROPKEY PropertyKey,
            ref UInt64 PropertyType,
            [MarshalAs(UnmanagedType.LPArray)] byte[] PropertyBuffer,
            Int32 PropertyBufferSize,
            ref Int32 RequiredSize,
            Int32 Flags
        );

        //https://docs.microsoft.com/en-us/windows/win32/api/setupapi/nf-setupapi-setupdienumdeviceinterfaces
        [DllImport(@"Setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern Boolean SetupDiEnumDeviceInterfaces(
            IntPtr DeviceInfoSet,
            IntPtr DeviceInfoData,
            ref Guid InterfaceClassGuid,
            Int32 MemberIndex,
            ref SP_DEVICE_INTERFACE_DATA DeviceInterfaceData
        );

        //https://docs.microsoft.com/en-us/windows/win32/api/setupapi/nf-setupapi-setupdigetdeviceinterfacedetailw
        [DllImport(@"Setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern Boolean SetupDiGetDeviceInterfaceDetailW(
            IntPtr DeviceInfoSet,
            ref SP_DEVICE_INTERFACE_DATA DeviceInterfaceData,
            ref SP_DEVICE_INTERFACE_DETAIL_DATA DeviceInterfaceDetailData,
            Int32 DeviceInterfaceDetailDataSize,
            ref UInt32 RequiredSize,
            ref SP_DEVINFO_DATA DeviceInfoData
        );

        //https://docs.microsoft.com/en-us/windows/win32/api/setupapi/ns-setupapi-sp_devinfo_data
        //https://www.pinvoke.net/default.aspx/Structures/SP_DEVINFO_DATA.html
        [StructLayout(LayoutKind.Sequential)]
        public struct SP_DEVINFO_DATA
        {
            public Int32 cbSize;
            public Guid ClassGuid;
            public UInt32 DevInst;
            public IntPtr Reserved;
        }


        //https://docs.microsoft.com/en-us/windows/win32/api/setupapi/ns-setupapi-sp_device_interface_data
        [StructLayout(LayoutKind.Sequential)]
        public struct SP_DEVICE_INTERFACE_DATA
        {
            public Int32 cbSize;
            public Guid interfaceClassGuid;
            public UInt32 flags;
            private UIntPtr reserved;
        }

        //https://docs.microsoft.com/zh-cn/windows/win32/api/setupapi/ns-setupapi-sp_device_interface_detail_data_w
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct SP_DEVICE_INTERFACE_DETAIL_DATA
        {
            public Int32 cbSize;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)] public string DevicePath;
        }

        public static readonly Int32 SPDRP_FRIENDLYNAME = 0x0000000C;
        public static readonly Int32 SPDRP_DEVICEDESC = 0x00000000;
        public static readonly Int32 SPDRP_ENUMERATOR_NAME = 0x00000016;
        public static readonly Int32 SPDRP_HARDWAREID = 0x00000001;

        public static readonly UInt32 DIGCF_PRESENT = 0x02;
        public static readonly UInt32 DIGCF_DEVINTERFACE = 0x10;
    }
}
