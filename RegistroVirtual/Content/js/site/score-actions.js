$(function () {
    $("#score-search-button").click(function () {
        var selectedClass = $("#selected-class").val();
        var selectedYear = $("#selected-year").val();
        var selectedTrimester = $("#selected-trimester").val();

        $.ajax({
            url: "/Score/LoadScores",
            dataType: "html",
            data: { selectedClass: selectedClass, selectedYear: selectedYear, selectedTrimester: selectedTrimester }
        }).done(function (response) {
            $("#scores-container").empty().html(response);
            CalculateExamPercentage();
            CalculateAssitance();
            bindScoreGridEvents();
        });
    });

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
            var Absebces = studentEntry.find("#Absebces").val();
            var AssistanceField = studentEntry.find("#AssistancePercentage");

            AssistanceField.val(parseInt(Absebces) + (parseInt(Belated) * 3));
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
                var RegisterProfileId = studentEntry.find("#RegisterProfileId").val();
                var DailyWorkPercentage = studentEntry.find("#DailyWorkPercentage").val();
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
                       "ExamScore": examPercentageField.val(),
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
                    "StudentId": StudentId,
                    "RegisterProfileId": RegisterProfileId,
                    "DailyWorkPercentage": DailyWorkPercentage,
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
                    alert("saved!!");
                    //$("#scores-container").empty().html(response);
                    //CalculateExamPercentage();
                    //bindSaveEvent();
                }
            });

        });

    }
})