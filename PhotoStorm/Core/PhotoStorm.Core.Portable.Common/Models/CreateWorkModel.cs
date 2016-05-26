using Newtonsoft.Json;

namespace PhotoStorm.Core.Portable.Common.Models
{
    [JsonObject("work")]
    public class CreateWorkModel
    {
        [JsonProperty("latitude")]
        public double Latitude { get; set; }

        [JsonProperty("longitude")]
        public double Longitude { get; set; }

        [JsonProperty("radius")]
        public int Radius { get; set; }

        [JsonIgnore]
        public bool IsValid => Radius > 1000;
    }
}
