using BluetoothLEBatteryMonitor.Service;
using System;
using System.Collections.Concurrent;
using System.Windows.Forms;
using Windows.Devices.Enumeration;

namespace BluetoothLEBatteryMonitor
{
    public partial class DeviceListForm : Form
    {
        public DeviceListForm()
        {
            InitializeComponent();
        }

        private void DeviceListForm_Load(object sender, EventArgs e)

        {
            ConcurrentDictionary<string, DeviceInformation> deviceInformationDict = DeviceManager.Instance.DeviceInformationDict;
            string aqsFilter = "(System.Devices.Aep.ProtocolId:=\"{bb7bb05e-5972-42b5-94fc-76eaa7084d49}\")";
            string[] bleAdditionalProperties = { "System.Devices.Aep.DeviceAddress", "System.Devices.Aep.Bluetooth.Le.IsConnectable", };
            DeviceWatcher watcher = DeviceInformation.CreateWatcher(aqsFilter, bleAdditionalProperties, DeviceInformationKind.AssociationEndpoint);
            watcher.Added += (DeviceWatcher deviceWatcher, DeviceInformation devInfo) =>
            {
                if (!String.IsNullOrWhiteSpace(devInfo.Name))
                {
                    deviceInformationDict.AddOrUpdate(devInfo.Id, devInfo, (k, v) => devInfo);
                }
            };
            watcher.Updated += (_, __) => { };
            watcher.EnumerationCompleted += (DeviceWatcher deviceWatcher, object arg) => { deviceWatcher.Stop(); };
            watcher.Stopped += (DeviceWatcher deviceWatcher, object arg) => { deviceWatcher.Start(); };
            watcher.Start();
            int width = DeviceListView.Width / 3;
            DeviceListView.Columns.Add("设备", width);
            DeviceListView.Columns.Add("状态", width);
            DeviceListView.Columns.Add("ID", width);
        }

        private void RenderTimer_Tick(object sender, EventArgs e)
        {
            ConcurrentDictionary<string, DeviceInformation> deviceInformationDict = DeviceManager.Instance.DeviceInformationDict;
            if (!deviceInformationDict.IsEmpty)
            {
                DeviceListView.BeginUpdate();
                DeviceListView.Items.Clear();
                foreach(DeviceInformation devInfo in deviceInformationDict.Values)
                {
                    ListViewItem listViewItem = new ListViewItem
                    {
                        Text = devInfo.Name
                    };
                    listViewItem.SubItems.Add(devInfo.Pairing.IsPaired ? "Paired " : "Unpair");
                    listViewItem.SubItems.Add(devInfo.Id);
                    this.DeviceListView.Items.Add(listViewItem);
                }
                DeviceListView.EndUpdate();
            }
        }

        private void DeviceListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            string id = DeviceListView.SelectedItems[0].SubItems[2].Text;
            DeviceManager.Instance.SelectDevice(id, this);
        }

        private void IconTimer_Tick(object sender, EventArgs e)
        {
            ChangeIcon(DeviceManager.Instance.GetDeviceName(), DeviceManager.Instance.GetBatteryLevel());
        }

        private void ChangeIcon(string name, int battery)
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
            else if(battery > 0)
            {
                this.NotifyIcon.Icon = BluetoothLEBatteryMonitor.Properties.Resources.Icon_Battery_Empty;
            }
            else
            {
                return;
            }
            this.NotifyIcon.Text = String.Format("{0} {1}%\n{2} Update", name, battery, DateTime.Now.ToString());
        }

        public void StartUpdate()
        {
            Hide();
            IconTimer.Start();
            ChangeIcon(DeviceManager.Instance.GetDeviceName(), DeviceManager.Instance.GetBatteryLevel());
        }


        public void Notify(string message)
        {
            NotifyIcon.ShowBalloonTip(300, "BluetoothLE Battery Montior", message, ToolTipIcon.Info);
        }

        private void NotifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Show();
            WindowState = FormWindowState.Normal;
        }

        private void DeviceListForm_SizeChanged(object sender, EventArgs e)
        {
            if (WindowState.Equals(FormWindowState.Minimized))
            {
                Hide();
            }
        }
    }
}
