//******************************************
//  Copyright (C) 2014-2015 Charles Nurse  *
//                                         *
//  Licensed under MIT License             *
//  (see included LICENSE)                 *
//                                         *
// *****************************************

using Newtonsoft.Json;

namespace FamilyTreeProject.Dnn.ViewModels
{
    public class SettingsViewModel
    {
        [JsonProperty("owner")]
        public string Owner { get; set; }
    }
}
