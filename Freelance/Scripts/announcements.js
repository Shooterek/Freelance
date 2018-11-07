$(document).ready(function() {
    $(document).on('keydown', '#page-number', function(event) {
        if (event.which === 13) {
            var url = location.href;
            var value = parseInt($('#page-number').val());
            var max = parseInt($('#page-number').attr('max'));
            var href = $('#page-number').attr('data-url');
            var parametersString = url.split("?")[1];

            href = href.substr(0, href.substr(0, href.lastIndexOf("/")).lastIndexOf("/"));
            if (value > max) {
                value = max;
            } else if (value < 1) {
                value = 1;
            }
            url = href + '/' + value;
            if (parametersString !== undefined) {
                url = `${url}?${parametersString}`;
            }
            location.href = url;
        }
    });

    $(document).on('click', '#next-page', function (event) {
        event.preventDefault();
        var pageNumber = $('#next-page').attr('data-page-number');
        changePage(pageNumber);
    });

    $(document).on('click', '#previous-page', function (event) {
        event.preventDefault();
        var pageNumber = $('#previous-page').attr('data-page-number');
        changePage(pageNumber);
    });

    $(document).on('click', '.item', function (event) {
        event.currentTarget.children[1].children[0].children[0].children[0].click();
    });

});

var changePage = function(value) {
    var url = location.href.substr(0, location.href.lastIndexOf('/'));
    var parameters = location.href.split("?")[1];
    url = `${url}/${value}`;
    if (parameters !== undefined) {
        url += `?${parameters}`;
    }
    location.href = url;
}