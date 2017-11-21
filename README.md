# AcuRiteSniffer
Reads weather data packets sent by AcuRite SmartHUBs and makes the data accessible locally via a simple embedded web server that returns JSON.

For this to work, your network must be be configured to mirror packets from the SmartHUB to the computer running this service.

This service is designed for people who are *very computer literate*.  If you don't know an IP address from a MAC address, you shouldn't waste your time with this project.

## Prerequisites

You will need one AcuRite SmartHUB, some compatible sensors, and a Windows PC running this service.  You will also need to set up your network such that packets sent from the SmartHUB to AcuRite's servers are mirrored to the PC running this service.

[WinPcap](https://www.winpcap.org/) must be installed on the PC.

## Mirroring network traffic

In order for the weather data to be captured, the network packets must first be sent to your PC.

### With a hub

The simplest way to mirror network the SmartHUB's traffic is to connect the SmartHUB and the PC both to a plain old ethernet hub (not a switch, but a hub).  All traffic that enters a hub through any port gets duplicated and sent to all the hub's other ports, and this achieves our goal.

### With a managed switch

Another way is to use a managed switch or router with port mirroring functionality.

### With a linux router

If your router firmware (such as DD-WRT or Tomato) is based on Linux and allows you to run custom shell commands, you can selectively mirror the traffic using `iptables`.

```
iptables -A PREROUTING -t mangle -s ipSource -j ROUTE --gw ipSniffer --tee
```

This command says to duplicate all packets originating from IP Address `ipSource` and send a copy to the Address `ipSniffer`.  It needs to be run every time your router boots up.  There is typically more than one way to achieve this.  In my router running [Tomato firmware](http://tomato.groov.pl/), I pasted this command into Administration > Scripts > WAN Up so the command would be executed whenever my internet connection comes online.

So if your SmartHUB's address is `192.168.0.135` and your PC's address is `192.168.0.100`, then you would run this command:

```
iptables -A PREROUTING -t mangle -s 192.168.0.135 -j ROUTE --gw 192.168.0.100 --tee
```

You can remove the rule by changing the `-A` to `-D`:

```
iptables -D PREROUTING -t mangle -s ipSource -j ROUTE --gw ipThisPC --tee
```

You can list this and similar rules that have been configured using the command:

```
iptables -t mangle -L
```

## Usage

1) Configure your network to mirror traffic from your SmartHUB to the PC you wish to run this service on.  There is some guidance above, but otherwise you are on your own.  Also, install WinPcap on the PC (this is used to sniff the SmartHUB's network traffic).
2) Download AcuRiteSniffer from [the releases section](https://github.com/bp2008/AcuRiteSniffer/releases) and extract wherever you like.
3) Run the executable.  It requires administrator permission in order to install itself as a service.
4) When the service manager window appears, click `Edit Settings` at the bottom of the window.
    * If desired, change the port number which the embedded web server listens on.
    * Enter the IP address of your SmartHUB.  You'll want to create a DHCP reservation for it, in your router, if you have not already.
    * Select the network interface that will be receiving packets from the SmartHUB.
5) In the service manager window, click `Install Service`, then click `Start Service`.
6) Load AcuRiteSniffer's web interface (default: http://127.0.0.1:45411/)
    * The default page of the web interface is a list of links you can use to access sensor data.  As sensors are detected, additional links are added to the page (though you must refresh the page to see them).
    * Sensor data is intended to be consumed in JSON format.

## File Templates

To facilitate integration with some 3rd-party software (notably, [Blue Iris](http://blueirissoftware.com/)), the service is capable of writing sensor data to simple text files on disk.

Within the service settings, there is a button `Edit text file definitions` which allows you to teach the service how you would like these files to be written.  Consider the example configuration:

```
[24C86E000000_12345678]Temperature: ##tempf##°\nHumidity: ##humidity##\nAt ##date## ##time##
[24C86E000000_00001234=myWindSpeed.txt]##windspeedmph## MPH
[24C86E000000_00001234=myWindDirection.txt]##winddir##
```

The first line specifies that data from the sensor with UniqueID `24C86E000000_12345678` should be output following this template: `Temperature: ##tempf##°\nHumidity: ##humidity##\nAt ##date## ##time##`

This would result in a text file named `24C86E000000_12345678.txt` with content similar to the following:

```
Temperature: 40°
Humidity: 25
At 2017-11-20 10:00:00 PM
```

The second line of the example configuration specifies that the wind speed from sensor `24C86E000000_00001234` should be output to a file named `myWindSpeed.txt`.

The third line specifies that the wind direction from the same sensor `24C86E000000_00001234` should be output to a file named `myWindDirection.txt`.

*Note 1: All such files are written using text encoding `Windows-1252`.*

*Note 2: Files are saved to the `SensorData` subdirectory next to the service executable.*

*Note 3: The /params page (hosted by the embedded web server) lists all the sensor UniqueIDs and the available parameters that you can use in a file template.*

## Building from source

This project is built with Visual Studio 2017 (Community Edition).  To build from source, you will also need my general-purpose utility library, which must be downloaded separately, here: https://github.com/bp2008/BPUtil
