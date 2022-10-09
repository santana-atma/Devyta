let table = null;
let baseUrl = "https://localhost:44307/api/RiwayatPengadaan";
let role = $("#role").val();
let isAdmin = role.toLowerCase() == "admin";
let UserId = $("#userId").val();
console.log(isAdmin);
$(document).ready(function () {
    table = $('#table_pengadaan').DataTable({
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
                data: "tanggal",
                render: function (data, type, row) {
                    return new Date(row.tanggal).toDateString();
                }
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
            {
                data: "supplier"
            },
            {
                data: "",
                render: function (data, type, row) {
                        return `<button class="btn btn-sm btn-success" data-toggle="modal" data-target="#pengadaanModal" onclick="Edit('${row.id}')">Edit</button>
                            <button class="btn btn-sm btn-danger" onclick="Delete('${row.id}');">Delete</button>`
                },
                visible: isAdmin
            }
        ]
    });
});

function validasiInputan(obj) {
    let error = 0;
    if (obj.nama == "") {
        $("#errorNama").html("Nama tidak boleh kosong")
        error++;
    }
    if (obj.satuan == "") {
        $("#errorSatuan").html("Satuan tidak boleh kosong")
        error++;
    }
    if (obj.tanggal == "") {
        $("#errorTanggal").html("Tanggal tidak boleh kosong")
        error++;
    }
    if (obj.jumlah == 0 || obj.jumlah == NaN) {
        $("#errorJumlah").html("Jumlah tidak boleh kosong")
        error++;
    }
    if (obj.supplier == "") {
        $("#errorSupplier").html("Supplier tidak boleh kosong")
        error++;
    }
    if (obj.harga == "" || obj.harga == NaN) {
        $("#errorHarga").html("Harga tidak boleh kosong")
        error++;
    }
    return error;
}

$('#pengadaanModal').on('hidden.bs.modal', function () {
    $("#idPengadaan").val(-1);
    $("#nama").val("");
    $("#tanggal").val("");
    $("#supplier").val("");
    $("#jumlah").val("");
    $("#harga").val("");
    $("#satuan").val("");
    $("#errorNama").html("")
    $("#errorSatuan").html("")
    $("#errorSupplier").html("")
    $("#errorTanggal").html("")
    $("#errorJumlah").html("")
    $("#errorHarga").html("")
});

function Insert()
{
    let nama = $("#nama").val();
    let satuan = $("#satuan").val();
    let tanggal = $("#tanggal").val();
    let supplier = $("#supplier").val();
    let jumlah = parseInt($("#jumlah").val());
    let harga = parseFloat($("#harga").val());
    let id = $("#idPengadaan").val();
    let obj = { nama, satuan, tanggal, supplier, jumlah, harga };
    let validation = validasiInputan(obj)
    if (validation == 0) {
        if (id == -1) { //kalo id -1 berarti add
            let data = {};
            data.petugasId = parseInt(UserId);
            data.nama = $("#nama").val();
            data.satuan = $("#satuan").val();
            data.tanggal = tanggal;
            data.supplier = supplier;
            data.jumlah = jumlah;
            data.harga = harga;
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
                    'Riwayat Pengadaan sukses ditambah',
                    'success'
                )
                table.ajax.reload();
                $('#pengadaanModal').modal('toggle');
                console.log(result);
            }).fail((error) => {
                //alert pemberitahuan jika gagal
                Swal.fire(
                    'Gagal',
                    'Riwayat Pengadaan gagal ditambah',
                    'error'
                )
                console.log(error);

            })
        } else { //kalo id tidak -1 berarti update
            let data = {};
            //ini masih hardcode
            data.petugasId = parseInt(UserId);
            data.nama = $("#nama").val();
            data.satuan = $("#satuan").val();
            data.tanggal = tanggal;
            data.supplier = supplier;
            data.jumlah = jumlah;
            data.harga = harga;
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
                    'Riwayat sukses diubah',
                    'success'
                )
                table.ajax.reload();
                $('#pengadaanModal').modal('toggle');
                console.log(result);
            }).fail((error) => {
                //alert pemberitahuan jika gagal
                Swal.fire(
                    'Gagal',
                    'Riwayat pengadaan gagal diubah',
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
        url: baseUrl + `/${id}`,
        type: "Get",
        contentType: "application/json;charset=utf-8"
    }).done((result) => {
        let { data } = result
        $("#idPengadaan").val(id)
        $("#nama").val(data.barang.nama)
        $("#satuan").val(data.barang.satuan)
        $("#supplier").val(data.supplier)
        $("#tanggal").val(new Date(data.tanggal).toISOString().substring(0, 10))
        $("#harga").val(data.harga)
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

function formatRupiah(price)
{
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