using System;
using System.Collections.Generic;
using System.Text;

namespace Yandex.API
{
    public class GeoSuggestResultItem
    {
        public GeoSuggestResultItemTitle Title { get; set; }

        public GeoSuggestResultItemTitle SubTitle { get; set; }

        public string[] Tags { get; set; }

        public GeoSuggestResultItemDistance Distance { get; set; }

        public GeoSuggestResultItemAddress Address { get; set; }

        public string Uri { get; set; }
    }
}
