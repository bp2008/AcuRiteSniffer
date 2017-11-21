# AcuRiteSniffer
Reads weather data packets sent by AcuRite SmartHUBs and makes the data accessible locally.  Requires that the network be configured to mirror packets to the computer running this service.

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

So if your SmartHUB's address is `192.168.0.135` and your PC's address is `192.168.0.100`, then you would run this command in your router at least once after your router boots up.

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

## Building from source

This project is built with Visual Studio 2017 (Community Edition).  To build from source, you will also need my general-purpose utility library, which must be downloaded separately, here: https://github.com/bp2008/BPUtil
