﻿using Newtonsoft.Json;

namespace Project.Core.Models.Request
{
    public class InboundRequest : InboundRequest<EmptyRequestData>
    {

    }
    public class InboundRequest<T>
        where T : class
    {
        [JsonProperty(PropertyName = "data", DefaultValueHandling = DefaultValueHandling.Include)]
        public T Data { get; set; }

    }
}
