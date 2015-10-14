/*
 Copyright (C) 2014-2015 Charles Nurse

 Licensed under MIT License
 (see included LICENSE)

 */

/**
 * Provides a Ancestors View component for the Family Tree Project
 *
 * @module components/individualList/individualList
 * @requires knockout, config, text!./individualList.html
 */
define("components/individualList/individualList", ["knockout", "config", "text!./individualList.html"], function(ko, config, htmlString) {

    var IndividualListViewModel = function(params, componentInfo) {
        var self = this;
        var settings = config.settings;

        self.resx = config.resx;

        self.searchText = ko.observable("");
        self.treeId = params.treeId;

        self.individuals = ko.observableArray([]);

        self.totalResults = ko.observable(0);
        self.pageSize = ko.observable(settings.pageSize);
        self.pageIndex = ko.observable(0)

        self.searchText.subscribe(function () {
            findIndividuals();
        });

        self.pageSize.subscribe(function() {
            findIndividuals();
        });

        var findIndividuals = function() {
            self.pageIndex(0);
            self.getIndividuals();
        };

        self.addIndividual = function() {

        };

        self.getIndividuals = function () {
            var params = {
                treeId: self.treeId(),
                searchTerm: self.searchText(),
                pageIndex: self.pageIndex(),
                pageSize: self.pageSize
            };

            config.individualService().getEntities("GetIndividuals",
                params,
                self.individuals,
                function () {
                    return new IndividualViewModel(self);
                },
                self.totalResults
            );
        };

        self.refresh = function () {
            self.getIndividuals();
        }

        ko.components.register("pager", { require: "components/pager/pager" });

        self.searchText('');
        self.getIndividuals();

    }

    var IndividualViewModel = function(parentViewModel) {
        var self = this;
        var settings = config.settings;

        self.resx = config.resx;

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

    // Return component definition
    return { viewModel: IndividualListViewModel, template: htmlString };
});
