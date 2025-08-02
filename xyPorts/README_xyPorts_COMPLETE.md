# xyPorts

**xyPorts** is a modular .NET library for managing, checking, and communicating over TCP and UDP ports. It offers simple wrapper classes for port analysis, message exchange, and listener control.

---

## Table of Contents

- [Overview](#overview)
- [Project Structure](#project-structure)
- [Available Classes](#available-classes)
- [Usage](#usage)
- [Dependencies](#dependencies)
- [License](#license)

---

## Overview

This project was created to provide simple and reusable tools for working with TCP and UDP network ports. In addition to sending and receiving messages, it supports checking port availability and dynamically starting or stopping listeners.

---

## Project Structure

```
xyPorts/
├── Basix/
│   ├── xyPort.cs
│   └── xyPortChecker.cs
├── TCP/
│   ├── xyTcpPort.cs
│   ├── xyTcpClient.cs
│   └── xyTcpListener.cs
├── UDP/
│   ├── xyUdpPort.cs
│   └── xyUdpClient.cs
└── Master/
    └── xyPortManager.cs
```

---

## Available Classes

### Namespace: `xyPorts.Master`

- **`xyPortManager`**  
  Top-level class to manage and control ports.  
  Holds instances of `xyPort`, `xyTcpPort`, and `xyUdpPort`.  
  Provides `ClosePort(int port)` to kill the process using the port.

---


### Namespace: `xyPorts.Basix`

- **`xyPort`**  
  Represents a generic port with properties like `IsListening`, `IsExclusive`, `PortID`, and `Information`.

- **`xyPortChecker`**  
  Provides methods to analyze open ports:  
  - `GetOpenPorts()`  
  - `IsPortAvaillable(int)`  
  - `ArePortsAvaillable(List<int>)`

---

### Namespace: `xyPorts.TCP`

- **`xyTcpPort`**  
  Specialized version of `xyPort` for TCP. Includes `TcpClient`, `TcpListener`, `xyTcpClient`, `xyTcpListener`, `ConnectionInformation`, `Statistics`.

- **`xyTcpClient`**  
  Methods to establish a connection and send messages:
  - `ConnectTCP(string ip, ushort port, string message)`
  - `SendMsgTcp(...)`
  - `ReceiveResponseTCP(...)`

- **`xyTcpListener`**  
  Starts and manages TCP listeners:  
  - `Listen(ushort port)`  
  - `StopListeningTcp(ushort port)`  
  - `SendConfirmationResponse(...)`  
  - `ReceivedMessage` as EventHandler  

---

### Namespace: `xyPorts.UDP`

- **`xyUdpPort`**  
  Specialized version of `xyPort` for UDP. Includes `UdpClient`, `xyUdpClient`, `UdpStatistics`, `UdpReceiveResult`.

- **`xyUdpClient`**  
  Methods for UDP communication:  
  - `ConnectUDP(string ip, ushort port, string message)`  
  - `ReceiveDataUDP(...)`  
  - `ClosePortUdp(...)`  

---

## Usage

```csharp
// Example: Check port availability
var checker = new xyPortChecker();
bool isFree = checker.IsPortAvaillable(13000);

// Open a TCP connection
xyTcpClient.ConnectTCP("127.0.0.1", 13000, "Hello TCP!");

// Send a UDP message
xyUdpClient.ConnectUDP("127.0.0.1", 12824, "Hello UDP!");

// Start a TCP listener
var listener = new xyTcpListener();
listener.Listen(13000);
```

---

## Dependencies

- .NET Standard / .NET Core
- `xyToolz` (for utilities like logging)
- `System.Net`, `System.Net.Sockets`
- `Microsoft.Extensions.Logging`

---

## License

This project is licensed under the **GNU General Public License v3.0** (GPLv3).  
See the [LICENSE](https://www.gnu.org/licenses/gpl-3.0.en.html) file for more information.

---

## Author

> Project by ThrashingLaggard
> Part of `xyToolz`  
