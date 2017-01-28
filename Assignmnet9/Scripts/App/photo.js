function previewImage(event) {
    // Get a reference to the image element
    var div = document.querySelector("#photoPreview");
    // Add an img element
    div.innerHTML = '<p><img src="' + URL.createObjectURL(event.target.files[0]) + '" alt="" width="300"></p>'
};