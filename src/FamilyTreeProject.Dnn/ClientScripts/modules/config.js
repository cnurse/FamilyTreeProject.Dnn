/*
 Copyright (C) 2014-2015 Charles Nurse
                                      
 Licensed under MIT License           
 (see included LICENSE)               
                                      
 */

/**
 * Provides configuration for the Family Tree Project
 *
 * @module config
 * @requires dnn.utility.js
 */
define("config", ["js!dnnUtility"], function () {

    /**
     *
     *
     */
    // ReSharper disable once InconsistentNaming
    var Config = function(){
        var self = this;
        self.resx = {};
        self.settings = {};
        self.sfSettings = {};

        // ReSharper disable once UseOfImplicitGlobalInFunctionScope
        var sf = dnn.sf();

        /**
         * Initialize the Configuration module
         *
         */
        self.init = function(appSettings, languageSettings, sfSettings) {
            self.resx = languageSettings;
            self.settings = appSettings;

            self.settings.$rootElement = $("#FTP-" + appSettings.moduleId);
            self.settings.pageSize = 10;

            self.settings.defaultFileUploadSettings = {
                moduleId: appSettings.moduleId,
                showOnStartup: false,
                serviceRoot: sfSettings.servicePath,
                fileUploadMethod: "File/UploadFile",
                maxFileSize: 29360128,
                maxFiles: 1,
                extensions: [],
                resources: languageSettings.fileUpload,
                width: 600,
                height: 450,
                parameters: {
                    isHostPortal: false
                },
                folderPath: "",
                folderPicker: {
                    disabled: true,
                    initialState: {
                        selectedItem: {
                            key: -1,
                            value: ""
                        }
                    }
                }
            };

            sf.init(sfSettings);
        };

        /**
         * The file REST Service
         *
         */
        self.fileService = function() {
            sf.serviceController = "File";
            return sf;
        }

        /**
         * The individual REST Service
         *
         */
        self. individualService = function () {
            sf.serviceController = "Individual";
            return sf;
        };

        /**
         * The tree REST Service
         *
         */
        self.treeService = function () {
            sf.serviceController = "Tree";
            return sf;
        };
    };

    return new Config();
});