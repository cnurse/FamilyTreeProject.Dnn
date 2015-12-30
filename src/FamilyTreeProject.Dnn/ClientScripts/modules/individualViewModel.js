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
define("individualViewModel", ["knockout", "config", "util"], function (ko, config, util) {

    // ReSharper disable once InconsistentNaming
    var IndividualViewModel = function (props) {
        var self = this;
        var defaultOptions = {
            nameStyle: "lastNameFirstName"
        }
        var options = $.extend({}, defaultOptions, props.options);

        var refresh = props.refresh;

        self.resx = config.resx;

        self.individual = props.individual;

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
            return (self.individual().sex() === "Male") ? self.resx.addFather : self.resx.addMother;
        });

        self.birth = ko.pureComputed(function () {
            var birth = self.individual().birth();
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
            var death = self.individual().death();
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
                return self.individual().imageId();
            },
            write: function(value) {
                self.individual().imageId(value);
            }
        });

        self.imageUrl = ko.pureComputed(function () {
            var imageUrl = self.individual().imageUrl();
            if (imageUrl === "") {
                imageUrl = (self.individual().sex() === "Male") ? config.settings.maleIcon : config.settings.femaleIcon;
            }
            return imageUrl;
        });

        self.isNull = ko.pureComputed(function() {
            return self.individual().individualId() === -1;
        });

        self.lived = ko.pureComputed(function () {
            return right(self.birth(), 4) + " - " + right(self.death(), 4);
        });

        self.name = ko.pureComputed(function () {
            var result;
            if (options.nameStyle === "lastNameFirstName") {
                result = self.individual().lastName() + ", " + self.individual().firstName();
            } else {
                result = self.individual().firstName() + " " + self.individual().lastName();
            }
            return result;
        });

        self.sex = ko.pureComputed(function (){
            return self.individual().sex();
        });

        self.deleteIndividual = function () {
            util.confirm(self.resx.deleteIndividualConfirmMessage, self.resx.yes, self.resx.no, function () {
                var params = {
                    id: self.individual().individualId(),
                    treeId: self.individual().treeId(),
                    lastName: self.individual().lastName(),
                    firstName: self.individual().firstName()
                };

                config.individualService().post("DeleteIndividual",
                    params,
                    function() {
                        props.onDeleted();
                    }
                );
            });
        };

        self.onFileUploaded = function () {
            var params = {
                id: self.individual().individualId(),
                treeId: self.individual().treeId(),
                lastName: self.individual().lastName(),
                firstName: self.individual().firstName(),
                imageId: self.individual().imageId()
            };

            config.individualService().post("SaveIndividual",
                params,
                function () {
                    refresh();
                }
            );
        }

        self.selectIndividual = function () {
            props.onSelected(self.individual());
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