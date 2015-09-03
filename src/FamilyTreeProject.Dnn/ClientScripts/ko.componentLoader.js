if (typeof ftp === 'undefined' || ftp === null) {
	ftp = {};
};

ftp.componentLoader = function ($, ko, settings) {

	var basePath = settings.basePath;

	var addCssToHead = function (css) {
		var h = document.getElementsByTagName('head')[0];
		var m = document.createElement('link');
		m.setAttribute('rel', 'stylesheet');
		m.setAttribute('type', 'text/css');
		m.setAttribute('href', css);
		h.appendChild(m);
	};

	var addJsToBody = function (js) {
		var b = document.getElementsByTagName('body')[0];
		var m = document.createElement('script');
		m.setAttribute('src', js);
		b.appendChild(m);
	};

	var configLoader = {
		getConfig: function (name, callback) {
			var jsFile = "/components/" + name + "/" + name + ".js";
			var template = "/components/" + name + "/" + name + ".html";
			var css = "/components/" + name + "/" + name + ".css";

			callback({ viewModel: { jsFile: jsFile, viewModel: jsFile + "ViewModel"}, template: { template: template, css: css } });
		}
	};

	var templateLoader = {
		loadTemplate: function (name, templateConfig, callback) {
			var template = templateConfig.template;
			var css = templateConfig.css;

			if (template) {
				// Uses jQuery's ajax facility to load the markup from a file
				$.get(basePath + template, function (markupString) {
					// We need an array of DOM nodes, not a string.
					// We can use the default loader to convert to the
					// required format.
					ko.components.defaultLoader.loadTemplate(name, markupString, callback);
				});
			} else {
				// Unrecognized config format. Let another loader handle it.
				callback(null);
			}

			//Add components custom css
			if (css) {
			    addCssToHead(basePath + css);
			}
		}
	};

	var viewModelLoader = {
		loadViewModel: function (name, viewModelConfig, callback) {
			var jsFile = viewModelConfig.jsFile;

			if (jsFile) {
				addJsToBody(basePath + jsFile);

				// You could use arbitrary logic, e.g., a third-party
				// code loader, to asynchronously supply the constructor.
				// For this example, just use a hard-coded constructor function.
				var viewModelConstructor = function (params) {
					this.prop1 = 123;
				};

				// We need a createViewModel function, not a plain constructor.
				// We can use the default loader to convert to the
				// required format.
				ko.components.defaultLoader.loadViewModel(name, viewModelConstructor, callback);
			} else {
				// Unrecognized config format. Let another loader handle it.
				callback(null);
			}
		}
	};

	var init = function() {
		ko.components.loaders.unshift(configLoader);
		ko.components.loaders.unshift(templateLoader);
		ko.components.loaders.unshift(viewModelLoader);
	}

	return {
	    init: init
	}
}


