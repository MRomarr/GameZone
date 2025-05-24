// Use Swal.fire directly with customClass instead of Swal.mixin
$('.js-delete').on('click', function () {
    var btn = $(this);
    const swal = Swal.mixin({
        customClass: {
            confirmButton: "btn btn-success",
            cancelButton: "btn btn-danger"
        },
        buttonsStyling: false
    });
    swal.fire({
        title: 'Are you sure that you need to delete this game?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Yes, delete it!',
        cancelButtonText: 'No, cancel!',
        reverseButtons: true
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: `/Cart/Remove/${btn.data('id')}`,
                method: 'DELETE',
                success: function () {
                    swal.fire(
                        'Deleted!',
                        'Game has been deleted.',
                        'success'
                    );
                    btn.parents('tr').fadeOut();
                },
                error: function () {
                    swal.fire(
                        'Oooops...',
                        'Something went wrong.',
                        'error'
                    );
                }
            });
        }
    });
});
