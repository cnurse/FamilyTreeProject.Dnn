//******************************************
//  Copyright (C) 2014-2015 Charles Nurse  *
//                                         *
//  Licensed under MIT License             *
//  (see included LICENSE)                 *
//                                         *
// *****************************************

using System.Web.UI.WebControls;
using Newtonsoft.Json;

namespace FamilyTreeProject.Dnn.ViewModels
{
    public class FileUploadViewModel
    {
        [JsonProperty("path")]
        public string Path { get; set; }

        [JsonProperty("orientation")]
        public Orientation Orientation { get; set; }

        [JsonProperty("alreadyExists")]
        public bool AlreadyExists { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("fileIconUrl")]
        public string FileIconUrl { get; set; }

        [JsonProperty("fileId")]
        public int FileId { get; set; }

        [JsonProperty("fileName")]
        public string FileName { get; set; }

        [JsonProperty("prompt")]
        public string Prompt { get; set; }

        [JsonProperty("treeId")]
        public int TreeId { get; set; }
    }
}
