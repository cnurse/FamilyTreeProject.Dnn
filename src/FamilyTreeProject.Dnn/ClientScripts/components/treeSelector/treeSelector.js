/*
 Copyright (C) 2014-2015 Charles Nurse
                                      
 Licensed under MIT License           
 (see included LICENSE)               
                                      
 */

/**
 * Provides a Tree Selector component for the Family Tree Project
 *
 * @module components/treeSelector/treeSelector
 * @requires knockout, config, text!./treeSelector.html
 */
define("components/treeSelector/treeSelector", ["knockout", "config", "text!./treeSelector.html"], function(ko, config, htmlString) {

    function treeSelectorViewModel(props) {
        var self = this;
        self.resx = config.resx;

        self.treeId = props.treeId;
        self.treeName = ko.observable("");
        self.trees = ko.observableArray([]);

        self.showCreate = ko.observable(false);
        self.showUpload = ko.observable(false);

        var getTrees = function () {
            var params = {};
            config.treeService().get("GetTrees", params,
                function (data) {
                    if (typeof data !== "undefined" && data != null) {
                        self.trees.removeAll();
                        var results = data.results;
                        var result;
                        switch (results.length)
                        {
                            case 0:
                                if (self.treeId() === undefined) {
                                    self.treeId(-1);
                                }
                                break;
                            case 1:
                                result = results[0];
                                self.trees.push({
                                    name: result.name,
                                    treeId: result.treeId
                                });
                                if (self.treeId() === undefined) {
                                    self.treeId(result.treeId);
                                    self.treeName(result.name);
                                }
                                break;
                            default:
                                for (var i = 0; i < results.length; i++) {
                                    result = results[i];
                                    if (self.treeId() === undefined && i === 0) {
                                        self.treeId(result.treeId);
                                        self.treeName(result.name);
                                    }
                                    self.trees.push({
                                        name: result.name,
                                        treeId: result.treeId
                                    });
                                }
                                break;
                        }
                    }
                }
            );
        }

        self.addTree = function () {
            self.showCreate(true);
        }

        self.init = function() {
            getTrees();
        }

        self.onTreeCreated = function() {
            getTrees();
        }

        self.onTreeUploaded = function () {
            getTrees();
        }

        self.uploadTree = function () {
            self.showUpload(true);
        };

        self.init();
    }

    // Return component definition
    return { viewModel: treeSelectorViewModel, template: htmlString };
});