/*
 Copyright (C) 2014-2015 Charles Nurse

 Licensed under MIT License
 (see included LICENSE)

 */

/**
 * Provides a Ancestors View component for the Family Tree Project
 *
 * @module components/ancestorsView/ancestorsView
 * @requires knockout, config, text!./ancestorsView.html
 */
define("components/ancestorsView/ancestorsView", ["knockout", "config", "text!./ancestorsView.html"], function(ko, config, htmlString) {

    function AncestorsViewViewModel(params, componentInfo) {
        var self = this;
        self.resx = config.resx;

    }

    // Return component definition
    return { viewModel: AncestorsViewViewModel, template: htmlString };
});
