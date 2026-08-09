A third party client for [Archipelago](archipelago.gg) aiming to provide better user experience and additional features.

Although this client was originally meant to work both locally and through the browser, only the local version is able to connect to Archipelago. You can still check out the browser version [here](https://trueniv.github.io/ArchipelagoAdvancedClient/), however due to technical limitations this version is not functional.

## This build is not fully tested or functional yet. It can connect to a room, but doesn't support most of the planned additional features yet.

## Running the desktop app

The desktop build uses [Photino](https://www.tryphotino.io/) to host the UI in a native window. Photino embeds the OS's own web engine rather than bundling one, so it has platform-specific runtime requirements:

- **Linux**: requires `webkit2gtk4.1` and `gtk3` to be installed. Note there are two incompatible WebKitGTK builds on modern distros — `webkit2gtk4.1` (GTK3, what Photino needs) and `webkitgtk6.0`/`webkitgtk-6.0` (GTK4, used by newer apps). Having only the GTK4 build installed is not enough; the GTK3 one is required specifically.
  - Fedora: `sudo dnf install webkit2gtk4.1 gtk3`
  - Debian/Ubuntu: `sudo apt install libwebkit2gtk-4.1-0 libgtk-3-0`
  - If the app exits immediately with a `DllNotFoundException` / `libwebkit2gtk-4.1.so.0: cannot open shared object file` error, this is the cause.
- **Windows**: requires the Microsoft Edge WebView2 Runtime, which ships preinstalled on most current Windows 10/11 systems. If missing, install it from [Microsoft's WebView2 page](https://developer.microsoft.com/microsoft-edge/webview2/).
