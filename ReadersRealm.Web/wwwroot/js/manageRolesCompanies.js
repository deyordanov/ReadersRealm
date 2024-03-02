$(document).ready(function () {
    $("#companies").hide();
    $("#companies-label").hide();
    function toggleCompaniesVisibility() {
        var isCompanySelected = $("#roles option:selected").toArray().some(option => option.text === "Company");
        if (isCompanySelected) {
            $("#companies").show();
            $("#companies-label").show();
        } else {
            $("#companies").hide();
            $("#companies-label").hide();
        }
    }

    toggleCompaniesVisibility();

    $("#roles").change(toggleCompaniesVisibility);
});
