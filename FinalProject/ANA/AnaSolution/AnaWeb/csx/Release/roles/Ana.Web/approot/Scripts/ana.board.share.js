/// <reference path="jquery.js" />

$(document).ready(function () {
    $('#search_user').each(function () {
        var link = $(this).data("url");
        var text = this;
        $(this).data("source", function (query, process) {
            $.ajax({
                type: "POST",
                url: link,
                data: { name: query },
                success: function (data) {
                    process(data);
                },
                error: function (data) {
                    alert("could not process request");
                }
            });
        });
    });

    $('[remove_share]').each(function () {
        $(this).click(function (event) {
            event.preventDefault();
            var link = this;

            $.post(link.href, { user_id: $(link).data("user-id") },
                function (data) {
                    window.location.href = window.location.href;
                    
                }
            );

        });
    });


    $('#share_user').each(function () {
        $(this).click(function (event) {
            event.preventDefault();
            var link = this;
            var result = $('#search_user').val();

            $.post(link.href, { user_name: result },
                function (data) {
                    window.location.href = window.location.href;
                }
            );

        });
    });

});