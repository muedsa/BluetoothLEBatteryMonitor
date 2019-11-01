using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BluetoothLEBatteryMonitor
{
    class BluetoothService
    {
        //蓝牙协议 ID https://docs.microsoft.com/zh-cn/windows/uwp/Devices-sensors/aep-service-class-ids
        private static readonly string UUID = "{E0CBF06C-CD8B-4647-BB8A-263B43F0F974}";
        //GATT Service UUIDs
        private static readonly string UUID1 = "{0000180F-0000-1000-8000-00805F9B34FB}"; //battery service
        private static readonly string UUID2 = "{00001800-0000-1000-8000-00805F9B34FB}"; //appearance
        private static readonly string UUID3 = "{0000180A-0000-1000-8000-00805F9B34FB}"; //device info

        public static List<BLEDevice> GetBLEDeviceList()
        {
            List<BLEDevice> list = DeviceService.GetDevicesWithoutBattery(Guid.Parse(UUID));
            DeviceService.AddDeviceInfo(Guid.Parse(UUID1), list);
            return list;
        }

/*        public static BLEDevice GetBLEDevice(string hwid)
        {
            List<BLEDevice> list = GetBLEDeviceList();
            return list.Find(device => device.Hwid.Equals(hwid));
        }*/
    }
}
