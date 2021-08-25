[![PortScanner](https://gridprotectionalliance.org/images/products/productTitles75/ACPScanner.png)](https://gridprotectionalliance.github.io/PortScanner/)

## IEEE C37.118 Serial Port Scanner and Configuration Loader

![Screen Shot](ScreenShot.png)

This tool is designed to operate with GPA synchrophasor products, e.g., the [openPDC](https://github.com/GridProtectionAlliance/openPDC), [SIEGate](https://github.com/GridProtectionAlliance/SIEGate) or the [openHistorian](https://github.com/GridProtectionAlliance/openHistorian).

The `AutoConfigPortScanner` scans serial ports on a system testing for Phasor Measurement Unit (PMU) connections using the [IEEE C37.118 protocol](https://standards.ieee.org/standard/C37_118_1-2011.html). Once a connection has been established, the tool will scan through a specified ID code set in order to receive a configuration frame from the device. Once a device connection has been detected and a configuration frame has been received, the tool will automatically configure a new connection to the device in the host GPA synchrophasor tool.

This tool is useful to automate configuration and setup of PMU devices connected serially in bulk. This can be the case when a utility has chosen a serial-based communications infrastructure where using [serial over Ethernet hubs](https://www.digi.com/products/networking/infrastructure-management/serial-connectivity/terminal-servers/connectportlts) can add hundreds of serial ports to a system.

## Usage with Command Line Parameters
```shell
AutoConfigPortScanner [options]
```

### Options

| Short | Long | Description |
|:-----:| ---- | ----------- |
| &#x2011;b | &#x2011;&#x2011;BaudRate | Defines the serial baud rate. Standard values: 110, 300, 600, 1200, 2400, 4800, 9600, 14400, 19200, 38400, 57600, 115200, 128000, or 256000. |
| &#x2011;d | &#x2011;&#x2011;DataBits | Defines the standard length of data bits per byte. Standard values: 5, 6, 7 or 8. |
| &#x2011;p | &#x2011;&#x2011;Parity | Defines the parity-checking protocol. Value is one of: Even, Mark, None, Odd or Space. |
| &#x2011;s | &#x2011;&#x2011;StopBits | Defines the standard number of stopbits per byte. Value is one of: None, One, OnePointFive or Two. |
| &#x2011;t | &#x2011;&#x2011;DtrEnable | Defines boolean value, true or false, that enables the Data Terminal Ready (DTR) signal during serial communication. |
| &#x2011;r | &#x2011;&#x2011;RtsEnable | Defines boolean value, true or false, indicating whether the Request to Send (RTS) signal is enabled during serial communication. |
| &#x2011;a | &#x2011;&#x2011;AutoStartParsingSequence | Defines boolean value, true or false, indicating whether added device configuration should be set to send parsing sequence to start connection. |
| &#x2011;n | &#x2011;&#x2011;ResponseTimeout | Defines the maximum time, in milliseconds, to wait for a serial response. |
| &#x2011;c | &#x2011;&#x2011;ConfigFrameTimeout | Defines the maximum time, in milliseconds, to wait for a configuration frame. |
| &#x2011;w | &#x2011;&#x2011;DisableDataDelay | Defined the delay time, in milliseconds, to wait after sending the DisableRealTimeData command to a device. |

## Example
* Scan serial ports using 9600 baud:
```shell
AutoConfigPortScanner -b=9600
```

## Default Settings File
Default settings for optional parameters, e.g., serial baud rate and parity, are configured in a `settings.ini` file. This file will be automatically created when the application is first run. The location for this configuration file is `C:\ProgramData\AutoConfigPortScanner\settings.ini`.

The original default values are initially commented out. Following is an example of the default settings file with an override:

```ini
[Serial]
; Defines the serial baud rate. Standard values: 110, 300, 600, 1200, 2400,
; 4800, 9600, 14400, 19200, 38400, 57600, 115200, 128000, or 256000.
;BaudRate=115200
BaudRate=9600

; Defines the standard length of data bits per byte. Standard values: 5, 6,
; 7 or 8.
;DataBits=8

; Defines the parity-checking protocol. Value is one of: Even, Mark, None, Odd
; or Space.
;Parity=None

; Defines the standard number of stopbits per byte. Value is one of: None, One,
; OnePointFive or Two.
;StopBits=One

; Defines the value that enables the Data Terminal Ready (DTR) signal during
; serial communication.
;DtrEnable=False

; Defines the value indicating whether the Request to Send (RTS) signal is
; enabled during serial communication.
;RtsEnable=False
```
![PortScanner](PortScanner.png)
