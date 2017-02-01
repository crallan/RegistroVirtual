$(function () {

    bindExtraclassEvents();

    function bindExtraclassEvents() {
        $('.edit-mode').hide();
        $('.edit-extraclass, .cancel-extraclass').on('click', function () {
            var tr = $(this).parents('tr:first');
            tr.find('.edit-mode, .display-mode').toggle();
        });

        $('.save-extraclass').on('click', function () {
            var tr = $(this).parents('tr:first');
            var ExtraclassId = $(this).attr("data-extraclass-id");
            var Percentage = tr.find("#Percentage").val();

            tr.find("#lblPercentage").text(Percentage);
            tr.find('.edit-mode, .display-mode').toggle();
            var ExtraclassModel =
            {
                "Id": ExtraclassId,
                "Percentage": Percentage
            };

            $.ajax({
                url: '/RegisterProfile/AddExtraclass/',
                data: JSON.stringify(ExtraclassModel),
                type: 'POST',
                dataType: "html",
                contentType: 'application/json; charset=utf-8',
                success: function (data) {
                    $("#extraclasses-container").empty().append(data);
                    bindExtraclassEvents();
                }
            });

        });
    }
})