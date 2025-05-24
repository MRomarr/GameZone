$(document).ready(function () {
    $(".add-to-cart").click(function () {
        var gameId = $(this).data("id");
        toastr.options = {
            "closeButton": false,
            "progressBar": true,
            "positionClass": "toast-bottom-right",
            "timeOut": "3000", // 3 seconds
        };
        $.ajax({
            url: '/Cart/Add',
            type: 'POST',
            data: { id: gameId },
            success: function (response) {
                if (response.success && response.alredyadded) {
                    toastr.info("Game already in cart!");
                } else if (response.success) {
                    toastr.success("Game added! Cart has " + response.count + " item(s).");
                }
            },
            error: function () {
                toastr.error("Error adding to cart.");
            }
        });
    });
});

