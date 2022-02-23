# Note
I try to get Bluetooh headphone battery level by BLE GattService, but this is not a good idea. 
Headphone/Headset can use HFP to report battery level. 
[Apple](https://developer.apple.com/accessories/Accessory-Design-Guidelines.pdf) extend HFP command, help headset report a headset state change: 
- Request: AT+XAPL=[vendorID]-[productID]-[version],[features]
- Response: AT+IPHONEACCEV=[Number of key/value pairs ],[key1 ],[val1 ],[key2 ],[val2 ],...

## Win10 20H2 support for Battery Monitor  
![Win10 2020H2 devices](https://github.com/MUedsa/BluetoothLEBatteryMonitor/blob/v2.0/win10%2020H2%20devices.png?raw=true)  

# BluetoothLEBatteryMonitor
Windows BluetoothLE Battery Monitor  
Version 2.0  
![screenshot1](https://github.com/MUedsa/BluetoothLEBatteryMonitor/blob/v2.0/screenshot1.png?raw=true)  
![screenshot2](https://github.com/MUedsa/BluetoothLEBatteryMonitor/blob/v2.0/screenshot2.png?raw=true)  
# Note
Use Universal Windows Platform (UWP) Windows Runtime API at Windows Forms  
# TODO 
- [ ] BEL feat: Replace timer loop to get BatteryLevel with event listener  

# References
- https://github.com/joric/bluetooth-battery-monitor
- https://github.com/sensboston/BLEConsole
