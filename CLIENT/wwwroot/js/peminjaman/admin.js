let table = null;
let baseUrl = "https://localhost:44307/api/Peminjaman";
$(document).ready(function () {
    table = $('#table_peminjaman').DataTable({
        ajax: {
            url: baseUrl,
            dataSrc: "data",
            dataType: "JSON"
        },
        columns: [
            {
                "data": null,
                "render": function (data, type, row, meta) {
                    return meta.row + meta.settings._iDisplayStart + 1;
                }
            },
            {
                data: "barang.nama",
            },
            {
                data: "karyawan.fullname",
            },
            {
                data: "tanggal_Pinjam",
                render: function (data, type, row) {
                    return new Date(row.tanggal_Pinjam).toDateString();
                }
            },
            {
                data: "tanggal_kembali",
                render: function (data, type, row) {
                    return new Date(row.tanggal_Kembali).toDateString();
                }
            },
            {
                data: "jumlah"
            },
            {
                data: "status"
            },

      
        ],
        
    });
});

