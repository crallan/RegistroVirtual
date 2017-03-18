$(function () {

    LoadScoreGrid($("#institution-selector").val());

    $("#institution-selector").on('change', function () {
        var selectedInstitution= $(this).val();
        LoadScoreGrid(selectedInstitution);
    });

    $('#save-user-button').on('click', function (e) {
        e.preventDefault();
        SaveSubjects();
        $("#save-user-form").submit();
    });

    function SaveSubjects() {
        var subjectEntries = $(".subjects-grid tbody tr");
        var SubjectAndClassesList = [];

        subjectEntries.each(function () {
            var tr = $(this);
            var SubjectId = tr.find("#hdnSubjectId").val();
            var SelectedClasses = tr.find(".chosen-select").val();

            var SubjectAndClasses =
            {
                "Id": SubjectId,
                "SelectedClasses": SelectedClasses
            };

            SubjectAndClassesList.push(SubjectAndClasses);
        });

        $.ajax({
            url: '/User/SaveSubjects/',
            data: JSON.stringify(SubjectAndClassesList),
            type: 'POST',
            dataType: "html",
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                LoadScoreGrid($("#institution-selector").val());
            }
        });
    }

    function LoadScoreGrid(institutionId) {
        $.ajax({
            url: "/User/LoadSubjects",
            dataType: "html",
            data: { institutionId: institutionId }
        }).done(function (response) {
            $("#subjects-container").empty().html(response);
            $(".class-selector").chosen();
        });
    }

})