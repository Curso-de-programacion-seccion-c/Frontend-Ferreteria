$(document).ready(function () {
    $(".edit-btn").each(function () {
        $(this).on("click", function () {
            const id = $(this).data("id");
            const code = $(this).data("code");
            const nombre = $(this).data("nombre");
            const precio = $(this).data("precio");
            const stock = $(this).data("stock");
            const descripcion = $(this).data("descripcion");
            const idCategoria = $(this).data("idcategoria");
            const idProveedor = $(this).data("idproveedor");

            Swal.fire({
                title: 'Editar Artículo',
                html: `
                    <input id="swal-nombre" class="swal2-input" placeholder="Nombre" value="${nombre}">
                    <input id="swal-precio" class="swal2-input" placeholder="Precio" value="${precio}">
                    <input id="swal-stock" class="swal2-input" placeholder="Stock" value="${stock}">
                    <input id="swal-descripcion" class="swal2-input" placeholder="Descripción" value="${descripcion}">
                    <input id="swal-idcategoria" class="swal2-input" placeholder="ID Categoría" value="${idCategoria}">
                    <input id="swal-idproveedor" class="swal2-input" placeholder="ID Proveedor" value="${idProveedor}">
                `,
                showCancelButton: true,
                confirmButtonText: 'Guardar',
                preConfirm: () => {
                    return {
                        id: parseInt(id),
                        code: parseInt(code),
                        nombre: $('#swal-nombre').val(),
                        precio: parseFloat($('#swal-precio').val()),
                        stock: parseInt($('#swal-stock').val()),
                        descripcion: $('#swal-descripcion').val(),
                        idCategoria: parseInt($('#swal-idcategoria').val()),
                        idProveedor: parseInt($('#swal-idproveedor').val())
                    };
                }
            }).then((result) => {
                if (result.isConfirmed) {
                    fetch('/Articulos/EditarArticulo', {
                        method: 'POST',
                        headers: {
                            'Content-Type': 'application/json',
                            'RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val()
                        },
                        body: JSON.stringify({
                            IdArticulo: result.value.id,
                            CodeArticulo: result.value.code,
                            NombreArticulo: result.value.nombre,
                            Precio: result.value.precio,
                            Stock: result.value.stock,
                            Descripcion: result.value.descripcion,
                            IdCategoria: result.value.idCategoria,
                            IdProveedor: result.value.idProveedor
                        })
                    }).then(async response => {
                        if (response.ok) {
                            Swal.fire('Actualizado', 'El artículo ha sido modificado.', 'success').then(() => {
                                const row = $(`button[data-id="${result.value.id}"]`).closest('tr');
                                row.children().eq(2).text(result.value.nombre);
                                row.children().eq(3).text(result.value.precio.toFixed(2));
                                row.children().eq(4).text(result.value.stock);
                                row.children().eq(5).text(result.value.descripcion);
                                row.children().eq(6).text(result.value.idCategoria);
                                row.children().eq(7).text(result.value.idProveedor);

                                const editBtn = row.find('.edit-btn').data({
                                    nombre: result.value.nombre,
                                    precio: result.value.precio,
                                    stock: result.value.stock,
                                    descripcion: result.value.descripcion,
                                    idcategoria: result.value.idCategoria,
                                    idproveedor: result.value.idProveedor
                                });
                            });
                        } else {
                            const errorText = await response.text();
                            Swal.fire('Error', `No se pudo actualizar: ${errorText}`, 'error');
                        }
                    });
                }
            });
        });
    });
});


//ELIMINAR ARTICULO
document.addEventListener("DOMContentLoaded", function () {
    const deleteButtons = document.querySelectorAll(".delete-btn");

    deleteButtons.forEach(button => {
        button.addEventListener("click", function () {
            const id = this.dataset.id;
            const nombre = this.dataset.nombre;

            Swal.fire({
                title: '¿Estás seguro?',
                text: `¿Deseas eliminar el artículo "${nombre}"?`,
                icon: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#d33',
                cancelButtonColor: '#3085d6',
                confirmButtonText: 'Eliminar',
                cancelButtonText: 'Cancelar'
            }).then((result) => {
                if (result.isConfirmed) {
                    fetch('/Articulos/EliminarArticulo', {
                        method: 'POST',
                        headers: {
                            'Content-Type': 'application/json',
                            'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
                        },
                        body: JSON.stringify({ idArticulo: parseInt(id) })
                    }).then(async response => {
                        if (response.ok) {
                            Swal.fire(
                                'Eliminado',
                                `El artículo "${nombre}" ha sido eliminado.`,
                                'success'
                            ).then(() => {
                                const row = document.querySelector(`button[data-id="${id}"]`).closest('tr');
                                row.remove();
                            });
                        } else {
                            const errorText = await response.text();
                            Swal.fire(
                                'Error',
                                `No se pudo eliminar el artículo: ${errorText}`,
                                'error'
                            );
                        }
                    });
                }
            });
        });
    });
});
