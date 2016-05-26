using System;
using Newtonsoft.Json;

namespace PhotoStorm.Core.Portable.Common.Models
{
    [JsonObject("work")]
    public class BaseWorkModel
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }
    }
}
