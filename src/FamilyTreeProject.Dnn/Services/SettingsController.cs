//******************************************
//  Copyright (C) 2014-2015 Charles Nurse  *
//                                         *
//  Licensed under MIT License             *
//  (see included LICENSE)                 *
//                                         *
// *****************************************

using System.Net.Http;
using System.Web.Http;
using DotNetNuke.Security;
using DotNetNuke.Web.Api;
using FamilyTreeProject.Dnn.ViewModels;

namespace FamilyTreeProject.Dnn.Services
{
    [SupportedModules("Ftp.FamilyTreeProject")]
    [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.Edit)]
    public class SettingsController : DnnApiController
    {
        [HttpGet]
        public HttpResponseMessage GetSettings()
        {
            var response = new
                            {
                                results = new SettingsViewModel(ActiveModule)
                            };

            return Request.CreateResponse(response);
        }

        /// <summary>
        /// Saves the module's settings
        /// </summary>
        /// <param name="settings">The settings to save</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public HttpResponseMessage SaveSettings(SettingsViewModel settings)
        {
            settings.Save(ActiveModule);

            var response = new
                            {
                                success = true
                            };

            return Request.CreateResponse(response);
        }
    }
}
