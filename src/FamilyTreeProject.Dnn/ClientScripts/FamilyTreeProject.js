if (typeof ftp === 'undefined' || ftp === null) {
    ftp = {};
};

ftp.FamilyTreeProject = function($, ko, settings, resx) {
    var $rootElement;

    var viewModel = {};

    var init = function (element) {
        $rootElement = $(element);

        var config = {
            settings: settings,
            resx: resx,
            $rootElement: $rootElement
        };

        //Build the ViewModel
        viewModel.resx = resx;

        ko.applyBindings(viewModel, $rootElement[0]);
    }

    return {
        init: init
    }
}