$(function () {

    $body = $("body");
    $(document).on({
        ajaxStart: function () { $body.addClass("loading"); },
        ajaxStop: function () { $body.removeClass("loading"); }
    });

    $("#score-search-button").click(function () {
        LoadGridScores();
    });

    function LoadGridScores() {
        var selectedClass = $("#selected-class").val();
        var selectedYear = $("#selected-year").val();
        var selectedTrimester = $("#selected-trimester").val();

        $.ajax({
            url: "/Score/LoadScores",
            dataType: "html",
            data: { selectedClass: selectedClass, selectedYear: selectedYear, selectedTrimester: selectedTrimester }
        }).done(function (response) {
            $("#scores-container").empty().html(response);
            UpdateZeroAsistanceFields();
            bindScoreGridEvents();
        });
    }

    function CalculateExamPercentage() {
        var studentScores = $(".score-item");

        studentScores.each(function () {
            var studentScore = $(this);
            var examScoreFields = studentScore.find(".exam-score");

            examScoreFields.each(function () {
                var examId = $(this).attr('data-exam-id');
                var examScore = $(this).val();
                var examPercentageField = studentScore.find("input.exam-percentage[data-exam-id='" + examId + "']");

                if (examPercentageField != null && examPercentageField != undefined) {
                    var examMaxPercentage = examPercentageField.attr('max');
                    examPercentageField.val((examScore * examMaxPercentage) / 100);
                }
            });
        });
    }

    function CalculateAssitance() {
        var studentScores = $(".score-item");

        studentScores.each(function () {
            var studentEntry = $(this);
            var Belated = studentEntry.find("#Belated").val();
            var Absences = studentEntry.find("#Absences").val();
            var AssistanceField = studentEntry.find("#AssistancePercentage");
            var NumberOfLessons = studentEntry.find("#NumberOfLessons").val();
            
            var percentage = ((parseInt(Absences) + (parseInt(Belated) * 3)) * 100) / parseInt(NumberOfLessons);
            var assistancePercentage = 0;

            if (percentage === 0)
            {
                assistancePercentage = 5;
            }
            else if (percentage >= 1 && percentage <= 12)
            {
                assistancePercentage = 4;
            }
            else if (percentage >= 13 && percentage <= 25)
            {
                assistancePercentage = 3;
            }
            else if (percentage >= 26 && percentage <= 38)
            {
                assistancePercentage = 2;
            }
            else if (percentage >= 39 && percentage <= 50)
            {
                assistancePercentage = 1;
            }

            AssistanceField.val(assistancePercentage);
        });
    }

    function UpdateZeroAsistanceFields() {
        var studentScores = $(".score-item");

        studentScores.each(function () {
            var studentEntry = $(this);
            var AssistanceField = studentEntry.find("#AssistancePercentage");
            var MaxAssistancePercentage = AssistanceField.attr('max');

            if (AssistanceField.val() === '0' && MaxAssistancePercentage != null) {
                AssistanceField.val(MaxAssistancePercentage);
            }
        });
    }

    function bindScoreGridEvents() {

        $(".assistance-related-field").on('change', function () {
            CalculateAssitance();
        });

        $('.scores-save-button').on('click', function () {

            var studentScores = $(".score-item");
            var Scores = [];

            studentScores.each(function () {
                var studentEntry = $(this);
                var StudentId = studentEntry.find("#StudentId").val();
                var ScoreId = studentEntry.find("#ScoreId").val();
                var ClassId = $("#selected-class").val();
                var YearCreated = $("#selected-year").val();
                var RegisterProfileId = studentEntry.find("#RegisterProfileId").val();
                var DailyWorkPercentage = studentEntry.find("#DailyWorkPercentage").val();
                var Absences = studentEntry.find("#Absences").val();
                var Belated = studentEntry.find("#Belated").val();
                var AssistancePercentage = studentEntry.find("#AssistancePercentage").val();
                var ConceptPercentage = studentEntry.find("#ConceptPercentage").val();

                var examScoreFields = studentEntry.find(".exam-score");
                var ExamResults = [];

                examScoreFields.each(function () {
                    var examId = $(this).attr('data-exam-id');
                    var examScore = $(this).val();
                    var examPercentageField = studentEntry.find("input.exam-percentage[data-exam-id='" + examId + "']");

                    var Exam =
                    {
                        "ExamId": examId,
                        "ExamScore": examScore,
                        "ExamPercentage": examPercentageField.val(),
                    };

                    ExamResults.push(Exam);
                });


                var extraclassWorkFields = studentEntry.find(".extraclass-score");
                var ExtraclasWorkResults = [];

                extraclassWorkFields.each(function () {
                    var extraclassId = $(this).attr('data-extraclass-id');
                    var extraclassPercentage = $(this).val();

                    var ExtraclassWork =
                    {
                        "ExtraclassWorkId": extraclassId,
                        "ExtraclassWorkPercentage": extraclassPercentage,
                    };

                    ExtraclasWorkResults.push(ExtraclassWork);
                });

                var ScoreModel =
                {
                    "Id": ScoreId,
                    "StudentId": StudentId,
                    "ClassId": ClassId,
                    "YearCreated": YearCreated,
                    "RegisterProfileId": RegisterProfileId,
                    "DailyWorkPercentage": DailyWorkPercentage,
                    "Absences": Absences,
                    "Belated": Belated,
                    "AssistancePercentage": AssistancePercentage,
                    "ConceptPercentage": ConceptPercentage,
                    "ExamResults": ExamResults,
                    "ExtraclasWorkResults": ExtraclasWorkResults
                };

                Scores.push(ScoreModel);
            })

            //Send a list of scores
            $.ajax({
                url: '/Score/Save/',
                data: JSON.stringify(Scores),
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                success: function (data) {
                    LoadGridScores();
                }
            });

        });

    }
})