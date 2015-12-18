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
define("config", [], function () {

    // ReSharper disable once InconsistentNaming
    var Services = function () {
        var self = this;

        var isLoaded = false;
        var loadingBarId;
        var serviceController = "";
        var serviceFramework;
        var baseServicepath;

        var loadingBar = function (loadingBarId) {
            if (isLoaded) return;
            var loadingbar = $(loadingBarId);
            var progressbar = $(loadingBarId + ' > div');
            var width = loadingbar.width();
            loadingbar.show();
            progressbar.css({ width: 0 }).animate({ width: 0.75 * width }, 300, 'linear', function () {
                var checkloaded = function () {
                    if (isLoaded) {
                        isLoaded = false;
                        clearTimeout(checkloaded);
                        checkloaded = null;
                        progressbar.animate({ width: width }, 100, 'linear', function () {
                            loadingbar.hide();
                        });
                    }
                    else {
                        setTimeout(checkloaded, 20);
                    }
                };
                checkloaded();
            });
        };

        var call = function (httpMethod, url, params, onSuccess, onFailure, loading, sync, silence) {
            var options = {
                url: url,
                beforeSend: serviceFramework.setModuleHeaders,
                type: httpMethod,
                async: sync === false,
                success: function (data) {
                    if (loadingBarId && !silence) isLoaded = true;
                    if (typeof loading === "function") {
                        loading(false);
                    }

                    if (typeof onSuccess === "function") {
                        onSuccess(data || {});
                    }
                },
                error: function (xhr, status, err) {
                    if (loadingBarId && !silence) isLoaded = true;
                    if (typeof loading === "function") {
                        loading(false);
                    }

                    if (typeof onFailure === "function") {
                        if (xhr) {
                            onFailure(xhr, status, err);
                        }
                        else {
                            onFailure(null, "Unknown error", "");
                        }
                    }
                }
            };

            if (httpMethod === "GET") {
                options.data = params;
            }
            else {
                options.contentType = "application/json; charset=UTF-8";
                options.data = JSON.stringify(params);
                options.dataType = "json";
            }

            if (typeof loading === "function") {
                loading(true);
            }

            if (loadingBarId && !silence) loadingBar(loadingBarId);
            return $.ajax(options);
        };

        var get = function (method, params, onSuccess, onFailure, loading) {
            var self = this;
            var url = baseServicepath + self.serviceController + "/" + method;
            return call("GET", url, params, onSuccess, onFailure, loading, false, false);
        };

        var init = function (settings) {
            loadingBarId = settings.loadingBarId;
            serviceFramework = settings.servicesFramework;
            baseServicepath = serviceFramework.getServiceRoot(settings.servicePath);
        }

        var post = function (method, params, onSuccess, onFailure, loading) {
            var self = this;
            var url = baseServicepath + self.serviceController + "/" + method;
            return call("POST", url, params, onSuccess, onFailure, loading, false, false);
        };

        var put = function (method, params, onSuccess, onFailure, loading) {
            var self = this;
            var url = baseServicepath + self.serviceController + "/" + method;
            return call("PUT", url, params, onSuccess, onFailure, loading, false, false);
        };

        return {
            get: get,
            init: init,
            post: post,
            put: put,
            serviceController: serviceController
        }
    }

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
        var services = new Services();

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

            services.init(sfSettings);
        };

        /**
         * The file REST Service
         *
         */
        self.fileService = function() {
            services.serviceController = "File";
            return services;
        }

        /**
         * The individual REST Service
         *
         */
        self. individualService = function () {
            services.serviceController = "Individual";
            return services;
        };

        /**
         * The settings REST Service
         *
         */
        self.settingsService = function () {
            services.serviceController = "Settings";
            return services;
        }

        /**
         * The tree REST Service
         *
         */
        self.treeService = function () {
            services.serviceController = "Tree";
            return services;
        };
    };

    return new Config();
});