using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Yandex.API
{
    public class GeoSuggestResultItemTitle
    {
        /// <summary>
        /// Object name.
        /// </summary>
        public string Text { get; set; }

        [JsonProperty("hl")]
        public GeoSuggestResultHighlight[] Highlights { get; set; }
    }
}
