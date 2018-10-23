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
        if(localization.length > 0) {
            location.href = addOrReplaceParam(location.href, "localization", localization, false);
        }
    });

    $(document).on('change', '.availability-menu', function (event) {
        location.href = addOrReplaceParam(location.href, "availability", event.currentTarget.value, true);
    });
});


var addOrReplaceParam = function (url, param, value, isArray) {
    let query = param + "=" + value;
    let shouldAddParam = true;
    if (url.indexOf("?") === -1) {
        url += "?" + param + "=" + value;
    }
    else if (isArray) {
        if (url.indexOf(param + "=" + value) === -1) {
            url += "&" + param + "=" + value;
        }
    }
    else {
        let parametersString = url.split("?")[1];
        if (parametersString !== undefined) {
            let parameters = parametersString.split("&");
            for (let i = 0; i < parameters.length; i++) {
                if (parameters[i].indexOf(param) !== -1) {
                    parameters[i] = query;
                    shouldAddParam = false;
                    break;
                }
            }
            if (shouldAddParam) {
                url += "&" + param + "=" + value;
            } else {
                alert();
                url = url.split("?")[0] + "?" + parameters.join("&");
            }
        }

    }
    return url;
}