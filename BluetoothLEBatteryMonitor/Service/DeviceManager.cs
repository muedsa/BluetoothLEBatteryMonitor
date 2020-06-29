using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using Windows.Devices.Enumeration;
using Windows.Storage.Streams;

namespace BluetoothLEBatteryMonitor.Service
{
    class DeviceManager
    {
        private static readonly DeviceManager instance = new DeviceManager();

        public static readonly Guid BATTERY_UUID = Guid.Parse("{0000180F-0000-1000-8000-00805F9B34FB}");
        public static readonly Guid BATTERY_LEVEL_UUID = Guid.Parse("{00002A19-0000-1000-8000-00805F9B34FB}");

        public ConcurrentDictionary<string, DeviceInformation> DeviceInformationDict = new ConcurrentDictionary<string, DeviceInformation>();

        private BluetoothLEDevice selectedBLEDev = null;
        private GattDeviceService selectGattService = null;
        private GattCharacteristic selectGattCharacteristic = null;
        static DeviceManager()
        {
        }

        private DeviceManager()
        {

        }

        public static DeviceManager Instance
        {
            get
            {
                return instance;
            }
        }

        public void SelectDevice(string id, DeviceListForm form)
        {
            Task<BluetoothLEDevice> bleTask = BluetoothLEDevice.FromIdAsync(id).AsTask();
            if(bleTask.Wait(3000, new CancellationTokenSource().Token))
            {
                selectedBLEDev = bleTask.Result;
                
                Task<GattDeviceServicesResult> batteryServiceTask = selectedBLEDev.GetGattServicesForUuidAsync(BATTERY_UUID, BluetoothCacheMode.Uncached).AsTask();
                if(batteryServiceTask.Wait(3000))
                {
                    if (GattCommunicationStatus.Success.Equals(batteryServiceTask.Result.Status)){
                        selectGattService = batteryServiceTask.Result.Services[0];
                        Task<GattCharacteristicsResult> gattCharacteristicsTask = selectGattService.GetCharacteristicsForUuidAsync(BATTERY_LEVEL_UUID, BluetoothCacheMode.Uncached).AsTask();
                        if(gattCharacteristicsTask.Wait(300))
                        {
                            if (GattCommunicationStatus.Success.Equals(gattCharacteristicsTask.Result.Status))
                            {
                                selectGattCharacteristic = gattCharacteristicsTask.Result.Characteristics[0];
                                form.Notify("BLE Device ID:" + selectedBLEDev.DeviceId);
                                form.StartUpdate();
                            }
                        }
                    }
                }

            }
        }

        public int GetBatteryLevel()
        {
            if(selectGattCharacteristic != null && BluetoothConnectionStatus.Connected.Equals(selectedBLEDev.ConnectionStatus))
            {
                Task<GattReadResult> gattReadTask = selectGattCharacteristic.ReadValueAsync(BluetoothCacheMode.Uncached).AsTask();
                if (gattReadTask.Wait(300))
                {
                    if (GattCommunicationStatus.Success.Equals(gattReadTask.Result.Status))
                    {
                        IBuffer buffer = gattReadTask.Result.Value;
                        byte[] data = new byte[buffer.Length];
                        DataReader.FromBuffer(buffer).ReadBytes(data);
                        return data[0];
                    }
                }
            }
            return -1;
        }

        public string GetDeviceName()
        {
            if(selectedBLEDev != null)
            {
                return selectedBLEDev.Name;
            }
            return "-";
        }
    }
}
