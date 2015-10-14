//******************************************
//  Copyright (C) 2014-2015 Charles Nurse  *
//                                         *
//  Licensed under MIT License             *
//  (see included LICENSE)                 *
//                                         *
// *****************************************

using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using FamilyTreeProject.Common;
using FamilyTreeProject.DomainServices;
using Newtonsoft.Json;

namespace FamilyTreeProject.Dnn.ViewModels
{
    public class IndividualViewModel
    {
        public IndividualViewModel(Individual individual)
        {
            Id = individual.Id;

            Birth = individual.BirthDate;
            Death = individual.DeathDate;
            FirstName = individual.FirstName;
            LastName = individual.LastName;
        }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("birth")]
        public string Birth { get; set; }

        [JsonProperty("death")]
        public string Death { get; set; }

        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [JsonProperty("father")]
        public IndividualViewModel Father { get; set; }

        [JsonProperty("mother")]
        public IndividualViewModel Mother { get; set; }

        [JsonProperty("fatherId")]
        public int FatherId { get; set; }

        [JsonProperty("motherId")]
        public int MotherId { get; set; }

        [JsonProperty("children")]
        public List<IndividualViewModel> Children { get; set; }

        [JsonProperty("spouses")]
        public List<IndividualViewModel> Spouses { get; set; }
    }
}