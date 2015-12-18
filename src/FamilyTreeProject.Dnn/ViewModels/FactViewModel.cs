//******************************************
//  Copyright (C) 2014-2015 Charles Nurse  *
//                                         *
//  Licensed under MIT License             *
//  (see included LICENSE)                 *
//                                         *
// *****************************************

using FamilyTreeProject.Common;
using Newtonsoft.Json;

namespace FamilyTreeProject.Dnn.ViewModels
{
    public class FactViewModel
    {
        public FactViewModel(Fact fact)
        {
            Date = fact.Date;
            FactType = fact.FactType.ToString();
            Place = fact.Place;
        }

        /// <summary>
        /// The date of the fact (if the fact is an event)
        /// </summary>
        [JsonProperty("date")]
        public string Date { get; set; }

        /// <summary>
        /// The type of the Fact
        /// </summary>
        [JsonProperty("factType")]
        public string FactType { get; set; }

        /// <summary>
        /// The place of the fact (if the fact is an event)
        /// </summary>
        [JsonProperty("place")]
        public string Place { get; set; }
    }
}