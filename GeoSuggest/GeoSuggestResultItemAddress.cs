using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Yandex.API
{
    public class GeoSuggestResultItemAddress
    {
        [JsonProperty("formatted_address")]
        public string FormattedAddress { get; set; }

        public GeoSuggestResultItemAddressComponent[] Component { get; set; }
    }

    public class GeoSuggestResultItemAddressComponent
    {
        public string Name { get; set; }

        public string[] Kind { get; set; }
    }
}
