$(document).ready(function () {
    $(document).on('mouseenter', '.item', function () {
        $(this).find("#hire-button").show();
    }).on('mouseleave', '.item', function () {
        $(this).find("#hire-button").hide();
    });

    $(document).on('click', '#search-button', function () {
        let url = location.href;
        let category = $('#search-dropdown :selected').text();
        url = url + '/category=' + category;
        location.href = url;
    });

    $(document).on('keydown', '#page-number', function (event) {
        if (event.which === 13) {
            let value = parseInt($('#page-number').val());
            let max = parseInt($('#page-number').attr('max'));
            let href = $('#page-number').attr('data-url');
            href = href.substr(0, href.substr(0, href.lastIndexOf("/")).lastIndexOf("/"));

            if (value > max) {
                value = max;
            }
            location.href = href + '/' + value;
        }
    });

    $(document).on('focusout', '#localization', function () {
        let localization = $('#localization').val();
        if(localization.length > 0)
        {
            location.href = location.href + '?localization=' + localization;
        }
    });
});