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
define("individual", ["knockout"], function (ko) {

    // ReSharper disable once InconsistentNaming
    var Individual = function (data, sex) {
        var self = this;

        if (data === undefined || data === null) {
            self.individualId = ko.observable(-1);
            self.lastName = ko.observable("");
            self.firstName = ko.observable("");
            self.name = ko.observable("");
            self.birth = ko.observable("");
            self.death = ko.observable("");
            self.imageId = ko.observable(-1);
            self.imageUrl = ko.observable("");
            self.sex = ko.observable(sex);
        } else {
            self.individualId = ko.observable(data.id);
            self.lastName = ko.observable(data.lastName);
            self.firstName = ko.observable(data.firstName);
            self.name = ko.observable(data.lastName + ", " + data.firstName);
            self.birth = ko.observable(data.birth);
            self.death = ko.observable(data.death);
            self.imageId = ko.observable(data.imageId);
            self.imageUrl = ko.observable(data.imageUrl);
            self.sex = ko.observable(data.sex);
        }
    }

    // Return component definition
    return function(parentViewModel, data) {
        return new Individual(parentViewModel, data);
    }
});