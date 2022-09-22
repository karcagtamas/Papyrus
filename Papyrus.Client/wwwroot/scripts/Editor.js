getEditorValueByReference = function (element, withCursorLocation) {
    if (withCursorLocation) {
        updateCursor(element);
    }
    var content = element.innerHTML;

    return content;
}

setEditorValueByReference = function (element, value, clientId) {
    var modifiedValue = value;

    if (value.includes("‎")) {
        modifiedValue = value.replace("‎", "<span id='" + clientId + "'></span>");
    } else {
        modifiedValue += "<span id='" + clientId + "'></span>";
    }

    element.innerHTML = modifiedValue;

    // Refocus
    var cursor = document.getElementById(clientId);
    if (cursor) {
        setCursorPosition(cursor, 0);
        cursor.remove();
    } else {
        setCursorPosition(element, 0);
    }
}

updateCursor = function (el) {
    try {
        var range = window.getSelection().getRangeAt(0);
        var container = range.startContainer;
        var pos = range.startOffset;

        if (container && container.nodeValue) {
            container.insertData(pos, "‎");
        }
    } catch (err) {
        console.log("CURSOR CANNOT BE UPDATED\n", err);
    }
}

getCursorPosition = function () {
    return window.getSelection().getRangeAt(0).startOffset;
}

setCursorPosition = function (element, position) {
    try {
        var selection = window.getSelection();
        var range = document.createRange();
        range.setStart(element, position);
        range.collapse(true);
        selection.removeAllRanges();
        selection.addRange(range);
        element.focus();
    }
    catch (err) {
        console.error(err);
    }
}