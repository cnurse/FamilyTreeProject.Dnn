/*
 Copyright (C) 2014-2015 Charles Nurse

 Licensed under MIT License
 (see included LICENSE)

 */

/**
 * Provides a Ancestors View component for the Family Tree Project
 *
 * @module components/individualList/individualList
 * @requires knockout, config, text!./individualList.html
 */
define("family", ["knockout", "individual"], function (ko, individualFactory) {

    // ReSharper disable once InconsistentNaming
    var Family = function (data, sex) {
        var self = this;

        var spouseSex = (sex === "Male") ? "Female" : "Male";

        if (data === undefined || data === null) {
            self.familyId = ko.observable(-1);
            self.spouse = ko.observable(individualFactory(null, spouseSex));
            self.children = ko.observableArray([]);
        } else {
            self.familyId = ko.observable(data.id);
            self.spouse = ko.observable(individualFactory(data.spouse, spouseSex));
            self.children = ko.observableArray([]);
            var children = data.children;
            for (var i = 0; i < children.length; i++) {
                self.children.push(ko.observable(individualFactory(children[i])));
            }
        }
    }

    // Return component definition
    return function(parentViewModel, data) {
        return new Family(parentViewModel, data);
    }
});