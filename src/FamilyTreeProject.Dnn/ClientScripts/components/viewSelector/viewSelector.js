/*
 Copyright (C) 2014-2015 Charles Nurse

 Licensed under MIT License
 (see included LICENSE)

 */

/**
 * Provides a View Selector component for the Family Tree Project
 *
 * @module components/viewSelector/viewSelector
 * @requires knockout, config, text!./viewSelector.html
 */
define("components/viewSelector/viewSelector", ["knockout", "config", "text!./viewSelector.html"], function(ko, config, htmlString) {

    function viewSelectorViewModel(params) {
        var self = this;
        self.resx = config.resx;

        self.activePanel = params.activePanel;
        self.panels = params.panels;
        self.selectedIndividualId = params.selectedIndividualId;

        var menuClick = function (panel) {
            if (self.activePanel() === panel) {
                return;
            }
            self.activePanel(panel);
        };

        self.activePanel.subscribe(function(newValue) {
            var listItem = $("#" + newValue + "-menu");
            listItem.parent().children().removeClass("selected");
            listItem.addClass("selected");
        });

        self.selectFamilyGroupView = function (data, e) {
            menuClick(self.panels.familyGroup);
        };

        self.selectIndividualList = function (data, e) {
            menuClick(self.panels.individuals);
        };

        self.selectTreeOverview = function (data, e) {
            menuClick(self.panels.tree);
        };
    }

    // Return component definition
    return { viewModel: viewSelectorViewModel, template: htmlString };
});
