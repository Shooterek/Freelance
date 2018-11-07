$(document).ready(function () {
    var url = location.href;
    $(document).on('focusout', ' #Localization', function (event) {
        var localization = event.currentTarget.value;
        url = changeParam(url, "localization", localization, false, false);
        if (isBigWindow() && url !== location.href) {
            location.href = url;
        }
    });

    $(document).on('focusout', '#MinWage', function (event) {
        var minWage = event.currentTarget.value;
        url = changeParam(url, "minWage", minWage, false, false);
        if (isBigWindow() && url !== location.href) {
            location.href = url;
        }
    });

    $(document).on('focusout', '#MaxWage', function (event) {
        var maxWage = event.currentTarget.value;
        url = changeParam(url, "maxWage", maxWage, false, false);
        if (isBigWindow() && url !== location.href) {
            location.href = url;
        }
    });

    $(document).on('change', '.availability-menu', function (event) {
        url = changeParam(url, "availability", event.currentTarget.value, true, event.currentTarget.checked);
        if (isBigWindow() && url !== location.href) {
            location.href = url;
        }
    });

    $(document).on('change', '#ServiceTypeId', function (event) {
        url = changeParam(url, "serviceType", event.currentTarget.value, false, event.currentTarget.checked);
        if (isBigWindow() && url !== location.href) {
            location.href = url;
        }
    });

    $(document).on('click', '#btn-show-announcements', function () {
        if (!isBigWindow() && url !== location.href) {
            location.href = url;
        }
    });
});


var changeParam = function (url, param, value, isArray, isChecked) {
    var newUrl = url.substr(0, url.lastIndexOf("/")) + "/1?";
    var query = param + "=" + value;
    var shouldAddParam = true;
    var parametersString = url.split("?")[1];

    if (url.indexOf("?") === -1 && value.length > 0) {
        url = newUrl + param + "=" + value;
    }
    else if (parametersString !== undefined) {
        var parameters = parametersString.split("&");
        if (isArray) {
            for (let i = 0; i < parameters.length; i++) {
                if (parameters[i].indexOf(query) !== -1 && !isChecked) {
                    parameters.splice(i, 1);
                    shouldAddParam = false;
                    break;
                }
            }
            if (shouldAddParam) {
                parameters.push(query);
            }
        }
        else {
            for (let i = 0; i < parameters.length; i++) {
                if (parameters[i].indexOf(param) !== -1) {
                    if (value.length === 0) {
                        parameters.splice(i, 1);
                    } else {
                        parameters[i] = query;
                    }
                    shouldAddParam = false;
                    break;
                }
            }
            if (shouldAddParam && value.length > 0) {
                parameters.push(query);
            }
        }
        if (parameters.length > 0) {
            url = newUrl + parameters.join("&");
        } else {
            url = newUrl.split("?")[0];
        }
    }
    return url;
}

var isBigWindow = function() {
    return window.innerWidth > 767;
}