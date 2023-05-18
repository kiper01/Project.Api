using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Project.Core.Models.Dto.Programs
{
    public class DtoGetProgramResponse
    {
        [JsonProperty(PropertyName = "id")]
        public Guid Id { get; set; }

        [JsonProperty(PropertyName = "UserId")]
        public Guid UserId { get; set; }

        [JsonProperty(PropertyName = "ProgramName")]
        public string ProgramName { get; set; }

        [JsonProperty(PropertyName = "TimeStart")]
        public DateTime TimeStart { get; set; }

        [JsonProperty(PropertyName = "TimeEnd")]
        public DateTime TimeEnd { get; set; }

        [JsonProperty(PropertyName = "TotalTime")]
        public DateTime TotalTime { get; set; }

        [JsonProperty(PropertyName = "Date")]
        public DateTime Date { get; set; }
    }
}
