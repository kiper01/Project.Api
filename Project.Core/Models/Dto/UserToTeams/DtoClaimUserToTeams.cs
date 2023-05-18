using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Project.Core.Models.Dto.UserToTeams
{
    public class DtoClaimUserToTeams
    {
        [JsonProperty(PropertyName = "id")]
        public Guid Id { get; set; }

        [JsonProperty(PropertyName = "UserId")]
        public Guid UserId { get; set; }

        [JsonProperty(PropertyName = "TeamsId")]
        public Guid TeamsId { get; set; }

        [JsonProperty(PropertyName = "status")]
        public Boolean status { get; set; }
    }
}
