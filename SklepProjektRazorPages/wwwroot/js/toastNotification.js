
// this file and function's sole purpose is to provide compatibility in case of changing the toast notification library

function showToast(title, message, style = "info") {
    if (style == "danger")
        style = "error";
    iziToast[style]({
        title: title,
        message: message
    });
}