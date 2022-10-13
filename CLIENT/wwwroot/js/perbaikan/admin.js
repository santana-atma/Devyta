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
                "width": "5%", 
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
                    const isDiperiksa = row.status == "DIPERIKSA";
                    const isDiperbaiki = row.status == "DALAM PERBAIKAN";
                    if (isDiperiksa) {
                        return `<button class="btn btn-sm btn-primary" data-toggle="modal" data-target="#perbaikanModal" onclick="Terima('${row.id}')">Terima</button>
                            <button class="btn btn-sm btn-danger"  onclick="Tolak('${row.id}');">Tolak</button>
                            <button class="btn btn-sm btn-success" ${isDiperiksa && "disabled"} onclick="Selesai('${row.id}');">Selesai</button>`
                    } else if (isDiperbaiki) {
                        return `<button class="btn btn-sm btn-primary" ${isDiperbaiki && "disabled"} data-toggle="modal" data-target="#perbaikanModal" onclick="Terima('${row.id}')">Terima</button>
                            <button class="btn btn-sm btn-danger" ${isDiperbaiki && "disabled"} onclick="Tolak('${row.id}');">Tolak</button>
                            <button class="btn btn-sm btn-success" onclick="Selesai('${row.id}');">Selesai</button>`
                    } else {
                        return `<button class="btn btn-sm btn-primary" disabled data-toggle="modal" data-target="#perbaikanModal" onclick="Terima('${row.id}')">Terima</button>
                            <button class="btn btn-sm btn-danger" disabled onclick="Tolak('${row.id}');">Tolak</button>
                            <button class="btn btn-sm btn-success" disabled onclick="Selesai('${row.id}');">Selesai</button>`
                    }
                    
                }
            },
        ],

    });
});


function Terima(id) {
    let data = {};
    //Get Data saat render
    $.ajax({
        url: baseUrl + `/${id}`,
        type: "GET",
        contentType: "application/json;charset=utf-8"
    }).done((result) => {
        let { data } = result

        $("#id_perbaikan").val(id);
        $("#barang_id").val(data.barang_Id);
        $("#karyawan_id").val(data.karyawan_Id);
        $("#keterangan").val(data.keterangan);
        $("#tanggal_terima").val(new Date(data.tanggal_Terima).toISOString().substring(0, 10));
        $("#tanggal_selesai").val("");
        $("#biaya").val(0);
        $("#jumlah").val(data.jumlah);
        $("#status").val("DALAM PERBAIKAN");
        console.log(result);
    }).fail((error) => {
        console.log(error);
    })
}

function Tolak(id) {
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Ya!'
    }).then((result) => {
        if (result.isConfirmed) {
            let data = {};
            //Get Data saat render
            $.ajax({
                url: baseUrl + `/${id}`,
                type: "GET",
                contentType: "application/json;charset=utf-8"
            }).done((result) => {
                let { data } = result
                let req = {
                    barang_Id : data.barang_Id,
                    karyawan_Id : data.karyawan_Id,
                    keterangan : data.keterangan,
                    tanggal_Terima : data.tanggal_Terima,
                    tanggal_Selesai : data.tanggal_Selesai,
                    jumlah: parseInt(data.jumlah),
                    status : "DITOLAK",
                    biaya : data.biaya
                }
                
                //isi dari object kalian buat sesuai dengan bentuk object yang akan di post
                $.ajax({
                    url: baseUrl + `/${id}`,
                    type: "PUT",
                    data: JSON.stringify(req), //jika terkena 415 unsupported media type (tambahkan headertype Json & JSON.Stringify();)
                    contentType: "application/json;charset=utf-8"
                }).done((result) => {
                    console.log(result);
                    table.ajax.reload();
                }).fail((error) => {
                    console.log(error);
                })
      

            }).fail((error) => {
                console.log(error);
            })
        }
    })
}



function Selesai(id) {
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Ya!'
    }).then((result) => {
        if (result.isConfirmed) {
            let data = {};
            //Get Data saat render
            $.ajax({
                url: baseUrl + `/${id}`,
                type: "GET",
                contentType: "application/json;charset=utf-8"
            }).done((result) => {
                let { data } = result

                $("#id_perbaikan").val(id);
                $("#barang_id").val(data.barang_Id);
                $("#karyawan_id").val(data.karyawan_Id);
                $("#keterangan").val(data.keterangan);
                $("#tanggal_terima").val(new Date(data.tanggal_Terima).toISOString().substring(0, 10));
                $("#tanggal_selesai").val(new Date(data.tanggal_Terima).toISOString().substring(0, 10));
                $("#biaya").val(data.biaya);
                $("#jumlah").val(data.jumlah);
                $("#status").val("SELESAI");
                console.log(result);
                Update();
            }).fail((error) => {
                console.log(error);
            })
        }
    })
    
}

function validasiInputan(obj) {
    let error = 0;
    if (obj.tanggal_Selesai == "") {
        $("#errorTanggal_selesai").html("Tanggal Selesai tidak boleh kosong")
        error++;
    }
    if (obj.biaya == "" || obj.biaya == NaN) {
        $("#errorBiaya").html("Biaya tidak boleh kosong")
        error++;
    }
    return error;
}

$('#perbaikanModal').on('hidden.bs.modal', function () {
    $("#tanggal_selesai").val("");
    $("#biaya").val(0);
    $("#keterangan").val("");
    $("#errorTanggal_selesai").html("");
    $("#errorBiaya").html("");
    $("#errorKeterangan").html("");
});


function Update() {

    let id_perbaikan = parseInt($("#id_perbaikan").val());
    let barang_id = parseInt($("#barang_id").val());
    let karyawan_id = parseInt($("#karyawan_id").val());
    let keterangan = $("#keterangan").val();
    let tanggal_Terima = $("#tanggal_terima").val();
    let tanggal_Selesai = $("#tanggal_selesai").val();
    let jumlah = parseInt($("#jumlah").val());
    let biaya = parseInt($("#biaya").val());
    let status = $("#status").val();
    let obj = { keterangan, tanggal_Selesai, biaya };
    let validation = validasiInputan(obj)
    if (validation == 0) {
       
            let data = {};

            data.barang_Id = barang_id;
            data.karyawan_Id = karyawan_id;
            data.keterangan = keterangan;
            data.tanggal_Terima = new Date(tanggal_Terima).toISOString().substring(0, 10);
            data.tanggal_Selesai = new Date(tanggal_Selesai).toISOString().substring(0, 10);
            data.jumlah = jumlah;
            data.status = status;
            data.biaya = biaya
            //isi dari object kalian buat sesuai dengan bentuk object yang akan di post
            $.ajax({
                url: id_perbaikan != "" ? baseUrl + `/${id_perbaikan}` : baseUrl,
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