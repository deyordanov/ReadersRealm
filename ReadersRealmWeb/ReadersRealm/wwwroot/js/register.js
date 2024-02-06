$(document).ready(function () {
    $("#companies").hide();
    $("#roles").change(function () {
        var selection = $("#roles option:selected").text();
        if (selection === "Company") {
            $("#companies").show();
        } else {
            $("#companies").hide();
        }
    });
});