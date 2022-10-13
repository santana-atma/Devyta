let baseUrl = "https://localhost:44307/api/";


function getAll() {
    $("#totalAset").html(displaySpinner)
    $.ajax({
        url: baseUrl +"Barang",
        type: "Get",
        contentType: "application/json;charset=utf-8"
    }).done((result) => {
        let { data } = result;
        $("#totalAset").html(data.length)
    }).fail((error) => {
        console.log(error);
    })
}

function displaySpinner() {
    return `<div class="spinner-border" role="status">
              <span class="visually-hidden"></span>
            </div>`
}

$(document).ready(function () {
    getAll();
})