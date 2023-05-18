using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Project.Core.Models.Dto.User
{
    public class DtoClaimUser
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; set; }

        [JsonProperty(PropertyName = "Login")]
        public string Login { get; set; }

        [JsonProperty(PropertyName = "PasswordHash")]
        public string PasswordHash { get; set; }

        [JsonProperty(PropertyName = "Deleted")]
        public Boolean Deleted { get; set; }

    }
}
