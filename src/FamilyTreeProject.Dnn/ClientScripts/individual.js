if (typeof ftp === 'undefined' || ftp === null) {
    ftp = {};
};

ftp.IndividualViewModel = function(parentViewModel, config) {
    var self = this;
    var resx = config.resx;
    var settings = config.settings;
    var util = config.util;
    var $rootElement = config.$rootElement;
    var ko = config.ko;

    self.parentViewModel = parentViewModel;
    self.rootViewModel = parentViewModel.rootViewModel;

    self.name = ko.observable('');
    self.birth = ko.observable('');
    self.death = ko.observable('');

    self.canEdit = ko.observable(false);
    self.selected = ko.observable(false);

    self.deleteIndividual = function(data) {
        
    }

    self.editIndividual = function (data) {

    }

    self.load = function (data) {
        self.canEdit(false);
        self.name(data.lastName + ", " + data.firstName);
        self.birth(data.birth);
        self.death(data.death);
    };

    self.toggleSelected = function () {
        self.selected(!self.selected());
    };
}