$(document).ready(function () {
    $('.bi-list').on('click', function () {
        
        $('.side-nav').toggleClass('d-none');
        
        $(this).toggleClass('active');
    });
});