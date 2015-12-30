/*
 Copyright (C) 2014-2015 Charles Nurse
                                      
 Licensed under MIT License           
 (see included LICENSE)               
                                      
 */

define("familyTreeProject", ["jquery", "knockout", "config"], function ($, ko, config) {
    var $rootElement;
    var activePanel;
    var viewModel = {};

    var init = function () {
        $rootElement = config.settings.$rootElement;

        activePanel = config.settings.initialPanel;
        $rootElement.find(config.settings.initialPanel).addClass("selected");

        //Build the ViewModel
        viewModel.resx = config.resx;

        viewModel.treeId = ko.observable();
        viewModel.selectedIndividualId = ko.observable();
        viewModel.panels = {
            familyGroup: config.settings.familyGroupPanel,
            individuals: config.settings.individualsPanel,
            tree: config.settings.treePanel
        };

        viewModel.activePanel = ko.observable(config.settings.initialPanel);

        viewModel.treeId.subscribe(function () {
             if (viewModel.treeId() === undefined || viewModel.treeId() === -1) {
                return;
             }
            viewModel.activePanel(config.settings.initialPanel);
         });

        viewModel.showCloseIcon = ko.computed(function () {
            return true;
        });

        viewModel.closeEdit = function () {
        };

        ko.components.register("treeSelector", { require: "components/treeSelector/treeSelector" });
        ko.components.register("viewSelector", { require: "components/viewSelector/viewSelector" });
        ko.components.register("treeView", { require: "components/treeView/treeView" });
        ko.components.register("familyGroupView", { require: "components/familyGroupView/familyGroupView" });
        ko.components.register("individualList", { require: "components/individualList/individualList" });
        ko.components.register("individualCard", { require: "components/individualCard/individualCard" });
        ko.components.register("individualDetail", { require: "components/individualDetail/individualDetail" });
        ko.components.register("individualLink", { require: "components/individualLink/individualLink" });
        ko.components.register("individualRow", { require: "components/individualRow/individualRow" });
        ko.components.register("editIndividual", { require: "components/editIndividual/editIndividual" });
        ko.components.register("editTree", { require: "components/editTree/editTree" });
        ko.components.register("uploadFile", { require: "components/uploadFile/uploadFile" });
        ko.components.register("pager", { require: "components/pager/pager" });

        ko.applyBindings(viewModel, $rootElement[0]);
    };

    return {
        init: init
    };
});
