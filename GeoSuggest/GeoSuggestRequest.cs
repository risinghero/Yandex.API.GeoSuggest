using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.Common;
using System.Globalization;
using System.Linq;
using System.Text;

namespace GeoSuggest
{
    /// <summary>
    /// Geo suggest request for Yandex Maps API
    /// </summary>
    /// <see cref="https://yandex.ru/dev/geosuggest/doc/en/request"/>
    public class GeoSuggestRequest
    {
        public GeoSuggestRequest(string text)
        {
            Text = text;
        }

        /// <summary>
        /// User input (prefix)
        /// </summary>
        public string Text { get; }

        /// <summary>
        /// Response language in the format of a two-letter ISO 639-1 code. For example, lang=en — English. Specified in ISO 639-1.
        /// </summary>
        public string Language { get; set; }

        /// <summary>
        /// Maximum number of hints. An integer up to 10. The default value is results=7.
        /// </summary>
        public int Results { get; set; } = 7;

        /// <summary>
        /// By default, the service highlights matches in the results and returns a set of index ranges that can be highlighted in the interface. The highlight=0 value disables highlighting.
        /// </summary>
        public bool Highlight { get; set; } = true;

        /// <summary>
        /// The coordinates of the center of the search window if it is set as a rectangle with the coordinates of the center and a specified size.
        /// </summary>
        public (decimal Longitude, decimal Latitude)? SearchCenter { get; set; }

        /// <summary>
        /// The width and height of the search window in degrees if it is set as a rectangle with the coordinates of the center and a specified size. The values are separated by a comma, no space.
        ///  If this parameter isn't specified, the default value is spn=0.1,0.1.
        /// </summary>
        public (decimal width, decimal height) SearchBounds { get; set; } = (0.1M, 0.1M);

        /// <summary>
        /// The coordinates of the lower-left and upper-right corners of the search window if it is set using the coordinates of opposite corners.
        /// </summary>
        public ((decimal Longitude, decimal Latitude) LowerLeft, (decimal Longitude, decimal Latitude) UpperRight)? BoundingBox { get; set; }

        /// <summary>
        /// User's GPS coordinates (geolocation) Used to calculate distances.
        /// Passed in addition to the window.If this parameter isn't specified, the center of the window will be taken for calculations by default.
        /// </summary>
        public (decimal Longitude, decimal Latitude)? UserCoordinate { get; set; }

        /// <summary>
        /// The value strict_bounds=1 is used to strictly limit the output and keep only objects that fall within the window. The window is advisory in nature and doesn't impose strict restrictions on search results, helping to select the most relevant hints.
        /// </summary>
        public bool StrictBounds { get; set; } = false;

        /// <summary>
        /// Sets the type of object in the response. Filtering works through the "or" operator. In case of simultaneous use, the larger type absorbs the smaller ones
        /// </summary>
        public GeoSuggestType[] Types { get; set; }

        /// <summary>
        /// Returns a post-component address in the response. To do this, use the value print_address=1.
        /// </summary>
        public bool PrintAddress { get; set; }

        /// <summary>
        /// Returns a list of organizations with an address up to the house number. To do this, use the value org_address_kind=house. The parameter can be used on its own or together with types=biz or any other object type that returns an address in the response.
        /// </summary>
        public GeoSuggestType? OrgAddressKind { get; set; }

        /// <summary>
        /// Used in the value attrs=uri. Returns the uri parameter in the response, the value of which can be used in the Geocoder API request to get additional information about the object.
        /// </summary>
        public bool ReturnUri { get; set; }

        public NameValueCollection Fill(NameValueCollection result)
        {
            result["text"] = Text;
            result["results"] = Results.ToString();
            result["highlight"] = Highlight ? "1" : "0";
            result["spn"] = $"{SearchBounds.width.ToString(CultureInfo.InvariantCulture)},{SearchBounds.height.ToString(CultureInfo.InvariantCulture)}";
            if (!String.IsNullOrEmpty(Language))
                result["lang"] = Language;
            if (SearchCenter != null)
                result["ll"] = $"{SearchCenter.Value.Longitude.ToString(CultureInfo.InvariantCulture)},{SearchCenter.Value.Latitude.ToString(CultureInfo.InvariantCulture)}";
            if (BoundingBox != null)
                result["bbox"] = $"{BoundingBox.Value.LowerLeft.Longitude.ToString(CultureInfo.InvariantCulture)},{BoundingBox.Value.LowerLeft.Latitude.ToString(CultureInfo.InvariantCulture)},{BoundingBox.Value.UpperRight.Longitude.ToString(CultureInfo.InvariantCulture)},{BoundingBox.Value.UpperRight.Latitude.ToString(CultureInfo.InvariantCulture)}";
            if (UserCoordinate != null)
                result["ull"] = $"{UserCoordinate.Value.Longitude.ToString(CultureInfo.InvariantCulture)},{UserCoordinate.Value.Latitude.ToString(CultureInfo.InvariantCulture)}";
            if (StrictBounds)
                result["strict_bounds"] = "1";
            if (Types != null)
                result["types"] = Types.Aggregate(String.Empty, (h, t) => string.IsNullOrEmpty(h) ? t.ToString().ToLower() : h + "," + t.ToString().ToLower());
            if (PrintAddress)
                result["print_address"] = "1";
            if (OrgAddressKind != null)
                result["org_address_kind"] = OrgAddressKind.ToString().ToLower();
            if (ReturnUri)
                result["attrs"] = "uri";
            return result;
        }

        public override string ToString() => Fill(new NameValueCollection()).ToString();

    }

    /// <summary>
    /// Type of object in the response
    /// </summary>
    public enum GeoSuggestType
    {
        /// <summary>
        /// all organizations
        /// </summary>
        Biz,
        /// <summary>
        /// all geographical objects
        /// </summary>
        Geo,
        /// <summary>
        ///  street
        /// </summary>
        Street,
        /// <summary>
        ///  subway station
        /// </summary>
        Metro,
        /// <summary>
        /// district, microdistrict, village
        /// </summary>
        District,
        /// <summary>
        /// village, town, residential complex, cottage settlement, gardening cooperative
        /// </summary>
        Locality,
        /// <summary>
        /// regional district
        /// </summary>
        Area,
        /// <summary>
        /// province, administrative district, federal city
        /// </summary>
        Province,
        /// <summary>
        /// country
        /// </summary>
        Country,
        /// <summary>
        /// house, building, unit
        /// </summary>
        House
    }
}
