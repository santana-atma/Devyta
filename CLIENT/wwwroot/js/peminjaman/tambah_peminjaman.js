let table = null;
let karyawan;
let baseUrl = "https://localhost:44307/api/Barang";
let postUrl = "https://localhost:44307/api/Peminjaman";
let getKaryawan = "https://localhost:44307/api/karyawan";
$(document).ready(function () {
    // Initialize select2

    table = $('#table_aset').DataTable({
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
                data: "nama"
            },
            {
                data: "stok"
            },
            {
                data: "satuan"
            },
            {
                data: "",
                render: function (data, type, row) {
                    const isOnStock = row.stok < 1 && 'disabled';
                    return `<button type="button" 
                            class="btn btn-primary"
                            data-toggle="modal" ${isOnStock}
                            data-target="#peminjamanModal"
                            onclick="Peminjaman('${row.id}');">
                                Pinjam
                                </button>`


                }
            },
        ]
    });

    $.ajax({
        url: getKaryawan,
        type: "GET",
        contentType: "application/json;charset=utf-8"
    }).done((result) => {
        let { data } = result;
        console.log(data);
        data.map((x) => {
            console.log(x.id)
            return $('#karyawan_id').append(`<option value="${x.id}">${x.fullName}</option>`);
        })
       
    }).fail((error) => {
        console.log(error);
    })

});

$('#peminjamanModal').on('hidden.bs.modal', function () {
    $("#karyawan_id").val(0);
    $("#tanggal_pinjam").val("");
    $("#tanggal_kembali").val("");
    $("#jumlah").val("");
    $("#errorBarang_id").html("")
    $("#errorKaryawan_id").html("")
    $("#errorTanggal_pinjam").html("")
    $("#errorTanggal_kembali").html("")
    $("#errorJumlah").html("")
});


function validasiInputan(obj) {
    let error = 0;
    if (obj.barang_id == 0 || obj.barang_id == NaN) {
        $("#errorBarang_id").html("ID Barang tidak boleh kosong")
        error++;
    }
    if (obj.karyawan_id == "" || obj.karyawan_id == NaN) {
        $("#errorKaryawan_id").html("Id Karyawan tidak boleh kosong")
        error++;
    }
    if (obj.tanggal_pinjam == "") {
        $("#errorTanggal_pinjam").html("Tanggal Pinjam tidak boleh kosong")
        error++;
    }
    if (obj.jumlah == 0 || obj.jumlah == NaN) {
        $("#errorJumlah").html("Jumlah tidak boleh kosong")
        error++;
    }
    return error;
}

function Peminjaman(id) {
    $("#barang_id").val(id);
}


function Insert() {
    const default_tanggal_kembali = "1999-01-01";
    let id = $("#idPeminjaman").val();
    let barang_id = parseInt($("#barang_id").val());
    let karyawan_id = parseInt($("#karyawan_id").val());
    let tanggal_pinjam = $("#tanggal_pinjam").val();
    let tanggal_kembali = $("#tanggal_kembali").val() == "" ? default_tanggal_kembali : $("#tanggal_kembali").val();
    let jumlah = parseInt($("#jumlah").val());
    let obj = { barang_id, karyawan_id, tanggal_pinjam, jumlah };
    let validation = validasiInputan(obj)
    if (validation == 0) {
        if (id == -1) { //kalo id -1 berarti add
            let data = {};
            //ini masih hardcode          
            data.barang_Id = barang_id;
            data.karyawan_Id = karyawan_id;
            data.tanggal_Pinjam = new Date(tanggal_pinjam).toISOString().substring(0, 10);
            data.tanggal_Kembali = new Date(tanggal_kembali).toISOString().substring(0, 10);;
            data.status = "PINJAM";
            data.jumlah = jumlah;

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
                    'Peminjaman sukses ditambahkan',
                    'success'
                )
                table.ajax.reload();
                $('#peminjamanModal').modal('toggle');
                console.log(result);
            }).fail((error) => {
                //alert pemberitahuan jika gagal
                Swal.fire(
                    'Gagal',
                    'Peminjaman gagal ditambahkan',
                    'error'
                )
                console.log(error);

            })
        }
    }
}

