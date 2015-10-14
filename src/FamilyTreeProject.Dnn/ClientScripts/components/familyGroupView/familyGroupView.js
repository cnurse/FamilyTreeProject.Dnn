/*
 Copyright (C) 2014-2015 Charles Nurse

 Licensed under MIT License
 (see included LICENSE)

 */

/**
 * Provides a Family Group View component for the Family Tree Project
 *
 * @module components/familyGroupView/familyGroupView
 * @requires knockout, config, text!./familyGroupView.html
 */
define("components/familyGroupView/familyGroupView", ["knockout", "config", "text!./familyGroupView.html"], function(ko, config, htmlString) {

    function FamilyGroupViewViewModel(params, componentInfo) {
        var self = this;
        self.resx = config.resx;

    }

    // Return component definition
    return { viewModel: FamilyGroupViewViewModel, template: htmlString };
});
