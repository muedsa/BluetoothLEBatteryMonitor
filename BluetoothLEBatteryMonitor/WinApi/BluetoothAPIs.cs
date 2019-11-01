using System;
using System.Runtime.InteropServices;

namespace BluetoothLEBatteryMonitor.WinApi
{
    class BluetoothAPIs
    {
        //https://docs.microsoft.com/zh-cn/windows/win32/api/bluetoothleapis/nf-bluetoothleapis-bluetoothgattgetservices
        [DllImport("BluetoothAPIs.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int BluetoothGATTGetServices(
            IntPtr hDevice,
            UInt16 ServicesBufferCount,
            IntPtr ServicesBuffer,
            ref UInt16 ServicesBufferActual,
            UInt32 Flags
        );

        //https://docs.microsoft.com/zh-cn/windows/win32/api/bthledef/ns-bthledef-bth_le_uuid
        [StructLayout(LayoutKind.Explicit)]
        public struct BTH_LE_UUID
        {
            [FieldOffset(0)]
            public Byte IsShortUuid;
            [FieldOffset(4)]
            public UInt16 ShortUuid;
            [FieldOffset(4)]
            public Guid LongUuid;
        }

        //https://docs.microsoft.com/zh-cn/windows/win32/api/bthledef/ns-bthledef-bth_le_gatt_service
        [StructLayout(LayoutKind.Sequential)]
        public struct BTH_LE_GATT_SERVICE
        {
            public BTH_LE_UUID ServiceUuid;
            public UInt16 AttributeHandle;
        }

        //https://docs.microsoft.com/en-us/windows/win32/api/bluetoothleapis/nf-bluetoothleapis-bluetoothgattgetcharacteristics
        [DllImport("BluetoothAPIs.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int BluetoothGATTGetCharacteristics(
            IntPtr hDevice,
            ref BTH_LE_GATT_SERVICE Service,
            UInt16 CharacteristicsBufferCount,
            IntPtr CharacteristicsBuffer,
            ref UInt16 CharacteristicsBufferActual,
            UInt32 Flags
        );

        //https://docs.microsoft.com/en-us/windows/win32/api/bthledef/ns-bthledef-bth_le_gatt_characteristic
        [StructLayout(LayoutKind.Sequential)]
        public struct BTH_LE_GATT_CHARACTERISTIC
        {
            public UInt16 ServiceHandle;
            public BTH_LE_UUID CharacteristicUuid;
            public UInt16 AttributeHandle;
            public UInt16 CharacteristicValueHandle;
            public Byte IsBroadcastable;
            public Byte IsReadable;
            public Byte IsWritable;
            public Byte IsWritableWithoutResponse;
            public Byte IsSignedWritable;
            public Byte IsNotifiable;
            public Byte IsIndicatable;
            public Byte HasExtendedProperties;
        }

        //https://docs.microsoft.com/en-us/windows/win32/api/bluetoothleapis/nf-bluetoothleapis-bluetoothgattgetcharacteristicvalue
        [DllImport("BluetoothAPIs.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int BluetoothGATTGetCharacteristicValue(
            IntPtr hDevice,
            ref BTH_LE_GATT_CHARACTERISTIC Characteristic,
            UInt32 CharacteristicValueDataSize,
            ref BTH_LE_GATT_CHARACTERISTIC_VALUE CharacteristicValue,
            out UInt16 CharacteristicValueSizeRequired,
            UInt32 Flags
        );

        //https://docs.microsoft.com/en-us/windows/win32/api/bthledef/ns-bthledef-bth_le_gatt_characteristic_value
        [StructLayout(LayoutKind.Sequential)]
        public struct BTH_LE_GATT_CHARACTERISTIC_VALUE
        {
            public UInt32 DataSize;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)] public byte[] Data;
        }

        public static readonly UInt32 BLUETOOTH_GATT_FLAG_NONE = 0;
        public static readonly UInt32 BLUETOOTH_GATT_FLAG_FORCE_READ_FROM_DEVICE = 0x00000004;
    }
}
