﻿$(function () {

    bindExamEvents();

    function bindExamEvents() {
        $('.edit-mode').hide();

        $('.edit-exam, .cancel-exam').on('click', function () {
            var tr = $(this).parents('tr:first');
            tr.find('.edit-mode, .display-mode').toggle();
        });

        $('.remove-exam').on('click', function () {
            var ExamId = $(this).attr("data-exam-id");
            var ExamModel =
            {
                "Id": ExamId
            };

            $.ajax({
                url: '/RegisterProfile/RemoveExam/',
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

        $('.save-exam').on('click', function () {
            var tr = $(this).parents('tr:first');
            var ExamId = $(this).attr("data-exam-id");
            var Name = tr.find("#Name").val();
            var Percentage = tr.find("#Percentage").val();
            var Score = tr.find("#Score").val();

            tr.find("#lblName").text(Name);
            tr.find("#lblPercentage").text(Percentage);
            tr.find("#lblScore").text(Score);
            tr.find('.edit-mode, .display-mode').toggle();
            var ExamModel =
            {
                "Id": ExamId,
                "Name": Name,
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