getEditorValueByReference = function (element) {
    updateCursor();
    var content = element.innerHTML;

    return content.replace("‎", "[{CURRENT_CURSOR}]");
}

setEditorValueByReference = function (element, value, clientId) {
    element.innerHTML = value;
    var cursor = document.getElementById(clientId);
    if (cursor) {
        setCursorPosition(cursor, 0);
        cursor.remove();
    } else {
        setCursorPosition(element, 0);
    }
}

updateCursor = function () {
    var range = window.getSelection().getRangeAt(0);
    var container = range.startContainer;
    var pos = range.startOffset;

    if (container && container.nodeValue) {
        container.insertData(pos, "‎");
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