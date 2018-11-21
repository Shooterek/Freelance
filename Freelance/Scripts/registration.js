$(document).ready(function() {
    $(document).on('click', '#add-profile-photo', function () {
        $('#Photo').click();
    });

    $(document).on('change', '#Photo', function () {
        $('.preview-area').empty();
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
    });
});