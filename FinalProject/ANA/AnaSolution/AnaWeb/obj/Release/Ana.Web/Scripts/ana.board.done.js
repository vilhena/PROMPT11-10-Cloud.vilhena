/// <reference path="jquery.js" />

$(document).ready(function () {
    $('[set_done]').each(function () {
        $(this).click(function (event) {
            event.preventDefault();
            var link = this;
            var i = $(link).find("i");

            $.ajax({
                type: "POST",
                url: link.href,
                success: function (data) {
                    if (data) {
                        $(link).addClass("btn-success");
                        $(i).addClass("icon-white");
                    } else {
                        $(link).removeClass("btn-success");
                        $(i).removeClass("icon-white");
                    }
                },
                error: function (data) {
                    alert("could not process request");
                }
            });


        });
    });
});