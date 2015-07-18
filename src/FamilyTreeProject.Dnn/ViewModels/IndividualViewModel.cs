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
    public class IndividualViewModel
    {
        public IndividualViewModel(Individual individual, int includeAncestors, int includeDescendants, bool includeSpouses)
        {
            Id = individual.Id;

            FatherId = individual.FatherId.GetValueOrDefault(0);
            if (individual.Father != null && includeAncestors > 0)
            {
                Father = new IndividualViewModel(individual.Father, includeAncestors - 1, 0, includeSpouses);
            }

            MotherId = individual.MotherId.GetValueOrDefault(0);
            if (individual.Mother != null && includeAncestors > 0)
            {
                Mother = new IndividualViewModel(individual.Mother, includeAncestors - 1, 0, includeSpouses);
            }

            FirstName = individual.FirstName;
            LastName = individual.LastName;

            if (includeDescendants > 0)
            {
                Children = new List<IndividualViewModel>();
                foreach (var child in individual.Children)
                {
                    Children.Add(new IndividualViewModel(child, 0, includeDescendants - 1, includeSpouses));
                }
            }

            if (includeSpouses)
            {
                Spouses = new List<IndividualViewModel>();
                foreach (var spouse in individual.Spouses)
                {
                    Spouses.Add(new IndividualViewModel(spouse, includeAncestors, 0, false));
                }
            }
        }

        [JsonProperty("id")]
        public int Id { get; set; }

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