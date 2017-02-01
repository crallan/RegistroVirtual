$(function () {

    bindExamEvents();

    function bindExamEvents() {
        $('.edit-mode').hide();
        $('.edit-exam, .cancel-exam').on('click', function () {
            var tr = $(this).parents('tr:first');
            tr.find('.edit-mode, .display-mode').toggle();
        });

        $('.save-exam').on('click', function () {
            var tr = $(this).parents('tr:first');
            var ExamId = $(this).attr("data-exam-id");
            var Percentage = tr.find("#Percentage").val();
            var Score = tr.find("#Score").val();

            tr.find("#lblPercentage").text(Percentage);
            tr.find("#lblScore").text(Score);
            tr.find('.edit-mode, .display-mode').toggle();
            var ExamModel =
            {
                "Id": ExamId,
                "Percentage": Percentage,
                "Score": Score
            };

            $.ajax({
                url: '/RegisterProfile/AddExam/',
                data: JSON.stringify(ExamModel),
                type: 'POST',
                dataType: "html",
                contentType: 'application/json; charset=utf-8',
                success: function (data) {
                    $("#exams-container").empty().html(data);
                    bindExamEvents();
                }
            });
        });
    }
})