# For Windows Developer
Google `Windows Management Interface`.  
use **Battery Percents Properties**
- Key = `{104EA319-6EE2-4701-BD47-8DDBF425BBE5} 2`
- Type = 3 (Byte)

`Data` is in percents

Look the [PowerShell Script](https://gist.github.com/muedsa/d5a94e2d5827d3b518063b9a2d2fdda3)

# Note
The project use a wrong way to get battery level(Headphone/Headset)  
I try to get Bluetooh headphone battery level  by BLE GattService, but this is not a good idea.  

# For headphone/headset 
Headphone/Headset can use HFP to report battery level to ios. We should use hfp to get headset's battery level.    
[Apple](https://developer.apple.com/accessories/Accessory-Design-Guidelines.pdf) extend HFP command, help headset report a headset state change:  
- Request: AT+XAPL=[vendorID]-[productID]-[version],[features]
- Response: AT+IPHONEACCEV=[Number of key/value pairs ],[key1 ],[val1 ],[key2 ],[val2 ],...  


Unfortunately, microsoft no provide api. You have to code Bluetooth Driver to supersed Microsoft Driver.  


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
