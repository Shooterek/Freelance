$(document).ready(function () {
    $(document).on('mouseenter', '.item', function () {
        $(this).find("#hire-button").show();
    }).on('mouseleave', '.item', function () {
        $(this).find("#hire-button").hide();
    });

    $(document).on('click', '#search-button', function () {
        var url = location.href;
        var category = $('#search-dropdown :selected').text();
        url = url + '/category=' + category;
        location.href = url;
    });

    $(document).on('keydown', '#page-number', function (event) {
        if (event.which === 13) {
            var value = parseInt($('#page-number').val());
            var max = parseInt($('#page-number').attr('max'));
            var href = $('#page-number').attr('data-url');
            href = href.substr(0, href.substr(0, href.lastIndexOf("/")).lastIndexOf("/"));

            if (value > max) {
                value = max;
            }
            location.href = href + '/' + value;
        }
    });

    $(document).on('focusout', '#localization', function (event) {
        var localization = event.currentTarget.value;
        if(localization.length > 0) {
            location.href = addOrReplaceParam(location.href, "localization", localization, false, false);
        }
    });

    $(document).on('focusout', '#minWage', function (event) {
        var minWage = event.currentTarget.value;
        location.href = addOrReplaceParam(location.href, "maxWage", minWage, false, false);
    });

    $(document).on('focusout', '#maxWage', function (event) {
        var minWage = event.currentTarget.value;
        location.href = addOrReplaceParam(location.href, "maxWage", minWage, false, false);
    });

    $(document).on('change', '.availability-menu', function (event) {
        location.href = addOrReplaceParam(location.href, "availability", event.currentTarget.value, true, event.currentTarget.checked);
        console.log(event.currentTarget);
    });
});


var addOrReplaceParam = function (url, param, value, isArray, isChecked) {
    var query = param + "=" + value;
    var shouldAddParam = true;
    var parametersString = url.split("?")[1];
    if (url.indexOf("?") === -1) {
        url += "?" + param + "=" + value;
    }
    else if (parametersString !== undefined) {
        var parameters = parametersString.split("&");
        if (isArray) {
            for (let i = 0; i < parameters.length; i++) {
                if (parameters[i].indexOf(query) !== -1) {
                    if (!isChecked) {
                        parameters.splice(i, 1);
                        shouldAddParam = false;
                        break;
                    }
                }
            }
            if (shouldAddParam) {
                parameters.push(query);
            }
        }
        else {
            for (let i = 0; i < parameters.length; i++) {
                if (parameters[i].indexOf(param) !== -1) {
                    parameters[i] = query;
                    shouldAddParam = false;
                    break;
                }
            }
            if (shouldAddParam) {
                parameters.push(query);
            }
        }
        if (parameters.length > 0) {
            url = url.split("?")[0] + "?" + parameters.join("&");
        } else {
            url = url.split("?")[0];
        }
    }
    return url;
}