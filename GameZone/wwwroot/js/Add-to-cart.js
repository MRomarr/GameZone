$(document).ready(function () {
    $(".add-to-cart").click(function () {
        var gameId = $(this).data("id");

        toastr.options = {
            "closeButton": false,
            "progressBar": true,
            "positionClass": "toast-bottom-right",
            "timeOut": "3000" //3 seconds
        };

        $.ajax({
            url: '/Cart/Add',
            type: 'POST',
            data: { id: gameId },
            success: function (response) {
                if (response.alreadyOwned) {
                    toastr.warning("You already own this game.");
                } else if (response.alreadyInCart) {
                    toastr.info("This game is already in your cart.");
                } else if (response.success) {
                    toastr.success("Game added to cart successfully!");
                } else {
                    toastr.error("Something went wrong.");
                }
                $('#cart-count').text(response.count);
            }

        });
    });
});