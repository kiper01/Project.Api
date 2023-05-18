using Newtonsoft.Json;
using System;

namespace Project.Core.Abstractions
{
    public class ModelBase
    {
        [JsonProperty(PropertyName = "id")]
        public Guid Id { get; set; } = Guid.NewGuid();
    }
}
