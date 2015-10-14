/*
 Copyright (C) 2014-2015 Charles Nurse
                                      
 Licensed under MIT License           
 (see included LICENSE)               
                                      
 */

/**
 * Provides an Upload File component for the Family Tree Project
 *
 * @module components/uploadFile/uploadFile
 * @requires knockout, config, text!./uploadFile.html
 */
define("components/uploadFile/uploadFile", ["jquery", "knockout", "config", "text!./uploadFile.html"], function ($, ko, config, htmlString) {

    // ReSharper disable once InconsistentNaming
    function UploadFileViewModel(props) {
        var self = this;
        var options = $.extend({}, config.settings.defaultFileUploadSettings, props.options);
        var treeId = -1;

        self.resx = config.resx;

        self.dialogTitle = props.title;
        self.isVisible = props.isVisible;
        self.width = options.width;
        self.height = options.height;
        self.newTreeId = props.newTreeId;

        self.onSuccess = props.onSuccess;

        self.widget = ko.observable();

        var getUserFolder = function () {
            config.fileService().get("GetUserFolder", {},
                function (data) {
                    if (typeof data !== "undefined" && data !== null) {
                        config.settings.userFolderId = data.userFolderId;
                    }
                }
            );
        };

        self.cancel = function () {
            self.widget().dialog("close");
        }

        self.close = function () {
            if (treeId > -1) {
                self.newTreeId(treeId);
                if (typeof self.onSuccess === "function") {
                    self.onSuccess();
                }
            }
        }

        self.init = function () {
            if (config.settings.userFolderId === undefined) {
                getUserFolder();
            }
            options.folderPicker.initialState.selectedItem.key = config.settings.userFolderId;

            // ReSharper disable once UseOfImplicitGlobalInFunctionScope
            var instance = new dnn.FileUploadPanel(options);
            var $parent = config.settings.$rootElement.find("#uploadFile");
            if ($parent.length !== 0) {
                $parent.append(instance.$element);
                instance.$element.on("onfileuploadcomplete", self.uploadComplete);
            }
        }

        self.uploadComplete = function(event, data) {
            treeId = JSON.parse(data).treeId;
        }

        self.init();
    }

    // Return component definition
    return { viewModel: UploadFileViewModel, template: htmlString };
});