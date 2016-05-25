using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PhotoStorm.Core.Portable.Works.Enums;

namespace PhotoStorm.WebApi.Models
{
    [JsonObject("work")]
    public class CreateWorkModel
    {
        [JsonProperty("device")]
        [JsonConverter(typeof(StringEnumConverter))]
        public WorkCreatorDevice Device { get; set; }

        [JsonProperty("latitude")]
        public double Latitude { get; set; }

        [JsonProperty("longitude")]
        public double Longitude { get; set; }

        [JsonProperty("radius")]
        public int Radius { get; set; }

        [JsonIgnore]
        public bool IsValid => Device != WorkCreatorDevice.Unknown;
    }
}
