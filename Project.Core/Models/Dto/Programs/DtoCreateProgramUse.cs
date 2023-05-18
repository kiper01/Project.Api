using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Project.Core.Models.Dto.Programs
{
    public class DtoCreateProgramUse
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; set; }

        [JsonProperty(PropertyName = "UserID")]
        public Guid UserId { get; set; }

        [JsonProperty(PropertyName = "ProgramName")]
        public string ProgramName { get; set; }

        [JsonProperty(PropertyName = "TimeStart")]
        public DateTime TimeStart { get; set; }

        [JsonProperty(PropertyName = "TimeEnd")]
        public DateTime TimeEnd { get; set; }
    }
}
