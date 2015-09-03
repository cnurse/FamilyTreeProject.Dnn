//******************************************
//  Copyright (C) 2014-2015 Charles Nurse  *
//                                         *
//  Licensed under MIT License             *
//  (see included LICENSE)                 *
//                                         *
// *****************************************

using System.Net.Http;
using System.Web.Http;
using DotNetNuke.Collections;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Security;
using DotNetNuke.Web.Api;
using FamilyTreeProject.Dnn.ViewModels;

namespace FamilyTreeProject.Dnn.Services
{
    [SupportedModules("Ftp.FamilyTreeProject")]
    [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.Edit)]
    public class SettingsController : DnnApiController
    {
        public const string OwnerKey = "FTP_Owner";

        [HttpGet]
        public HttpResponseMessage GetSettings()
        {
            var viewModel = new SettingsViewModel
                                    {
                                        Owner = ActiveModule.ModuleSettings.GetValueOrDefault(OwnerKey, "user")
                                    };

            var response = new
            {
                success = true,
                data = new
                        {
                            results = viewModel
                        }
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
            ModuleController.Instance.UpdateModuleSetting(ActiveModule.ModuleID, OwnerKey, settings.Owner.ToLowerInvariant());

            var response = new
                            {
                                success = true
                            };

            return Request.CreateResponse(response);
        }
    }
}
