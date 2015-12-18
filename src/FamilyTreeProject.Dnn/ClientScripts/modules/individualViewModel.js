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
define("individualViewModel", ["knockout", "config"], function (ko, config) {

    // ReSharper disable once InconsistentNaming
    var IndividualViewModel = function (params) {
        var self = this;
        var defaultOptions = {
            nameStyle: "lastNameFirstName"
        }
        var options = $.extend({}, defaultOptions, params.options);

        var individual = params.individual;
        var refresh = params.refresh;
        var treeId = params.treeId;

        self.resx = config.resx;

        self.selected = ko.observable(false);
        self.canEdit = ko.observable(false);
        self.showUpload = ko.observable(false);
        self.showUploadButton = ko.observable(false);

        var right = function(str, n) {
            if (n <= 0)
                return "";
            else if (n > str.length)
                return str;
            else {
                var iLen = str.length;
                return str.substring(iLen, iLen - n);
            }
        };

        self.addParent = ko.pureComputed(function() {
            return (individual().sex() === "Male") ? self.resx.addFather : self.resx.addMother;
        });

        self.birth = ko.pureComputed(function () {
            var birth = individual().birth();
            var result = "";
            if (birth !== undefined && birth !== null) {
                if (birth.factType === "Birth") {
                    result = self.resx.birthFormat.replace("{0}", birth.date);
                } else {
                    result = self.resx.baptismFormat.replace("{0}", birth.date);
                }
            }
            return result;
        });

        self.death = ko.pureComputed(function () {
            var death = individual().death();
            var result = "";
            if (death !== undefined && death !== null) {
                if (death.factType === "Death") {
                    result = self.resx.deathFormat.replace("{0}", death.date);
                } else {
                    result = self.resx.buriedFormat.replace("{0}", death.date);
                }
            }
            return result;
        });

        self.disableButton = function () {
            self.showUploadButton(false);
        };

        self.enableButton = function () {
            self.showUploadButton(true);
        };

        self.imageId = ko.pureComputed({
            read: function () {
                return individual().imageId();
            },
            write: function(value) {
                individual().imageId(value);
            }
        });

        self.imageUrl = ko.pureComputed(function () {
            var imageUrl = individual().imageUrl();
            if (imageUrl === "") {
                imageUrl = (individual().sex() === "Male") ? config.settings.maleIcon : config.settings.femaleIcon;
            }
            return imageUrl;
        });

        self.isNull = ko.pureComputed(function() {
            return individual().individualId() === -1;
        });

        self.lived = ko.pureComputed(function () {
            return right(self.birth(), 4) + " - " + right(self.death(), 4);
        });

        self.name = ko.pureComputed(function () {
            var result;
            if (options.nameStyle === "lastNameFirstName") {
                result = individual().lastName() + ", " + individual().firstName();
            } else {
                result = individual().firstName() + " " + individual().lastName();
            }
            return result;
        });

        self.editIndividual = function () {
        }

        self.deleteIndividual = function () {
        }

        self.onFileUploaded = function () {
            var params = {
                id: individual().individualId(),
                treeId: treeId(),
                lastName: individual().lastName(),
                firstName: individual().firstName(),
                imageId: individual().imageId()
            };

            config.individualService().post("SaveIndividual",
                params,
                function () {
                    refresh();
                }
            );
        }

        self.selectIndividual = function () {
            params.onSelected(individual());
        }

        self.toggleSelected = function () {
            self.selected(!self.selected());
        }; 

        self.uploadFile = function () {
            self.showUpload(true);
        };
    }

    // Return component definition
    return function(params) {
        return new IndividualViewModel(params);
    };
});