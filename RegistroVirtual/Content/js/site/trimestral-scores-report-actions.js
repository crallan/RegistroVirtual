$(function () {

    $body = $("body");
    $(document).on({
        ajaxStart: function () { $body.addClass("loading"); },
        ajaxStop: function () { $body.removeClass("loading"); }
    });

    GetRelatedClasses($("#selected-subject").val());

    $("#print-report-button").click(function ()
    {
        var selectedClass = $("#selected-class").val();
        var selectedYear = $("#selected-year").val();
        var selectedTrimester = $("#selected-trimester").val();
        var selectedSubject = $("#selected-subject").val();

        //Replace the predifined QueryString param with correct values
        this.href = this.href.replace("cParam", selectedClass);
        this.href = this.href.replace("yParam", selectedYear);
        this.href = this.href.replace("tParam", selectedTrimester);
        this.href = this.href.replace("sParam", selectedSubject);
    });


    $("#selected-subject").on('change', function () {
        GetRelatedClasses($(this).val());
    });

    function GetRelatedClasses(subject) {
        $.ajax({
            url: "/TrimestralScoresReport/GetRelatedClasses",
            dataType: "json",
            data: { selectedSubject: subject }
        }).done(function (response) {
            if (response.classes.length > 0) {
                $("#selected-class").empty();
                $.each(response.classes, function (index, element) {
                    $('#selected-class')
                        .append($("<option></option>")
                                   .attr("value", element.Id)
                                   .text(element.Name));
                });
            }
        });
    }
})