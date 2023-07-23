// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$('#pageSizeSelect').on('change', function (e) {
    var form = document.querySelector('#pageSizeForm');
    form.submit();
});