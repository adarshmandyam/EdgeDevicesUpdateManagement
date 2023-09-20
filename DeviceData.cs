using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdgeHubTriggerFunctions
{
    public class DeviceData
    {
        [JsonProperty("deviceId")]
        public string DeviceId { get; set; }        

        [JsonProperty("temp")]
        public double TruckTemperature { get; set; }

        [JsonProperty("lat")]
        public double TruckLatitude { get; set; }

        [JsonProperty("lngt")]
        public double TruckLongitude { get; set; }

        [JsonProperty("eventEnqueuedUtcTime")]
        public DateTime EnqueueTimeUtc { get; set; }        
    }
}
