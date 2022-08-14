function fetchWithAuth(url, token) {
    const headers = new window.Headers();
    headers.set("Authorization", `Bearer ${token}`);

    return window.fetch(url, { headers, method: 'GET' });
}

async function displayProtectedFile(url, token) {
    const response = await fetchWithAuth(url, token);

    const blob = await response.blob();

    var urlCreator = window.URL || window.webkitURL;

    const objectUrl = urlCreator.createObjectURL(blob);

    return objectUrl;
}