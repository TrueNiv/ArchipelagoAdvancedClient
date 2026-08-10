// Small interop helpers for Chat.razor's auto-scroll-to-bottom behavior.
// Loaded via IJSRuntime's standard ES module import (Microsoft.JSInterop), not the low-level
// JSHost/[JSImport] API used by the browser WebSocket transport - this is the mainstream Blazor
// interop path and works the same way across every render mode/hosting model.

export function isAtBottom(element, thresholdPx) {
    if (!element) return true;
    const threshold = thresholdPx ?? 8;
    return element.scrollHeight - element.scrollTop - element.clientHeight <= threshold;
}

export function scrollToBottom(element) {
    if (!element) return;
    element.scrollTop = element.scrollHeight;
}
