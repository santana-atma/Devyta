let table = null;
let baseUrl = "https://localhost:44307/api/Barang";
let role = $("#role").val();
let isAdmin = role.toLowerCase() == "admin";
let totalAset = 0;
$(document).ready(function () {
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
                    if (isAdmin && row.stok == 0) //jika admin dan barang stok kosong
                    {
                        return `<button class="btn btn-sm btn-primary" data-toggle="modal" data-target="#detailAset" onclick="Detail('${row.id}');">Detail</button>
                                <button class="btn btn-sm btn-success" data-toggle="modal" data-target="#createModal" onclick="Edit('${row.id}')">Edit</button>
                                <button class="btn btn-sm btn-danger" onclick="Delete('${row.id}');">Delete</button>`
                    }
                    else if (isAdmin == false) //jika bukan admin hanya bisa lihat detail barang
                    {
                        return `<button class="btn btn-sm btn-primary" data-toggle="modal" data-target="#detailAset" onclick="Detail('${row.id}');">Detail</button>`
                    }
                    else
                    {
                        return `<button class="btn btn-sm btn-primary" data-toggle="modal" data-target="#detailAset" onclick="Detail('${row.id}');">Detail</button>
                                <button class="btn btn-sm btn-success" data-toggle="modal" data-target="#createModal" onclick="Edit('${row.id}')">Edit</button>
                                <button class="btn btn-sm btn-danger disabled">Delete</button>`
                    }
                },
            },
        ]
    });
});

$('#createModal').on('hidden.bs.modal', function () {
    $("#idAset").val(-1);
    $("#nama").val("");
    $("#satuan").val("");
    $("#errorNama").html("")
    $("#errorSatuan").html("")
});


function validasiInputan(nama, satuan) {
    let error = 0;
    if (nama == "") {
        $("#errorNama").html("Nama tidak boleh kosong")
        error++;
    }
    if (satuan == "") {
        $("#errorSatuan").html("Satuan tidak boleh kosong")
        error++;
    }
    return error;
}
function Insert() {
    let nama = $("#nama").val();
    let satuan = $("#satuan").val();
    let id = $("#idAset").val();
    let validation = validasiInputan(nama, satuan)
    if (validation == 0) {
        if (id == -1) { //kalo id -1 berarti add
            let data = {};
            //ini ngambil value dari tiap inputan di form nya
            data.nama = $("#nama").val();
            data.satuan = $("#satuan").val();
            //isi dari object kalian buat sesuai dengan bentuk object yang akan di post
            $.ajax({
                url: baseUrl,
                type: "POST",
                data: JSON.stringify(data), //jika terkena 415 unsupported media type (tambahkan headertype Json & JSON.Stringify();)
                contentType: "application/json;charset=utf-8"
            }).done((result) => {
                //buat alert pemberitahuan jika success
                Swal.fire(
                    'Berhasil',
                    'Aset Sukses ditambah',
                    'success'
                )
                table.ajax.reload();
                $('#createModal').modal('toggle');
                console.log(result);
            }).fail((error) => {
                //alert pemberitahuan jika gagal
                Swal.fire(
                    'Gagal',
                    error.responseJSON.message,
                    'error'
                )
                console.log(error);

            })
        } else { //kalo id tidak -1 berarti update
            let data = {};
            //ini ngambil value dari tiap inputan di form nya
            data.nama = $("#nama").val();
            data.satuan = $("#satuan").val();
            //isi dari object kalian buat sesuai dengan bentuk object yang akan di post
            $.ajax({
                url: baseUrl + `/${id}`,
                type: "Put",
                data: JSON.stringify(data), //jika terkena 415 unsupported media type (tambahkan headertype Json & JSON.Stringify();)
                contentType: "application/json;charset=utf-8"
            }).done((result) => {
                //buat alert pemberitahuan jika success
                Swal.fire(
                    'Berhasil',
                    'Aset Sukses diubah',
                    'success'
                )
                table.ajax.reload();
                $('#createModal').modal('toggle');
                console.log(result);
            }).fail((error) => {
                //alert pemberitahuan jika gagal
                Swal.fire(
                    'Gagal',
                    'Aset Gagal diubah',
                    'error'
                )
                console.log(error);
            })
        }
    }
}

function Edit(id)
{
    let data = {};
    //ini ngambil value dari tiap inputan di form nya
    $.ajax({
        url: baseUrl+ `/${id}`,
        type: "Get",
        contentType: "application/json;charset=utf-8"
    }).done((result) => {
        let { data } = result
        $("#idAset").val(id)
        $("#nama").val(data.barang.nama)
        $("#satuan").val(data.barang.satuan)
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
                    'Aset Sukses dihapus',
                    'success'
                )
                table.ajax.reload();
                console.log(result);
            }).fail((error) => {
                //alert pemberitahuan jika gagal
                Swal.fire(
                    'Oops',
                    'Aset Gagal dihapus',
                    'error'
                )
                console.log(error);

            })
        }
    })
}

function Detail(id)
{
    $.ajax({
        url: baseUrl + `/${id}`,
        type: "Get",
        contentType: "application/json;charset=utf-8"
    }).done((result) => {
        let { data } = result;
        console.log(data)

        $("#id_Aset").html(data.barang.id)
        $("#namaAset").html(data.barang.nama)
        $("#stokAset").html(data.barang.stok)
        let totalAset = 0;
        data.riwayat_Pengadaan.map(x => {

            totalAset += x.jumlah; 
        })
        $("#totalAset").html(totalAset);
    }).fail((error) => {
        console.log(error);
    })

    //table riwayat
    $('#detailAsetTable').DataTable({
        destroy: true,
        "autoWidth": false,
        pageLength: 5,
        lengthMenu: [[5, 10, 20, -1], [5, 10, 20, 'Todos']],
        ajax: {
            url: baseUrl + `/${id}`,
            dataSrc: "data.riwayat_Pengadaan",
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
                data: "tanggal",
                render: function (data, type, row) {
                    return new Date(row.tanggal).toDateString();
                }
            },
            {
                data: "supplier"
            },
            {
                data: "barang.satuan"
            },
            {
                data: "jumlah"
            },
            {
                data: "harga",
                render: function (data, type, row) {
                    return `${formatRupiah(row.harga)}`
                }
            },
        ]
    });

    
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

