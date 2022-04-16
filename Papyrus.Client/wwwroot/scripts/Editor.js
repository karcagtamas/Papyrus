let observer;

startEditorObserve = function (element) {
    observer = new MutationObserver((records) => console.log(records[0]));

    observer.observe(element, {
        characterData: true,
        subtree: true,
        attributes: true,
        childList: true,
    });
}

getEditorValueByReference = function (element) {
    return element.innerHTML;
}

setEditorValueByReference = function (element, value) {
    console.log(value);
    //var target = element.createTextNode("\u0001");
    //element.getSelection().getRangeAt(0).insertNode(target);
    //element.innerHTML = value;
    //var position = element.innerHTML.indexOf("\u0001");
    //element.getSelection().setPosition(position);
    //target.parentNode.removeChild(target);
}

getCursorPositon = function (element) {
    return window.getSelection().getRangeAt(0).startOffset;
}