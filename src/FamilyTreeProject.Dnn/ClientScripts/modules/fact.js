/*
 Copyright (C) 2014-2015 Charles Nurse

 Licensed under MIT License
 (see included LICENSE)

 */

/**
 * Provides a Fact module for the Family Tree Project
 *
 * @module components/individualList/individualList
 * @requires knockout, config, text!./individualList.html
 */
define("fact", ["knockout"], function (ko) {

    // ReSharper disable once InconsistentNaming
    var Fact = function (data) {
        var self = this;

        if (data === undefined || data === null) {
            self.date = ko.observable("");
            self.factType = ko.observable("");
            self.place = ko.observableArray("");
        } else {
            self.date = ko.observable(data.date);
            self.factType = ko.observable(data.factType);
            self.place = ko.observable(data.place);
        }
    }

    // Return component definition
    return function(parentViewModel, data) {
        return new Fact(parentViewModel, data);
    }
});