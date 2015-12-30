/*
 Copyright (C) 2014-2015 Charles Nurse
                                      
 Licensed under MIT License           
 (see included LICENSE)               
                                      
 */

/**
 * Provides utility functions for the Family Tree Project
 *
 * @module util
 */
define("util", ["config"], function (config) {

    /**
     *
     *
     */
    // ReSharper disable once InconsistentNaming
    var Util = function () {
        var self = this;
        var resx = config.resx;

        var alertConfirm = function (text, confirmBtnText, cancelBtnText, confirmHandler, cancelHandler) {
            $('#confirmation-dialog > div.dnnDialog').html(text);
            $('#confirmation-dialog a#confirmbtn').html(confirmBtnText).unbind('click').bind('click', function () {
                if (typeof confirmHandler === 'function') confirmHandler.apply();
                $('#confirmation-dialog').fadeOut(200, 'linear', function () { $('#mask').hide(); });
            });

            var $cancelBtn = $('#confirmation-dialog a#cancelbtn');
            if (cancelBtnText !== '') {
                $cancelBtn.html(cancelBtnText).unbind('click').bind('click', function () {
                    if (typeof cancelHandler === 'function') cancelHandler.apply();
                    $('#confirmation-dialog').fadeOut(200, 'linear', function () { $('#mask').hide(); });
                });
                $cancelBtn.show();
            }
            else {
                $cancelBtn.hide();
            }

            $('#mask').show();
            $('#confirmation-dialog').fadeIn(200, 'linear');

            $(window).off('keydown.confirmDialog').on('keydown.confirmDialog', function (evt) {

                if (evt.keyCode === 27) {
                    $(window).off('keydown.confirmDialog');
                    $('#confirmation-dialog a#cancelbtn').trigger('click');
                }
            });
        };

        self.alert = function (text, closeBtnText, closeBtnHandler) {
            $('#confirmation-dialog > div.dnnDialogHeader').html(resx.alert);
            alertConfirm(text, closeBtnText, "", closeBtnHandler, null);
        };

        self.confirm = function (text, confirmBtnText, cancelBtnText, confirmHandler, cancelHandler) {
            $('#confirmation-dialog > div.dnnDialogHeader').html(resx.confirm);
            alertConfirm(text, confirmBtnText, cancelBtnText, confirmHandler, cancelHandler);
        };


    };

    return new Util();
});