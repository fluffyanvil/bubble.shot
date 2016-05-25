using System;
using Newtonsoft.Json;

namespace PhotoStorm.WebApi.Models
{
    [JsonObject("work")]
    public class BaseWorkModel
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }
    }
}
