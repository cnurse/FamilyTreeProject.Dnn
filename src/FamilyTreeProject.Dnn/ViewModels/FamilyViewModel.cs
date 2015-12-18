//******************************************
//  Copyright (C) 2014-2015 Charles Nurse  *
//                                         *
//  Licensed under MIT License             *
//  (see included LICENSE)                 *
//                                         *
// *****************************************

using System.Collections.Generic;
using Newtonsoft.Json;

namespace FamilyTreeProject.Dnn.ViewModels
{
    public class FamilyViewModel
    {
        public FamilyViewModel(Family family)
        {
            Id = family.Id;
            HusbandId = family.HusbandId.GetValueOrDefault(0);
            WifeId = family.WifeId.GetValueOrDefault(0);
            Children = new List<IndividualViewModel>();
        }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("children")]
        public List<IndividualViewModel> Children { get; set; }

        [JsonProperty("husband")]
        public IndividualViewModel Husband { get; set; }

        [JsonProperty("husbandId")]
        public int HusbandId { get; set; }

        [JsonProperty("wife")]
        public IndividualViewModel Wife { get; set; }

        [JsonProperty("wifeId")]
        public int WifeId { get; set; }
    }
}