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
define("components/treeView/treeView",
        ["knockout", "config", "text!./treeView.html", "individual"],
        function (ko, config, htmlString, individualFactory) {

    /**
     * Provides the Tree View View Model for the Family Tree Project
     *
     * @constructor 
     * @param params - the parameters pass in from the component markup
     */
    function treeViewViewModel(params) {
        var self = this;
        self.resx = config.resx;

        self.activePanel = params.activePanel;
        self.treeId = params.treeId;
        self.selectedIndividualId = params.selectedIndividualId;

        self.name = ko.observable("");
        self.title = ko.observable("");
        self.description = ko.observable("");
        self.imageUrl = ko.observable("");
        self.imageId = ko.observable(0);

        self.homeIndividual = ko.observable(individualFactory(null));
        self.lastViewedIndividual = ko.observable(individualFactory(null));

        self.individualCount = ko.observable(0);
        self.familyCount = ko.observable(0);
        self.factCount = ko.observable(0);

        self.showUploadButton = ko.observable(false);
        self.showEdit = ko.observable(false);
        self.showUpload = ko.observable(false);

        self.treeId.subscribe(function () {
            self.getTree();
        });

        self.disableButton = function () {
            self.showUploadButton(false);
        }

        self.editTree = function () {
            self.showEdit(true);
        }

        self.enableButton = function() {
            self.showUploadButton(true);
        }

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
                        self.imageUrl(data.results.imageUrl);
                        self.homeIndividual(individualFactory(data.results.homeIndividual));
                        self.lastViewedIndividual(individualFactory(data.results.lastViewedIndividual));

                        self.individualCount(data.results.individualCount);
                        self.familyCount(data.results.familyCount);
                        self.factCount(data.results.factCount);
                    }
                );
            }
        }

        self.onFileUploaded = function () {
            var params = {
                treeId: self.treeId(),
                name: self.name(),
                title: self.title(),
                description: self.description(),
                imageId: self.imageId()
            };

            config.treeService().post("SaveTree",
                params,
                function () {
                    self.getTree();
                }
            );

        }

        self.onTreeUpdated = function () {
            self.getTree();
        }

        self.uploadFile = function () {
            self.showUpload(true);
        };

        self.viewIndividual = function (data) {
            self.selectedIndividualId(data.individualId());
            self.activePanel(config.settings.familyGroupPanel);
        }

        self.viewIndividuals = function() {
            self.activePanel(config.settings.individualsPanel);
        }

        self.getTree();
    }

    // Return component definition
    return { viewModel: treeViewViewModel, template: htmlString };
});
