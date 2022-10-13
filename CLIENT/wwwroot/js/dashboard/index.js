let baseUrl = "https://localhost:44307/api/";
let namaAset = [];
let totalPerbaikan = [];
let totalPenjam = [];
let totalKeseluruhan = [];

function getAll(callback) {
    $.ajax({
        url: baseUrl +"Dashboard",
        type: "Get",
        contentType: "application/json;charset=utf-8"
    }).done((result) => {
        let { data } = result;
        data.forEach((item) => {
            namaAset.push(item.namaAset)
            totalKeseluruhan.push(item.total_Keseluruhan)
            totalPenjam.push(item.total_Peminjaman)
            totalPerbaikan.push(item.total_Perbaikan)
        })
        callback()
    }).fail((error) => {
        console.log(error);
    })
}

function GetTotalAngka() {
    $("#totalAset").html(displaySpinner)
    $("#totalAdmin").html(displaySpinner)
    $("#totalStaff").html(displaySpinner)
    $("#totalPeminjam").html(displaySpinner)
    $.ajax({
        url: baseUrl + "Dashboard/TotalData",
        type: "Get",
        contentType: "application/json;charset=utf-8"
    }).done((result) => {
        let { data } = result;
        $("#totalAset").html(data.totalAset)
        $("#totalAdmin").html(data.totalAdmin)
        $("#totalStaff").html(data.totalStaff)
        $("#totalPeminjam").html(data.totalPeminjaman)
       console.log(data)
    }).fail((error) => {
        console.log(error);
    })
}

function displaySpinner() {
    return `<div class="spinner-border" role="status">
              <span class="visually-hidden"></span>
            </div>`
}
const config = () => {
    var myBarChart = new Chart(document.getElementById('myChart'), {
        type: 'bar',
        data: data,
    });
};
$(document).ready(function () {
    getAll(config);
    GetTotalAngka()

})


var data = {
    labels: namaAset,
    datasets: [
        {
            label: "Total Pinjam",
            backgroundColor: "blue",
            data: totalPenjam
        },
        {
            label: "Total Keseluruhan",
            backgroundColor: "red",
            data: totalKeseluruhan
        },
        {
            label: "Total Perbaikan",
            backgroundColor: "green",
            data: totalPerbaikan
        }
    ]
};



