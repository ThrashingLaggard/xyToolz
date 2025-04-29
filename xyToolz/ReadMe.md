
---

## Modules

- **xyToolz**: Base library with all reusable helpers (e.g., `xyFiles`, `xyLog`, `xyJson`, `xyRsa`)
- **xyPorts**: Networking tools to handle TCP/UDP ports, listeners and connections
- **xyAndroid**: Android-specific utilities like permissions, foreground service handling
- **xyAvalonia**: Frontend console for debugging output and simple GUI interactions
- **xyToolz_Exec**: CLI runner for executing and testing helpers interactively

---

## Key Namespaces

### `xyToolz`

- `xyFiles`, `xyDirUtils` — File and folder operations
- `xyJson` — Read/write/serialize JSON
- `xyLog`, `xyLogFormatter`, `xyLogArchiver` — Central logging system
- `xyDataProtector`, `xyRsa`, `xyHashHelper` — Crypto & data security
- `xyConversion`, `xyColQol`, `xyCard`, `xyPathHelper` — Various helper utilities

### `xyPorts`

- `xyPortChecker`, `xyPortManager` — Check port usage, kill processes
- `xyTcpClient`, `xyTcpListener`, `xyTcpPort` — TCP tools
- `xyUdpClient`, `xyUdpPort` — UDP tools

### `xyAndroid`

- `xyFilesAndroid`, `xyALog`, `xyForeGroundService`
- `IStoragePermissionService` — Permission abstraction

### `xyAvalonia`

- `ConsoleTextBoxWriter`, `MainWindowViewModel` — Debugging frontend

---

## Usage Examples

```csharp
// Logging
xyLog.Log("Application started.");

// Read JSON from file
var obj = xyJson.ReadFromFile<MyConfig>("config.json");

// Encrypt a string with RSA
string encrypted = xyRsa.Encrypt("Hello", publicKey);

// List open TCP ports
var openPorts = new xyPortChecker().GetOpenPorts();
