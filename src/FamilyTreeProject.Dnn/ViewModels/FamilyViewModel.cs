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
            Children = new List<IndividualViewModel>();
        }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("children")]
        public List<IndividualViewModel> Children { get; set; }

        [JsonProperty("spouse")]
        public IndividualViewModel Spouse { get; set; }
    }
}