/*
 Copyright (C) 2014-2015 Charles Nurse

 Licensed under MIT License
 (see included LICENSE)

 */

/**
 * Provides an Individual Row component for the Family Tree Project
 *
 * @module components/individualRow/individualRow
 * @requires knockout, config, text!./individualRow.html
 */
define("components/individualRow/individualRow",
        ["jquery", "knockout", "config", "individualViewModel", "text!./individualRow.html"],
        function ($, ko, config, individualViewModel, htmlString) {

            // Return component definition
    return { viewModel: individualViewModel, template: htmlString };
});
