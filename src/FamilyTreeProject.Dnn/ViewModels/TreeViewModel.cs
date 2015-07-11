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
    public class TreeViewModel
    {
        public TreeViewModel(Tree tree)
        {
            Name = tree.Name;
            TreeId = tree.TreeId;
        }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("treeId")]
        public int TreeId { get; set; }
    }
}