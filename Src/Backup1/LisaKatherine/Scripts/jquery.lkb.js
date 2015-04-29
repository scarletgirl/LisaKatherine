$(document).ready(function () {
    $(".articlebody a").attr("target", "_blank");
    $(".articlebody a.readMore").removeAttr("target");

    $('div.view').hide();
    $('div.view:first').show();
    $('div.slide').click(function () {
        $(this).next().slideToggle(400);
        return false;
    });
});

function showCurrentBlogMonth(id) {
    $('div.view:first').hide();
    $('div#' + id + '_view').show();
}
