let table = null;
let baseUrl = "https://localhost:44307/api/Perbaikan";

$(document).ready(function () {
    table = $('#table_perbaikan').DataTable({
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
                data: "keterangan",
            },
            {
                data: "biaya",
                render: function (data, type, row) {
                    return `${formatRupiah(row.biaya)}`
                }
            },
            {
                data: "jumlah"
            },
            {
                data: "status"
            },
            {
                data: "tanggal_Terima",
                render: function (data, type, row) {
                    return new Date(row.tanggal_Terima).toDateString();
                }
            },
            {
                data: "tanggal_Selesai",
                render: function (data, type, row) {
                    if (row.tanggal_Selesai == "1999-01-01T00:00:00") {
                        return `-`
                    } else {
                        return new Date(row.tanggal_Selesai).toDateString();
                    }
                }
            },


            {
                data: "",
                render: function (data, type, row) {
                    const isNotDiperiksa = row.status != "DIPERIKSA" && 'disabled';
                    return `<button class="btn btn-sm btn-success" ${isNotDiperiksa} data-toggle="modal" data-target="#perbaikanModal" onclick="Edit('${row.id}')">Edit</button>
                            <button class="btn btn-sm btn-danger" ${isNotDiperiksa} onclick="Delete('${row.id}');">Delete</button>`
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
    $("#errorBarang_id").html("");
    $("#errorKaryawan_id").html("");
    $("#errorKeterangan").html("");
    $("#errorTanggal_terima").html("");
    $("#errorJumlah").html("");
});


function Update() {
    const default_tanggal_selesai = "1999-01-01";
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
            url: baseUrl,
            type: "PUT",
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

function Edit(id) {
    let data = {};
    //Get Data saat render
    $.ajax({
        url: baseUrl + `/${id}`,
        type: "GET",
        contentType: "application/json;charset=utf-8"
    }).done((result) => {
        let { data } = result
        $("#barang_id").val(data.barang_Id);
        $("#karyawan_id").val(data.karyawan_Id);
        $("#keterangan").val(data.keterangan);
        $("#tanggal_terima").val(new Date(data.tanggal_Terima).toISOString().substring(0, 10));
        $("#jumlah").val(data.jumlah);
        console.log(result);
    }).fail((error) => {
        console.log(error);
    })
}

function Delete(id) {
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: baseUrl + `/${id}`,
                type: "Delete",
                contentType: "application/json;charset=utf-8"
            }).done((result) => {
                //buat alert pemberitahuan jika success
                Swal.fire(
                    'Berhasil',
                    'Riwayat sukses dihapus',
                    'success'
                )
                table.ajax.reload();
                console.log(result);
            }).fail((error) => {
                //alert pemberitahuan jika gagal
                Swal.fire(
                    'Oops',
                    'Riwayat gagal dihapus',
                    'error'
                )
                console.log(error);

            })
        }
    })
}

function formatRupiah(price) {
    if (price === 0 || price === null) {
        return price;
    } else {
        let rupiah = "";
        let angkarev = price.toString().split("").reverse().join("");
        for (let i = 0; i < angkarev.length; i++) if (i % 3 === 0) rupiah += angkarev.substr(i, 3) + ".";
        return (
            "Rp " +
            rupiah
                .split("", rupiah.length - 1)
                .reverse()
                .join("")
        );
    }
    return rupiah
}