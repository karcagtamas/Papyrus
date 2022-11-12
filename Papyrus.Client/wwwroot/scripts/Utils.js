function fetchWithAuth(url, token) {
    const headers = new window.Headers();
    headers.set("Authorization", `Bearer ${token}`);

    return window.fetch(url, { headers, method: "GET" });
}

async function displayProtectedFile(url, token) {
    const response = await fetchWithAuth(url, token);

    const blob = await response.blob();

    var urlCreator = window.URL || window.webkitURL;

    const objectUrl = urlCreator.createObjectURL(blob);

    return objectUrl;
}

function copyToClipboard(text) {
    if (!navigator.clipboard) {
        fallbackCopyTextToClipboard(text);
        return;
    }
    navigator.clipboard.writeText(text).then(
        function () {
            console.log("Async: Copying to clipboard was successful!");
        },
        function (err) {
            console.error("Async: Could not copy text: ", err);
        }
    );
}

function fallbackCopyTextToClipboard(text) {
    var textArea = document.createElement("textarea");
    textArea.value = text;

    // Avoid scrolling to bottom
    textArea.style.top = "0";
    textArea.style.left = "0";
    textArea.style.position = "fixed";

    document.body.appendChild(textArea);
    textArea.focus();
    textArea.select();

    try {
        var successful = document.execCommand("copy");
        var msg = successful ? "successful" : "unsuccessful";
        console.log("Fallback: Copying text command was " + msg);
    } catch (err) {
        console.error("Fallback: Oops, unable to copy", err);
    }

    document.body.removeChild(textArea);
}
