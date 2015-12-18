/*
 Copyright (C) 2014-2015 Charles Nurse

 Licensed under MIT License
 (see included LICENSE)

 */

/**
 * Provides an Individual Detail component for the Family Tree Project
 *
 * @module components/individualDetail/individualDetail
 * @requires knockout, config, text!./individualDetail.html
 */
define("components/individualDetail/individualDetail",
        ["jquery", "knockout", "config", "individualViewModel", "text!./individualDetail.html"],
        function ($, ko, config, individualViewModel,htmlString) {

    // Return component definition
    return { viewModel: individualViewModel, template: htmlString };
});
