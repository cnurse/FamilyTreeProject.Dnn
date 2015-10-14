/*
 Copyright (C) 2014-2015 Charles Nurse

 Licensed under MIT License
 (see included LICENSE)

 */

/**
 * Provides a Pager component for the Family Tree Project
 *
 * @module components/pager/pager
 * @requires knockout, config, text!./pager.html
 */
define("components/pager/pager", ["knockout", "config", "text!./pager.html", "js!dnnPager"], function(ko, config, htmlString) {

    function PagerViewModel(params, componentInfo) {
        var self = this;
        var settings = config.settings;

        self.resx = config.resx;

        self.totalResults = params.totalResults;
        self.pageSize = params.pageSize;
        self.pageSizeOptions = ko.observableArray([
            { text: 10, value: 10 },
            { text: 20, value: 20 },
            { text: 50, value: 50 },
            { text: 100, value: 100 }
        ]);
        self.pager_PageDesc = self.resx.pager_PageDesc;
        self.pager_PagerFormat = self.resx.individuals_PagerFormat;
        self.pager_NoPagerFormat = self.resx.individuals_NoPagerFormat;

        dnn.koPager().init(self, { ko: ko});
    }

    // Return component definition
    return { viewModel: PagerViewModel, template: htmlString };
});
