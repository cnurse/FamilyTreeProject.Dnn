/*
 Copyright (C) 2014-2015 Charles Nurse

 Licensed under MIT License
 (see included LICENSE)

 */

/**
 * Provides a Family Group View component for the Family Tree Project
 *
 * @module components/familyGroupView/familyGroupView
 * @requires knockout, config, text!./familyGroupView.html
 */
define("components/familyGroupView/familyGroupView",
        ["knockout", "config", "text!./familyGroupView.html", "individual"],
        function (ko, config, htmlString, individualFactory) {

    function familyGroupViewViewModel(params) {
        var self = this;
        self.resx = config.resx;

        self.treeId = params.treeId;
        self.selectedIndividualId = params.selectedIndividualId;

        self.individual = ko.observable(individualFactory(null));

        self.father = ko.observable(individualFactory(null, "Male"));
        self.mother = ko.observable(individualFactory(null, "Female"));

        self.families = ko.observableArray([]);

        self.getIndividual = function() {
            var params = {
                treeId: self.treeId(),
                id: self.selectedIndividualId(),
                includeAncestors: 1,
                includeFamilies: true
            };

            config.individualService().get("GetIndividual", params,
                function (data) {
                    if (typeof data !== "undefined" && data != null) {
                        var result = data.results;
                        self.individual(individualFactory(result));

                        self.father(individualFactory(result.father, "Male"));
                        self.mother(individualFactory(result.mother, "Female"));
                    }
                }
            );

        };

        self.viewIndividual = function (data) {
            self.selectedIndividualId(data.individualId());
            self.getIndividual();
        }

        self.selectedIndividualId.subscribe = function() {
            self.getIndividual();
        };

        self.getIndividual();
    }

    // Return component definition
    return { viewModel: familyGroupViewViewModel, template: htmlString };
});
