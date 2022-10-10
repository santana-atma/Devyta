let table = null;
let baseUrl = "https://localhost:44307/api/Peminjaman";
let postUrl = "https://localhost:44307/api/Perbaikan";
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
                data: "tanggal_Kembali",
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

            {
                data: "",
                render: function (data, type, row) {
                    const isKembali = row.status == "KEMBALI" && 'disabled';
                    return `<button class="btn btn-sm btn-success" 
                            ${isKembali} 
                            data-toggle="modal"
                            data-target="#perbaikanModal"
                            onclick="Perbaikan(${row.barang_Id},${row.karyawan_Id})">Perbaikan</button>
                            `
                }
            },
        ],

    });
});


function validasiInputan(obj) {
    let error = 0;
    if (obj.tanggal_Terima == "") {
        $("#errorTanggal_terima").html("Tanggal Terima tidak boleh kosong")
        error++;
    }
    if (obj.keterangan == "") {
        $("#errorKeterangan").html("Keterangan tidak boleh kosong")
        error++;
    }
    if (obj.jumlah == 0 || obj.jumlah == NaN) {
        $("#errorJumlah").html("Jumlah tidak boleh kosong")
        error++;
    }
    return error;
}

$('#perbaikanModal').on('hidden.bs.modal', function () {
    $("#barang_id").val("");
    $("#karyawan_id").val("");
    $("#keterangan").val("");
    $("#tanggal_terima").val("");
    $("#jumlah").val("");
    $("#errorKeterangan").html("");
    $("#errorTanggal_terima").html("");
    $("#errorJumlah").html("");
});

function Perbaikan(barang_Id, karyawan_Id) {
    $("#barang_id").val(barang_Id);
    $("#karyawan_id").val(karyawan_Id);
}

function Insert() {
    const default_tanggal_selesai = "1999-01-01";
    console.log($("#tanggal_selesai").val());
    let barang_id = parseInt($("#barang_id").val());
    let karyawan_id = parseInt($("#karyawan_id").val());
    let keterangan = $("#keterangan").val();
    let tanggal_Terima = $("#tanggal_terima").val();
    let tanggal_Selesai = default_tanggal_selesai;
    let jumlah = parseInt($("#jumlah").val());
    let obj = { keterangan, tanggal_Terima, jumlah };
    let validation = validasiInputan(obj)
    if (validation == 0) {
       
            let data = {};

            data.barang_Id = barang_id;
            data.karyawan_Id = karyawan_id;
            data.keterangan = keterangan;
            data.tanggal_Terima = new Date(tanggal_Terima).toISOString().substring(0, 10);
            data.tanggal_Selesai = tanggal_Selesai;
            data.jumlah = jumlah;
            data.status = "DIPERIKSA";
            //isi dari object kalian buat sesuai dengan bentuk object yang akan di post
            $.ajax({
                url: postUrl,
                type: "POST",
                data: JSON.stringify(data), //jika terkena 415 unsupported media type (tambahkan headertype Json & JSON.Stringify();)
                contentType: "application/json;charset=utf-8"
            }).done((result) => {
                //buat alert pemberitahuan jika success
                Swal.fire(
                    'Berhasil',
                    'Perbaikan sukses ditambahkan',
                    'success'
                )
                table.ajax.reload();
                $('#perbaikanModal').modal('toggle');
                console.log(result);
            }).fail((error) => {
                //alert pemberitahuan jika gagal
                Swal.fire(
                    'Gagal',
                    'Perbaikan gagal ditambahkan',
                    'error'
                )
                console.log(error);
            })
        
    }
}