using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BluetoothLEBatteryMonitor
{
    class BLEDevice
    {
        public string Hwid;
        public string Name;
        public bool Status;
        public int Battery;

        public BLEDevice(string hwid, string name, bool status, int battery)
        {
            this.Hwid = hwid;
            this.Name = name;
            this.Status = status;
            this.Battery = battery;
        }
    }
}
