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
        public TreeViewModel()
        {
            TreeId = -1;
        }

        public TreeViewModel(Tree tree)
        {
            Description = tree.Description;
            Name = tree.Name;
            Title = tree.Title;
            TreeId = tree.TreeId;
        }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("factCount")]
        public int FactCount { get; set; }

        [JsonProperty("familyCount")]
        public int FamilyCount { get; set; }

        [JsonProperty("homeIndividual")]
        public IndividualViewModel HomeIndividual { get; set; }

        [JsonProperty("imageId")]
        public int ImageId { get; set; }

        [JsonProperty("imageUrl")]
        public string ImageUrl { get; set; }

        [JsonProperty("individualCount")]
        public int IndividualCount { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("lastViewedIndividual")]
        public IndividualViewModel LastViewedIndividual { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("treeId")]
        public int TreeId { get; set; }
    }
}