using BluetoothLEBatteryMonitor.WinApi;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace BluetoothLEBatteryMonitor
{
    class DeviceService
    {
        public static List<BLEDevice> GetDevicesWithoutBattery(Guid guid)
        {
            List<BLEDevice> list = new List<BLEDevice>();
            IntPtr hdi = SetupAPI.SetupDiGetClassDevsW(ref guid, null, IntPtr.Zero, SetupAPI.DIGCF_PRESENT);
            SetupAPI.SP_DEVINFO_DATA devInfoData = new SetupAPI.SP_DEVINFO_DATA();
            devInfoData.cbSize = Marshal.SizeOf(devInfoData);
            int i = 0;
            while (SetupAPI.SetupDiEnumDeviceInfo(hdi, i, ref devInfoData))
            {
                i++;
                String hwid = GetDeviceInfo(hdi, devInfoData, SetupAPI.SPDRP_HARDWAREID);
                if (hwid.StartsWith("BTHLE\\Dev_"))
                {
                    String friendlyName = GetDeviceInfo(hdi, devInfoData, SetupAPI.SPDRP_FRIENDLYNAME);
                    //String deviceDesc = GetDeviceInfo(hdi, devInfoData, SetupAPI.SPDRP_DEVICEDESC);
                    //String enumeratorName = GetDeviceInfo(hdi, devInfoData, SetupAPI.SPDRP_ENUMERATOR_NAME);
                    bool status = GetConnectedStatus(hdi, devInfoData);
                    BLEDevice device = new BLEDevice(hwid, friendlyName, status, 0);
                    list.Add(device);
                }
            }
            return list;
        }

        private static String GetDeviceInfo(IntPtr hdi, SetupAPI.SP_DEVINFO_DATA devInfoData, Int32 propertyName)
        {
            StringBuilder buffer = new StringBuilder();
            Int32 bufferSize = 0;
            while (!SetupAPI.SetupDiGetDeviceRegistryPropertyW(hdi, ref devInfoData, propertyName, IntPtr.Zero, buffer, bufferSize, ref bufferSize))
            {
                int err = Marshal.GetLastWin32Error();
                if (err == Kernel32.ERROR_INSUFFICIENT_BUFFER)
                {
                    buffer = new StringBuilder(bufferSize);
                }
                else if (err == Kernel32.ERROR_INVALID_DATA)
                {
                    break;
                }
                else
                {
                    throw new Exception(err.ToString());
                }
            }
            return buffer.ToString();
        }

        private static Boolean GetConnectedStatus(IntPtr hdi, SetupAPI.SP_DEVINFO_DATA devInfoData)
        {
            Int32 DN_DEVICE_DISCONNECTED = 0x2000000;
            UInt64 ulPropertyType = new UInt64();
            Int32 dwSize = 4;
            byte[] devst = new byte[4];
            SetupAPI.DEVPROPKEY key = new SetupAPI.DEVPROPKEY();
            key.fmtid = Guid.Parse("{4340a6c5-93fa-4706-972c-7b648008a5a7}");
            key.pid = 2;
            while (!SetupAPI.SetupDiGetDevicePropertyW(hdi, ref devInfoData, ref key, ref ulPropertyType, devst, dwSize, ref dwSize, 0))
            {
                int err = Marshal.GetLastWin32Error();
                if (err == Kernel32.ERROR_INSUFFICIENT_BUFFER)
                {
                    devst = new byte[dwSize];
                }
                else if (err == Kernel32.ERROR_INVALID_DATA)
                {
                    break;
                }
                else
                {
                    throw new Exception(err.ToString());
                }
            }
            int status = BitConverter.ToInt32(devst, 0);
            return !Convert.ToBoolean(status & DN_DEVICE_DISCONNECTED);
        }


        public static void AddDeviceInfo(Guid guid, List<BLEDevice> list)
        {
            if(list.Count > 0)
            {
                IntPtr hdi = SetupAPI.SetupDiGetClassDevsW(ref guid, null, IntPtr.Zero, SetupAPI.DIGCF_PRESENT | SetupAPI.DIGCF_DEVINTERFACE);
                SetupAPI.SP_DEVINFO_DATA dd = new SetupAPI.SP_DEVINFO_DATA();
                dd.cbSize = Marshal.SizeOf(dd);
                SetupAPI.SP_DEVICE_INTERFACE_DATA did = new SetupAPI.SP_DEVICE_INTERFACE_DATA();
                did.cbSize = Marshal.SizeOf(did);
                int i = 0;
                while (SetupAPI.SetupDiEnumDeviceInterfaces(hdi, IntPtr.Zero, ref guid, i, ref did))
                {
                    i += 1;
                    SetupAPI.SP_DEVICE_INTERFACE_DETAIL_DATA didd = new SetupAPI.SP_DEVICE_INTERFACE_DETAIL_DATA();
                    didd.cbSize = IntPtr.Size == 8 ? 8 : 4 + Marshal.SystemDefaultCharSize;
                    UInt32 nRequiredSize = 0;
                    SetupAPI.SetupDiGetDeviceInterfaceDetailW(hdi, ref did, ref didd, 256, ref nRequiredSize, ref dd);
                    foreach (BLEDevice device in list)
                    {
                        if (device.Status && didd.DevicePath.Contains(device.Hwid.Split(new char[1] {'_'})[1]))
                        {
                            Communication(didd.DevicePath, device);
                        }
                    }
                }
            }
        }

        private static void Communication(string path, BLEDevice device)
        {
            IntPtr hDevice = Kernel32.CreateFileW(path, FileAccess.Read, FileShare.ReadWrite, IntPtr.Zero, FileMode.Open, 0, IntPtr.Zero);
            UInt16 servicesBufferCount = 0;
            BluetoothAPIs.BluetoothGATTGetServices(hDevice, 0, IntPtr.Zero, ref servicesBufferCount, BluetoothAPIs.BLUETOOTH_GATT_FLAG_NONE);
            IntPtr servicesBufferPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(BluetoothAPIs.BTH_LE_GATT_SERVICE)) * servicesBufferCount);
            BluetoothAPIs.BluetoothGATTGetServices(hDevice, servicesBufferCount, servicesBufferPtr, ref servicesBufferCount, BluetoothAPIs.BLUETOOTH_GATT_FLAG_NONE);
            for (int i = 0; i < servicesBufferCount; i++)
            {
                BluetoothAPIs.BTH_LE_GATT_SERVICE service = Marshal.PtrToStructure<BluetoothAPIs.BTH_LE_GATT_SERVICE>(servicesBufferPtr + i * Marshal.SizeOf(typeof(BluetoothAPIs.BTH_LE_GATT_SERVICE)));
                UInt16 characteristicsBufferCount = 0;
                int hr = BluetoothAPIs.BluetoothGATTGetCharacteristics(hDevice, ref service, 0, IntPtr.Zero, ref characteristicsBufferCount, BluetoothAPIs.BLUETOOTH_GATT_FLAG_NONE);
                IntPtr characteristicsBufferPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(BluetoothAPIs.BTH_LE_GATT_CHARACTERISTIC)) * characteristicsBufferCount);
                UInt16 characteristicsBufferActual = 0;
                BluetoothAPIs.BluetoothGATTGetCharacteristics(hDevice, ref service, characteristicsBufferCount, characteristicsBufferPtr, ref characteristicsBufferActual, BluetoothAPIs.BLUETOOTH_GATT_FLAG_NONE);
                for (int n = 0; n < characteristicsBufferActual; n++)
                {
                    BluetoothAPIs.BTH_LE_GATT_CHARACTERISTIC characteristic = Marshal.PtrToStructure<BluetoothAPIs.BTH_LE_GATT_CHARACTERISTIC>(characteristicsBufferPtr + n * Marshal.SizeOf(typeof(BluetoothAPIs.BTH_LE_GATT_CHARACTERISTIC)));
                    BluetoothAPIs.BTH_LE_GATT_CHARACTERISTIC_VALUE characteristicValue = new BluetoothAPIs.BTH_LE_GATT_CHARACTERISTIC_VALUE();
                    UInt16 characteristicValueSizeRequired = 0;
                    BluetoothAPIs.BluetoothGATTGetCharacteristicValue(hDevice, ref characteristic, 256, ref characteristicValue, out characteristicValueSizeRequired, BluetoothAPIs.BLUETOOTH_GATT_FLAG_NONE);
                    if (characteristic.CharacteristicUuid.ShortUuid == 0x2A19) // battery level
                    {
                        device.Battery = characteristicValue.Data[0];
                    }
                }
                Marshal.FreeHGlobal(characteristicsBufferPtr);
            }
            Marshal.FreeHGlobal(servicesBufferPtr);
        }
    }
}
