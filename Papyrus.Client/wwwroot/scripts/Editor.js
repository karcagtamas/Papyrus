getEditorValueByReference = function (element, withCursorLocation) {
    if (withCursorLocation) {
        updateCursor();
    }
    var content = element.innerHTML;

    return content;
}

setEditorValueByReference = function (element, value, clientId) {
    element.innerHTML = value.replace("‎", "<span id='" + clientId + "' />");
    var cursor = document.getElementById(clientId);
    if (cursor) {
        setCursorPosition(cursor, 0);
        cursor.remove();
    } else {
        setCursorPosition(element, 0);
    }
}

updateCursor = function () {
    try {
        var range = window.getSelection().getRangeAt(0);
        var container = range.startContainer;
        var pos = range.startOffset;

        if (container && container.nodeValue) {
            container.insertData(pos, "‎");
        }
    } catch (err) {
        console.log("CURSOR CANNOT BE UPDATED", err);
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