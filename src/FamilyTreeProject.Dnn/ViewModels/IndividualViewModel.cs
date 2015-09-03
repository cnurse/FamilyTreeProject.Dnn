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
        public IndividualViewModel(Individual individual, IFactService factService, int includeAncestors, int includeDescendants, bool includeSpouses)
        {
            var ind = individual.Clone();
            Id = ind.Id;

            //FatherId = individual.FatherId.GetValueOrDefault(0);
            //if (individual.Father != null && includeAncestors > 0)
            //{
            //    Father = new IndividualViewModel(individual.Father, includeAncestors - 1, 0, includeSpouses);
            //}

            //MotherId = individual.MotherId.GetValueOrDefault(0);
            //if (individual.Mother != null && includeAncestors > 0)
            //{
            //    Mother = new IndividualViewModel(individual.Mother, includeAncestors - 1, 0, includeSpouses);
            //}


            ind.Facts = factService.Get(ind.TreeId, f => f.OwnerId == ind.Id && f.OwnerType == EntityType.Individual).ToList();

            Birth = ind.BirthDate;
            Death = ind.DeathDate;
            FirstName = ind.FirstName;
            LastName = ind.LastName;

            //if (includeDescendants > 0)
            //{
            //    Children = new List<IndividualViewModel>();
            //    foreach (var child in individual.Children)
            //    {
            //        Children.Add(new IndividualViewModel(child, 0, includeDescendants - 1, includeSpouses));
            //    }
            //}

            //if (includeSpouses)
            //{
            //    Spouses = new List<IndividualViewModel>();
            //    foreach (var spouse in individual.Spouses)
            //    {
            //        Spouses.Add(new IndividualViewModel(spouse, includeAncestors, 0, false));
            //    }
            //}
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