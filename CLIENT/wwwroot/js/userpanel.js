function getDbUser() {
    $.ajax({
        url: "https://localhost:44307/api/Karyawan"
    }).done((result) => {
        test = "";
        $.each(result.data, function (key, val) {
            let deletebtn = `<button class="btn btn-danger" onClick="confirmDelete(${val.id})"><i class="fas fa-trash"></i></button>`;
            let editbtn = `<button class="btn btn-primary" onClick="" data-toggle="modal" data-target="#editusermodal"><i class="fas fa-edit"></i></button>`;
            test += `
        <tr>
            <td>${val.id}</td>
            <td>${val.fullName}</td>
            <td>${val.email}</td>
            <td>${val.departemen}</td>
            <td>${val.role}</td>
            <td>${val.role == "Admin" ? "" : editbtn + deletebtn }</td>

        </tr>`;
            $("#userData").html(test);
        })
    }).fail((error) => {
        console.log(error);
    })
}

function deleteUser(id) {
    $.ajax({
        url: 'https://localhost:44307/api/Karyawan/' + id,
        type: 'DELETE',
    }).done((result) => {
        console.log(result);
    }).fail((error) => {
        console.log(error);
    })
}
function confirmDelete(id) {
    Swal.fire({
        title: 'Anda yakin?',
        text: "Akun ini tidak bisa dikembalikan kembali!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes'
    }).then((result) => {
        if (result.isConfirmed) {
            deleteUser(id);
            getDbUser();
            //location.reload();
            Swal.fire(
                'Deleted!',
                'Akun ini sudah dihapus !.',
                'success'
            );
            
        }
    })
}

function InsertUser() {

    let newUser = {}
    
    newUser = {
        Id: 0,
        Fullname: $("#addNama").val(),
        Email: $("#addEmail").val(),
        Alamat: $("#addAlamat").val(),
        Telp: $("#addTelepon").val().toString(),
        Departemen: $("#addDepartemen").val(),
        Role: parseInt($("#addRole").val()),
        Password: $("#addPassword").val()
    }
    
    console.log(newUser);
    $.ajax({
        url: 'https://localhost:44307/api/register/',
        type: 'POST',
        data: JSON.stringify(newUser),
        contentType: "application/json;charset=utf-8"
    
    }).done((result) => {
        if (result.statusCode == 201) {
            Swal.fire({
                icon: 'success',
                title: 'Akun sukses disimpan',
                showConfirmButton: false,
                timer: 1500
            });
            $('#addUser').modal('hide');
            $("#formaddUser")[0].reset();
            getDbUser();

        } else {
            Swal.fire({
                icon: 'error',
                title: 'Oops...',
                text: 'Cek ulang data yang and isi !',
            })
        }
        console.log(result);
    }).fail((error) => {
        console.log(error);
    })

}


getDbUser();
