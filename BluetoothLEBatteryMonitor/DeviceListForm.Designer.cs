namespace BluetoothLEBatteryMonitor
{
    partial class DeviceListForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DeviceListForm));
            this.DeviceListView = new System.Windows.Forms.ListView();
            this.NotifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.Timer = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // DeviceListView
            // 
            this.DeviceListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DeviceListView.HideSelection = false;
            this.DeviceListView.Location = new System.Drawing.Point(0, 0);
            this.DeviceListView.Name = "DeviceListView";
            this.DeviceListView.Size = new System.Drawing.Size(284, 361);
            this.DeviceListView.TabIndex = 0;
            this.DeviceListView.UseCompatibleStateImageBehavior = false;
            this.DeviceListView.View = System.Windows.Forms.View.Details;
            this.DeviceListView.SelectedIndexChanged += new System.EventHandler(this.DeviceListView_SelectedIndexChanged);
            // 
            // NotifyIcon
            // 
            this.NotifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("NotifyIcon.Icon")));
            this.NotifyIcon.Text = "Bluetooth Battery Monitor";
            this.NotifyIcon.Visible = true;
            this.NotifyIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.NotifyIcon_MouseDoubleClick);
            // 
            // Timer
            // 
            this.Timer.Interval = 300000;
            this.Timer.Tick += new System.EventHandler(this.Timer_Tick);
            // 
            // DeviceListForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 361);
            this.Controls.Add(this.DeviceListView);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "DeviceListForm";
            this.Text = "DeviceList";
            this.Load += new System.EventHandler(this.DeviceListForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView DeviceListView;
        private System.Windows.Forms.NotifyIcon NotifyIcon;
        private System.Windows.Forms.Timer Timer;
    }
}

