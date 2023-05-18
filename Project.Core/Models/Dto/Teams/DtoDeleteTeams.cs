using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Project.Core.Models.Dto.Teams
{
    public class DtoDeleteTeams
    {
        [JsonProperty(PropertyName = "id")]
        public Guid Id { get; set; }

        [JsonProperty(PropertyName = "TeamsName")]
        public string TeamsName { get; set; }

        [JsonProperty(PropertyName = "Passwd")]
        public string Passwd { get; set; }

    }
}
