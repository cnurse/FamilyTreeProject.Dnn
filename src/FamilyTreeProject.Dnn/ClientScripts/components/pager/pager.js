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
define("components/pager/pager", ["knockout", "config", "text!./pager.html"], function(ko, config, htmlString) {

    function pagerViewModel(params) {
        var self = this;
        self.resx = config.resx;

        self.totalResults = params.totalResults;
        self.pageIndex = params.pageIndex;
        self.pageSize = params.pageSize;
        self.refresh = params.refresh;

        self.pageSizeOptions = ko.observableArray([
            { text: 10, value: 10 },
            { text: 20, value: 20 },
            { text: 50, value: 50 },
            { text: 100, value: 100 }
        ]);
        self.pager_PageDesc = self.resx.pager_PageDesc;
        self.pager_PagerFormat = self.resx.individuals_PagerFormat;
        self.pager_NoPagerFormat = self.resx.individuals_NoPagerFormat;

        self.startIndex = ko.computed(function () {
            return self.pageIndex() * self.pageSize() + 1;
        });

        self.endIndex = ko.computed(function () {
            return Math.min((self.pageIndex() + 1) * self.pageSize(), self.totalResults());
        });

        self.currentPage = ko.computed(function () {
            return self.pageIndex() + 1;
        });

        self.totalPages = ko.computed(function () {
            if (typeof self.totalResults === 'function' && self.totalResults())
                return Math.ceil(self.totalResults() / self.pageSize());
            return 1;
        });

        self.pagerVisible = ko.computed(function () {
            return self.totalPages() > 1;
        });

        self.pagerItemsDescription = ko.computed(function () {
            var pagerFormat = self.pager_PagerFormat;
            var nopagerFormat = self.pager_NoPagerFormat;
            if (self.pagerVisible())
                return pagerFormat.replace('{0}', self.startIndex()).replace('{1}', self.endIndex()).replace('{2}', self.totalResults());
            return nopagerFormat.replace('{0}', self.totalResults());
        });

        self.pagerDescription = ko.computed(function () {
            var pagerFormat = self.pager_PageDesc;
            if (self.pagerVisible())
                return pagerFormat.replace('{0}', self.currentPage()).replace('{1}', self.totalPages());
            return '';
        });

        self.pagerPrevClass = ko.computed(function () {
            return 'prev' + (self.pageIndex() < 1 ? ' disabled' : '');
        });

        self.pagerNextClass = ko.computed(function () {
            return 'next' + (self.pageIndex() >= self.totalPages() - 1 ? ' disabled' : '');
        });

        self.prev = function () {
            if (self.pageIndex() <= 0) return;
            self.pageIndex(self.pageIndex() - 1);
            self.refresh();
        };

        self.next = function () {
            if (self.pageIndex() >= self.totalPages() - 1) return;
            self.pageIndex(self.pageIndex() + 1);
            self.refresh();
        };
    }

    // Return component definition
    return { viewModel: pagerViewModel, template: htmlString };
});
