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

        self.selectedCSSClass = ko.observable("");
        self.selectedText = ko.observable("");

        var menuClick = function (panel) {
            if (self.activePanel() === panel) {
                return;
            }
            self.activePanel(panel);
        };

        var updateSelectedItem = function() {
            switch (self.activePanel()) {
                case "individuals":
                    self.selectedText(self.resx.individualList);
                    self.selectedCSSClass("fa-list");
                    break;
                case "familyGroup":
                    self.selectedText(self.resx.familyGroupView);
                    self.selectedCSSClass("fa-users");
                    break;
                default:
                    self.selectedText(self.resx.treeOverView);
                    self.selectedCSSClass("fa-sitemap");
                    break;
            }
        }

        self.activePanel.subscribe(function(newValue) {
            var listItem = $("#" + newValue + "-menu");
            listItem.parent().children().removeClass("active");
            listItem.addClass("active");

            updateSelectedItem();
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

        updateSelectedItem();
    }

    // Return component definition
    return { viewModel: viewSelectorViewModel, template: htmlString };
});
