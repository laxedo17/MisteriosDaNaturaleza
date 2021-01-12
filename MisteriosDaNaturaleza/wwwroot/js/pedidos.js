var taboaDeDatos;

$(document).ready(function () {
    var url = window.location.search;
    if (url.includes("aceptados")) {
        leerTaboaDeDatos("GetTodosPedidosAceptados");
    }
    else {
        if (url.includes("pendientes")) {
            leerTaboaDeDatos("GetTodosPedidosPendientes");
        }
        else {
            leerTaboaDeDatos("GetTodosPedidos");
        }
    }
})

function leerTaboaDeDatos(url) {
    taboaDeDatos = $('#tblData').DataTable(
        {
            "ajax": {
                "url": "/admin/pedido/" + url,
                "type": "GET",
                "datatype": "json"
            },
            "columns": [
                {
                    "data": "nome", "width": "20%"
                },
                {
                    "data": "telefono", "width": "15%"
                },
                {
                    "data": "email", "width": "15%"
                },
                {
                    "data": "contaDestinos", "width": "15%"
                },
                {
                    "data": "estado", "width": "15%"
                },
                {
                    "data": "id",
                    "render": function (data) {
                        return `<div class="text-center">
                                    <a href="/Admin/pedido/Detalles/${data}" class='btn btn-success text-white' style='cursor:pointer, width:100px;'>
                                    <i class='far fa-edit'></i> Detalles
                                    </a>
                                    &nbsp;
                                </div>
                                `;
                    }, "width": "15%"
                }
            ],
            "language":
            {
                "emptyTable": "Non se atoparon rexistros"
            },
            "width":"100%"
        });
}