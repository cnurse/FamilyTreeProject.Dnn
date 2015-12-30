/*
 Copyright (C) 2014-2015 Charles Nurse
                                      
 Licensed under MIT License           
 (see included LICENSE)               
                                      
 */

/**
 * Provides a Edit Individual component for the Family Tree Project
 *
 * @module components/editIndividual/editIndividual
 * @requires knockout, config, text!./editIndividual.html
 */
define("components/editIndividual/editIndividual", ["knockout", "config", "text!./editIndividual.html"], function (ko, config, htmlString) {

    function editIndividualViewModel(props) {
        var self = this;
        self.resx = config.resx;

        self.dialogTitle = props.dialogTitle;
        self.message = props.message;
        self.isVisible = props.isVisible;
        self.onSuccess = props.onSuccess;

        if (typeof(props.individual) === "undefined" || props.individual == null) {
            self.treeId = props.treeId;
            self.individualId = ko.observable(props.individualId());
            self.newIndividualId = props.newIndividualId;
            self.firstName = ko.observable("");
            self.lastName = ko.observable("");
            self.sex = ko.observable("");
        } else {
            self.treeId = props.individual().treeId;
            self.individualId = props.individual().individualId;
            self.newIndividualId = props.individual().individualId;
            self.firstName = props.individual().firstName;
            self.lastName = props.individual().lastName;
            self.sex = props.individual().sex;
        }

        self.widget = ko.observable();

        self.cancel = function () {
            self.widget().dialog("close");
        }

        self.close = function () {

        }

        self.sexName = ko.pureComputed(function () {
            var individualId = 0;
            if (typeof self.individualId === "function") {
                individualId = self.individualId();
            }
            return "IndividualSex" + individualId;
        });

        self.saveIndividual = function () {
            var params = {
                id: self.individualId(),
                treeId: self.treeId(),
                firstName: self.firstName(),
                lastName: self.lastName(),
                sex: self.sex()
            };

            config.individualService().post("SaveIndividual",
                params,
                function (data) {
                    self.newIndividualId(data.id);
                    self.cancel();
                    self.onSuccess();
                }
            );
        }
    }

    // Return component definition
    return { viewModel: editIndividualViewModel, template: htmlString };
});