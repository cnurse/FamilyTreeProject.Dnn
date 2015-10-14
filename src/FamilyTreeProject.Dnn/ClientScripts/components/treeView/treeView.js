/*
 Copyright (C) 2014-2015 Charles Nurse

 Licensed under MIT License
 (see included LICENSE)

 */

/**
 * Provides a Tree View component for the Family Tree Project
 *
 * @module components/treeView/treeView
 * @requires knockout, config, text!./treeView.html
 */
define("components/treeView/treeView", ["knockout", "config", "text!./treeView.html"], function(ko, config, htmlString) {

// ReSharper disable once InconsistentNaming
    function TreeViewViewModel(params, componentInfo) {
        var self = this;
        self.resx = config.resx;

        self.treeId = params.treeId;
        self.name = ko.observable("");
        self.title = ko.observable("");
        self.description = ko.observable("");
        self.homeIndividual = ko.observable("");
        self.lastViewedIndividual = ko.observable("");
        self.individualCount = ko.observable(0);
        self.familyCount = ko.observable(0);

        self.treeId.subscribe(function () {
            self.getTree();
        });

        self.getTree = function () {
            if (self.treeId() > 0) {
                var params = {
                    treeId: self.treeId()
                };

                config.treeService().get("GetTree",
                    params,
                    function (data) {
                        self.name(data.results.name);
                        self.title(data.results.title);
                        self.description(data.results.description);
                        if (data.results.homeIndividual !== null) {
                            self.homeIndividual(data.results.homeIndividual.lastName + ", " + data.results.homeIndividual.firstName);
                        }
                        if (data.results.lastViewedIndividual !== null) {
                            self.lastViewedIndividual(data.results.lastViewedIndividual.lastName + ", " + data.results.lastViewedIndividual.firstName);
                        }
                        self.individualCount(data.results.individualCount);
                        self.familyCount(data.results.familyCount);
                    }
                );
            }
        }

        self.getTree();
    }

    // Return component definition
    return { viewModel: TreeViewViewModel, template: htmlString };
});
