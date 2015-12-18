//******************************************
//  Copyright (C) 2014-2015 Charles Nurse  *
//                                         *
//  Licensed under MIT License             *
//  (see included LICENSE)                 *
//                                         *
// *****************************************

using DotNetNuke.Collections;
using DotNetNuke.Entities.Modules;
using Newtonsoft.Json;

namespace FamilyTreeProject.Dnn.ViewModels
{
    public class SettingsViewModel
    {
        private const string OwnerKey = "FTP_Owner";

        public SettingsViewModel()
        {
        }

        public SettingsViewModel(ModuleInfo module)
        {
            Owner = module.ModuleSettings.GetValueOrDefault(OwnerKey, "user");
        }

        [JsonProperty("owner")]
        public string Owner { get; set; }

        public void Save(ModuleInfo module)
        {
            ModuleController.Instance.UpdateModuleSetting(module.ModuleID, OwnerKey, Owner.ToLowerInvariant());
        }
    }
}
