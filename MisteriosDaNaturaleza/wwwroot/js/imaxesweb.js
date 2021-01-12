var taboaDeDatos;

$(document).ready(function () {
    leerTaboaDeDatos();
})

function leerTaboaDeDatos() {
    taboaDeDatos = $('#tblData').DataTable(
        {
            "ajax": {
                "url": "/admin/imaxesWeb/GetTodo",
                "type": "GET",
                "datatype": "json"
            },
            "columns": [
                {
                    "data": "nome", "width": "50%"
                },
                {
                    "data": "id",
                    "render": function (data) {
                        return `<div class="text-center">
                                    <a href="/Admin/imaxesWeb/ActualInsertar/${data}" class='btn btn-success text-white' style='cursor:pointer, width:100px;'>
                                    <i class='far fa-edit'></i> Editar
                                    </a>
                                    &nbsp;
                                    <a onclick=Eliminar("/Admin/imaxesWeb/Eliminar/${data}") class='btn btn-danger text-white' style='cursor:pointer, width:100px;'>
                                    <i class='far fa-trash-alt'></i> Eliminar
                                    </a>
                                </div>
                                `;
                    }, "width": "30%"
                }
            ],
            "language":
            {
                "emptyTable": "Non se atoparon rexistros"
            },
            "width":"100%"
        });
}

function Eliminar(url) {
    swal({
        title: "Estas seguro/a de que queres borrar?",
        text: "Non sera posible restaurar o contido",
        type: "warning",
        showCancelButton: true,
        cancelButtonText: "Cancelar",
        confirmButtonColor: "#DD6B55",
        confirmButtonText: "Si, borra!",
        closeOnConfirm: true
    }, function () {
            $.ajax({
                type: 'DELETE',
                url: url,
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        taboaDeDatos.ajax.reload();
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            });
    });
}