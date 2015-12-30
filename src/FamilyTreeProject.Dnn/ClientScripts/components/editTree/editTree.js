/*
 Copyright (C) 2014-2015 Charles Nurse
                                      
 Licensed under MIT License           
 (see included LICENSE)               
                                      
 */

/**
 * Provides a Edit Tree component for the Family Tree Project
 *
 * @module components/editTree/editTree
 * @requires knockout, config, text!./editTree.html
 */
define("components/editTree/editTree", ["knockout", "config", "text!./editTree.html"], function (ko, config, htmlString) {

    function editTreeViewModel(props) {
        var self = this;
        self.resx = config.resx;

        self.dialogTitle = props.dialogTitle;
        self.isVisible = props.isVisible;

        self.onSuccess = props.onSuccess;

        self.treeId = ko.observable(props.treeId());
        self.newTreeId = props.newTreeId;

        if (self.treeId() === -1) {
            self.name = ko.observable("");
            self.title = ko.observable("");
            self.description = ko.observable("");
        } else {
            self.name = props.name;
            self.title = props.title;
            self.description = props.description;
        }

        self.widget = ko.observable();

        self.cancel = function() {
            self.widget().dialog("close");
        }

        self.close = function () {

        }

        self.saveTree = function () {
            var params = {
                treeId: self.treeId(),
                name: self.name(),
                title: self.title(),
                description: self.description()
            };

            config.treeService().post("SaveTree",
                params,
                function (data) {
                    self.newTreeId(data.treeId);
                    self.cancel();
                    self.onSuccess();
                }
            );
        }
    }

    // Return component definition
    return { viewModel: editTreeViewModel, template: htmlString };
});