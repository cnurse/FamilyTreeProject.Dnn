/*
 Copyright (C) 2014-2015 Charles Nurse
                                      
 Licensed under MIT License           
 (see included LICENSE)               
                                      
 */

define("quickSettings", ["jquery", "knockout", "config"], function($, ko, config) {
    var $rootElement;

    var viewModel = {};

    var init = function(element) {
        $rootElement = $(element);

        viewModel.owner = ko.observable("");

        viewModel.getSettings = function() {
            config.settingsService().get("GetSettings", {},
                function(data) {
                    if (typeof data !== "undefined" && data != null) {
                        //Success
                        viewModel.owner(data.results.owner);
                    }
                },
                function() {
                    //Failure
                });
        };

        viewModel.saveSettings = function() {
            var params = {
                owner: viewModel.owner()
            };

            config.settingsService().post("SaveSettings", params,
                function(data) {
                    if (data.success === true) {
                        //Success
                    } else {
                        //Error
                        util.alert(data.message, resx.ok);
                    }
                },
                function() {
                    //Failure
                }
            );

        };

        ko.applyBindings(viewModel, $rootElement[0]);

        $(element).dnnQuickSettings({
            moduleId: config.settings.moduleId,
            onSave: viewModel.saveSettings
        });

        viewModel.getSettings();
    }

    return {
        init: init
    }
});