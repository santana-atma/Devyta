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

            {
                data: "",
                render: function (data, type, row) {
                    const isKembali = row.status == "KEMBALI" && 'disabled';
                    return `<button class="btn btn-sm btn-warning" ${isKembali} onclick="Pengembalian('${row.id}');">Kembalikan</button>
                            <button class="btn btn-sm btn-success" ${isKembali} data-toggle="modal" data-target="#peminjamanModal" onclick="Edit('${row.id}')">Edit</button>
                            <button class="btn btn-sm btn-danger" ${isKembali} onclick="Delete('${row.id}');">Delete</button>`
                }
            },
        ],
        
    });
});

function validasiInputan(obj) {
    let error = 0;
    if (obj.barang_id == 0 || obj.barang_id == NaN) {
        $("#errorBarang_id").html("ID Barang tidak boleh kosong")
        error++;
    }
    if (obj.karyawan_id == 0 || obj.karyawan_id == NaN) {
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

$('#peminjamanModal').on('hidden.bs.modal', function () {
    $("#barang_id").val("");
    $("#karyawan_id").val("");
    $("#tanggal_pinjam").val("");
    $("#tanggal_kembali").val("");
    $("#jumlah").val("");
    $("#errorBarang_id").html("")
    $("#errorKaryawan_id").html("")
    $("#errorTanggal_pinjam").html("")
    $("#errorTanggal_kembali").html("")
    $("#errorJumlah").html("")
});

function Update() {
    const default_tanggal_kembali = "1999-01-01";
    console.log($("#tanggal_kembali").val());
    let id = $("#idPeminjaman").val();
    let barang_id = parseInt($("#barang_id").val());
    let karyawan_id = parseInt($("#karyawan_id").val());
    let tanggal_pinjam = $("#tanggal_pinjam").val();
    let tanggal_kembali = $("#tanggal_kembali").val() == "" ? default_tanggal_kembali : $("#tanggal_kembali").val();
    let jumlah = parseInt($("#jumlah").val());
    let obj = { barang_id, karyawan_id, tanggal_pinjam, jumlah };
    let validation = validasiInputan(obj)
    if (validation == 0) {
        if (id != -1) { //kalo id -1 berarti add
             //kalo id tidak -1 berarti update
            let data = {};
            //ini masih hardcode
            data.barang_Id = barang_id;
            data.karyawan_Id = karyawan_id;
            data.tanggal_Pinjam = tanggal_pinjam;
            data.tanggal_Kembali = tanggal_kembali;
            data.jumlah = jumlah;
            //isi dari object kalian buat sesuai dengan bentuk object yang akan di post
            $.ajax({
                url: baseUrl + `/${id}`,
                type: "PUT",
                data: JSON.stringify(data), //jika terkena 415 unsupported media type (tambahkan headertype Json & JSON.Stringify();)
                contentType: "application/json;charset=utf-8"
            }).done((result) => {
                //buat alert pemberitahuan jika success
                Swal.fire(
                    'Berhasil',
                    'Peminjaman sukses diubah',
                    'success'
                )
                table.ajax.reload();
                $('#peminjamanModal').modal('toggle');
                console.log(result);
                $("#idPeminjaman").val(-1)
            }).fail((error) => {
                //alert pemberitahuan jika gagal
                Swal.fire(
                    'Gagal',
                    'Peminjaman gagal diubah',
                    'error'
                )
                console.log(error);
            })
        }
    }
}

function Pengembalian(id) {
    let newData = {};
    $.ajax({
        url: baseUrl + `/${id}`,
        type: "GET",
        contentType: "application/json;charset=utf-8"
    }).done((result) => {
        let { data } = result
        console.log("Data : ", data);
        //ini masih hardcode          
        newData.barang_id = data.barang_Id;
        newData.karyawan_id = data.karyawan_Id;
        newData.tanggal_pinjam = data.tanggal_Pinjam;
        newData.tanggal_kembali = new Date().toISOString().substring(0, 10);
        newData.status = "KEMBALI";
        newData.jumlah = data.jumlah;
        //buat alert pemberitahuan jika success

        $.ajax({
            url: baseUrl + `/${id}`,
            type: "PUT",
            data: JSON.stringify(newData), //jika terkena 415 unsupported media type (tambahkan headertype Json & JSON.Stringify();)
            contentType: "application/json;charset=utf-8"
        }).done((result) => {
            //buat alert pemberitahuan jika success
            Swal.fire(
                'Berhasil',
                'Pengembalian aset sukses',
                'success'
            )
            table.ajax.reload();
            console.log(result);
        }).fail((error) => {
            //alert pemberitahuan jika gagal
            Swal.fire(
                'Gagal',
                'Pengembalian aset gagal',
                'error'
            )
            console.log(error);
        })

    }).fail((error) => {
        console.log(error);
    });
    console.log("New Data : ", newData);


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
        $("#idPeminjaman").val(id)
        $("#barang_id").val(data.barang_Id)
        $("#karyawan_id").val(data.karyawan_Id)
        $("#tanggal_pinjam").val(new Date(data.tanggal_Pinjam).toISOString().substring(0, 10))
        $("#tanggal_kembali").val(new Date(data.tanggal_Kembali == '1999-01-01' ? null : data.tanggal_Kembali).toISOString().substring(0, 10))
        $("#jumlah").val(data.jumlah)
        console.log(result)
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

