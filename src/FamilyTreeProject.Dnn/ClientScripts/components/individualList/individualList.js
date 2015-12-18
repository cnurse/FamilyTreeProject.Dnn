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
define("components/individualList/individualList",
        ["knockout", "config", "text!./individualList.html", "individual"],
        function (ko, config, htmlString, individualFactory) {

    /**
     * Provides the Individual List View Model for the Family Tree Project
     *
     * @constructor 
     * @param params - the parameters passed in from the component markup
     */
    // ReSharper disable once InconsistentNaming
    var IndividualListViewModel = function(params) {
        var self = this;
        var settings = config.settings;

        self.resx = config.resx;

        self.activePanel = params.activePanel;
        self.treeId = params.treeId;
        self.selectedIndividualId = params.selectedIndividualId;

        self.individuals = ko.observableArray([]);

        self.searchText = ko.observable("");
        self.totalResults = ko.observable(0);
        self.pageSize = ko.observable(settings.pageSize);
        self.pageIndex = ko.observable(0);

        var findIndividuals = function () {
            self.pageIndex(0);
            self.getIndividuals();
        };

        self.searchText.subscribe(function () {
            findIndividuals();
        });

        self.pageSize.subscribe(function() {
            findIndividuals();
        });

        self.addIndividual = function() {

        };

        self.getIndividuals = function () {
            var params = {
                treeId: self.treeId(),
                searchTerm: self.searchText(),
                pageIndex: self.pageIndex(),
                pageSize: self.pageSize
            };

            config.individualService().get("GetIndividuals", params,
                function(data) {
                    if (typeof data !== "undefined" && data != null) {
                        self.individuals.removeAll();
                        var results = data.results;
                        for (var i = 0; i < results.length; i++) {
                            var result = results[i];
                            self.individuals.push(individualFactory(result));
                        }

                        self.totalResults(data.total);
                    }
                }
            );
        };

        self.refresh = function () {
            self.getIndividuals();
        }

        self.viewIndividual = function (data) {
            self.selectedIndividualId(data.individualId());
            self.activePanel(config.settings.familyGroupPanel);
        }

        if (!ko.components.isRegistered("pager")) {
            ko.components.register("pager", { require: "components/pager/pager" });
        }

        self.searchText('');
        self.getIndividuals();
    }

    // Return component definition
    return { viewModel: IndividualListViewModel, template: htmlString };
});
