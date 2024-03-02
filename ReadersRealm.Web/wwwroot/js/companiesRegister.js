$(document).ready(function () {
    $("#companies").hide();
    function toggleCompaniesVisibility() {
        var selection = $("#roles option:selected").text();
        if (selection === "Company") {
            $("#companies").show();
        } else {
            $("#companies").hide();
        }
    }

    toggleCompaniesVisibility();

    $("#roles").change(toggleCompaniesVisibility);
});