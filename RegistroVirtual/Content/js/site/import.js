$(function () {
    $('#download-template').on('click', function () {
        $.ajax({
            url: '/Student/DownloadImportTemplate/',
            type: 'GET',
            success: function (data) {
                window.open(data, '_blank');
            }
        });
    });
})