﻿$(document).ready(function () {
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
        location.href = url;
    });

    $(document).on('click', '#add-photos', function () {
        $('#Photos').click();
    });

    $(document).on('click', '#Photos', function () {
        $(this).val("");
    });

    $(document).on('change', '#Photos', function () {
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

    $(document).on('click', '.preview', function () {
        var id = $(this).attr('id');
        var selector = `#photo${id}`;
        $(selector).modal('show');
    });

    $(document).on('click', '#add-next', function () {
        if (areInputsTouchedAndCorrect("#add-1 :input.form-control")) {
            $('#add-1').toggleClass('hidden');
            $('#add-2').toggleClass('hidden');
        }
    });

    $("#add-offer-form").submit(function (e) {
        var form = $(this);
        var url = form.attr('action');

        $.ajax({
            type: "POST",
            url: url,
            data: form.serialize(), // serializes the form's elements.
            success: function (data, textStatus, xhr) {
                location.href = location.href;
            },
            error: function(data) {
                alert("Nie udało się dodać oferty");
            }
        });

        e.preventDefault(); // avoid to execute the actual submit of the form.
    });

    $(document).on('click', '#add-previous', function () {
        $('#add-2').toggleClass('hidden');
        $('#add-1').toggleClass('hidden');
    });

    $(document).on('focusout', '.disable-next-button', function () {
        $(this).toggleClass('disable-next-button');
    });

    $(document).on('click', '#show-more-offers', function () {
        $(this).addClass('hidden');
        $('#second-part').toggleClass('hidden');
    });

    $(document).on('click', 'div.rating.pointer', function (event) {
        var userId = $(this).attr('data-user-id');
        var user = $(this).attr('data-user');
        $.ajax({
            type: "GET",
            url: "/opinions/getopinions?userId=" + userId,
            success: function (result2) {
                var modal = $('#opinionsModal');
                modal.find('.modal-title').text('Opinie o użytkowniku ' + user);
                $('#modalBody').html(result2);
                $('#opinionsModal').modal('show');
            }
        });
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

var areInputsTouchedAndCorrect = function (selector) {
    var isCorrect = true;
    $(selector).each((index, element) => {
        isCorrect = !$(element).valid() ? false : isCorrect;
    });

    return isCorrect;
}