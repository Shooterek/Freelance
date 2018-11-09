$(document).ready(function () {
    var url = location.href;
    $(document).on('keydown', '#page-number', function(event) {
        if (event.which === 13) {
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

    $(document).on('change', '#sorting', function (event) {
        url = changeParam(url, "sort", event.currentTarget.value, false, false);
        if (isBigWindow() && url !== location.href) {
            location.href = url;
        }
    });

    $(document).on('click', '#add-announcement-photos', function (event) {
        $('#files').click();
    });

    $(document).on('click', '#files', function (event) {
        $(this).val("");
    });

    $(document).on('change', '#files', function (event) {
        $('.preview-area').empty();
        var maxAmountOfFiles = $(this).attr('data-max-amount');
        var fileCount = this.files.length;
        if (fileCount > maxAmountOfFiles) {
            $(this).val("");
        } else {
            var fileList = this.files;

            var anyWindow = window.URL || window.webkitURL;

            for (var i = 0; i < fileList.length; i++) {
                //get a blob to play with
                var objectUrl = anyWindow.createObjectURL(fileList[i]);
                // for the next line to work, you need something class="preview-area" in your html
                $('.preview-area').append('<img class="preview img-responsive pull-left" src="' + objectUrl + '" />');
                // get rid of the blob
                window.URL.revokeObjectURL(fileList[i]);
            }
        }
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