using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Project.Core.Models.Dto
{
    public class DtoItem
    {
        [JsonProperty(PropertyName = "id")]
        public Guid Id { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
    }
}