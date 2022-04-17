getEditorValueByReference = function (element) {
    return element.innerHTML;
}

setEditorValueByReference = function (element, value) {
    element.innerHTML = value;
}

getCursorPosition = function () {
    return window.getSelection().getRangeAt(0).startOffset;
}

setCursorPosition = function (element, position) {
    var selection = window.getSelection();
    console.log(selection);
    var range = document.createRange();
    range.setStart(element.childNodes[0], position);
    range.collapse(true);
    selection.removeAllRanges();
    selection.addRange(range);
    element.focus();
}