function getDbUser() {
    $.ajax({
        url: "https://localhost:44307/api/Karyawan"
    }).done((result) => {
        test = "";
        $.each(result.data, function (key, val) {
            let deletebtn = `<button class="btn btn-danger" onClick="confirmDelete(${val.id})"><i class="fas fa-trash"></i></button>`;
            let editbtn = `<button class="btn btn-primary" onClick="confirmDelete(${val.id})"><i class="fas fa-edit"></i></button>`;
            test += `
        <tr>
            <td>${val.id}</td>
            <td>${val.fullName}</td>
            <td>${val.email}</td>
            <td>${val.alamat}</td>
            <td>${val.telp}</td>
            <td>${val.role}</td>
            <td>${val.role == "Admin" ? "" :  deletebtn }</td>

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
            Swal.fire(
                'Deleted!',
                'Akun ini sudah dihapus !.',
                'success'
            );
            
        }
    })
}

getDbUser();