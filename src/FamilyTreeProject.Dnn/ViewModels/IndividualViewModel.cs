//******************************************
//  Copyright (C) 2014-2015 Charles Nurse  *
//                                         *
//  Licensed under MIT License             *
//  (see included LICENSE)                 *
//                                         *
// *****************************************

using System.Collections.Generic;
using System.Linq;
using FamilyTreeProject.Common;
using Newtonsoft.Json;

namespace FamilyTreeProject.Dnn.ViewModels
{
    public class IndividualViewModel
    {
        public IndividualViewModel()
        {
            Id = -1;
        }

        public IndividualViewModel(Individual individual)
        {
            Id = individual.Id;
            TreeId = individual.TreeId;

            FirstName = individual.FirstName;
            LastName = individual.LastName;
            FatherId = individual.FatherId ?? -1;
            MotherId = individual.MotherId ?? -1;

            ImageId = individual.ImageId;

            Sex = individual.Sex.ToString();

            Birth = GetBirth(individual);
            Death = GetDeath(individual);

        }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("birth")]
        public FactViewModel Birth { get; set; }

        [JsonProperty("death")]
        public FactViewModel Death { get; set; }

        [JsonProperty("facts")]
        public List<FactViewModel> Facts { get; set; }

        [JsonProperty("families")]
        public List<FamilyViewModel> Families { get; set; }

        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("imageId")]
        public int ImageId { get; set; }

        [JsonProperty("imageUrl")]
        public string ImageUrl { get; set; }

        [JsonProperty("father")]
        public IndividualViewModel Father { get; set; }

        [JsonProperty("fatherId")]
        public int FatherId { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [JsonProperty("mother")]
        public IndividualViewModel Mother { get; set; }

        [JsonProperty("motherId")]
        public int MotherId { get; set; }

        [JsonProperty("sex")]
        public string Sex { get; set; }

        [JsonProperty("treeId")]
        public int TreeId { get; set; }

        private FactViewModel GetBirth(Individual ind)
        {
            FactViewModel fact = null;
            var birth = (from Fact e in ind.Facts
                             where e.FactType == FactType.Birth
                             select e).FirstOrDefault();
            if (birth == null)
            {
                var baptism = (from Fact e in ind.Facts
                                where e.FactType == FactType.Baptism
                                select e).FirstOrDefault();
                if (baptism != null)
                {
                    fact = new FactViewModel(baptism);
                }
            }
            else
            {
                fact = new FactViewModel(birth);
            }

            return fact;
        }

        private FactViewModel GetDeath(Individual ind)
        {
            FactViewModel fact = null;
            var death = (from Fact e in ind.Facts
                             where e.FactType == FactType.Death
                             select e).FirstOrDefault();
            if (death == null)
            {
                var burial = (from Fact e in ind.Facts
                                  where e.FactType == FactType.Burial
                                  select e).FirstOrDefault();
                if (burial != null)
                {
                    fact = new FactViewModel(burial);
                }
            }
            else
            {
                fact = new FactViewModel(death);
            }

            return fact;
        }
    }
}