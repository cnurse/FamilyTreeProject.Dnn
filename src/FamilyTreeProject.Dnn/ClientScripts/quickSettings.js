if (typeof ftp === "undefined" || ftp === null) {
    ftp = {};
};

ftp.QuickSettings = function ($, ko, options, resx) {
    var opts = $.extend({}, ftp.QuickSettings.defaultOptions, options);
    var $rootElement;

    var sf = dnn.sf();
    sf.init(opts);
    sf.settingsService = function () {
        sf.serviceController = "Settings";
        return sf;
    };

    var viewModel = {};

    var init = function (element) {
        $rootElement = $(element);

        viewModel.owner = ko.observable("");

        viewModel.getSettings = function() {
            sf.settingsService().get("GetSettings", {},
                function(data) {
                    if (typeof data !== "undefined" && data != null && data.success === true) {
                        //Success
                        viewModel.owner(data.data.results.owner);
                    }
                },
                function() {
                    //Failure
                });
        };

        viewModel.saveSettings = function () {
            var params = {
                owner: viewModel.owner()
            };

            sf.settingsService().post("SaveSettings", params,
                function (data) {
                    if (data.success === true) {
                        //Success
                    } else {
                        //Error
                        util.alert(data.message, resx.ok);
                    }
                },
                function () {
                    //Failure
                }
            );

        };

        ko.applyBindings(viewModel, $rootElement[0]);

        $(element).dnnQuickSettings({
            moduleId: opts.moduleId,
            onSave: viewModel.saveSettings
        });

        viewModel.getSettings();
    }

    return {
        init: init
    }
};

ftp.QuickSettings.defaultOptions = {

};