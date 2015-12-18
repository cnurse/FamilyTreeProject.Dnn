/*
 Copyright (C) 2014-2015 Charles Nurse

 Licensed under MIT License
 (see included LICENSE)

 */

/**
 * Provides an Individual Link component for the Family Tree Project
 *
 * @module components/individualLink/individualLink
 * @requires knockout, config, text!./individualLink.html
 */
define("components/individualLink/individualLink",
        ["jquery", "knockout", "config", "individualViewModel", "text!./individualLink.html"],
        function ($, ko, config, individualViewModel, htmlString) {

    // Return component definition
    return { viewModel: individualViewModel, template: htmlString };
});
