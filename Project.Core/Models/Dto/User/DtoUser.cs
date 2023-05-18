using Newtonsoft.Json;
using System;

namespace Project.Core.Models.Dto
{
    public class DtoUser
    {
        [JsonProperty(PropertyName = "id")]
        public Guid Id { get; set; }

        [JsonProperty(PropertyName = "login")]
        public string Login { get; set; }

        [JsonProperty(PropertyName = "deleted")]
        public Boolean Deleted { get; set; }


    }

}
