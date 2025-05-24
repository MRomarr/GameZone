$(document).ready(function () {
    $('#searchBox').on('keyup', function () {
        var term = $(this).val().trim();

        if (term.length > 0) {
            $.getJSON('/Home/SearchGames', { term: term }, function (data) {
                let items = '';

                $.each(data, function (i, item) {
                    items += `<li class="list-group-item"><a href="/Home/Details/${item.id}">${item.label}</a></li>`;
                });

                $('#results').html(items);
            });
        } else {
            $('#results').empty();
        }
    });
});