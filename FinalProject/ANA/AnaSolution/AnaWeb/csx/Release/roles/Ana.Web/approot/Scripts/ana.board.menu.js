/// <reference path="jquery.js" />

$(document).ready(function () {
    $('#board-menu-items li').each(function () {
        if ($(this).find('a').length > 0 && $(this).find('a').attr('href') == $(location).attr('pathname')) {
            $(this).addClass('active');
        }
    });

});