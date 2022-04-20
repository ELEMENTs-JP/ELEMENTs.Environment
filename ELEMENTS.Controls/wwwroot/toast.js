
export function initShow(btnID, toastID, dotnethelper)
{
    try
    {
        var btn = document.getElementById(btnID);
        if (btn === null) { return; }

        var element = document.getElementById(toastID);
        if (element === null) { return; }

        // Create toast instance
        var myToast = new bootstrap.Toast(element, {
            autohide: true
        });

        btn.addEventListener("click", function () {
            myToast.show();
        });
    }
    catch (e) {
        alert(e);
    }
}
export function showToast(toastID) {
    try {

        var element = document.getElementById(toastID);
        if (element === null) { return; }

        // Create toast instance
        var myToast = new bootstrap.Toast(element, {
            autohide: true
        });

        if (myToast !== null) { myToast.show(); }

    }
    catch (e) {
        alert(e);
    }
}

export function showToastWithMessage(toastID, title, titleID, message, messageID) {
    try {

        var element = document.getElementById(toastID);
        if (element === null) { return; }

        var titleELEMENT = document.getElementById(titleID);
        if (titleELEMENT === null) { return; }
        titleELEMENT.innerText = title;

        var msgELEMENT = document.getElementById(messageID);
        if (msgELEMENT === null) { return; }
        msgELEMENT.innerText = message;

        // Create toast instance
        var myToast = new bootstrap.Toast(element, {
            autohide: true
        });

        if (myToast !== null) { myToast.show(); }

    }
    catch (e) {
        alert(e);
    }
}

