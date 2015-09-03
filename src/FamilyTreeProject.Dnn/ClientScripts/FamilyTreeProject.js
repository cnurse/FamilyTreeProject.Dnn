if (typeof ftp === 'undefined' || ftp === null) {
    ftp = {};
};

ftp.FamilyTreeProject = function ($, ko, settings, resx) {
    var $rootElement;
    var activePanel;
    var viewModel = {};

    var init = function (element) {
        $rootElement = $(element);

        activePanel = settings.initialPanel;
        $rootElement.find("#individuals-menu").addClass("selected");

        var util = dnn.utility(settings, resx);

        util.individualService = function () {
            util.sf.serviceController = "Individual";
            return util.sf;
        };

        util.treeService = function () {
            util.sf.serviceController = "Tree";
            return util.sf;
        };

        var config = {
            settings: settings,
            resx: resx,
            util: util,
            mode: ko.observable("individualsList"),
            $rootElement: $rootElement,
            ko: ko
        };

        var componentLoader = ftp.componentLoader($, ko, settings);
        componentLoader.init();

        //ko.components.register("individualList" { template:});

        var menuClick = function (target, panel) {
            $rootElement.find(".ftpMenu li").removeClass("selected");
            $rootElement.find(".ftpPanel").hide();

            var listItem = $(target);

            if (listItem.is("li") === false) {
                listItem = listItem.closest('li');
            }

            listItem.addClass("selected");

            if (activePanel === panel) {
                return;
            }

            $(panel).show();

            activePanel = panel;
        };

        //Build the ViewModel
        viewModel.resx = resx;

        viewModel.mode = config.mode;
        viewModel.treeId = ko.observable(-1);
        viewModel.treeName = ko.observable("");
        viewModel.trees = ko.observableArray([]);

        viewModel.treeId.subscribe(function () {
            if (viewModel.treeId() === undefined || viewModel.treeId() === -1) {
                return;
            }
            viewModel.individualList.pageIndex(0);
            viewModel.individualList.searchText('');
            viewModel.individualList.getIndividuals();
        });

        var getTrees = function () {
            var params = {};
            util.treeService().get("GetTrees", params,
                function (data) {
                    if (typeof data !== "undefined" && data != null && data.success === true) {
                        viewModel.trees.removeAll();
                        viewModel.trees.push({
                            name: resx.selectTree,
                            treeId: -1
                        });

                        var results = data.data.results;
                        var result;
                        if (results.length > 1) {
                            for (var i = 0; i < results.length; i++) {
                                result = results[i];
                                viewModel.trees.push({
                                    name: result.name,
                                    treeId: result.treeId
                                });
                            }
                        } else {
                            result = results[0];
                            viewModel.trees.push({
                                name: result.name,
                                treeId: result.treeId
                            });
                            viewModel.treeId(result.treeId);
                            viewModel.treeName(result.name);
                        }
                    }
                }
            );
        }

        viewModel.pageSizeOptions = ko.observableArray([
                                        { text: 10, value: 10 },
                                        { text: 20, value: 20 },
                                        { text: 50, value: 50 },
                                        { text: 100, value: 100 }
                                    ]);

        //viewModel.individualList = new ftp.IndividualListViewModel(viewModel, config);
        //viewModel.individualList.init();

        viewModel.showCloseIcon = ko.computed(function () {
            return true;
        });

        viewModel.closeEdit = function () {
        };

        viewModel.selectAncestorView = function (data, e) {
            menuClick(e.target, settings.familyGroupViewPanel);
            viewModel.mode("ancestorView");
        };

        viewModel.selectFamilyGroupView = function (data, e) {
            menuClick(e.target, settings.familyGroupViewPanel);
            viewModel.mode("familyGroupView");
        };

        viewModel.selectIndividualList = function (data, e) {
            menuClick(e.target, settings.individualListPanel);
            viewModel.mode("individualsList");
        };

        viewModel.selectTreeOverview = function (data, e) {
            menuClick(e.target, settings.familyGroupViewPanel);
            viewModel.mode("treeOverview");
        };

        ko.applyBindings(viewModel, $rootElement[0]);

        getTrees();
    };

    var loadPanel = function(panelTemplate, panelPlaceholder) {
        
    }


    var setUpMenu = function() {
        $(".ftpMenu > ul > li").each(function (evt) {
            evt.preventDefault();
            var $this = $(this);
            var panelTemplate = $this.data('panel-template');

            if (panelTemplate === undefined) return;

            loadPanel(panelTemplate, $(".ftpMenu #ftp-panel"));
        });

    };

    return {
        init: init
    };
};
