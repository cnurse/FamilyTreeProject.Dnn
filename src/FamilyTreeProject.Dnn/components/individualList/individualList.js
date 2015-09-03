if (typeof ftp === 'undefined' || ftp === null) {
    ftp = {};
};

ftp.IndividualListViewModel = function (ko, rootViewModel, config) {
    var self = this;
    var resx = config.resx;
    var settings = config.settings;
    var util = config.util;

    self.rootViewModel = rootViewModel;

    self.mode = config.mode;
    self.searchText = ko.observable("");

    self.individuals = ko.observableArray([]);

    self.totalResults = ko.observable(0);
    self.pageSize = ko.observable(settings.pageSize);
    self.pager_PageDesc = resx.pager_PageDesc;
    self.pager_PagerFormat = resx.individuals_PagerFormat;
    self.pager_NoPagerFormat = resx.individuals_NoPagerFormat;

    var findIndividuals = function() {
        self.pageIndex(0);
        self.getIndividuals();
    };

    self.addIndividual = function() {

    };

    self.getIndividuals = function () {
        var params = {
            treeId: rootViewModel.treeId(),
            searchTerm: self.searchText(),
            pageIndex: self.pageIndex(),
            pageSize: self.pageSize
        };

        util.individualService().getEntities("GetIndividuals",
            params,
            self.individuals,
            function () {
                return new ftp.IndividualViewModel(self, config);
            },
            self.totalResults
        );
    };

    self.init = function() {
        dnn.koPager().init(self, config);
        self.searchText.subscribe(function () {
            findIndividuals();
        });
        self.pageSize.subscribe(function() {
            findIndividuals();
        });
    };

    self.refresh = function () {
        self.getIndividuals();
    }
};
