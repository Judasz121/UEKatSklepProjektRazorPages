function showToast(title, message, style = "info") {
    iziToast[style]({
        title: title,
        message: message
    })
}