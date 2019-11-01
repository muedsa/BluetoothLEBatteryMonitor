using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace BluetoothLEBatteryMonitor
{
    public partial class DeviceListForm : Form
    {
        private List<BLEDevice> Devices;
        private string Hwid;
        public DeviceListForm()
        {
            InitializeComponent();
        }

        private void DeviceListForm_Load(object sender, EventArgs e)
        {
            int width = this.DeviceListView.Width / 4;
            this.DeviceListView.Columns.Add("设备", width);
            this.DeviceListView.Columns.Add("状态", width);
            this.DeviceListView.Columns.Add("电量", width);
            this.DeviceListView.Columns.Add("Hwid", width);
            UpdateDeviceList();
        }

        private void UpdateDeviceList()
        {
            this.Devices = BluetoothService.GetBLEDeviceList();
            if (Devices.Count > 0)
            {
                this.DeviceListView.BeginUpdate();
                this.DeviceListView.Items.Clear();
                foreach (BLEDevice device in this.Devices)
                {
                    ListViewItem listViewItem = new ListViewItem();
                    listViewItem.Text = device.Name;
                    listViewItem.SubItems.Add(device.Status? "connect":"disconnect");
                    listViewItem.SubItems.Add(device.Battery.ToString());
                    listViewItem.SubItems.Add(device.Hwid);
                    this.DeviceListView.Items.Add(listViewItem);
                }
                this.DeviceListView.EndUpdate();
            }
        }

        private void DeviceListView_SizeChanged(object sender, EventArgs e)
        {
            int width = this.DeviceListView.Width / 4;
            foreach (ColumnHeader column in this.DeviceListView.Columns)
            {
                column.Width = width;
            }
        }

        private void DeviceListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.Hwid = this.Devices[this.DeviceListView.SelectedItems[0].Index].Hwid;
            this.Hide();
            UpdateIcon();
            this.Timer.Start();
        }

        private void UpdateIcon()
        {
            UpdateDeviceList();
            if (this.Hwid != null)
            {
                BLEDevice device = this.Devices.Find(i => i.Hwid.Equals(this.Hwid));
                if (device != null)
                {
                    ChangeIcon(device.Name, device.Status, device.Battery);
                }
                else
                {
                    ChangeIcon("", false, 0);
                }
            }
        }
        private void ChangeIcon(string name, bool status, int battery)
        {
            if(status)
            {
                if (battery >= 90)
                {
                    this.NotifyIcon.Icon = BluetoothLEBatteryMonitor.Properties.Resources.Icon_Battery_Full;
                }
                else if (battery >= 70)
                {
                    this.NotifyIcon.Icon = BluetoothLEBatteryMonitor.Properties.Resources.Icon_Battery_Three_Quarters;
                }
                else if (battery >= 50)
                {
                    this.NotifyIcon.Icon = BluetoothLEBatteryMonitor.Properties.Resources.Icon_Battery_Half;
                }
                else if (battery >= 30)
                {
                    this.NotifyIcon.Icon = BluetoothLEBatteryMonitor.Properties.Resources.Icon_Battery_Quarter;
                }
                else
                {
                    this.NotifyIcon.Icon = BluetoothLEBatteryMonitor.Properties.Resources.Icon_Battery_Empty;
                }
                this.NotifyIcon.Text = String.Format("{0} {1}%\nBluetooth Battery Monitor", name, battery);
                
            }
            else
            {
                this.NotifyIcon.Icon = BluetoothLEBatteryMonitor.Properties.Resources.Icon_Unlink;
                this.NotifyIcon.Text = "Device disconnect or no selected\nBluetooth Battery Monitor";
                MessageBox.Show("Device disconnect or no selected");
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            UpdateIcon();
        }

        private void NotifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
        }
    }
}
