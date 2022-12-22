function setDialog() {
    $("#dialog").dialog({
        autoOpen: false,
        draggable: false,
        resizable: false,
        modal: true,
        height: 300,
        width: 320,
        show: {
            effect: "blind",
            duration: 1000
        },
        hide: {
            effect: "fade",
            duration: 1000
        }
    });
}

function showDialog() {
    $("#dialog").dialog("open");
}