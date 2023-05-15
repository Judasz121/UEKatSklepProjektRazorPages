function sendAjax(pagePath, handler, data, customOptions = {}) {
    let antiForgeryToken = null;
    try {
        antiForgeryToken = $(`[name="__RequestVerificationToken"]`).val();
    }
    catch (error) {
        showToast("Anti Forgery Token", "needs to be outputted to the page: @Html.AntiForgeryToken() ", "danger");
        return false;
    }
    let url = Boolean(pagePath) === false ? window.location.pathname : pagePath;
    url += "?handler=" + handler;
    return $.ajax({
        url: url,
        method: "POST",
        contentType: 'application/x-www-form-urlencoded',
        dataType: 'json',
        headers: {
            'RequestVerificationToken': antiForgeryToken
        },
        data: data,
        ...customOptions,
    });
}