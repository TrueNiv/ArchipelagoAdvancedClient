// Companion module for Archipelago.MultiClient.Net's browser socket transport
// (Helpers/BrowserWebSocketInterop.cs in the Archipelago.MultiClient.Net submodule).
//
// System.Net.WebSockets.ClientWebSocket.ConnectAsync hangs indefinitely under single-threaded
// Blazor WebAssembly, so the browser build talks to a plain native WebSocket through this module
// instead. Loaded via JSHost.ImportAsync at the path BrowserWebSocketInterop.ModulePath, which
// must match this file's location relative to wwwroot.
//
// Deliberately request/response only (no JS-to-.NET callbacks): .NET polls wsGetState/
// wsDequeueMessage on a timer, which keeps this file's C# counterpart to the simplest, most
// well-supported primitive-typed [JSImport] surface (string/int only).

let nextHandle = 1;
const sockets = new Map();

export function wsConnect(url) {
    const handle = nextHandle++;
    const entry = { ws: null, queue: [], closeCode: 1006, closeReason: "", errored: false };
    sockets.set(handle, entry);

    try {
        const ws = new WebSocket(url);
        entry.ws = ws;

        ws.onmessage = (event) => {
            entry.queue.push(typeof event.data === "string" ? event.data : "");
        };
        ws.onclose = (event) => {
            entry.closeCode = event.code;
            entry.closeReason = event.reason || "";
        };
        ws.onerror = () => {
            entry.errored = true;
        };
    } catch {
        entry.errored = true;
    }

    return handle;
}

export function wsGetState(handle) {
    const entry = sockets.get(handle);
    if (!entry || !entry.ws || entry.errored) return 3; // CLOSED
    return entry.ws.readyState;
}

export function wsDequeueMessage(handle) {
    const entry = sockets.get(handle);
    if (!entry || entry.queue.length === 0) return null;
    return entry.queue.shift();
}

export function wsGetCloseCode(handle) {
    const entry = sockets.get(handle);
    return entry ? entry.closeCode : 1006;
}

export function wsGetCloseReason(handle) {
    const entry = sockets.get(handle);
    return entry ? entry.closeReason : "";
}

export function wsSend(handle, text) {
    const entry = sockets.get(handle);
    if (entry && entry.ws) entry.ws.send(text);
}

export function wsClose(handle) {
    const entry = sockets.get(handle);
    if (entry && entry.ws) entry.ws.close();
    sockets.delete(handle);
}
