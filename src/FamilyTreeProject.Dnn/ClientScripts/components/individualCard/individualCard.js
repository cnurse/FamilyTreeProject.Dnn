/*
 Copyright (C) 2014-2015 Charles Nurse

 Licensed under MIT License
 (see included LICENSE)

 */

/**
 * Provides an Individual Card component for the Family Tree Project
 *
 * @module components/individualCard/individualCard
 * @requires knockout, config, text!./individualCard.html
 */
define("components/individualCard/individualCard",
        ["jquery", "knockout", "config", "individualViewModel", "text!./individualCard.html"],
        function ($, ko, config, individualViewModel,htmlString) {

    // Return component definition
    return { viewModel: individualViewModel, template: htmlString };
});
