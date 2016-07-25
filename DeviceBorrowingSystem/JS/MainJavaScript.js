

function OpenEquipmentDetailPage(id) {
    alert("Click!");
    //var hdc = UPDATE("EquipmentDetail.aspx?id=" + id, "", "width=580px; height=400px");
    //width = screen.width;
    //height = screen.height;

    //hdc.moveTo((width - 580) / 2, (height - 400) / 2);
}

function SelectImgFile() {
    document.getElementById("idFile").click();
}

function __doPostBack(eventTarget, eventArgument) {
    if (!theForm.onsubmit || (theForm.onsubmit() != false)) {
        theForm.__EVENTTARGET.value = eventTarget;
        theForm.__EVENTARGUMENT.value = eventArgument;
        theForm.submit();
    }
}

function GetTimeZone() {
    var timeZone = new Date().getTimezoneOffset() / 60 * -1;
    if (timeZone == null)
        timeZone = 8;
    return timeZone;
}